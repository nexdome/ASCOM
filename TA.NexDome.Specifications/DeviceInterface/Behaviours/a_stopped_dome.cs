using Machine.Specifications;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.Specifications.DeviceInterface.Behaviours
    {
    [Behaviors]
    internal class a_stopped_dome : device_controller_behaviour
        {
        It should_not_be_rotating = () => Controller.AzimuthMotorActive.ShouldBeFalse();
        It should_should_not_be_moving_at_all = () => Controller.IsMoving.ShouldBeFalse();
        It should_not_have_a_rotation_direction =
            () => Controller.AzimuthDirection.ShouldEqual(RotationDirection.None);
        It should_draw_no_shutter_current = () => Controller.ShutterMotorCurrent.ShouldEqual(0);
        It should_not_have_a_shutter_direction = () => Controller.ShutterMovementDirection.ShouldEqual(ShutterDirection.None);
        It should_have_a_stationary_shutter = () => Controller.ShutterMotorActive.ShouldBeFalse();
        }
    }