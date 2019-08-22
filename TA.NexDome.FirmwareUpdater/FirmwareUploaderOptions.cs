// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.FirmwareUpdater
    {
    using CommandLine;

    internal class FirmwareUploaderOptions
        {
        [Option('p', "PortName", Required = true, HelpText = "Specify the port name where the device is connected.")]
        public string PortName { get; private set; }

        [Option(
            'h',
            "HexFile",
            Required = true,
            HelpText = "Specify the name of a *.hex file containing the firmware image.")]
        public string HexFile { get; private set; }

        [Option('v', "Verbose", Required = false, Default = false, HelpText = "Show verbose output.")]
        public bool Verbose { get; private set; }
        }
    }