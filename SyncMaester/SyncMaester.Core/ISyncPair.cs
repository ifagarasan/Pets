using Kore.IO.Util;

namespace SyncMaester.Core
{
    public interface ISyncPair
    {
        string Source{ get; set; }

        string Destination { get; set; }

        SyncLevel Level { get; set; }
    }
}