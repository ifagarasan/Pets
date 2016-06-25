using Kore.IO.Retrievers;
using Kore.IO.Sync;

namespace SyncMaester.Core
{
    public interface IDiffBuilder
    {
        event FileFoundDelegate SourceFileFound;

        event FileFoundDelegate DestinationFileFound;

        IFolderDiff Build(ISyncPair syncPair);
    }
}