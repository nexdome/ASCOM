// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.SharedTypes
    {
    public enum ShutterLinkState
        {
        [DisplayEquivalent("Initializing")]
        Start,
        [DisplayEquivalent("Waiting")]
        WaitAT,
        [DisplayEquivalent("Configuring")]
        Config,
        [DisplayEquivalent("Listening")]
        Detect,
        [DisplayEquivalent("Online")]
        Online
        }
    }