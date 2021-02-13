// This file is part of the TA.NexDome.AscomServer project
//
// Copyright © 2015-2020 Tigra Astronomy, all rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so. The Software comes with no warranty of any kind.
// You make use of the Software entirely at your own risk and assume all liability arising from your use thereof.
//
// File: ControllerStatusFactory.cs  Last modified: 2020-07-21@22:29 by Tim Long

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;

namespace TA.NexDome.Common
    {
    /// <summary>A factory class that creates immutable instances of various types of status object.</summary>
    public sealed class ControllerStatusFactory
        {
        private static readonly char[] FieldDelimiters = {','};

        private static readonly Regex RotatorStatusRegex;
        private static readonly Regex ShutterStatusRegex;

        private readonly IClock timeSource;

        static ControllerStatusFactory()
            {
            RotatorStatusPattern = @"^(?<Status>:SER(,(?<Values>-?\d{1,6}))+)#$";
            RotatorStatusRegex = new Regex(
                RotatorStatusPattern,
                RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);
            ShutterStatusPattern = @"^(?<Status>:SES(,(?<Values>-?\d{1,6}))+)#$";
            ShutterStatusRegex = new Regex(
                ShutterStatusPattern,
                RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);
            }

        public ControllerStatusFactory(IClock timeSource)
            {
            Contract.Requires(timeSource != null);
            this.timeSource = timeSource;
            }

        public static string RotatorStatusPattern { get; }

        public static string ShutterStatusPattern { get; }

        /// <summary>
        ///     Creates an instance of a rotator status from the notification string received from the NexDome
        ///     hardware.
        /// </summary>
        /// <param name="status">The status string received from the controller hardware.</param>
        /// <returns>An object implementing <see cref="IRotatorStatus" /> populated with the status values.</returns>
        public IRotatorStatus FromRotatorStatusPacket(string status)
            {
            var match = RotatorStatusRegex.Match(status);
            var captures = match.Groups["Values"].Captures;
            var valueCollection = captures.Cast<Capture>().Select(p => p.Value);
            return RotatorStatus.FromValueCollection(valueCollection);
            }

        /// <summary>
        ///     Creates an instance of a shutter status from the notification string received from the NexDome
        ///     hardware.
        /// </summary>
        /// <param name="status">The status string received from the controller hardware.</param>
        /// <returns>An object implementing <see cref="IShutterStatus" /> populated with the status values.</returns>
        public IShutterStatus FromShutterStatusPacket(string status)
            {
            var match = ShutterStatusRegex.Match(status);
            var captures = match.Groups["Values"].Captures;
            var valueCollection = captures.Cast<Capture>().Select(p => p.Value);
            return ShutterStatus.FromValueCollection(valueCollection);
            }

        /// <summary>
        ///     An immutable snapshot of the rotator status at a moment in time. Implements the
        ///     <see cref="T:TA.NexDome.Common.IRotatorStatus" />
        /// </summary>
        private class RotatorStatus : IRotatorStatus
            {
            /// <inheritdoc />
            public bool AtHome { get; private set; }

            /// <inheritdoc />
            public int Azimuth { get; private set; }

            /// <inheritdoc />
            public int DeadZone { get; private set; }

            /// <inheritdoc />
            public int DomeCircumference { get; private set; }

            /// <inheritdoc />
            public int HomePosition { get; private set; }

            /// <summary>Creates and populates a RotatorStatus instance from a collection of string values.</summary>
            /// <param name="valueCollection">
            ///     A collection of strings containing the status values that will need
            ///     to be parsed.
            /// </param>
            /// <returns>IRotatorStatus.</returns>
            public static IRotatorStatus FromValueCollection(IEnumerable<string> valueCollection)
                {
                var values = valueCollection.ToArray();
                var status = new RotatorStatus
                    {
                    Azimuth = int.Parse(values[0]),
                    AtHome = values[1] == "1",
                    DomeCircumference = int.Parse(values[2]),
                    HomePosition = int.Parse(values[3]),
                    DeadZone = int.Parse(values[4]),    // Fixes #20
                    };
                return status;
                }

            /// <inheritdoc />
            public override string ToString() =>
                $"{nameof(AtHome)}: {AtHome}, {nameof(Azimuth)}: {Azimuth}, {nameof(DeadZone)}: {DeadZone}, {nameof(DomeCircumference)}: {DomeCircumference}, {nameof(HomePosition)}: {HomePosition}";
            }

        private class ShutterStatus : IShutterStatus
            {
            /// <inheritdoc />
            public int Position { get; private set; }

            /// <inheritdoc />
            public int LimitOfTravel { get; private set; }

            /// <inheritdoc />
            public bool OpenSensorActive { get; private set; }

            /// <inheritdoc />
            public bool ClosedSensorActive { get; private set; }

            /// <summary>Creates and populates a ShutterStatus instance from a collection of string values.</summary>
            /// <param name="valueCollection">
            ///     A collection of strings containing the status values that will need
            ///     to be parsed.
            /// </param>
            /// <returns>
            ///     <see cref="IShutterStatus" />
            /// </returns>
            public static IShutterStatus FromValueCollection(IEnumerable<string> valueCollection)
                {
                var values = valueCollection.ToArray();
                var status = new ShutterStatus
                    {
                    Position = int.Parse(values[0]),
                    LimitOfTravel = int.Parse(values[1]),
                    OpenSensorActive = values[2] == "1",
                    ClosedSensorActive = values[3] == "1"
                    };
                return status;
                }

            /// <inheritdoc />
            public override string ToString() =>
                $"{nameof(ClosedSensorActive)}: {ClosedSensorActive}, {nameof(OpenSensorActive)}: {OpenSensorActive}, {nameof(Position)}: {Position}";
            }
        }
    }