using Kore.IO;

namespace SyncMaester.Core
{
    public interface IScanInfo
    {
        uint SourceFiles { get; }
        uint DestinationFiles { get; }

        void Clear();

        void NewSourceFileFound(IKoreFileInfo file);
        void NewDestinationFileFound(IKoreFileInfo file);
    }
}