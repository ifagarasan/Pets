using Kore.IO;

namespace SyncMaester.Core
{
    public interface ISyncInfo
    {
        uint SourceFiles { get; }
        uint DestinationFiles { get; }
        SyncStatus Status { get; }

        void Clear();

        void NewSourceFileFound(IKoreFileInfo file);
        void NewDestinationFileFound(IKoreFileInfo file);

        void Complete();
    }
}