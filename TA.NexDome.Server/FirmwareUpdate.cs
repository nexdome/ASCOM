// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.Server
    {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Management;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    using ConsoleControlAPI;

    using TA.NexDome.Server.Properties;

    public partial class FirmwareUpdate : Form
        {
        #region Native methods for manipulating the console control
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr window, int message, int wparam, int lparam);

        private const int SbBottom = 0x7;
        private const int WmVscroll = 0x115;
        #endregion

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

        private void ProcessInterface_OnProcessError(object sender, ProcessEventArgs args)
            {
            updateClickCommand.CanExecuteChanged();
            }

        private void ProcessInterface_OnProcessExit(object sender, ProcessEventArgs args)
            {
            Cursor.Current = Cursors.Default;
            updateClickCommand.CanExecuteChanged();
            if (args.Code == 0)
                return; // Zero return code == SUCCESS
            UpdateProcessError.SetError(UpdateCommand, $"Update process FAILED with code {args.Code}");
            }

        /// <summary>
        ///     Executes the firmware update using the users selections, which are assumed to be valid
        ///     at this point.
        /// </summary>
        private void ExecuteUpdateFirmware()
            {
            Cursor.Current = Cursors.WaitCursor;
            ClearErrors();
            ConsoleOutput.ClearOutput();
            string hexFile = FirmwareImageName.SelectedValue.ToString();
            string comPort = ComPortName.SelectedValue.ToString();
            string utilityDirectory = Path.Combine(Settings.Default.FirmwareImageDirectory, "Uploader");
            string uploaderCommand = Path.Combine(utilityDirectory, Settings.Default.FirmwareUploaderExecutable);
            var argumentBuilder = new StringBuilder();
            argumentBuilder.Append($"--PortName {comPort} --HexFile {hexFile}");
            if (Settings.Default.FirmwareUploadVerboseOutput)
                argumentBuilder.Append(" --Verbose");
            string uploaderArguments = argumentBuilder.ToString();
            ConsoleOutput.StartProcess(uploaderCommand, uploaderArguments);
            updateClickCommand.CanExecuteChanged();
            }

        private void ClearErrors()
            {
            UpdateProcessError.Clear();
            }

        /// <summary>
        ///     Determines whether a firmware update operation is currently valid.
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
                        select new FirmwareImageDescriptor { FullyQualifiedPath = file, DisplayName = shortName };
            return query;
            }

        private IEnumerable<ComPortDescriptor> EnumerateComPorts(bool showAll = false)
            {
            var wmi = new ManagementObjectSearcher(@"root\CIMV2", "SELECT * FROM Win32_SerialPort");
            var candidatePorts = wmi.Get();
            if (showAll)
                {
                var allPorts = from item in candidatePorts.Cast<ManagementBaseObject>()
                               let portName = item["DeviceId"].ToString()
                               let caption = item["Caption"].ToString()
                               orderby portName
                               select new ComPortDescriptor { PortName = portName, Caption = caption };
                return allPorts;
                }

            var arduinoPorts = from item in candidatePorts.Cast<ManagementBaseObject>()
                               let portName = item["DeviceId"].ToString()
                               let caption = item["Caption"].ToString()
                               where caption.Contains("Arduino")
                               orderby portName
                               select new ComPortDescriptor { PortName = portName, Caption = caption };
            return arduinoPorts;
            }

        private void FirmwareUpdate_Load(object sender, EventArgs e)
            {
            var originalCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            string firmwareImageDirectory = Settings.Default.FirmwareImageDirectory;
            firmwareImages = EnumerateFirmwareImages(firmwareImageDirectory).ToList();
            FirmwareImageName.DataSource = firmwareImages;
            FirmwareImageName.ValueMember = "FullyQualifiedPath";
            FirmwareImageName.DisplayMember = "DisplayName";
            PopulateComPortsComboBox();
            Cursor.Current = originalCursor;
            }

        private void PopulateComPortsComboBox(bool showAll = false)
            {
            ComPortName.BeginUpdate();
            comPortDescriptors?.Clear();
            comPortDescriptors = EnumerateComPorts(showAll).ToList();
            ComPortName.DataSource = comPortDescriptors;
            ComPortName.DisplayMember = "Caption";
            ComPortName.ValueMember = "PortName";
            ComPortName.EndUpdate();
            ComPortName.Invalidate();
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

        private void ShowAllComPorts_CheckedChanged(object sender, EventArgs e)
            {
            PopulateComPortsComboBox(ShowAllComPorts.Checked);
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

        /// <summary>
        /// Handles the OnConsoleOutput event of the ConsoleOutput control.
        /// Sends a native Windows message that causes scroll-to-bottom
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="ConsoleControl.ConsoleEventArgs"/> instance containing the event data.</param>
        private void ConsoleOutput_OnConsoleOutput(object sender, ConsoleControl.ConsoleEventArgs args)
            {
            var console = ConsoleOutput.InternalRichTextBox;
            SendMessage(console.Handle, WmVscroll, SbBottom, 0x0);
            }
        }
    }