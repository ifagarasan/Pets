using Kore.IO.Sync;

namespace SyncMaester.Core
{
    public interface IFolderDiffResult
    {
        IFolderDiff FolderDiff { get; }
        ISyncPair SyncPair { get; }
    }
}