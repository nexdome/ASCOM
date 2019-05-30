using System;
using System.Threading;
using System.Threading.Tasks;
using NLog.Fluent;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.DeviceInterface.StateMachine.Shutter
    {
    class ShutterStateBase : IShutterState
        {
        private CancellationTokenSource timeoutCancellation;

        protected ShutterStateBase(ControllerStateMachine machine)
            {
            Machine = machine;
            }

        protected ControllerStateMachine Machine { get; }

        /// <inheritdoc />
        public virtual string Name => this.GetType().Name;

        /// <inheritdoc />
        public virtual void OnEnter() => Log.Debug().Message("Entering {state}", Name).Write();

        /// <inheritdoc />
        public virtual void OnExit()
            {
            CancelTimeout();
            Log.Debug().Message("Exiting {state}", Name);
            }

        /// <inheritdoc />
        public virtual void ShutterMovementDetected() { }

        /// <inheritdoc />
        public virtual void StatusUpdateReceived(IShutterStatus status) =>
            Log.Debug().Message("Status received: {status}", status).Write();

        /// <inheritdoc />
        public void OpenShutter() { }

        /// <inheritdoc />
        public void CloseShutter() { }

        /// <inheritdoc />
        public virtual void RequestHardwareStatus() => Log.Debug().Message("Request status").Write();

        /// <inheritdoc />
        public virtual void LinkStateReceived(ShutterLinkState state) => Log.Debug().Message("Link state {state}/{displayState}", state, state.DisplayEquivalent()).Write();

        /// <summary>
        ///     Cancels any existing timeout and starts a new one with the specified time interval.
        /// </summary>
        /// <param name="timeout">The timeout interval.</param>
        protected void ResetTimeout(TimeSpan timeout)
            {
            timeoutCancellation?.Cancel();
            timeoutCancellation = new CancellationTokenSource();
            ResetTimeoutAsync(timeout, timeoutCancellation.Token);
            }

        /// <summary>
        ///     Uses <see cref="Task.Delay"/> to queue up a deferred timeout handler. 
        ///     <see cref="HandleTimeout"/> will be called after the specified delay, unless 
        ///     <see cref="CancelTimeout"/> is called.
        /// </summary>
        /// <param name="timeout">The time span before the timeout handler should be called.</param>
        /// <param name="cancel">A <see cref="CancellationToken" /> that can be used to cancel the
        /// timeout.</param>
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

        /// <summary>
        ///     Called when the state's timeout expires. Derived classes that wish to handle a
        ///     timeout should override this method.
        /// </summary>
        protected virtual void HandleTimeout()
            {
            Log.Warn().Message("state timed out").Write();
            }

        /// <summary>
        ///     Cancels the state's timeout. Note that because cancellation is cooperative, it is
        ///     not guaranteed that cancellation will be successful.
        /// </summary>
        protected void CancelTimeout()
            {
            timeoutCancellation?.Cancel();
            }

        public virtual void ShutterDirectionReceived(ShutterDirection direction) => Log.Debug().Message("Shutter direction {direction}", direction).Write();
        }
    }