using Kore.IO.Sync;

namespace SyncMaester.Core
{
    public interface IDiffBuilder
    {
        IFolderDiff Build(ISyncPair syncPair);
    }
}