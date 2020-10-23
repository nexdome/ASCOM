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
// File: LogHelper.cs  Last modified: 2020-07-21@22:29 by Tim Long

using System;
using JetBrains.Annotations;
using TA.Utils.Core.Diagnostics;

namespace TA.NexDome.Common
    {
    public static class LogHelper
        {
        /// <summary>
        ///     Logs an error to the log output and throws the specified exception type with the formatted log
        ///     message.
        /// </summary>
        /// <typeparam name="TException">The type of exception to be thrown.</typeparam>
        /// <param name="format">The message format string (supports NLog structured logging syntax).</param>
        /// <param name="parameters">
        ///     The parameters to be used to populate the log message using the format
        ///     string.
        /// </param>
        /// <exception cref="T:System.Exception">
        ///     Thrown if an instance of <typeparamref name="TException" />
        ///     could not be constructed.
        /// </exception>
        [StringFormatMethod("format")]
        public static void LogAndThrow<TException>(ILog log, string format, params object[] parameters)
            where TException : Exception, new()
            {
            var exceptionToThrow = LogAndBuild<TException>(log, format, parameters);
            throw exceptionToThrow;
            }

        [StringFormatMethod("format")]
        public static Exception LogAndBuild<TException>(ILog log, string format, params object[] parameters)
            where TException : Exception, new()
            {
            var message = string.Format(format, parameters);
            Exception exceptionToThrow;
            try
                {
                exceptionToThrow = (TException)Activator.CreateInstance(typeof(TException), message);
                }
            // ReSharper disable once CatchAllClause
            catch (Exception unexpected)
                {
                exceptionToThrow = new Exception(message, unexpected);
                }
            log.Error().Message(message).Exception(exceptionToThrow).Write();
            return exceptionToThrow;
            }
        }
    }