// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.Specifications.Helpers
    {
    using System.IO;
    using System.Reflection;

    static class TestData
        {
        internal static string FromEmbeddedResource(string resourceName)
            {
            var asm = Assembly.GetExecutingAssembly();
            string asmName = asm.GetName().Name;
            string resourceRoot = $"{asmName}.TestData";
            string resource = $"{resourceRoot}.{resourceName}";
            using (var stream = asm.GetManifestResourceStream(resource))
                {
                var reader = new StreamReader(stream);
                return reader.ReadToEnd();
                }
            }
        }
    }