using CommandLine;

namespace TA.NexDome.FirmwareUpdater
{
    class FirmwareUploaderOptions
    {
    [Option('p',"PortName", Required = true, HelpText = "Specify the port name where the device is connected.")]
    public string PortName { get; private set; }

    [Option('h',"HexFile", Required = true, HelpText = "Specify the name of a *.hex file containing the firmware image.")]
    public string HexFile { get; private set; }

    [Option('v', "Verbose", Required = false, Default = false, HelpText = "Show verbose output.")]
    public bool Verbose { get; private set; }
    }
}
