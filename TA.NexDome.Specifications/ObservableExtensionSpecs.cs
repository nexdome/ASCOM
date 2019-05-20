// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Machine.Specifications;
using TA.Ascom.ReactiveCommunications.Diagnostics;
using TA.NexDome.DeviceInterface;
using TA.NexDome.Specifications.Helpers;
using ObservableExtensions = TA.NexDome.DeviceInterface.ObservableExtensions;

namespace TA.NexDome.Specifications
{
    [Subject(typeof(ObservableExtensions), "Encoder Ticks")]
    internal class when_an_encoder_tick_is_received
    {
        Establish context = () => source = "P99\nP100\nP101\n".ToObservable();
        Because of = () => source.AzimuthEncoderTicks().SubscribeAndWaitForCompletion(tick => tickHistory.Add(tick));
        It should_receive_the_encoder_ticks = () => tickHistory.ShouldEqual(expectedTicks);
        static readonly List<int> expectedTicks = new List<int> {99, 100, 101};
        static IObservable<char> source;
        static readonly List<int> tickHistory = new List<int>();
    }

    [Subject(typeof(ObservableExtensions), "Shutter Current Readings")]
    internal class when_a_shutter_current_reading_is_received
    {
        Establish context = () => source = "Z8\nZ10\nZ11\n".ToObservable();

        Because of = () => source
            .ShutterCurrentReadings().Trace("Unbelievable")
            .SubscribeAndWaitForCompletion(item => elementHistory.Add(item));

        It should_receive_the_current_readings = () => elementHistory.ShouldEqual(expectedElements);
        static readonly List<int> elementHistory = new List<int>();
        static readonly List<int> expectedElements = new List<int> {8, 10, 11};
        static IObservable<char> source;
    }
}