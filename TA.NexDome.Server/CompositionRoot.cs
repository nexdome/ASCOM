﻿// This file is part of the TA.NexDome.AscomServer project
//
// Copyright © 2015-2020 Tigra Astronomy, all rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so. The Software comes with no warranty of any kind.
// You make use of the Software entirely at your own risk and assume all liability arising from your use thereof.
//
// File: CompositionRoot.cs  Last modified: 2020-07-21@02:49 by Tim Long

using System;
using System.Text;
using Ninject;
using Ninject.Activation;
using Ninject.Infrastructure.Disposal;
using Ninject.Modules;
using Ninject.Syntax;
using NLog.Fluent;
using TA.Ascom.ReactiveCommunications;
using TA.NexDome.Common;
using TA.NexDome.DeviceInterface;
using TA.NexDome.DeviceInterface.StateMachine;
using TA.Utils.Core;
using TA.Utils.Core.Diagnostics;
using TA.Utils.Logging.NLog;
using static TA.NexDome.Server.Properties.Settings;

namespace TA.NexDome.Server
    {
    public static class CompositionRoot
        {
        static CompositionRoot()
            {
            Kernel = new StandardKernel(new CoreModule());
            }

        private static SessionScopeObject CurrentScope { get; set; }

        public static IKernel Kernel { get; }

        public static void BeginSessionScope()
            {
            var scope = new SessionScopeObject();
            Log.Info().Message("Beginning session scope {scope}", scope).Write();
            CurrentScope = scope;
            }

        public static IBindingNamedWithOrOnSyntax<T> InSessionScope<T>(this IBindingInSyntax<T> binding)
            {
            return binding.InScope(ctx => CurrentScope);
            }

        public static void EndSessionScope()
            {
            Log.Info().Message("Ending session scope {scope}", CurrentScope).Write();
            CurrentScope?.Dispose();
            CurrentScope = null;
            }
        }

    internal class SessionScopeObject : INotifyWhenDisposed
        {
        private static int scopeId;

        public SessionScopeObject()
            {
            ++scopeId;
            }

        public int ScopeId => scopeId;

        /// <inheritdoc />
        public virtual void Dispose()
            {
            try
                {
                Disposed?.Invoke(this, EventArgs.Empty);
                }
            finally
                {
                IsDisposed = true;
                }
            }

        /// <inheritdoc />
        public bool IsDisposed { get; private set; }

        /// <inheritdoc />
        public event EventHandler Disposed;

        /// <inheritdoc />
        public override string ToString() => ScopeId.ToString();
        }

    internal class CoreModule : NinjectModule
        {
        public override void Load()
            {
            Bind<DeviceController>().ToSelf().InSessionScope();
            Bind<ICommunicationChannel>().ToMethod(BuildCommunicationsChannel).InSessionScope();
            Bind<ChannelFactory>().ToSelf().InSessionScope();
            Bind<IClock>().To<SystemDateTimeUtcClock>().InSingletonScope();
            Bind<IControllerActions>().To<RxControllerActions>().InSessionScope();
            Bind<DeviceControllerOptions>().ToMethod(BuildDeviceOptions).InSessionScope();
            Bind<ReactiveTransactionProcessor>().ToSelf().InSessionScope();
            Bind<TransactionObserver>().ToSelf().InSessionScope();
            Bind<ITransactionProcessor>().ToMethod(BuildTransactionProcessor).InSessionScope();
            Bind<ILog>().ToMethod(CreateSessionLogger).InSessionScope();
            }

        private ILog CreateSessionLogger(IContext arg)
            {
            var scope = Maybe<SessionScopeObject>.From(arg.GetScope() as SessionScopeObject);
            var log = new LoggingService();
            return log.WithAmbientProperty("Session", scope)
                .WithAmbientProperty("Correlator", SharedResources.LogCorrelator);
            }

        private ITransactionProcessor BuildTransactionProcessor(IContext arg)
            {
            var observer = Kernel.Get<TransactionObserver>();
            var processor = Kernel.Get<ReactiveTransactionProcessor>();
            processor.SubscribeTransactionObserver(observer);
            return processor;
            }

        private ICommunicationChannel BuildCommunicationsChannel(IContext context)
            {
            var channelFactory = Kernel.Get<ChannelFactory>();
            var channel = channelFactory.FromConnectionString(Default.ConnectionString);
            ((SerialDeviceEndpoint)channel.Endpoint).Encoding=Encoding.ASCII;
            return channel;
            }

        private DeviceControllerOptions BuildDeviceOptions(IContext arg)
            {
            var options = new DeviceControllerOptions
                {
                HomeAzimuth = Default.HomeSensorAzimuth,
                MaximumShutterCloseTime =
                    TimeSpan.FromSeconds((double)Default.ShutterOpenCloseTimeSeconds),
                MaximumFullRotationTime = TimeSpan.FromSeconds((double)Default.FullRotationTimeSeconds),
                ShutterTickTimeout = Default.ShutterTickTimeout,
                RotatorTickTimeout = Default.RotatorTickTimeout,
                RotatorMaximumSpeed = Default.RotatorMaximumSpeed,
                RotatorRampTime = TimeSpan.FromMilliseconds(Default.RotatorRampTimeMilliseconds),
                ShutterMaximumSpeed = Default.ShutterMaximumSpeed,
                ShutterRampTime =
                    TimeSpan.FromMilliseconds(Default.ShutterAccelerationRampTimeMilliseconds),
                ParkAzimuth = Default.ParkAzimuth,
                TimeToWaitForShutterOnConnect = Default.OnConnectWaitForShutterOnline,
                ShutterIsInstalled = Default.ShutterIsInstalled,
                ShutterLowBatteryThresholdVolts = Default.ShutterLowVoltsThreshold,
                ShutterLowVoltsNotificationTimeToLive = Default.ShutterLowVoltsNotificationTimeToLive,
                EnableAutoCloseOnLowBattery = Default.ShutterAutoCloseOnLowBattery
                };
            return options;
            }
        }
    }