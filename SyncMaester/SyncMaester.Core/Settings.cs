using System;
using System.Collections.Generic;

namespace SyncMaester.Core
{
    [Serializable]
    public class Settings : ISettings
    {
        public Settings()
        {
            SyncPair = new SyncPair();
        }

        public ISyncPair SyncPair { get; set; }
    }
}