// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: CompositionRoot.cs  Last modified: 2018-06-17@17:22 by Tim Long

using System;
using Ninject;
using Ninject.Activation;
using Ninject.Modules;
using Ninject.Syntax;
using NLog.Fluent;
using TA.Ascom.ReactiveCommunications;
using TA.NexDome.DeviceInterface;
using TA.NexDome.DeviceInterface.StateMachine;
using TA.NexDome.Server.Properties;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.Server
    {
    public static class CompositionRoot
        {
        static CompositionRoot()
            {
            Kernel = new StandardKernel(new CoreModule());
            }

        private static ScopeObject CurrentScope { get; set; }

        public static IKernel Kernel { get; }

        public static void BeginSessionScope()
            {
            var scope = new ScopeObject();
            Log.Info()
                .Message($"Beginning session scope id={scope.ScopeId}")
                .Write();
            CurrentScope = scope;
            }

        public static IBindingNamedWithOrOnSyntax<T> InSessionScope<T>(this IBindingInSyntax<T> binding)
            {
            return binding.InScope(ctx => CurrentScope);
            }
        }

    internal class ScopeObject
        {
        private static int scopeId;

        public ScopeObject()
            {
            ++scopeId;
            }

        public int ScopeId => scopeId;
        }

    internal class CoreModule : NinjectModule
        {
        public override void Load()
            {
            Bind<DeviceController>().ToSelf().InSessionScope();
            Bind<ICommunicationChannel>()
                .ToMethod(BuildCommunicationsChannel)
                .InSessionScope();
            Bind<ChannelFactory>().ToSelf().InSessionScope();
            Bind<IClock>().To<SystemDateTimeUtcClock>().InSingletonScope();
            Bind<IControllerActions>().To<RxControllerActions>().InSessionScope();
            Bind<DeviceControllerOptions>().ToMethod(BuildDeviceOptions).InSessionScope();
            }

        private ICommunicationChannel BuildCommunicationsChannel(IContext context)
            {
            var channelFactory = Kernel.Get<ChannelFactory>();
            var channel = channelFactory.FromConnectionString(Settings.Default.ConnectionString);
            return channel;
            }

        private DeviceControllerOptions BuildDeviceOptions(IContext arg)
            {
            var options = new DeviceControllerOptions
                {
                PerformShutterRecovery = Settings.Default.PerformShutterRecovery,
                MaximumShutterCloseTime = TimeSpan.FromSeconds((double) Settings.Default.ShutterOpenCloseTimeSeconds),
                MaximumFullRotationTime = TimeSpan.FromSeconds((double) Settings.Default.FullRotationTimeSeconds),
                KeepAliveTimerInterval = Settings.Default.KeepAliveTimerPeriod,
                CurrentDrawDetectionThreshold = Settings.Default.CurrentDrawDetectionThreshold,
                IgnoreHardwareShutterSensor = Settings.Default.IgnoreHardwareShutterSensor,
                ShutterTickTimeout = Settings.Default.ShutterTickTimeout
                };
            return options;
            }
        }
    }