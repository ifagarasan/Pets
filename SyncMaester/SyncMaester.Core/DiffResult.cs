using Kore.IO.Sync;

namespace SyncMaester.Core
{
    public class DiffResult : IDiffResult
    {
        public DiffResult(IFolderDiff folderDiff, ISyncPair syncPair)
        {
            FolderDiff = folderDiff;
            SyncPair = syncPair;
        }

        public IFolderDiff FolderDiff { get; }
        public ISyncPair SyncPair { get; }
    }
}