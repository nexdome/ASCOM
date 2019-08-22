// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.DeviceInterface
    {
    using System;
    using System.Runtime.Serialization;

    using TA.Ascom.ReactiveCommunications;

    /// <inheritdoc />
    /// <summary>
    ///     Thrown when a transaction cannot be completed.
    /// </summary>
    [Serializable]
    public class TransactionException : Exception
        {
        /// <inheritdoc />
        public TransactionException()
            : base("The transaction failed") { }

        /// <inheritdoc />
        public TransactionException(string message)
            : base(message) { }

        /// <inheritdoc />
        public TransactionException(string message, Exception inner)
            : base(message, inner) { }

        /// <inheritdoc />
        protected TransactionException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        /// <summary>
        ///     Gets a reference to the failed transaction that generated the exception
        /// </summary>
        public DeviceTransaction Transaction { get; set; }
        }
    }