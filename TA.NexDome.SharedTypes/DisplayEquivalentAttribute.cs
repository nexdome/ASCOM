// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.SharedTypes
    {
    using System;

    /// <summary>
    ///     When applied to an enum member or field, specifies a string that should be used for
    ///     display purposes instead of the identifier name. This can be useful within code that
    ///     must render HTML markup from an enumerated type.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    /// <seealso cref="EnumExtensions.DisplayEquivalent" />
    [AttributeUsage(AttributeTargets.Field)]
    internal sealed class DisplayEquivalentAttribute : Attribute
        {
        public DisplayEquivalentAttribute(string text) => Value = text;

        public string Value { get; }
        }
    }