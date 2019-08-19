// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.FirmwareUpdater
    {
    using System.Threading.Tasks;

    internal class Program
        {
        private static async Task Main(string[] args)
            {
            var app = new FirmwareUpdaterApp(args);
            await app.Run();
            }
        }
    }