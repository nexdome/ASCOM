// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using TA.Utils.Core;

namespace TA.NexDome.DeviceInterface
    {
    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Text.RegularExpressions;

    using TA.Ascom.ReactiveCommunications;
    using TA.Ascom.ReactiveCommunications.Diagnostics;
    using TA.NexDome.SharedTypes;

    internal class SemVerTransaction : DeviceTransaction
        {
        private const string VersionResponsePattern = @"^:FR[RS]?(?<SemVer>[^#]+)#$";

        private static readonly Regex versionResponseExpression = new Regex(
            VersionResponsePattern,
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture
            | RegexOptions.Singleline);

        public SemVerTransaction(string command)
            : base(command.EnsureEncapsulation()) { }

        public SemanticVersion SemanticVersion { get; private set; }

        /// <inheritdoc />
        public override void ObserveResponse(IObservable<char> source)
            {
            var strings = source.DelimitedMessageStrings(':','#').Trace("DelimitedMessageStrings");
            var semverResponses = from response in strings
                                 let match = versionResponseExpression.Match(response)
                                 where match.Success
                                 let versionString = match.Groups["SemVer"].Value
                                 where SemanticVersion.IsValid(versionString)
                                 select versionString;
            semverResponses
                .Trace("SemVer")
                .Take(1)
                .Subscribe(OnNext, OnError, OnCompleted);
            }

        /// <inheritdoc />
        protected override void OnCompleted()
            {
            SemanticVersion = new SemanticVersion(Response.Single());
            base.OnCompleted();
            }
        }
    }