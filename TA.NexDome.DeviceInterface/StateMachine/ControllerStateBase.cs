// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: ControllerStateBase.cs  Last modified: 2018-03-30@03:22 by Tim Long

using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using NLog.Fluent;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.DeviceInterface.StateMachine
    {
    internal abstract class ControllerStateBase : IControllerState
        {
        protected readonly ControllerStateMachine machine;
        private CancellationTokenSource timeoutCancellation;

        protected ControllerStateBase(ControllerStateMachine machine)
            {
            Contract.Requires(machine != null);
            this.machine = machine;
            }

        public virtual void OnEnter() { }

        public virtual void OnExit()
            {
            timeoutCancellation?.Cancel();
            }

        public virtual void RotationDetected() { }

        public virtual void ShutterMovementDetected() { }

        public virtual void StatusUpdateReceived(IHardwareStatus status) { }

        public virtual string Name => GetType().Name;

        public virtual void RotateToAzimuthDegrees(double azimuth) { }

        public virtual void OpenShutter() { }

        public virtual void CloseShutter() { }

        public virtual void RotateToHomePosition() { }

        public virtual void SetUserOutputPins(Octet newState) { }

        protected void ResetTimeout(TimeSpan timeout)
            {
            timeoutCancellation?.Cancel();
            timeoutCancellation = new CancellationTokenSource();
            var timeoutCancellationToken = timeoutCancellation.Token;
            ResetTimeoutAsync(timeout, timeoutCancellationToken);
            }

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
                Log.Warn()
                    .Exception(ex)
                    .Message("Exception while awaiting state timeout. This is unexpected.")
                    .Write();
                }
            }

        protected virtual void HandleTimeout()
            {
            Log.Warn().Message("state timed out").Write();
            }

        protected void CancelTimeout()
            {
            timeoutCancellation?.Cancel();
            }

        public virtual void RequestHardwareStatus() {}
        }
    }