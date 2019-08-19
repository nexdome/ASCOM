// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.Server
    {
    using System;

    internal static class GitVersionExtensions
        {
        public static string VersionString(this Type gitVersionInformationType, string propertyName)
            {
            var versionField = gitVersionInformationType.GetField("Major");
            return versionField.GetValue(null).ToString();
            }
        }
    }