using System.Collections.Generic;

namespace SyncMaester.Core
{
    public interface ISyncPair
    {
        IList<string> DestinationFolders { get; set; }
        string SourceFolder { get; set; }
    }
}