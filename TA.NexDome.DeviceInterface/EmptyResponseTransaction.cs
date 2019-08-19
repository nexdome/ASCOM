// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.DeviceInterface
    {
    using System;
    using System.Reactive.Linq;
    using System.Text;

    using TA.Ascom.ReactiveCommunications;
    using TA.Ascom.ReactiveCommunications.Diagnostics;

    internal class EmptyResponseTransaction : DeviceTransaction
        {
        private readonly string expectedResponse;

        /// <inheritdoc />
        public EmptyResponseTransaction(string command)
            : base(command.EnsureEncapsulation())
            {
            string rawCommand = Command.TrimStart('@').TrimEnd('\r', '\n');
            int commaPosition = rawCommand.IndexOf(','); // Comma separates the command verb from the arguments
            string verb = commaPosition > 0 ? rawCommand.Remove(commaPosition) : rawCommand;
            var builder = new StringBuilder();
            expectedResponse = builder.Append(':').Append(verb).Append('#').ToString();
            }

        /// <inheritdoc />
        public override void ObserveResponse(IObservable<char> source)
            {
            var validResponses = source.DelimitedMessageStrings().Where(r => r == expectedResponse);
            validResponses.Trace("Ack").Take(1).Subscribe(OnNext, OnError, OnCompleted);
            }
        }
    }