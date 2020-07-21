// This file is part of the TA.NexDome.AscomServer project
//
// Copyright © 2015-2020 Tigra Astronomy, all rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so. The Software comes with no warranty of any kind.
// You make use of the Software entirely at your own risk and assume all liability arising from your use thereof.
//
// File: ClientConnectionManager.cs  Last modified: 2020-07-20@14:48 by Tim Long

using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Threading;
using TA.NexDome.DeviceInterface;
using TA.Utils.Core;
using TA.Utils.Core.Diagnostics;

namespace TA.NexDome.Server
    {
    /// <summary>
    ///     Manages client (driver) connections to the shared device controller. Uses the Reader Writer
    ///     Lock pattern to ensure thread safety.
    /// </summary>

    [ReaderWriterSynchronized]
    public class ClientConnectionManager
        {
        private readonly ILog log;

        private readonly bool performActionsOnOpen;

        /// <summary>Initializes a new instance of the <see cref="ClientConnectionManager" /> class.</summary>
        /// <param name="factory">
        ///     A factory class that can create and destroy transaction processors (and by implication, the
        ///     entire communications stack).
        /// </param>
        public ClientConnectionManager()
            : this(performActionsOnOpen: true) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientConnectionManager" /> class and allows
        /// control of whether an On Connected actions will be performed. This is primarily intended for
        /// use in unit testing so is not visible to clients.
        /// </summary>
        /// <param name="performActionsOnOpen">if set to <c>true</c> [perform actions on open].</param>
        internal ClientConnectionManager(bool performActionsOnOpen)
            {
            this.performActionsOnOpen = performActionsOnOpen;
            log = CompositionRoot.Kernel.Get<ILog>();
            Clients = new List<ClientStatus>();
            MaybeControllerInstance = Maybe<DeviceController>.Empty;
            }

        internal List<ClientStatus> Clients { get; }

        /// <summary>Gets the number of connected clients.</summary>
        /// <value>The connected client count.</value>
        public int RegisteredClientCount => Clients.Count();

        public int OnlineClientCount => Clients.Count(p => p.Online);

        /// <summary>Gets the controller instance if it has been created.</summary>
        /// <value>The controller instance.</value>
        internal Maybe<DeviceController> MaybeControllerInstance
            {
            get;
            private set;
            }

        /// <summary>Gets the controller instance, ensuring that it is open and ready for use.</summary>
        /// <param name="clientId">
        ///     The client must provide it's ID which has previously been obtained by calling
        ///     <see cref="RegisterClient" />.
        /// </param>
        /// <returns>IIntegraController.</returns>
        /// <exception cref="System.InvalidOperationException">
        ///     Clients must release previous controller
        ///     instances before requesting another
        /// </exception>
        [Writer]
        public Maybe<DeviceController> GoOnline(Guid clientId)
            {
            log.Info($"Go online for client {clientId}");
            ClientStatus client = null;
            try
                {
                client = Clients.Single(p => p.Equals(clientId));
                }
            catch (InvalidOperationException e)
                {
                string message = $"Attempt to go online with unregistered client {clientId}";
                log.Error()
                    .Exception(e)
                    .Message("Attempt to go online with unregistered client {clientId}", clientId)
                    .Write();
                return Maybe<DeviceController>.Empty;
                }

            try
                {
                EnsureControllerInstanceCreatedAndOpen();
                }
            catch (TimeoutException tex)
                {
                log.Error()
                    .Exception(tex)
                    .Message("Not connected because state machine timed out waiting for Ready state")
                    .Write();
                DestroyControllerInstance();
                return Maybe<DeviceController>.Empty;
                }

            client.Online = true;
            RaiseClientStatusChanged();
            return MaybeControllerInstance;
            }

        internal event EventHandler<EventArgs> ClientStatusChanged;

        protected void RaiseClientStatusChanged()
            {
            ClientStatusChanged?.Invoke(this, EventArgs.Empty);
            }

        [Writer]
        private void EnsureControllerInstanceCreatedAndOpen()
            {
            if (!MaybeControllerInstance.Any())
                {
                CompositionRoot.BeginSessionScope();
                var controller = CompositionRoot.Kernel.Get<DeviceController>();
                MaybeControllerInstance = controller.AsMaybe();
                }

            var instance = MaybeControllerInstance.Single();
            if (!instance.IsConnected)
                instance.Open(performActionsOnOpen);
            }

        [Writer]
        private void DestroyControllerInstance()
            {
            if (MaybeControllerInstance.Any())
                MaybeControllerInstance.Single().Dispose();
            MaybeControllerInstance = Maybe<DeviceController>.Empty;
            CompositionRoot.EndSessionScope();
            }

        [Writer]
        public void GoOffline(Guid clientId)
            {
            log.Info($"Go offline for client {clientId}");
            ClientStatus client = null;
            try
                {
                client = Clients.Single(p => p.Equals(clientId));
                }
            catch (InvalidOperationException e)
                {
                string message = $"Attempt to go offline by unecognized client {clientId}";
                log.Error()
                    .Exception(e)
                    .Message("Attempt to go offline by unregistered client {clientId}", clientId)
                    .Write();
                }

            client.Online = false;
            RaiseClientStatusChanged();
            if (OnlineClientCount == 0)
                {
                log.Warn("The last client has gone offline - closing connection");

                if (MaybeControllerInstance.Any())
                    {
                    var controller = MaybeControllerInstance.Single();
                    controller.Close();
                    }

                MaybeControllerInstance = Maybe<DeviceController>.Empty;
                }
            }

        /// <summary>Determines whether the client with the specified ID is registered.</summary>
        /// <param name="clientId">The client unique identifier.</param>
        /// <returns><c>true</c> if the client is connected; otherwise, <c>false</c>.</returns>
        [Reader]
        public bool IsClientRegistered(Guid clientId) => Clients.Any(p => p.Equals(clientId));

        /// <summary>Gets a new unique client identifier.</summary>
        /// <returns>Guid.</returns>
        [Writer]
        public Guid RegisterClient(string name = null)
            {
            var id = Guid.NewGuid();
            var status = new ClientStatus {ClientId = id, Name = name ?? id.ToString(), Online = false};
            Clients.Add(status);
            RaiseClientStatusChanged();
            return id;
            }

        [Writer]
        public void UnregisterClient(Guid clientId)
            {
            log.Info($"Unregistering client {clientId}");
            int previousClientCount = RegisteredClientCount;
            try
                {
                Clients.Remove(Clients.Single(p => p.Equals(clientId)));
                RaiseClientStatusChanged();
                }
            catch (InvalidOperationException e)
                {
                log.Error()
                    .Exception(e)
                    .Message("Attempt to unregister unknown client {clientId}", clientId)
                    .Write();
                }

            if (previousClientCount == 1 && RegisteredClientCount == 0)
                {
                DestroyControllerInstance();
                Server.TerminateLocalServer();
                }
            }
        }
    }