// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;
using NLog;

namespace TA.NexDome.SharedTypes
    {
    public sealed class ControllerStatusFactory
        {
        private const string rotatorStatusPattern = @"^(?<Status>SER(,(?<Values>\d{1,6}))+)#$";
        private static readonly Logger log = LogManager.GetCurrentClassLogger();
        private static readonly char[] fieldDelimiters = {','};
        private static readonly Regex rotatorStatusRegex = new Regex(rotatorStatusPattern,
            RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);
        private readonly IClock timeSource;

        public ControllerStatusFactory(IClock timeSource)
            {
            Contract.Requires(timeSource != null);
            this.timeSource = timeSource;
            }

        /// <summary>
        ///     Creates a <see cref="IHardwareStatus" /> object from the text of a status packet
        /// </summary>
        public IHardwareStatus FromStatusPacket(string packet)
            {
            Contract.Requires(!string.IsNullOrEmpty(packet));
            Contract.Ensures(Contract.Result<IHardwareStatus>() != null);
            var elements = packet.Split(fieldDelimiters);
            switch (elements[0])
                {
                    case "V4":
                        return ParseV4StatusElements(elements);
                    default:
                        throw new ApplicationException("Unsupported firmware version");
                }
            }

        private HardwareStatus ParseV4StatusElements(IReadOnlyList<string> elements)
            {
            log.Info("V4 status");
            var elementsLength = elements.Count;
            if (elementsLength != 23)
                {
                var message = $"V4 GINF packet had wrong number of elements: expected 23, found {elementsLength}";
                log.Error(message);
                var ex = new ArgumentException(message, "status");
                ex.Data["V4 Status Elements"] = elements;
                ex.Data["ExpectedElements"] = 23;
                ex.Data["ActualElements"] = elementsLength;
                throw ex;
                }

            try
                {
                var status = new HardwareStatus
                    {
                    TimeStamp = timeSource.GetCurrentTime(),
                    DomeCircumference = 0,
                    HomePosition = 0,
                    AtHome = false,
                    DeadZone = 0
                    };
                return status;
                }
            catch (Exception ex)
                {
                log.Error(ex, "Exception while creating status information");
                throw;
                }
            }

        /// <summary>
        ///     Creates an instance of a rotator status from the notification string received from the
        ///     NexDome hardware.
        /// </summary>
        /// <param name="status">
        ///     The status string received from the controller hardware.
        /// </param>
        /// <returns>
        ///     An object implementing <see cref="IRotatorStatus" /> populated with the status values.
        /// </returns>
        public static IRotatorStatus FromRotatorStatusPacket(string status)
            {
            var match = rotatorStatusRegex.Match(status);
            var captures = match.Groups["Values"].Captures;
            var valueCollection = captures.Cast<Capture>().Select(p => p.Value);
            return RotatorStatus.FromValueCollection(valueCollection);
            }

        /// <summary>
        ///     An immutable snapshot of the rotator status at a moment in time.
        ///     Implements the <see cref="T:TA.NexDome.SharedTypes.IRotatorStatus" />
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

            /// <summary>
            ///     Creates and populates a RotatorStatus instance from a collection of string values.
            /// </summary>
            /// <param name="valueCollection">A collection of strings containing the status values that will need to be parsed.</param>
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
                    DeadZone = 0 //int.Parse(values[4].Value),
                    };
                return status;
                }
            }
        }
    }