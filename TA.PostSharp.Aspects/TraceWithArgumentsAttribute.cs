// This file is part of the TA.NexDome.AscomServer project
//
// Copyright © 2016-2019 Tigra Astronomy, all rights reserved.
//
// File: NLogTraceWithArgumentsAttribute.cs  Last modified: 2019-10-07@17:58 by Tim Long

using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using NLog;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using TA.Utils.Core.Diagnostics;
using TA.Utils.Logging.NLog;

namespace TA.PostSharp.Aspects
    {
    /// <summary>
    ///     Class NLogTraceWithArgumentsAttribute. This class cannot be inherited. Traces member entry and exit, with
    ///     argument values and return values.
    /// </summary>
    [Serializable]
    [AttributeUsage(
        AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method |
        AttributeTargets.Property | AttributeTargets.Struct)]
    [ProvideAspectRole("Trace")]
    [AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, "ASCOM")]
    [AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.After, "Threading")]
    public sealed class TraceWithArgumentsAttribute : OnMethodBoundaryAspect
        {
        [NonSerialized] private static int indent;
        [NonSerialized] private readonly LogSeverity logSeverity;
        [NonSerialized] private string enteringMessage;
        [NonSerialized] private string exitingMessage;
        [NonSerialized] private string loggerName;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TraceWithArgumentsAttribute" /> class.
        /// </summary>
        /// <param name="level">The log severity at which to log. Optional, default: <see cref="LogSeverity.Trace"/></param>
        public TraceWithArgumentsAttribute(LogSeverity level = LogSeverity.Trace) => logSeverity = level;

        private IFluentLogBuilder GetLogBuilder(string name)
            {
            var logService = new LoggingService();
            switch (logSeverity)
                {
                    case LogSeverity.None:
                    case LogSeverity.Trace:
                        return logService.Trace(name);
                    case LogSeverity.Debug:
                        return logService.Debug(name);
                    case LogSeverity.Info:
                        return logService.Info(name);
                    case LogSeverity.Warn:
                        return logService.Warn(name);
                    case LogSeverity.Error:
                        return logService.Error(name);
                    case LogSeverity.Fatal:
                        return logService.Fatal(name);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        /// <summary>
        ///     Method executed <b>before</b> the body of methods to which this aspect is applied.
        /// </summary>
        /// <param name="args">
        ///     Event arguments specifying which method
        ///     is being executed, which are its arguments, and how should the execution continue
        ///     after the execution of
        ///     <see cref="M:PostSharp.Aspects.IOnMethodBoundaryAspect.OnEntry(PostSharp.Aspects.MethodExecutionArgs)" />.
        /// </param>
        public override void OnEntry(MethodExecutionArgs args)
            {
            base.OnEntry(args);
            var useIndent = Interlocked.Increment(ref indent);
            LogMethodEntryWithParameters(args, useIndent);
            }

        /// <summary>
        ///     Method executed <b>after</b> the body of methods to which this aspect is applied,
        ///     even when the method exists with an exception (this method is invoked from
        ///     the <c>finally</c> block).
        /// </summary>
        /// <param name="args">
        ///     Event arguments specifying which method
        ///     is being executed and which are its arguments.
        /// </param>
        public override void OnExit(MethodExecutionArgs args)
            {
            base.OnExit(args);
            LogMethodExit(args, indent);
            Interlocked.Decrement(ref indent);
            }

        /// <summary>
        ///     Initializes the current aspect. Invoked only once at runtime from the static constructor of type declaring
        ///     the target method.
        /// </summary>
        /// <param name="method">Method to which the current aspect is applied.</param>
        public override void RuntimeInitialize(MethodBase method)
            {
            if (method.DeclaringType != null) loggerName = method.DeclaringType.FullName;
            loggerName = method.Name;
            enteringMessage = "Enter " + loggerName + '(';
            exitingMessage = "Exit " + loggerName + "()";
            }

        private void LogMethodEntryWithParameters(MethodExecutionArgs args, int indent = 0)
            {
            var log = GetLogBuilder(loggerName);
            var builder = new StringBuilder();
            if (indent < 0) indent = 0;
            builder.Append(' ', indent);
            builder.Append(enteringMessage);
            var parameters = args.Method.GetParameters();
            var logProperties = args.Arguments.Zip(parameters, (o, p) => new {Value = o, Name = p.Name});

            foreach (var property in logProperties)
                {
                log.Property(property.Name, property.Value);
                if (property.Value is null)
                    builder.Append("null");
                else
                    {
                    builder.Append(property.Name);
                    builder.Append('=');
                    builder.Append(property.Value);
                    }
                builder.Append(", ");
                }
            if (args.Arguments.Count > 0)
                builder.Length -= 2; // Remove the last comma
            builder.Append(')');
            log.Message(builder.ToString()).Write();
            }

        private void LogMethodExit(MethodExecutionArgs args, int indent = 0)
            {
            var log = GetLogBuilder(loggerName);
            var builder = new StringBuilder();
            if (indent < 0) indent = 0;
            builder.Append(' ', indent);
            builder.Append(exitingMessage);
            if (args.ReturnValue != null)
                {
                builder.Append(" == ");
                builder.Append(args.ReturnValue);
                }
            log.Message(builder.ToString()).Property("returnValue", args.ReturnValue).Write();
            }
        }
    }