// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Machine.Specifications;
using TA.NexDome.DeviceInterface;
using TA.NexDome.Specifications.Helpers;
using ObservableExtensions = TA.NexDome.DeviceInterface.ObservableExtensions;

namespace TA.NexDome.Specifications.ObservableExtensions
{
    [Subject(typeof(NexDome.DeviceInterface.ObservableExtensions), "Encoder Ticks")]
    internal class when_an_encoder_tick_is_received
    {
        Establish context = () => source = "P99\nP100\nP101\n".ToObservable();
        Because of = () => source.AzimuthEncoderTicks().SubscribeAndWaitForCompletion(tick => tickHistory.Add(tick));
        It should_receive_the_encoder_ticks = () => tickHistory.ShouldEqual(expectedTicks);
        static readonly List<int> expectedTicks = new List<int> {99, 100, 101};
        static IObservable<char> source;
        static readonly List<int> tickHistory = new List<int>();
    }
}