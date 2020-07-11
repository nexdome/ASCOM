// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.SharedTypes
    {
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    ///     Implements storage and comparison of semantic versions as defined at http://semver.org/.
    /// </summary>
    /// <remarks>
    ///     This class was inspired by Michael F. Collins and based on his blog article at
    ///     http://www.michaelfcollins3.me/blog/2013/01/23/semantic_versioning_dotnet.html
    /// </remarks>
    public sealed class SemanticVersion : IEquatable<SemanticVersion>, IComparable, IComparable<SemanticVersion>
        {
        internal const string SemanticVersionPattern =
            @"^(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)(-(?<prerelease>[A-Za-z0-9\-\.]+))?(\+(?<build>[A-Za-z0-9\-\.]+))?";

        private const string AllDigitsPattern = @"^[0-9]+$";

        private static readonly Regex SemanticVersionRegex = new Regex(
            SemanticVersionPattern,
            RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex AllDigitsRegex = new Regex(
            AllDigitsPattern,
            RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        ///     Initializes a new instance of the <see cref="SemanticVersion" /> class from a version encoded in a string.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <exception cref="System.ArgumentException">version</exception>
        public SemanticVersion(string version)
            {
            Contract.Requires(!string.IsNullOrEmpty(version));
            Contract.Ensures(MajorVersion >= 0);
            Contract.Ensures(MinorVersion >= 0);
            Contract.Ensures(PatchVersion >= 0);
            Contract.Ensures(PrereleaseVersion != null);
            Contract.Ensures(BuildVersion != null);

            var match = SemanticVersionRegex.Match(version);
            if (!match.Success)
                {
                string message = $"The version number '{version}' is not a valid semantic version number.";
                throw new ArgumentException(message, nameof(version));
                }

            MajorVersion = int.Parse(match.Groups["major"].Value, CultureInfo.InvariantCulture);
            MinorVersion = int.Parse(match.Groups["minor"].Value, CultureInfo.InvariantCulture);
            PatchVersion = int.Parse(match.Groups["patch"].Value, CultureInfo.InvariantCulture);
            PrereleaseVersion = match.Groups["prerelease"].Success
                                    ? new Maybe<string>(match.Groups["prerelease"].Value)
                                    : Maybe<string>.Empty;
            BuildVersion = match.Groups["build"].Success
                               ? new Maybe<string>(match.Groups["build"].Value)
                               : Maybe<string>.Empty;
            }

        public SemanticVersion(int majorVersion, int minorVersion, int patchVersion)
            {
            Contract.Requires(majorVersion >= 0);
            Contract.Requires(minorVersion >= 0);
            Contract.Requires(patchVersion >= 0);
            Contract.Ensures(MajorVersion >= 0);
            Contract.Ensures(MinorVersion >= 0);
            Contract.Ensures(PatchVersion >= 0);
            Contract.Ensures(PrereleaseVersion != null);
            Contract.Ensures(BuildVersion != null);

            MajorVersion = majorVersion;
            MinorVersion = minorVersion;
            PatchVersion = patchVersion;
            PrereleaseVersion = Maybe<string>.Empty;
            BuildVersion = Maybe<string>.Empty;
            }

        /// <summary>
        ///     Gets the build version, if any.
        /// </summary>
        /// <value>The build version.</value>
        public Maybe<string> BuildVersion { get; }

        /// <summary>
        ///     Gets the major version.
        /// </summary>
        /// <value>The major version.</value>
        public int MajorVersion { get; }

        /// <summary>
        ///     Gets the minor version.
        /// </summary>
        /// <value>The minor version.</value>
        public int MinorVersion { get; }

        /// <summary>
        ///     Gets the patch version.
        /// </summary>
        /// <value>The patch version.</value>
        public int PatchVersion { get; }

        /// <summary>
        ///     Gets the prerelease version, if any.
        /// </summary>
        /// <value>The prerelease version.</value>
        public Maybe<string> PrereleaseVersion { get; }

        [ContractInvariantMethod]
        private void ObjectInvariant()
            {
            Contract.Invariant(MajorVersion >= 0);
            Contract.Invariant(MinorVersion >= 0);
            Contract.Invariant(PatchVersion >= 0);
            Contract.Invariant(BuildVersion != null);
            Contract.Invariant(PrereleaseVersion != null);
            }

        /// <summary>
        ///     Returns a semantic version string.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        public override string ToString()
            {
            Contract.Ensures(Contract.Result<string>() != null);
            var builder = new StringBuilder();
            builder.Append($"{MajorVersion}.{MinorVersion}.{PatchVersion}");
            if (PrereleaseVersion.Any())
                builder.Append($"-{PrereleaseVersion.Single()}");
            if (BuildVersion.Any())
                builder.Append($"+{BuildVersion.Single()}");
            return builder.ToString();
            }

        public static bool IsValid(string candidate)
            {
            return SemanticVersionRegex.IsMatch(candidate);
            }

        #region Equality members
        public bool Equals(SemanticVersion other)
            {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return BuildVersion.Equals(other.BuildVersion) && MajorVersion == other.MajorVersion
                                                           && MinorVersion == other.MinorVersion
                                                           && PatchVersion == other.PatchVersion
                                                           && PrereleaseVersion.Equals(other.PrereleaseVersion);
            }

        public override bool Equals(object obj)
            {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj is SemanticVersion && Equals((SemanticVersion)obj);
            }

        public override int GetHashCode()
            {
            unchecked
                {
                int hashCode = MaybeHashCode(BuildVersion);
                hashCode = (hashCode * 397) ^ MajorVersion;
                hashCode = (hashCode * 397) ^ MinorVersion;
                hashCode = (hashCode * 397) ^ PatchVersion;
                hashCode = (hashCode * 397) ^ MaybeHashCode(PrereleaseVersion);
                return hashCode;
                }
            }

        private int MaybeHashCode(Maybe<string> item)
            {
            return item.Any() ? item.Single().GetHashCode() : string.Empty.GetHashCode();
            }

        public static bool operator ==(SemanticVersion left, SemanticVersion right)
            {
            return Equals(left, right);
            }

        public static bool operator !=(SemanticVersion left, SemanticVersion right)
            {
            return !Equals(left, right);
            }

        #endregion Equality members

        #region IComparable members

        /// <summary>
        ///     Compares the current instance with another object of the same type and returns an integer that indicates
        ///     whether the current instance precedes, follows, or occurs in the same position in the sort order as the
        ///     comparison object.
        /// </summary>
        /// <returns>
        ///     A value that indicates the relative order of the objects being compared. The return value has these
        ///     meanings: Value Meaning Less than zero This instance precedes <paramref name="comparison" /> in the sort
        ///     order. Zero This instance occurs in the same position in the sort order as
        ///     <paramref name="comparison" />. Greater than zero This instance follows <paramref name="comparison" />
        ///     in the sort order.
        /// </returns>
        /// <param name="comparison">An object to compare with this instance. </param>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="comparison" /> is not the same type as this instance.
        /// </exception>
        /// <filterpriority>
        ///     2
        /// </filterpriority>
        /// <exception cref="ArgumentNullException"><paramref name="comparison" /> is <see langword="null" />.</exception>
        public int CompareTo(object comparison)
            {
            if (ReferenceEquals(comparison, null))
                throw new ArgumentNullException(nameof(comparison));
            var otherVersion = comparison as SemanticVersion;
            if (otherVersion == null)
                throw new ArgumentException("Must be an instance of SemanticVersion.", nameof(comparison));
            return CompareTo(otherVersion);
            }

        /// <summary>
        ///     Compares the current instance with another object of the same type and returns an integer that indicates
        ///     whether the current instance precedes, follows, or occurs in the same position in the sort order as the
        ///     <paramref name="comparison" /> object.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Semantic Versions have a very specific and somewhat counterintuitive order of precedence. Comparison
        ///         begins with the major version and proceeds to the minor version, patch, prerelease tag and build
        ///         metadata tag. The order of precedence is always returned as soon as it can be determined.
        ///     </para>
        ///     <para>
        ///         If order cannot be determined from the major, minor and patch versions, then comparison proceeds to
        ///         the prerelease tag and then the build metadata tag. These fields can contain multiple segments
        ///         separated by the '.' character. each dot-separated segment is considered separately and where
        ///         possible is converted to an integer, so that <c>beta.9</c> sorts before <c>beta.10</c>.
        ///     </para>
        ///     <para>
        ///         Note that any version with a prerelease tag sorts lower than the same version without a prerelease
        ///         tag. Put another way: a release version is greater than a prerelease version.
        ///     </para>
        ///     <para>
        ///         The specification states that build metadata should be ignored when determining precedence. That
        ///         doesn't seem like a very sensible approach to us, since builds have to appear in some sort of order
        ///         and 'random' didn't strike us as an amazingly useful outcome. Therefore we have chosen to deviate
        ///         from the specification and include it as the last item in the list of comparisons when determining
        ///         the collation sequence. We treat the build metadata in a similar way to the prerelease tag, giving
        ///         it the lowest precedence but nonetheless yielding a more deterministic result when comparing and
        ///         sorting semantic versions.
        ///     </para>
        /// </remarks>
        /// <param name="comparison">An object to compare with this instance.</param>
        /// <returns>
        ///     A value that indicates the relative order of the objects being compared.
        /// </returns>
        public int CompareTo(SemanticVersion comparison)
            {
            if (comparison == null)
                throw new ArgumentNullException(nameof(comparison));
            if (ReferenceEquals(this, comparison))
                return 0;
            int result = MajorVersion.CompareTo(comparison.MajorVersion);
            if (result != 0)
                return result;
            result = MinorVersion.CompareTo(comparison.MinorVersion);
            if (result != 0)
                return result;
            result = PatchVersion.CompareTo(comparison.PatchVersion);
            if (result != 0)
                return result;
            result = ComparePrereleaseVersions(PrereleaseVersion, comparison.PrereleaseVersion);
            if (result != 0)
                return result;
            return CompareBuildVersions(BuildVersion, comparison.BuildVersion);
            }

        private static int CompareBuildVersions(Maybe<string> leftVersion, Maybe<string> rightVersion)
            {
            if (leftVersion.None && rightVersion.None)
                return 0; // equal if both absent
            if (leftVersion.Any() && rightVersion.None)
                return 1;
            if (leftVersion.None && rightVersion.Any())
                return -1;
            int result = CompareSegmentBySegment(leftVersion.Single(), rightVersion.Single());
            return result;
            }

        private static int CompareSegmentBySegment(string left, string right)
            {
            Contract.Requires(!string.IsNullOrEmpty(left));
            Contract.Requires(!string.IsNullOrEmpty(right));
            int result;
            var dotDelimiter = new[] { '.' };
            var leftSegments = left.Split(dotDelimiter, StringSplitOptions.RemoveEmptyEntries);
            var rightSegments = right.Split(dotDelimiter, StringSplitOptions.RemoveEmptyEntries);
            int longest = Math.Max(leftSegments.Length, rightSegments.Length);
            for (int i = 0; i < longest; i++)
                {
                // If we've run out of segments on either side, that side is the lesser version.
                if (i >= leftSegments.Length)
                    return -1;
                if (i >= rightSegments.Length)
                    return 1;

                // Compare the next segment to see if we can determine inequality.
                result = CompareSegmentPreferNumericSort(leftSegments[i], rightSegments[i]);
                if (result != 0)
                    return result;

                // We haven't determined inequality, so we have to go around to the next segment.
                }

            // If we've run out of segments on both sides, they must be equal by definition.
            return 0;
            }

        private static int CompareSegmentPreferNumericSort(string left, string right)
            {
            if (AllDigitsRegex.IsMatch(left) && AllDigitsRegex.IsMatch(right))
                {
                int value1 = int.Parse(left, CultureInfo.InvariantCulture);
                int value2 = int.Parse(right, CultureInfo.InvariantCulture);
                return value1.CompareTo(value2);
                }

            return string.Compare(left, right, StringComparison.Ordinal);
            }

        private static int ComparePrereleaseVersions(Maybe<string> leftVersion, Maybe<string> rightVersion)
            {
            // If the prerelease segment is absent in both instances, then they are considered equal.
            if (leftVersion.None && rightVersion.None)
                return 0;

            // By definition, a prerelease version is less than the absence of a prerelease version - this is a bit counterintuitive.
            if (leftVersion.Any() && rightVersion.None)
                return -1;
            if (leftVersion.None && rightVersion.Any())
                return 1;
            int result = CompareSegmentBySegment(leftVersion.Single(), rightVersion.Single());
            return result;
            }

        public static bool operator <(SemanticVersion version, SemanticVersion other)
            {
            Contract.Requires(null != version);
            Contract.Requires(null != other);
            return version.CompareTo(other) < 0;
            }

        public static bool operator >(SemanticVersion version, SemanticVersion other)
            {
            Contract.Requires(null != version);
            Contract.Requires(null != other);
            return version.CompareTo(other) > 0;
            }

        public static bool operator <=(SemanticVersion left, SemanticVersion right)
            {
            Contract.Requires(null != left);
            Contract.Requires(null != right);
            if (left.Equals(right)) return true;
            return left.CompareTo(right) < 0;
            }

        public static bool operator >=(SemanticVersion left, SemanticVersion right)
            {
            Contract.Requires(null != left);
            Contract.Requires(null != right);
            if (left.Equals(right)) return true;
            return left.CompareTo(right) > 0;
            }
        #endregion
        }
    }