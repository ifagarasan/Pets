using Kore.IO.Sync;

namespace SyncMaester.Core
{
    public interface IDiffResult
    {
        ISyncPair SyncPair { get; }
        IFolderDiff FolderDiff { get; }
    }
}