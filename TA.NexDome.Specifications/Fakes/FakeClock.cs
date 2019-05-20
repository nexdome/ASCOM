// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: FakeClock.cs  Last modified: 2018-03-28@18:42 by Tim Long

using System;
using System.Diagnostics.Contracts;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.Specifications.Fakes
    {
    internal class FakeClock : IClock
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