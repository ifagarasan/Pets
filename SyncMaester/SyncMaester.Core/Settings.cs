using System.Collections.Generic;

namespace SyncMaester.Core
{
    public class Settings : ISettings
    {
        public ISyncPair SyncPair { get; set; }
    }
}