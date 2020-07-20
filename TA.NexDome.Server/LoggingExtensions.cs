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
// File: LoggingExtensions.cs  Last modified: 2020-07-20@18:53 by Tim Long

using System.Collections;
using System.Configuration;
using TA.Utils.Core.Diagnostics;

namespace TA.NexDome.Server
    {
    internal static class LoggingExtensions
        {
        public static IFluentLogBuilder WithSettings(this IFluentLogBuilder logBuilder, SettingsContext settings)
            {
            var settingsKeys = settings.Keys;
            var settingsValues = settings.Values;
            // The settings infrastructure is pre-generics so we have to enumerate manually.
            IEnumerator keys = settingsKeys.GetEnumerator();
            IEnumerator values = settingsValues.GetEnumerator();
            while (keys.MoveNext() && values.MoveNext())
                {
                logBuilder.Property(keys.Current.ToString(), values.Current);
                }
            return logBuilder;
            }
        }
    }