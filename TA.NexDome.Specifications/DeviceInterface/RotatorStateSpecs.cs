using Machine.Specifications;
using TA.NexDome.DeviceInterface.StateMachine;
using TA.NexDome.SharedTypes;
using TA.NexDome.Specifications.Contexts;

namespace TA.NexDome.Specifications.DeviceInterface
    {
    [Subject(typeof(ControllerStateMachine), "startup")]
    internal class when_creating_the_controller_state_machine : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder.Build();
        It should_have_rotator_in_ready_state = () => Machine.RotatorState.ShouldBeOfExactType<ReadyState>();
        It should_have_shutter_in_offline_state;
        It should_not_be_at_home = () => Machine.AtHome.ShouldBeFalse();
        It should_be_at_azimuth_zero = () => Machine.AzimuthEncoderPosition.ShouldEqual(0);
        }

    internal class ReadyState : IRotatorState {
        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public void OnEnter() { }

        /// <inheritdoc />
        public void OnExit() { }

        /// <inheritdoc />
        public void RotationDetected() { }

        /// <inheritdoc />
        public void StatusUpdateReceived(IRotatorStatus status) { }

        /// <inheritdoc />
        public void RotateToAzimuthDegrees(double azimuth) { }

        /// <inheritdoc />
        public void RotateToHomePosition() { }

        /// <inheritdoc />
        public void RequestHardwareStatus() { }
        }
    }