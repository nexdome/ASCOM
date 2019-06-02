// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using TA.Ascom.ReactiveCommunications;
using TA.Ascom.ReactiveCommunications.Diagnostics;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.DeviceInterface
    {
    internal static class ObservableExtensions
        {
        /// <summary>
        ///     Extracts azimuth encoder ticks from a source sequence and emits
        ///     the encoder values as an observable sequence of integers.
        /// </summary>
        /// <param name="source"></param>
        public static IObservable<int> AzimuthEncoderTicks(this IObservable<char> source)
            {
            const string azimuthEncoderPattern = @"^P(?<Azimuth>\d{1,4})[^0-9]";
            var azimuthEncoderRegex =
                new Regex(azimuthEncoderPattern, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
            var buffers = source.Publish(s => s.BufferByPredicates(p => p == 'P', q => !char.IsDigit(q)));
            var azimuthValues = from buffer in buffers
                                let message = new string(buffer.ToArray())
                                let patternMatch = azimuthEncoderRegex.Match(message)
                                where patternMatch.Success
                                let azimuth = int.Parse(patternMatch.Groups["Azimuth"].Value)
                                select azimuth;
            return azimuthValues.Trace("EncoderTicks");
            }

        public static IObservable<int> ShutterCurrentReadings(this IObservable<char> source)
            {
            const string shutterCurrentPattern = @"^Z(?<Current>\d{1,3})";
            var shutterCurrentRegex =
                new Regex(shutterCurrentPattern, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
            var buffers = source.Publish(s => s.BufferByPredicates(p => p == 'Z', q => !char.IsDigit(q)));
            var shutterCurrentValues = from buffer in buffers
                                       let message = new string(buffer.ToArray())
                                       let patternMatch = shutterCurrentRegex.Match(message)
                                       where patternMatch.Success
                                       let shutterCurrent = int.Parse(patternMatch.Groups["Current"].Value)
                                       select shutterCurrent;
            return shutterCurrentValues.Trace("ShutterCurrent");
            }

        public static IObservable<IList<char>> BufferByPredicates(this IObservable<char> source, Predicate<char> bufferOpening, Predicate<char> bufferClosing) => source.Buffer(source.Where(c => bufferOpening(c)), x => source.Where(c => bufferClosing(c)));

        public static IObservable<IRotatorStatus> RotatorStatusUpdates(this IObservable<char> source, ControllerStatusFactory factory)
            {
            var responses = source.Publish(s =>
                s.BufferByPredicates(p => p == ':', q => q == '#'));
            var statusValues = from response in responses
                               let message = new string(response.ToArray())
                               where message.StartsWith(Constants.RotatorStatusReply)
                               let status = factory.FromRotatorStatusPacket(message)
                               select status;
            return statusValues.Trace("IRotatorStatus");
            }

        public static IObservable<RotationDirection> RotatorDirectionUpdates(this IObservable<char> source)
            {
            var tokenizedResponses = source.DelimitedMessageStrings();
            var shutterDirectionSequence = from notification in tokenizedResponses
                                           where RotatorDirections.Contains(notification)
                                           let ordinal = ShutterDirections.IndexOf(notification)
                                           let direction = ordinal == 1
                                               ? RotationDirection.CounterClockwise
                                               : RotationDirection.Clockwise
                                           select direction;
            return shutterDirectionSequence.Trace("RotatorDirection");
            }

        public static IObservable<IShutterStatus> ShutterStatusUpdates(this IObservable<char> source, ControllerStatusFactory factory)
            {
            var responses = source.Publish(s =>
                s.BufferByPredicates(p => p == ':', q => q == '#'));
            var statusValues = from response in responses
                               let message = new string(response.ToArray())
                               where message.StartsWith(Constants.ShutterStatusReply)
                               let status = factory.FromShutterStatusPacket(message)
                               select status;
            return statusValues.Trace("IShutterStatus");
            }

        public static IObservable<ShutterDirection> ShutterDirectionUpdates(this IObservable<char> source)
            {
            var tokenizedResponses = source.DelimitedMessageStrings();
            var shutterDirectionSequence = from notification in tokenizedResponses
                                           where ShutterDirections.Contains(notification)
                                           let ordinal = ShutterDirections.IndexOf(notification)
                                           let direction = (ShutterDirection)ordinal
                                           select direction;
            return shutterDirectionSequence.Trace("ShutterDirection");
            }

        private static readonly List<string> ShutterDirections = new List<string>() { "None", ":Closing#", ":Opening#" };
        private static readonly List<string> RotatorDirections = new List<string>() { "None", ":Left#", ":Right#" };
        }
    }