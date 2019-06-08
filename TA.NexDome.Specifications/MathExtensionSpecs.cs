using Machine.Specifications;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.Specifications
    {
    [Subject(typeof(MathExtensions), "clip")]
    internal class when_clipping_a_value_and_the_value_is_within_range
        {
        const float input = 123.45f;
        const float maximum = 200.00f;
        const float minimum = 100.00f;
        Because of = () => actual = input.Clip(minimum, maximum);
        It should_evaluate_to_the_input = () => actual.ShouldEqual(input);
        static float actual;
        }

    [Subject(typeof(MathExtensions), "clip")]
    internal class when_clipping_a_value_and_the_value_is_above_maximum
        {
        const float input = 200.01f;
        const float maximum = 200.00f;
        const float minimum = 100.00f;
        Because of = () => actual = input.Clip(minimum, maximum);
        It should_evaluate_to_maximum = () => actual.ShouldEqual(maximum);
        static float actual;
        }

    [Subject(typeof(MathExtensions), "clip")]
    internal class when_clipping_a_value_and_the_value_is_below_minimum
        {
        const float input = 99.99f;
        const float maximum = 200.00f;
        const float minimum = 100.00f;
        Because of = () => actual = input.Clip(minimum, maximum);
        It should_evaluate_to_minimum = () => actual.ShouldEqual(minimum);
        static float actual;
        }
    }