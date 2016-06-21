using System.IO;
using Kore.IO.Sync;
using Kore.IO.Util;

namespace SyncMaester.Core
{
    public class DiffInfoBuilder : IDiffInfoBuilder
    {
        public IDiffInfo BuildInfo(IFolderDiffResult folderDiffResult)
        {
            //TODO: consolidate by validation

            var source = folderDiffResult.FolderDiff.Source.FullName;
            var destination = folderDiffResult.FolderDiff.Destination.FullName;

            if (folderDiffResult.SyncPair.Level == SyncLevel.Parent)
                destination = Path.Combine(destination, Path.GetFileName(source)); //TODO: add Name to KoreFolderInfo

            return new DiffInfo
            {
                Source = new KoreFolderInfo(source),
                Destination = new KoreFolderInfo(destination)
            };
        }
    }
}