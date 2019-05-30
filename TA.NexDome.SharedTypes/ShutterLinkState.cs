namespace TA.NexDome.SharedTypes {
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