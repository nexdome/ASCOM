using System;

namespace TA.NexDome.SharedTypes
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