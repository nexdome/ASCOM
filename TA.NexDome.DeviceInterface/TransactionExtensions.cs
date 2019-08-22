// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.DeviceInterface
    {
    using System;
    using System.Diagnostics.Contracts;
    using System.Text;

    using JetBrains.Annotations;

    using NLog;

    using TA.Ascom.ReactiveCommunications;
    using TA.NexDome.SharedTypes;

    internal static class TransactionExtensions
        {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///     Raises a TransactionException for the given transaction.
        /// </summary>
        /// <param name="transaction">The transaction causing the exception.</param>
        /// <param name="log">
        ///     An <see cref="ILogger" /> instance to which the exception message will be logged at
        ///     Error severity.
        /// </param>
        /// <exception cref="TransactionException">Always thrown.</exception>
        public static void RaiseException([NotNull] this DeviceTransaction transaction, ILogger log = null)
            {
            Contract.Requires(transaction != null);
            string message = $"Transaction {transaction} failed: {transaction.ErrorMessage}";
            log?.Error(message);
            throw new TransactionException(message) { Transaction = transaction };
            }

        public static void ThrowIfFailed(this DeviceTransaction transaction)
            {
            if (transaction.Failed)
                transaction.RaiseException(log);
            }

        public static string EnsureEncapsulation(this string command)
            {
            var builder = new StringBuilder();
            if (!command.StartsWith(Constants.CommandPrefix))
                builder.Append(Constants.CommandPrefix);
            builder.Append(command);
            if (!command.EndsWith(Environment.NewLine))
                builder.AppendLine();
            return builder.ToString();
            }
        }
    }