// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.SharedTypes
    {
    using System;
    using System.Linq;
    using System.Reflection;

    public static class EnumExtensions
        {
        /// <summary>
        ///     Returns the equivalent display text for a given Enum value
        /// </summary>
        /// <remarks>
        ///     Inspired by: http://blogs.msdn.com/b/abhinaba/archive/2005/10/21/483337.aspx
        /// </remarks>
        /// <param name="en"></param>
        /// <returns></returns>
        public static string DisplayEquivalent(this Enum en)
            {
            string enumValueName = en.ToString();
            var type = en.GetType();
            var fields = type.GetTypeInfo().DeclaredFields;
            var fieldInfo = fields.Single(p => p.Name == enumValueName);
            var attribute =
                (DisplayEquivalentAttribute)fieldInfo?.GetCustomAttribute(typeof(DisplayEquivalentAttribute));
            if (fieldInfo == null || attribute == null)
                return enumValueName;
            string displayEquivalent = attribute.Value;
            return string.IsNullOrWhiteSpace(displayEquivalent) ? enumValueName : displayEquivalent;
            }
        }
    }