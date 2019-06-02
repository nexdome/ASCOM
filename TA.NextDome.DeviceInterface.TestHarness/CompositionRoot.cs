// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: CompositionRoot.cs  Last modified: 2018-06-17@17:22 by Tim Long

using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Ninject;
using Ninject.Activation;
using Ninject.Modules;
using Ninject.Syntax;
using NLog.Fluent;
using TA.Ascom.ReactiveCommunications;
using TA.NexDome.DeviceInterface;
using TA.NexDome.DeviceInterface.StateMachine;
using TA.NexDome.SharedTypes;

namespace TA.NextDome.DeviceInterface.TestHarness
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
            Bind<ChannelFactory>().ToMethod(BuildChannelFactory).InSessionScope();
            Bind<IClock>().To<SystemDateTimeUtcClock>().InSingletonScope();
            Bind<IControllerActions>().To<RxControllerActions>().InSessionScope();
            Bind<DeviceControllerOptions>().ToMethod(BuildDeviceOptions).InSessionScope();
            }

        private ICommunicationChannel BuildCommunicationsChannel(IContext context)
            {
            var settings = LoadSettings();
            var channelFactory = Kernel.Get<ChannelFactory>();
            var channel = channelFactory.FromConnectionString((string) settings["ConnectionString"]);
            return channel;
            }

        private ChannelFactory BuildChannelFactory(IContext arg)
            {
            var factory = new ChannelFactory();
            return factory;
            }

        private DeviceControllerOptions BuildDeviceOptions(IContext arg)
            {
            var options = new DeviceControllerOptions
                {
                PerformShutterRecovery = false,
                MaximumShutterCloseTime = TimeSpan.FromSeconds(120),
                MaximumFullRotationTime = TimeSpan.FromSeconds(120),
                KeepAliveTimerInterval = TimeSpan.FromSeconds(600),
                CurrentDrawDetectionThreshold = 20,
                IgnoreHardwareShutterSensor = false,
                ShutterTickTimeout = TimeSpan.FromSeconds(2.5),
                RotatorTickTimeout = TimeSpan.FromSeconds(2.5),
                };
            return options;
            }

        private JObject LoadSettings()
            {
            var location = Assembly.GetExecutingAssembly().Location;
            var workingDirectory = Path.GetDirectoryName(location);
            var sourceFile = Path.Combine(workingDirectory, "TestSettings.json");
            JObject settings = JObject.Parse(File.ReadAllText(sourceFile));
            return settings;
            }
        }
    }