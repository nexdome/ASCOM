// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.Specifications.Helpers
    {
    using System;
    using System.Threading;

    static class ObservableTestExtensions
        {
        public static void SubscribeAndWaitForCompletion<T>(this IObservable<T> sequence, Action<T> observer)
            {
            var sequenceComplete = new ManualResetEvent(false);
            var subscription = sequence.Subscribe(onNext: observer, onCompleted: () => sequenceComplete.Set());
            sequenceComplete.WaitOne();
            subscription.Dispose();
            sequenceComplete.Dispose();
            }
        }
    }