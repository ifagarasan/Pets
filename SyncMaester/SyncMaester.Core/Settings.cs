using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SyncMaester.Core
{
    public class Settings: ISettings
    {
        public Settings()
        {
            SyncPairs = new List<ISyncPair>();
        }

        public IList<ISyncPair> SyncPairs { get; set; }
    }
}