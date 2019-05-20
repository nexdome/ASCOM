using System.Collections.Generic;
using System.Linq;
using TA.Ascom.ReactiveCommunications;

namespace TA.NexDome.Specifications.Fakes
    {
    class FakeTransactionProcessor : ITransactionProcessor
        {
        readonly IEnumerable<string> fakeResponses;
        readonly IEnumerator<string> responseEnumerator;

        public FakeTransactionProcessor(IEnumerable<string> fakeResponses)
            {
            var enumerable = fakeResponses.ToList();
            this.fakeResponses = enumerable;
            responseEnumerator = enumerable.GetEnumerator();
            }

        public List<DeviceTransaction> ProcessedTransactions { get; } = new List<DeviceTransaction>();

        public void CommitTransaction(DeviceTransaction transaction)
            {
            var moreResponses = responseEnumerator.MoveNext();
            if (moreResponses)
                transaction.SimulateCompletionWithResponse(responseEnumerator.Current);
            else
                transaction.TimedOut("Timeout");
            ProcessedTransactions.Add(transaction);
            }
        }
    }