// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.Server
    {
    using System;
    using System.Threading;

    /// <summary>
    ///     Summary description for GarbageCollection.
    /// </summary>
    internal class GarbageCollection
        {
        protected volatile bool m_bContinueThread;

        protected ManualResetEvent m_EventThreadEnded;

        protected bool m_GCWatchStopped;

        protected int m_iInterval;

        public GarbageCollection(int iInterval)
            {
            m_bContinueThread = true;
            m_GCWatchStopped = false;
            m_iInterval = iInterval;
            m_EventThreadEnded = new ManualResetEvent(false);
            }

        public void GCWatch()
            {
            // Pause for a moment to provide a delay to make threads more apparent.
            while (ContinueThread())
                {
                GC.Collect();
                Thread.Sleep(m_iInterval);
                }

            m_EventThreadEnded.Set();
            }

        protected bool ContinueThread()
            {
            lock (this) return m_bContinueThread;
            }

        public void StopThread()
            {
            lock (this) m_bContinueThread = false;
            }

        public void WaitForThreadToStop()
            {
            m_EventThreadEnded.WaitOne();
            m_EventThreadEnded.Reset();
            }
        }
    }