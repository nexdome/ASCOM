// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.Server
    {
    using System;

    internal class AppDomainIsolated<TWorker> : IDisposable
        where TWorker : MarshalByRefObject
        {
        private AppDomain domain;

        public AppDomainIsolated()
            {
            domain = AppDomain.CreateDomain(
                "Isolated:" + Guid.NewGuid(),
                null,
                AppDomain.CurrentDomain.SetupInformation);
            var type = typeof(TWorker);
            Worker = (TWorker)domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
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