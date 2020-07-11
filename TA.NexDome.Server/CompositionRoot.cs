// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using static TA.NexDome.Server.Properties.Settings;

namespace TA.NexDome.Server
    {
    using System;

    using Ninject;
    using Ninject.Activation;
    using Ninject.Infrastructure.Disposal;
    using Ninject.Modules;
    using Ninject.Syntax;

    using NLog.Fluent;

    using TA.Ascom.ReactiveCommunications;
    using TA.NexDome.DeviceInterface;
    using TA.NexDome.DeviceInterface.StateMachine;
    using TA.NexDome.SharedTypes;

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

    internal class ScopeObject : INotifyWhenDisposed
        {
        private static int scopeId;

        public ScopeObject()
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
        public override string ToString() => $"{nameof(ScopeId)}: {ScopeId}";
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