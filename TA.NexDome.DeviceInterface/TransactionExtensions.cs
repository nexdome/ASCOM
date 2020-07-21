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
// File: TransactionExtensions.cs  Last modified: 2020-07-21@22:47 by Tim Long

using System;
using System.Diagnostics.Contracts;
using System.Text;
using JetBrains.Annotations;
using TA.Ascom.ReactiveCommunications;
using TA.Ascom.ReactiveCommunications.Diagnostics;
using TA.NexDome.SharedTypes;
using TA.Utils.Core.Diagnostics;

namespace TA.NexDome.DeviceInterface
    {
    internal static class TransactionExtensions
        {
        /// <summary>Raises a TransactionException for the given transaction.</summary>
        /// <param name="transaction">The transaction causing the exception.</param>
        /// <param name="log">
        ///     An <see cref="ILogger" /> instance to which the exception message will be logged at Error
        ///     severity.
        /// </param>
        /// <exception cref="TransactionException">Always thrown.</exception>
        public static void RaiseException([NotNull] this DeviceTransaction transaction, ILog log = null)
            {
            Contract.Requires(transaction != null);
            string message = $"Transaction {transaction} failed: {transaction.ErrorMessage}";
            var ex = new TransactionException(message) {Transaction = transaction};
            log?.Error()
                .Exception(ex)
                .Transaction(transaction)
                .Message(message).Write();
            throw ex;
            }

        public static void ThrowIfFailed(this DeviceTransaction transaction, ILog log = null)
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