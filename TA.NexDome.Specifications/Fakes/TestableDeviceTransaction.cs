using System;
using TA.Ascom.ReactiveCommunications;

namespace TA.NexDome.Specifications.Fakes
    {
    internal class TestableDeviceTransaction : DeviceTransaction
        {
        readonly DeviceTransaction sourceTransaction;

        public TestableDeviceTransaction(DeviceTransaction sourceTransaction) : base(sourceTransaction.Command)
            {
            this.sourceTransaction = sourceTransaction;
            }

        public override void ObserveResponse(IObservable<char> source)
            {
            throw new NotImplementedException();
            }

        void SetResponse(string response)
            {
            Response = new Maybe<string>(response);
            }

        internal void SignalCompletion(string fakeResponse)
            {
            SetResponse(fakeResponse);
            OnCompleted();
            }

        public void SignalError(string error)
            {
            OnError(new TimeoutException(error));
            }
        }
    }