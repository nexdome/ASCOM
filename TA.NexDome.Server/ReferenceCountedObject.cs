// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.Server
    {
    using System.Runtime.InteropServices;

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