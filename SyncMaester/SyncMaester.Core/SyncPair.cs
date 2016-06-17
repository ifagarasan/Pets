using Kore.IO.Util;

namespace SyncMaester.Core
{
    public class SyncPair : ISyncPair
    {
        public IKoreFolderInfo Source { get; set; }

        public IKoreFolderInfo Destination { get; set; }
    }
}