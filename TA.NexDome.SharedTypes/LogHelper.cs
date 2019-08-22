// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.SharedTypes
    {
    using System;

    using JetBrains.Annotations;

    using NLog;
    using NLog.Fluent;

    public static class LogHelper
        {
        /// <summary>
        ///     Logs an error to the log output and throws the specified exception type with the
        ///     formatted log message.
        /// </summary>
        /// <typeparam name="TException">The type of exception to be thrown.</typeparam>
        /// <param name="format">
        ///     The message format string (supports NLog structured logging syntax).
        /// </param>
        /// <param name="parameters">
        ///     The parameters to be used to populate the log message using the format string.
        /// </param>
        /// <exception cref="T:System.Exception">
        ///     Thrown if an instance of <typeparamref name="TException" /> could not be constructed.
        /// </exception>
        [StringFormatMethod("format")]
        public static void LogAndThrow<TException>(string format, params object[] parameters)
            where TException : Exception, new()
            {
            var exceptionToThrow = LogAndBuild<TException>(format, parameters);
            throw exceptionToThrow;
            }

        [StringFormatMethod("format")]
        public static Exception LogAndBuild<TException>(string format, params object[] parameters)
            where TException : Exception, new()
            {
            var logBuilder = Log.Error().Message(format, parameters);
            var logEvent = logBuilder.LogEventInfo;
            LogManager.GetCurrentClassLogger().Log(logEvent);
            string message = logEvent.FormattedMessage;
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
            return exceptionToThrow;
            }
        }
    }