using Kore.IO.Sync;

namespace SyncMaester.Core
{
    internal class FolderDiffResult: IFolderDiffResult
    {
        public IFolderDiff FolderDiff { get; }
        public ISyncPair SyncPair { get; }

        public FolderDiffResult(ISyncPair syncPair, IFolderDiff folderDiff)
        {
            SyncPair = syncPair;
            FolderDiff = folderDiff;
        }
    }
}