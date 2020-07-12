using Machine.Specifications;
using TA.NexDome.DeviceInterface;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.Specifications.Math
    {
    [Subject(typeof(ValueConverters), "Volts/ADU conversion")]
    class when_converting_15_volts_to_analog_digital_units
        {
        const decimal volts = 15.0M;
        const uint expectedAdu = 1023;
        It should_convert = () => volts.VoltsToAdu().ShouldEqual(expectedAdu);
        }

    [Subject(typeof(ValueConverters), "Volts/ADU conversion")]
    class when_converting_max_adu_to_volts
        {
        const uint adu = 1023;
        const decimal expectedVolts = 15.0M;
        It should_convert = () => adu.AduToVolts().ShouldEqual(expectedVolts);
        }
    }