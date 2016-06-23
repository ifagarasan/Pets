using System;

namespace SyncMaester.Core
{
    [Serializable]
    public class SyncPair : ISyncPair
    {
        public string Source { get; set; }

        public string Destination { get; set; }

        public SyncLevel Level { get; set; }
    }
}