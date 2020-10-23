// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using TA.NexDome.Common;

namespace TA.NexDome.DeviceInterface
    {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Text.RegularExpressions;

    using TA.Ascom.ReactiveCommunications;
    using TA.Ascom.ReactiveCommunications.Diagnostics;

    internal static class ObservableExtensions
        {
        private static readonly List<string> ShutterDirections = new List<string> { "None", ":close#", ":open#" };

        private static readonly List<string> RotatorDirections = new List<string> { "None", ":left#", ":right#" };

        /// <summary>
        ///     Creates a sequence of buffers, where the start and end of each buffer are triggered when
        ///     the input sequence matches a predicate. This is used by many of the other extension
        ///     methods in this class and is a handy way to buffer up responses based on start and stop
        ///     characters.
        /// </summary>
        /// <param name="source">The source sequence, an <see cref="IObservable{char}" />.</param>
        /// <param name="bufferOpening">The buffer opening predicate.</param>
        /// <param name="bufferClosing">The buffer closing predicate.</param>
        /// <returns>An observable sequence of buffers.</returns>
        public static IObservable<IList<char>> BufferByPredicates(
            this IObservable<char> source,
            Predicate<char> bufferOpening,
            Predicate<char> bufferClosing) =>
            source.Buffer(source.Where(c => bufferOpening(c)), x => source.Where(c => bufferClosing(c)));

        /// <summary>
        ///     Extracts shutter position ticks from a source sequence and emits
        ///     the position values as an observable sequence of integers.
        /// </summary>
        /// <param name="source">The input sequence.</param>
        public static IObservable<int> ShutterPositionTicks(this IObservable<char> source)
            {
            const string pattern = @"^S(?<Position>-?\d{1,6})[^0-9]";
            var regex = new Regex(
                pattern,
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
            var azimuthEncoderRegex = new Regex(
                azimuthEncoderPattern,
                RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);
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
            var linkStateRegex = new Regex(
                linkStatePattern,
                RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);
            var buffers = source.Publish(s => s.BufferByPredicates(p => p == 'X', char.IsWhiteSpace));
            var linkStateValues = from buffer in buffers
                                  let message = new string(buffer.ToArray())
                                  let patternMatch = linkStateRegex.Match(message)
                                  where patternMatch.Success
                                  let linkState = (ShutterLinkState)Enum.Parse(
                                      typeof(ShutterLinkState),
                                      patternMatch.Groups["State"].Value)
                                  select linkState;
            return linkStateValues.Trace("Link State");
            }

        public static IObservable<bool> RainSensorUpdates(this IObservable<char> source)
            {
            const string rainSensorPattern = @"^:(?<State>(rainstopped|rain))#$";
            var rainSensorRegex = new Regex(
                rainSensorPattern,
                RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant
                | RegexOptions.IgnoreCase);
            var buffers = source.DelimitedMessageStrings();
            var rainSensorValues = from buffer in buffers
                                   let message = new string(buffer.ToArray())
                                   let patternMatch = rainSensorRegex.Match(message.ToLower())
                                   where patternMatch.Success
                                   let isRaining = patternMatch.Groups["State"].Value == "rain"
                                   select isRaining;
            return rainSensorValues.Trace("Rain");
            }

        public static IObservable<IRotatorStatus> RotatorStatusUpdates(
            this IObservable<char> source,
            ControllerStatusFactory factory)
            {
            var regex = new Regex(
                ControllerStatusFactory.RotatorStatusPattern,
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);
            var responses = source.Publish(s => s.BufferByPredicates(p => p == ':', q => q == '#'));
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

        public static IObservable<IShutterStatus> ShutterStatusUpdates(
            this IObservable<char> source,
            ControllerStatusFactory factory)
            {
            var regex = new Regex(
                ControllerStatusFactory.ShutterStatusPattern,
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);
            var responses = source.Publish(s => s.BufferByPredicates(p => p == ':', q => q == '#'));
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
            var regex = new Regex(
                voltageUpdatePattern,
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);
            var responses = source.DelimitedMessageStrings();
            var observableVolts = from response in responses
                                  let message = new string(response.ToArray())
                                  let match = regex.Match(message)
                                  where match.Success
                                  let measurement = uint.Parse(match.Groups["Value"].Value)
                                  let volts = (float)measurement.AduToVolts()
                                  select volts;
            return observableVolts.Trace("Volts");
            }

        /// <summary>
        /// Transform a sequence of characters into a sequence that emits <c>true</c>
        /// whenever a low volts notification is received, and <c>false</c> when no
        /// notification has been received for a period of time.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <param name="notificationTimeToLive">
        /// The time that a positive low volts indication is considered valid, after which false is emitted.
        /// </param>
        /// <returns>
        /// An observable sequence <see cref="IObservable{bool}"/> that represents the battery low volts condition.
        /// </returns>
        /// <remarks>
        ///   <para>
        ///   This is a somewhat complex Rx query so it is worth explaining.</para>
        ///   <para>First, we tokenize the input stream into response messages (<c>source.DelimitedMessageStrings()</c>).</para>
        ///   <para>Next, we test each response to see if is a low volts notification, using a regular expression.
        ///   If it is, we emit <c>true</c>. We now have a sequence of booleans containing only true, in
        ///   <c>lowVoltsNotifications</c>.</para>
        ///   <para>This is the tricky bit. Now we transform that sequence into a sequence of sequences of {true, false} where the two elements
        ///   are separated by a time delay.
        ///   Finally we use the <c>Select()</c> operator to select only the most recently produced sequence.
        /// </para>
        ///   <para>
        ///   The effect is that, as long as the tuple sequences are being produced close together,
        ///   we only ever see the first element of the sequence (true). Only when the source sequence
        ///   is quiet for the required time period do we see the second element (false).
        /// </para>
        ///   <para>
        ///   Credit to @Enigmativity for this solution: https://stackoverflow.com/a/62748101/9851
        ///   </para>
        /// </remarks>
        public static IObservable<bool> LowVoltsNotifications(this IObservable<char> source, TimeSpan notificationTimeToLive)
            {
            const string lowVoltsPattern = @"^:Volts#$";
            var regex = new Regex(lowVoltsPattern,
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);
            var responses = source.DelimitedMessageStrings();
            // emit 'true' whenever a low volts notification is received.
            var lowVoltsNotifications = from response in responses
                                     let message = new string(response.ToArray())
                                     let match = regex.Match(message)
                                     where match.Success
                                     select true;
            // emit 'false' when lowVoltsNotifications hasn't produced a value for 60 seconds
            var observableVoltsLowOrSafe = lowVoltsNotifications
                    .Select(x =>
                        Observable
                            .Timer(notificationTimeToLive)
                            .Select(y => false)
                            .StartWith(true))
                    .Switch();
            return observableVoltsLowOrSafe.Trace("LowVolts");
            }
        }
    }