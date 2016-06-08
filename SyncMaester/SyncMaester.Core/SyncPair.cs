using System.Collections.Generic;

namespace SyncMaester.Core
{
    public class SyncPair: ISyncPair
    {
        public SyncPair()
        {
            DestinationFolders = new List<string>();
        }

        public IList<string> DestinationFolders { get; set; }
        public string SourceFolder { get; set; }
    }
}