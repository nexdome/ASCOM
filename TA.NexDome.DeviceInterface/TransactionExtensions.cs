// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: TransactionExtensions.cs  Last modified: 2018-03-02@00:55 by Tim Long

using System.Diagnostics.Contracts;
using JetBrains.Annotations;
using NLog;
using TA.Ascom.ReactiveCommunications;

namespace TA.NexDome.DeviceInterface
    {
    internal static class TransactionExtensions
        {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///     Raises a TransactionException for the given transaction.
        /// </summary>
        /// <param name="transaction">The transaction causing the exception.</param>
        /// <param name="log">
        ///     An <see cref="ILogger" /> instance to which the exception message will be
        ///     logged at Error severity.
        /// </param>
        /// <exception cref="TransactionException">Always thrown.</exception>
        public static void RaiseException([NotNull] this DeviceTransaction transaction, ILogger log = null)
            {
            Contract.Requires(transaction != null);
            var message = $"Transaction {transaction} failed: {transaction.ErrorMessage}";
            log?.Error(message);
            throw new TransactionException(message) {Transaction = transaction};
            }

        public static void ThrowIfFailed(this DeviceTransaction transaction)
            {
            if (transaction.Failed)
                transaction.RaiseException(log);
            }
        }
    }