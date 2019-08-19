// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.SharedTypes
    {
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    ///     An exception that is thrown when the driver detects a firmware version in the
    ///     Intelligent Handset that it cannot properly support.
    /// </summary>
    [Serializable]
    public class UnsupportedFirmwareVersionException : Exception
        {
        private const int ErrUnsupportedFirmware = unchecked((int)0x80040F00);

        /// <summary>
        ///     Initializes a new instance of the <see cref="UnsupportedFirmwareVersionException" /> class.
        /// </summary>
        public UnsupportedFirmwareVersionException()
            : base("Unsupported firmware version") { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UnsupportedFirmwareVersionException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public UnsupportedFirmwareVersionException(string message)
            : base(message) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UnsupportedFirmwareVersionException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public UnsupportedFirmwareVersionException(string message, Exception inner)
            : base(message, inner) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UnsupportedFirmwareVersionException" /> class.
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="actual">The actual.</param>
        public UnsupportedFirmwareVersionException(SemanticVersion expected, SemanticVersion actual)
            : base(
                $"This driver requires firmware version {expected} or later but detected version {actual}. Please contact NexDome to arrange an upgrade.") { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UnsupportedFirmwareVersionException" /> class.
        /// </summary>
        /// <param name="info">
        ///     The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object
        ///     data about the exception being thrown.
        /// </param>
        /// <param name="context">
        ///     The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual
        ///     information about the source or destination.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     The <paramref name="info" /> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        ///     The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0).
        /// </exception>
        protected UnsupportedFirmwareVersionException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
        }
    }