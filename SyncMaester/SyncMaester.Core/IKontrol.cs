using Kore.IO.Sync;

namespace SyncMaester.Core
{
    public interface IKontrol
    {
        void AddSyncPair(ISyncPair syncPair);
        IFolderDiff BuildDiff();
        void ProcessFolderDiff(IFolderDiff folderDiff);
    }
}