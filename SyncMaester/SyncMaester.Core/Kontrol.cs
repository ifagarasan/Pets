using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyncMaester.Core
{
    public class Kontrol
    {
        private readonly ISettings _settings;

        public Kontrol(ISettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            _settings = settings;
        }

        public void AddSyncPair(ISyncPair syncPair)
        {
            if (syncPair == null)
                throw new ArgumentNullException(nameof(syncPair));

            _settings.SyncPairs.Add(syncPair);
        }

        public bool RemoveSyncPair(ISyncPair syncPair)
        {
            return _settings.SyncPairs.Remove(syncPair);
        }
    }
}
