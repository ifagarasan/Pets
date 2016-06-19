using Kore.IO.Sync;
using Kore.IO.Util;

namespace SyncMaester.Core
{
    public interface IKontrol
    {
        void AddSyncPair(ISyncPair syncPair);
        IFolderDiff BuildDiff();
        void ProcessFolderDiff(IFolderDiff folderDiff);
        void WriteSettings(IKoreFileInfo destination);
        void ReadSettings(IKoreFileInfo source);
    }
}