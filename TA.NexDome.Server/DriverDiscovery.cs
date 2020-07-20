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
// File: DriverDiscovery.cs  Last modified: 2020-07-20@15:41 by Tim Long

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ASCOM;
using NLog.Fluent;

namespace TA.NexDome.Server
    {
    /// <summary>
    ///     Discovers and stores a list of all types within this assembly that implement one of the ASCOM
    ///     device interfaces. These are identified by the presence of a
    ///     <see cref="ServedClassNameAttribute" /> on the class.
    /// </summary>
    /// <seealso cref="System.MarshalByRefObject" />
    internal class DriverDiscovery
        {
        /// <summary>Gets the full assembly name of the LocalServer assembly</summary>
        public string DiscoveredAssemblyName { get; private set; } = string.Empty;

        /// <summary>
        ///     Gets the discovered types that were found to be decorated with the
        ///     <see cref="ServedClassNameAttribute" /> attribute.
        /// </summary>
        /// <value>A list of the discovered types.</value>
        public List<Type> DiscoveredTypes { get; } = new List<Type>();

        /// <summary>
        ///     Discovers types within the LocalServer assembly that are decorated with the
        ///     <see cref="ServedClassNameAttribute" />, which identifies an ASCOM driver that should be served
        ///     by the LocalServer.
        /// </summary>
        public void DiscoverServedClasses()
            {
            Log.Info().Message("Loading served COM classes").Write();
            var thisAssembly = Assembly.GetExecutingAssembly();
            var assemblyName = thisAssembly.GetName();
            var assemblyFullName = assemblyName.FullName;
            var assemblyDisplayName = assemblyName.Name;
            var types = thisAssembly.GetTypes();
            Log.Trace()
                .Message("Reflection found {typeCount} types in assembly {assemblyName}",
                    types.Length, assemblyDisplayName)
                .Write();
            var servedClasses = from type in types.AsParallel()
                                let memberInfo = (MemberInfo)type
                                let safeAttributes = CustomAttributeData.GetCustomAttributes(memberInfo)
                                where safeAttributes.Any(
                                    p => p.AttributeType.Name == nameof(ServedClassNameAttribute))
                                select type;
            var discoveredTypes = servedClasses.ToList();
            if (discoveredTypes.Any())
                {
                DiscoveredTypes.AddRange(discoveredTypes);
                DiscoveredAssemblyName = assemblyFullName;
                Log.Info()
                    .Message("Discovered {servedClassCount} served classes in assembly {assembly}",
                        discoveredTypes.Count, assemblyName)
                    .Write();
                }
            }
        }
    }