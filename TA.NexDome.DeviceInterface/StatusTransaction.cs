using System;
using System.Linq;
using System.Reactive.Linq;
using TA.Ascom.ReactiveCommunications;
using TA.Ascom.ReactiveCommunications.Diagnostics;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.DeviceInterface {
    internal class StatusTransaction : DeviceTransaction
        {
        private readonly ControllerStatusFactory factory;

        public StatusTransaction(ControllerStatusFactory factory) : base(Constants.CmdGetInfo)
            {
            this.factory = factory;
            }

        public IHardwareStatus HardwareStatus { get; private set; }

        public override void ObserveResponse(IObservable<char> source)
            {
            var statusResponse = source
                .DelimitedMessageStrings('V', '\n')
                .Trace("StatusTransaction")
                .Take(1)
                .Subscribe(OnNext, OnError, OnCompleted);
            }

        protected override void OnCompleted()
            {
            if (Response.Any())
                {
                var responseString = Response.Single();
                var status = factory.FromStatusPacket(responseString);
                HardwareStatus = status;
                }
            base.OnCompleted();
            }
        }
    }