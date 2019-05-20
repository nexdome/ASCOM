// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: TransactionException.cs  Last modified: 2018-03-02@00:53 by Tim Long

using System;
using System.Runtime.Serialization;
using TA.Ascom.ReactiveCommunications;

namespace TA.NexDome.DeviceInterface
    {
    /// <inheritdoc />
    /// <summary>
    ///     Thrown when a transaction cannot be completed.
    /// </summary>
    [Serializable]
    public class TransactionException : Exception
        {
        /// <inheritdoc />
        public TransactionException() : base("The transaction failed") { }

        /// <inheritdoc />
        public TransactionException(string message) : base(message) { }

        /// <inheritdoc />
        public TransactionException(string message, Exception inner) : base(message, inner) { }

        /// <inheritdoc />
        protected TransactionException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }

        /// <summary>
        ///     Gets a reference to the failed transaction that generated the exception
        /// </summary>
        public DeviceTransaction Transaction { get; set; }
        }
    }