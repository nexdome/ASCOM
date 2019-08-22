// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.SharedTypes
    {
    using System;
    using System.Linq;
    using System.Reflection;

    public static class GitVersionExtensions
        {
        private static string GitVersionField(this Type gitVersionInformationType, string fieldName)
            {
            var versionField = gitVersionInformationType?.GetField(fieldName);
            return versionField?.GetValue(null).ToString() ?? "undefined";
            }

        private static Type GitVersion()
            {
            var assembly = Assembly.GetExecutingAssembly();
            var type = assembly.GetTypes().SingleOrDefault(t => t.Name == "GitVersionInformation");
            return type;
            }

        public static string GitInformationalVersion => GitVersion().GitVersionField("InformationalVersion");
        public static string GitCommitSha => GitVersion().GitVersionField("Sha");
        public static string GitCommitShortSha => GitVersion().GitVersionField("ShortSha");
        public static string GitCommitDate => GitVersion().GitVersionField("CommitDate");
        public static string GitSemVer => GitVersion().GitVersionField("SemVer");
        public static string GitFullSemVer => GitVersion().GitVersionField("FullSemVer");
        public static string GitBuildMetadata => GitVersion().GitVersionField("FullBuildMetaData");

        public static string GitMajorVersion => GitVersion().GitVersionField("Major");
        public static string GitMinorVersion => GitVersion().GitVersionField("Minor");
        public static string GitPatchVersion => GitVersion().GitVersionField("Patch");
        }
    }