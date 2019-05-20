// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: TestData.cs  Last modified: 2018-06-16@16:20 by Tim Long

using System.IO;
using System.Reflection;

namespace TA.NexDome.Specifications.Helpers
    {
    internal static class TestData
        {
        internal static string FromEmbeddedResource(string resourceName)
            {
            var asm = Assembly.GetExecutingAssembly();
            var asmName = asm.GetName().Name;
            var resourceRoot = $"{asmName}.TestData";
            var resource = $"{resourceRoot}.{resourceName}";
            using (var stream = asm.GetManifestResourceStream(resource))
                {
                var reader = new StreamReader(stream);
                return reader.ReadToEnd();
                }
            }
        }
    }