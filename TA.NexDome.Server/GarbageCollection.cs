// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: GarbageCollection.cs  Last modified: 2018-03-28@22:19 by Tim Long

using System;
using System.Threading;

namespace TA.NexDome.Server
    {
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
            lock (this)
                {
                return m_bContinueThread;
                }
            }

        public void StopThread()
            {
            lock (this)
                {
                m_bContinueThread = false;
                }
            }

        public void WaitForThreadToStop()
            {
            m_EventThreadEnded.WaitOne();
            m_EventThreadEnded.Reset();
            }
        }
    }