// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using System;

namespace TA.NexDome.Common
    {
    public interface IClock
        {
        DateTime GetCurrentTime();
        }

    public class SystemDateTimeUtcClock : IClock
        {
        public DateTime GetCurrentTime()
            {
            return DateTime.UtcNow;
            }
        }
    }