using System;

namespace TA.NexDome.DeviceInterface.StateMachine.Rotator {
    class RotatingState : RotatorStateBase {

        /// <summary>
        ///     While rotating, azimuth position updates are expected to arrive at least this often.
        ///     If they do not, then it will be assumed that rotation may have stopped and we must
        ///     attempt to request a status update.
        /// </summary>
        private static readonly TimeSpan EncoderTickTimeout = TimeSpan.FromSeconds(2.5);

        /// <inheritdoc />
        public RotatingState(ControllerStateMachine machine) : base(machine) { }

        /// <inheritdoc />
        public override void OnEnter()
            {
            base.OnEnter();
            Machine.AzimuthMotorActive = true;
            ResetTimeout(EncoderTickTimeout);
            }
        }
    }