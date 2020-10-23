// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using TA.NexDome.Common;

namespace TA.NexDome.Specifications.Fakes
    {
    using System;
    using System.Diagnostics.Contracts;

    class FakeClock : IClock
        {
        DateTime currentTime;

        public FakeClock(DateTime initialTime)
            {
            currentTime = initialTime;
            }

        [Pure]
        public DateTime GetCurrentTime() => currentTime;

        public void AdvanceBy(TimeSpan amount)
            {
            Contract.Requires(amount > TimeSpan.Zero);
            currentTime += amount;
            }

        public void AdvanceTo(DateTime time)
            {
            Contract.Requires(time > GetCurrentTime());
            Contract.Requires(time.Kind == DateTimeKind.Utc);
            currentTime = time;
            }

        [ContractInvariantMethod]
        void ObjectInvariant()
            {
            Contract.Invariant(currentTime.Kind == DateTimeKind.Utc);
            }
        }
    }