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
        /// Creates a sequence of buffers, where the start and end of each buffer are triggered when
        /// the input sequence matches a predicate. This is used by many of the other extension
        /// methods in this class and is a handy way to buffer up responses based on start and stop
        /// characters.
        /// </summary>
        /// <param name="source">The source sequence, an <see cref="IObservable{char}"/>.</param>
        /// <param name="bufferOpening">The buffer opening predicate.</param>
        /// <param name="bufferClosing">The buffer closing predicate.</param>
        /// <returns>An observable sequence of buffers.</returns>
        public static IObservable<IList<char>> BufferByPredicates(this IObservable<char> source, Predicate<char> bufferOpening, Predicate<char> bufferClosing) => source.Buffer(source.Where(c => bufferOpening(c)), x => source.Where(c => bufferClosing(c)));

        /// <summary>
        ///     Extracts shutter position ticks from a source sequence and emits
        ///     the position values as an observable sequence of integers.
        /// </summary>
        /// <param name="source">The input sequence.</param>
        public static IObservable<int> ShutterPositionTicks(this IObservable<char> source)
            {
            const string pattern = @"^S(?<Position>-?\d{1,6})[^0-9]";
            var regex =
                new Regex(pattern,
                    RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);
            var buffers = source.Publish(s => s.BufferByPredicates(p => p == 'S', q => !char.IsDigit(q)));
            var positionValues = from buffer in buffers
                                 let message = new string(buffer.ToArray())
                                 let patternMatch = regex.Match(message)
                                 where patternMatch.Success
                                 let position = int.Parse(patternMatch.Groups["Position"].Value)
                                 select position;
            return positionValues.Trace("ShutterPosition");
            }

        /// <summary>
        ///     Extracts azimuth encoder ticks from a source sequence and emits
        ///     the encoder values as an observable sequence of integers.
        /// </summary>
        /// <param name="source"></param>
        public static IObservable<int> AzimuthEncoderTicks(this IObservable<char> source)
            {
            const string azimuthEncoderPattern = @"^P(?<Azimuth>-?\d{1,6})[^0-9]";
            var azimuthEncoderRegex =
                new Regex(azimuthEncoderPattern, RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);
            var buffers = source.Publish(s => s.BufferByPredicates(p => p == 'P', q => !char.IsDigit(q)));
            var azimuthValues = from buffer in buffers
                                let message = new string(buffer.ToArray())
                                let patternMatch = azimuthEncoderRegex.Match(message)
                                where patternMatch.Success
                                let azimuth = int.Parse(patternMatch.Groups["Azimuth"].Value)
                                select azimuth;
            return azimuthValues.Trace("EncoderTicks");
            }

        public static IObservable<ShutterLinkState> LinkStatusUpdates(this IObservable<char> source)
            {
            const string linkStatePattern = @"^XB->(?<State>[a-zA-Z]+)[\r\n]?$";
            var linkStateRegex = new Regex(linkStatePattern, RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);
            var buffers = source.Publish(s => s.BufferByPredicates(p => p == 'X', char.IsWhiteSpace));
            var linkStateValues = from buffer in buffers
                                  let message = new string(buffer.ToArray())
                                  let patternMatch = linkStateRegex.Match(message)
                                  where patternMatch.Success
                                  let linkState = (ShutterLinkState)Enum.Parse(typeof(ShutterLinkState), patternMatch.Groups["State"].Value)
                                  select linkState;
            return linkStateValues.Trace("Link State");
            }

        public static IObservable<bool> RainSensorUpdates(this IObservable<char> source)
            {
            const string rainSensorPattern = @"^:(?<State>(rainstopped|rain))#$";
            var rainSensorRegex = new Regex(rainSensorPattern,
                RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant |
                RegexOptions.IgnoreCase);
            var buffers = source.DelimitedMessageStrings();
            var rainSensorValues = from buffer in buffers
                                   let message = new string(buffer.ToArray())
                                   let patternMatch = rainSensorRegex.Match(message.ToLower())
                                   where patternMatch.Success
                                   let isRaining = patternMatch.Groups["State"].Value == "rain"
                                   select isRaining;
            return rainSensorValues.Trace("Rain");
            }


        public static IObservable<IRotatorStatus> RotatorStatusUpdates(this IObservable<char> source, ControllerStatusFactory factory)
            {
            var regex = new Regex(ControllerStatusFactory.RotatorStatusPattern,
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);
            var responses = source.Publish(s =>
                s.BufferByPredicates(p => p == ':', q => q == '#'));
            var statusValues = from response in responses
                               let message = new string(response.ToArray())
                               where message.StartsWith(Constants.RotatorStatusReply)
                               where regex.IsMatch(message)
                               let status = factory.FromRotatorStatusPacket(message)
                               select status;
            return statusValues.Trace("IRotatorStatus");
            }

        public static IObservable<RotationDirection> RotatorDirectionUpdates(this IObservable<char> source)
            {
            var responses = source.DelimitedMessageStrings();
            var directions = from notification in responses
                             where RotatorDirections.Contains(notification, StringComparer.InvariantCultureIgnoreCase)
                             let ordinal = RotatorDirections.IndexOf(notification.ToLowerInvariant())
                             let direction = ordinal == 1
                                 ? RotationDirection.CounterClockwise
                                 : RotationDirection.Clockwise
                             select direction;
            return directions.Trace("RotatorDirection");
            }

        public static IObservable<IShutterStatus> ShutterStatusUpdates(this IObservable<char> source, ControllerStatusFactory factory)
            {
            var regex = new Regex(ControllerStatusFactory.ShutterStatusPattern,
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);
            var responses = source.Publish(s =>
                s.BufferByPredicates(p => p == ':', q => q == '#'));
            var statusValues = from response in responses
                               let message = new string(response.ToArray())
                               where message.StartsWith(Constants.ShutterStatusReply)
                               where regex.IsMatch(message)
                               let status = factory.FromShutterStatusPacket(message)
                               select status;
            return statusValues.Trace("IShutterStatus");
            }

        public static IObservable<ShutterDirection> ShutterDirectionUpdates(this IObservable<char> source)
            {
            var responses = source.DelimitedMessageStrings();
            var directions = from notification in responses
                             where ShutterDirections.Contains(notification, StringComparer.InvariantCultureIgnoreCase)
                             let ordinal = ShutterDirections.IndexOf(notification.ToLowerInvariant())
                             let direction = (ShutterDirection)ordinal
                             select direction;
            return directions.Trace("ShutterDirection");
            }

        public static IObservable<float> BatteryVoltageUpdates(this IObservable<char> source)
            {
            const string voltageUpdatePattern = @"^:BV(?<Value>\d{1,5})#$";
            const float aduToVref = 5f / 1023 * 3f;
            var regex = new Regex(voltageUpdatePattern, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);
            var responses = source.DelimitedMessageStrings();
            var observableVolts = from response in responses
                               let message = new string(response.ToArray())
                               let match = regex.Match(message)
                               where match.Success
                               let measurement = int.Parse(match.Groups["Value"].Value)
                               let volts = measurement * aduToVref
                               select volts;
            return observableVolts.Trace("Volts");
            }

        private static readonly List<string> ShutterDirections = new List<string>() { "None", ":close#", ":open#" };
        private static readonly List<string> RotatorDirections = new List<string>() { "None", ":left#", ":right#" };
        }
    }