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
// File: RotatorStateBase.cs  Last modified: 2020-07-21@21:41 by Tim Long

using System;
using System.Threading;
using System.Threading.Tasks;
using TA.NexDome.Common;
using TA.Utils.Core.Diagnostics;

namespace TA.NexDome.DeviceInterface.StateMachine.Rotator
    {
    internal abstract class RotatorStateBase : IRotatorState
        {
        protected readonly ILog Log;
        private CancellationTokenSource timeoutCancellation;

        protected RotatorStateBase(ControllerStateMachine machine)
            {
            Machine = machine;
            Log = machine.Logger;
            }

        public ControllerStateMachine Machine { get; }

        /// <inheritdoc />
        public virtual string Name => GetType().Name;

        /// <inheritdoc />
        public virtual void OnEnter() => Log.Debug().Message("Entering {state}", Name).Write();

        /// <inheritdoc />
        public virtual void OnExit()
            {
            CancelTimeout();
            Log.Debug().Message("Exiting {state}", Name);
            }

        /// <inheritdoc />
        public virtual void RotationDetected() => Log.Debug().Message("Rotation detected").Write();

        /// <inheritdoc />
        public virtual void StatusUpdateReceived(IRotatorStatus status) =>
            Log.Debug().Message("Status received: {status}", status).Write();

        /// <inheritdoc />
        public virtual void RotateToAzimuthDegrees(double azimuth) =>
            Log.Debug().Message("Rotate to azimuth {azimuth}", azimuth).Write();

        /// <inheritdoc />
        public virtual void RotateToStepPosition(int targetPosition) =>
            Log.Debug().Message("Rotate to position {position}", targetPosition).Write();

        /// <inheritdoc />
        public virtual void RotateToHomePosition() => Log.Debug().Message("Rotate to Home").Write();

        /// <inheritdoc />
        public virtual void RequestHardwareStatus() => Log.Debug().Message("Request status").Write();

        /// <inheritdoc />
        public virtual void HardStopRequested() => Log.Debug().Message("Hard stop").Write();

        /// <summary>Cancels any existing timeout and starts a new one with the specified time interval.</summary>
        /// <param name="timeout">The timeout interval.</param>
        protected void ResetTimeout(TimeSpan timeout)
            {
            timeoutCancellation?.Cancel();
            timeoutCancellation = new CancellationTokenSource();
            ResetTimeoutAsync(timeout, timeoutCancellation.Token);
            }

        /// <summary>
        ///     Uses <see cref="Task.Delay" /> to queue up a deferred timeout handler.
        ///     <see cref="HandleTimeout" /> will be called after the specified delay, unless
        ///     <see cref="CancelTimeout" /> is called.
        /// </summary>
        /// <param name="timeout">The time span before the timeout handler should be called.</param>
        /// <param name="cancel">A <see cref="CancellationToken" /> that can be used to cancel the timeout.</param>
        private async void ResetTimeoutAsync(TimeSpan timeout, CancellationToken cancel)
            {
            /*
             * This code needs to be protected in a try-catch block because
             * it is async void and an unhandled exception here would
             * crash the process.
             */
            try
                {
                await Task.Delay(timeout, cancel);
                if (cancel.IsCancellationRequested)
                    return;
                HandleTimeout();
                }
            catch (TaskCanceledException)
                {
                // This is expected, no action necessary.
                }
            catch (Exception ex)
                {
                Log.Warn().Exception(ex).Message("Exception while awaiting state timeout. This is unexpected.").Write();
                }
            }

        /// <summary>
        ///     Called when the state's timeout expires. Derived classes that wish to handle a timeout should
        ///     override this method.
        /// </summary>
        protected internal virtual void HandleTimeout()
            {
            Log.Warn().Message("state timed out").Write();
            }

        /// <summary>
        ///     Cancels the state's timeout. Note that because cancellation is cooperative, it is not
        ///     guaranteed that cancellation will be successful.
        /// </summary>
        protected void CancelTimeout()
            {
            timeoutCancellation?.Cancel();
            }
        }
    }