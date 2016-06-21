using Kore.IO.Util;

namespace SyncMaester.Core
{
    public class DiffInfo : IDiffInfo
    {
        public IKoreFolderInfo Source { get; set; }

        public IKoreFolderInfo Destination { get; set; }
    }
}