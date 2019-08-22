// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.Specifications
    {
    using Machine.Specifications;

    using TA.NexDome.SharedTypes;

    [Subject(typeof(MathExtensions), "clip")]
    class when_clipping_a_value_and_the_value_is_within_range
        {
        Because of = () => actual = input.Clip(minimum, maximum);

        It should_evaluate_to_the_input = () => actual.ShouldEqual(input);

        static float actual;

        const float input = 123.45f;

        const float maximum = 200.00f;

        const float minimum = 100.00f;
        }

    [Subject(typeof(MathExtensions), "clip")]
    class when_clipping_a_value_and_the_value_is_above_maximum
        {
        Because of = () => actual = input.Clip(minimum, maximum);

        It should_evaluate_to_maximum = () => actual.ShouldEqual(maximum);

        static float actual;

        const float input = 200.01f;

        const float maximum = 200.00f;

        const float minimum = 100.00f;
        }

    [Subject(typeof(MathExtensions), "clip")]
    class when_clipping_a_value_and_the_value_is_below_minimum
        {
        Because of = () => actual = input.Clip(minimum, maximum);

        It should_evaluate_to_minimum = () => actual.ShouldEqual(minimum);

        static float actual;

        const float input = 99.99f;

        const float maximum = 200.00f;

        const float minimum = 100.00f;
        }
    }