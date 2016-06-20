using System;
using System.Collections.Generic;

namespace SyncMaester.Core
{
    [Serializable]
    public class Settings : ISettings
    {
        public Settings()
        {
            SyncPairs = new List<ISyncPair>();
        }

        public List<ISyncPair> SyncPairs { get; set; }
    }
}