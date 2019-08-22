// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.SharedTypes
    {
    using System;
    using System.Diagnostics.Contracts;
    using System.Text;

    public static class AsciiExtensions
        {
        /// <summary>
        ///     Utility function. Expands non-printable ASCII characters into mnemonic human-readable
        ///     form.
        /// </summary>
        /// <returns>
        ///     Returns a new string with non-printing characters replaced by human-readable mnemonics.
        /// </returns>
        public static string ExpandAscii(this string text)
            {
            Contract.Requires(text != null);
            Contract.Ensures(Contract.Result<string>() != null);
            var expanded = new StringBuilder();
            foreach (char c in text)
                {
                byte b = (byte)c;
                string strAscii = Enum.GetName(typeof(AsciiSymbols), b);
                if (strAscii != null)
                    expanded.Append("<" + strAscii + ">");
                else
                    expanded.Append(c);
                }

            return expanded.ToString();
            }

        public static string ExpandAscii(this char c)
            {
            Contract.Ensures(Contract.Result<string>() != null);
            return c.ToString().ExpandAscii();
            }
        }
    }