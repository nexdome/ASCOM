// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
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

        public static IObservable<IList<char>> BufferByPredicates(this IObservable<char> source,
            Predicate<char> bufferOpening, Predicate<char> bufferClosing)
        {
            return source.Buffer(source.Where(c => bufferOpening(c)), x => source.Where(c => bufferClosing(c)));
        }

        public static IObservable<IHardwareStatus> StatusUpdates(this IObservable<char> source,
            ControllerStatusFactory factory)
        {
            const string validStatusCharacters = "V+-0123456789,";
            const string statusPattern = @"^(?<Status>V4(,\d{1,3}){22})";
            var statusRegex = new Regex(statusPattern, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
            var buffers = source.Publish(s =>
                s.BufferByPredicates(p => p == 'V', q => !validStatusCharacters.Contains(q)));
            var statusValues = from buffer in buffers
                let message = new string(buffer.ToArray())
                let patternMatch = statusRegex.Match(message)
                where patternMatch.Success
                let status = patternMatch.Groups["Status"].Value
                let harwareStatus = factory.FromStatusPacket(status)
                select harwareStatus;
            return statusValues.Trace("StatusUpdates");
        }

        public static IObservable<ShutterDirection> ShutterDirectionUpdates(this IObservable<char> source)
        {
            // Note: The zero-based index in the string must match the ordinal values in ShutterDirection
            const string shutterMovementIndicators = "SCO";
            var shutterDirectionSequence = from c in source
                where shutterMovementIndicators.Contains(c)
                let ordinal = shutterMovementIndicators.IndexOf(c)
                let direction = (ShutterDirection) ordinal
                select direction;
            return shutterDirectionSequence.Trace("ShutterDirection");
        }
    }
}