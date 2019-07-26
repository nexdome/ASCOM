using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TA.NexDome.Server.Properties;

namespace TA.NexDome.Server
    {
    public partial class FirmwareUpdate : Form
        {
        private readonly ClickCommand updateClickCommand;
        private IList<ComPortDescriptor> comPortDescriptors;
        private List<FirmwareImageDescriptor> firmwareImages;

        public FirmwareUpdate()
            {
            InitializeComponent();
            updateClickCommand = UpdateCommand.AttachCommand(ExecuteUpdateFirmware, CanUpdateFirmware);
            ConsoleOutput.ProcessInterface.OnProcessExit += ProcessInterface_OnProcessExit;
            ConsoleOutput.ProcessInterface.OnProcessError += ProcessInterface_OnProcessError;
            }

        private void ProcessInterface_OnProcessError(object sender, ConsoleControlAPI.ProcessEventArgs args)
            {
            updateClickCommand.CanExecuteChanged();
            }

        private void ProcessInterface_OnProcessExit(object sender, ConsoleControlAPI.ProcessEventArgs args)
            {
            Cursor.Current = Cursors.Default;
            updateClickCommand.CanExecuteChanged();
            if (args.Code == 0)
                return; // Zero return code == SUCCESS
            UpdateProcessError.SetError(UpdateCommand, $"Update process FAILED with code {args.Code}");
            }

        /// <summary>
        /// Executes the firmware update using the users selections, which are assumed to be valid at this point.
        /// </summary>
        private void ExecuteUpdateFirmware()
            {
            Cursor.Current = Cursors.WaitCursor;
            ClearErrors();
            ConsoleOutput.ClearOutput();
            var hexFile = FirmwareImageName.SelectedValue.ToString();
            var comPort = ComPortName.SelectedValue.ToString();
            var utilityDirectory = Path.Combine(Settings.Default.FirmwareImageDirectory, "Uploader");
            var uploaderCommand = Path.Combine(utilityDirectory, Settings.Default.FirmwareUploaderExecutable);
            var argumentBuilder = new StringBuilder();
            argumentBuilder.Append($"--PortName {comPort} --HexFile {hexFile}");
            if (Settings.Default.FirmwareUploadVerboseOutput)
                argumentBuilder.Append(" --Verbose");
            var uploaderArguments = argumentBuilder.ToString();
            ConsoleOutput.StartProcess(uploaderCommand, uploaderArguments);
            updateClickCommand.CanExecuteChanged();
            }

        private void ClearErrors()
            {
            UpdateProcessError.Clear();
            }

        /// <summary>
        /// Determines whether a firmware update operation is currently valid.
        /// </summary>
        private bool CanUpdateFirmware()
            {
            if (!(FirmwareImageName.SelectedItem is FirmwareImageDescriptor))
                return false;
            if (!(ComPortName.SelectedItem is ComPortDescriptor))
                return false;
            return !ConsoleOutput.IsProcessRunning;
            }

        private IEnumerable<FirmwareImageDescriptor> EnumerateFirmwareImages(string baseDirectory)
            {
            var imageFiles = Directory.EnumerateFiles(baseDirectory, "*.hex");
            var query = from file in imageFiles
                        let shortName = Path.GetFileName(file)
                        select new FirmwareImageDescriptor {FullyQualifiedPath = file, DisplayName = shortName};
            return query;
            }

        private IEnumerable<ComPortDescriptor> EnumerateComPorts()
            {
            ManagementBaseObject thing;
            var wmi = new ManagementObjectSearcher(@"root\CIMV2", "SELECT * FROM Win32_SerialPort");
            var candidatePorts = wmi.Get();
            var query = from item in candidatePorts.Cast<ManagementBaseObject>()
                        let portName = item["DeviceId"].ToString()
                        let caption = item["Caption"].ToString()
                        where caption.Contains("Arduino")
                        orderby portName
                        select new ComPortDescriptor {PortName = portName, Caption = caption};
            return query;
            }

        private class ComPortDescriptor
            {
            public string PortName { get; set; }
            public string Caption { get; set; }
            }

        private class FirmwareImageDescriptor
            {
            public string DisplayName { get; set; }

            public string FullyQualifiedPath { get; set; }
            }

        private void FirmwareUpdate_Load(object sender, EventArgs e)
            {
            var originalCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            var firmwareImageDirectory = Settings.Default.FirmwareImageDirectory;
            firmwareImages = EnumerateFirmwareImages(firmwareImageDirectory).ToList();
            FirmwareImageName.DataSource = firmwareImages;
            FirmwareImageName.ValueMember = "FullyQualifiedPath";
            FirmwareImageName.DisplayMember = "DisplayName";
            comPortDescriptors = EnumerateComPorts().ToList();
            ComPortName.DataSource = comPortDescriptors;
            ComPortName.DisplayMember = "Caption";
            ComPortName.ValueMember = "PortName";
            Cursor.Current = originalCursor;
            }

        private void FirmwareImageName_SelectedIndexChanged(object sender, EventArgs e)
            {
            ClearErrors();
            updateClickCommand.CanExecuteChanged();
            }

        private void ComPortName_SelectedIndexChanged(object sender, EventArgs e)
            {
            ClearErrors();
            updateClickCommand.CanExecuteChanged();
            }
        }
    }
