using System.Collections.Generic;

namespace SyncMaester.Core
{
    public interface ISyncManager
    {
        void Sync(ISettings syncPairs);
    }
}