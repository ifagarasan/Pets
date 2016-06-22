using System.IO;
using Kore.IO.Util;

namespace SyncMaester.Core
{
    public class DiffInfoBuilder : IDiffInfoBuilder
    {
        public IDiffInfo BuildInfo(ISyncPair syncPair)
        {
            var source = syncPair.Source;
            var destination = syncPair.Destination;

            if (syncPair.Level == SyncLevel.Parent)
                destination = Path.Combine(destination, new KoreFolderInfo(syncPair.Source).Name);

            return new DiffInfo
            {
                Source = new KoreFolderInfo(source),
                Destination = new KoreFolderInfo(destination)
            };
        }
    }
}