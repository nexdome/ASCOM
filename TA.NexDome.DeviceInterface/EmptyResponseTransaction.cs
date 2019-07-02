using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using TA.Ascom.ReactiveCommunications;
using TA.Ascom.ReactiveCommunications.Diagnostics;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.DeviceInterface
    {

    class EmptyResponseTransaction : DeviceTransaction
        {
        private string expectedResponse;

        /// <inheritdoc />
        public EmptyResponseTransaction(string command) : base(command.EnsureEncapsulation())
            {
            var rawCommand = Command.TrimStart('@').TrimEnd('\r', '\n');
            var commaPosition = rawCommand.IndexOf(',');    // Comma separates the command verb from the arguments
            var verb = (commaPosition > 0) ? rawCommand.Remove(commaPosition) : rawCommand;
            var builder = new StringBuilder();
            expectedResponse = builder.Append(':').Append(verb).Append('#').ToString();
            }

        /// <inheritdoc />
        public override void ObserveResponse(IObservable<char> source)
            {
            var validResponses = source.DelimitedMessageStrings().Where(r => r == expectedResponse);
            validResponses.Trace("Ack")
                .Take(1)
                .Subscribe(OnNext, OnError, OnCompleted);
            }
        }
    }
