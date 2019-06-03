// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: AppDomainIsolated.cs  Last modified: 2018-03-28@22:20 by Tim Long

using System;

namespace TA.NexDome.Server
    {
    internal class AppDomainIsolated<TWorker> : IDisposable
        where TWorker : MarshalByRefObject
        {
        private AppDomain domain;

        public AppDomainIsolated()
            {
            domain = AppDomain.CreateDomain("Isolated:" + Guid.NewGuid(),
                null, AppDomain.CurrentDomain.SetupInformation);
            var type = typeof(TWorker);
            Worker = (TWorker) domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
            }

        /// <summary>
        ///     Gets the worker object that will run in the isolated app domain.
        /// </summary>
        /// <value>The worker.</value>
        public TWorker Worker { get; }

        public void Dispose()
            {
            if (domain != null)
                {
                AppDomain.Unload(domain);

                domain = null;
                }
            }
        }
    }