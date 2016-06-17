using Kore.IO.Util;

namespace SyncMaester.Core
{
    public interface ISyncPair
    {
        IKoreFolderInfo Source{ get; set; }
        IKoreFolderInfo Destination { get; set; }
    }
}