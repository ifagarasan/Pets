using Kore.IO.Util;

namespace SyncMaester.Core
{
    public interface IDiffInfo
    {
        IKoreFolderInfo Source { get; }

        IKoreFolderInfo Destination { get; }
    }
}