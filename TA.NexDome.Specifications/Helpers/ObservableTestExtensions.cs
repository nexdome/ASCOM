using System;
using System.Threading;

namespace TA.NexDome.Specifications.Helpers
{
    internal static class ObservableTestExtensions
    {

    public static void SubscribeAndWaitForCompletion<T>(this IObservable<T> sequence, Action<T> observer)
        {
        var sequenceComplete = new ManualResetEvent(false);
        var subscription = sequence.Subscribe(
            onNext: observer,
            onCompleted: () => sequenceComplete.Set()
            );
        sequenceComplete.WaitOne();
        subscription.Dispose();
        sequenceComplete.Dispose();
        }
    }
}
