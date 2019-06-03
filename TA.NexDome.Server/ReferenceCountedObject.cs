// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: ReferenceCountedObject.cs  Last modified: 2018-03-28@22:19 by Tim Long

using System.Runtime.InteropServices;

namespace TA.NexDome.Server
    {
    [ComVisible(false)]
    public abstract class ReferenceCountedObject
        {
        public ReferenceCountedObject()
            {
            // We increment the global count of objects.
            Server.CountObject();
            }

        ~ReferenceCountedObject()
            {
            // We decrement the global count of objects.
            Server.UncountObject();
            // We then immediately test to see if we the conditions
            // are right to attempt to terminate this server application.
            Server.ExitIf();
            }
        }
    }