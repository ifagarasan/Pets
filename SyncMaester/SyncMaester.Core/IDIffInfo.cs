using Kore.IO;

namespace SyncMaester.Core
{
    public interface IDiffInfo
    {
        IKoreFolderInfo Source { get; }

        IKoreFolderInfo Destination { get; }
    }
}