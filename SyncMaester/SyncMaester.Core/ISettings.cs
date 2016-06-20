using System.Collections.Generic;

namespace SyncMaester.Core
{
    public interface ISettings
    {
        List<ISyncPair> SyncPairs { get; set; }
    }
}