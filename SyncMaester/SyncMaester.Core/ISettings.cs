using System.Collections.Generic;

namespace SyncMaester.Core
{
    public interface ISettings
    {
        ISyncPair SyncPair { get; set; }
    }
}