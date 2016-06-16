using Kore.IO.Sync;

namespace SyncMaester.Core
{
    public interface IFolderDiffProcessor
    {
        void Process(IFolderDiff folderDiff);
    }
}