using System.Collections.Generic;

namespace SyncMaester.Core
{
    public interface ISyncManager
    {
        ISyncInfo SyncInfo { get; }

        void Sync(ISettings settings);
    }
}