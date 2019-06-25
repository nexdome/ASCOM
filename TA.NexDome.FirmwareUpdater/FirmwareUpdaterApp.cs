using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CommandLine;
using RJCP.IO.Ports;

namespace TA.NexDome.FirmwareUpdater
{
    class FirmwareUpdaterApp
    {
    private readonly string[] commandLineArguments;
    readonly List<string> errorMessages = new List<string>();


        public FirmwareUpdaterApp(string[] args)
        {
        commandLineArguments = args;
        }

    public async Task Run()
        {
        var caseInsensitiveParser = new Parser(with =>
            {
            with.CaseSensitive = false;
            with.IgnoreUnknownArguments = false;
            with.HelpWriter = Console.Out;
            with.AutoVersion = true;
            with.AutoHelp = true;
            });

        FirmwareUploaderOptions parsedOptions = new FirmwareUploaderOptions();
            try
                {
            
                var result = caseInsensitiveParser.ParseArguments<FirmwareUploaderOptions>(commandLineArguments);
                result
                    .WithParsed(options => parsedOptions = options)
                    .WithNotParsed(errors => errors.ToList().ForEach(e => errorMessages.Add(e.ToString())));
                }
        catch (Exception ex)
            {
            errorMessages.Add(ex.Message);
            }

        if (errorMessages.Any())
            Environment.Exit(-1);

        await PerformUpload(parsedOptions);
        }

    async Task PerformUpload(FirmwareUploaderOptions options)
        {
        Console.WriteLine($"COM port: {options.PortName}");
        Console.WriteLine($"File: {options.HexFile}");
        var ports = SerialPortStream.GetPortNames().ToList();
        //var descriptions = SerialPortStream.GetPortDescriptions().ToList();
        if (!ports.Contains(options.PortName))
            throw new InvalidOperationException($"The specified port does not exist on the system. Please make sure the device is connected and try again.");

        var bootloaderPort = await RunBootLoader(options.PortName, ports);
        if (string.IsNullOrWhiteSpace(bootloaderPort))
            throw new InvalidOperationException("Unable to detect boot-loader port. Firmware upload failed.");
        Console.WriteLine($"Detected bootloader port on {bootloaderPort}");
        var exitCode = await InvokeAvrDude(bootloaderPort, options.HexFile, options.Verbose);
        Console.WriteLine($"Programmer returned with exit code {exitCode}");
        var disposition = exitCode == 0 ? "SUCCESSFUL" : "FAILED";
        Console.WriteLine();
        Console.WriteLine($"*******************************************************");
        Console.WriteLine($"*                                                      ");
        Console.WriteLine($"*   Firmware update {disposition}");
        Console.WriteLine($"*                                                      ");
        Console.WriteLine($"*******************************************************");
        }

    private async Task<int> InvokeAvrDude(string bootloaderPort, string hexFile, bool showAvrDudeOutput = false)
        {
        // AVRDude is in the .\avrdude subdirectory
        var me = Assembly.GetExecutingAssembly();
        var myDirectory = Path.GetDirectoryName(me.Location);
        var avrDudeDirectory = Path.Combine(myDirectory, "avrdude");
        var avrDudeExecutable = Path.Combine(avrDudeDirectory, "avrdude.exe");
        var avrDudeConfig = Path.Combine(avrDudeDirectory, "avrdude.conf");
        var firmwareImage = Path.GetFullPath(hexFile);
        var avrDudeOptions =
            $"-C{avrDudeConfig} -v -patmega32u4 -cavr109 -b57600 -P{bootloaderPort} -D -Uflash:w:{firmwareImage}:i";
        TextWriter outputWriter = showAvrDudeOutput ? Console.Out : TextWriter.Null;
        TextWriter errorWriter = showAvrDudeOutput ? Console.Error : TextWriter.Null;
        var exitCode = await AsyncProcess.StartProcess(avrDudeExecutable, avrDudeOptions, avrDudeDirectory, 20000,
            outputWriter, errorWriter);
        return exitCode;
        }

        private async Task<string> RunBootLoader(string nonBootLoaderPort, List<string> baselinePorts)
        {
        var port = new SerialPortStream(nonBootLoaderPort, 1200);
        port.Open();
        await Task.Delay(TimeSpan.FromSeconds(1));
        port.Close();
        int triesRemaining = 8;
        while (triesRemaining > 0)
            {
            await Task.Delay(TimeSpan.FromSeconds(1));
            var candidates = SerialPortStream.GetPortNames().ToList();
            var candidateBootloaderPort = DetectNewPort(baselinePorts, candidates);
            if (!string.IsNullOrWhiteSpace(candidateBootloaderPort))
                return candidateBootloaderPort;
            --triesRemaining;
            }
        return string.Empty;
        }

    /// <summary>
    /// Detects whether a new COM port has appeared on the system and returns it,
    /// or String.Empty if non found.
    /// </summary>
    private string DetectNewPort(List<string> baselinePorts, List<string> portCandidates)
        {
        foreach (var candidate in portCandidates)
            {
            if (!baselinePorts.Contains(candidate))
                return candidate;
            }
        return string.Empty;
        }
    }
}
