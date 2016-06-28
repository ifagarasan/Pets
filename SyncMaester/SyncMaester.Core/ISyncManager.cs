using System.Collections.Generic;

namespace SyncMaester.Core
{
    public interface ISyncManager
    {
        IScanInfo ScanInfo { get; }

        void Sync(ISettings settings);
    }
}