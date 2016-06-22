using System.IO;
using Kore.IO.Sync;
using Kore.IO.Util;
using static Kore.Validation.ObjectValidation;

namespace SyncMaester.Core
{
    public class DiffInfoBuilder : IDiffInfoBuilder
    {
        public IDiffInfo BuildInfo(IFolderDiffResult folderDiffResult)
        {
            IsNotNull(folderDiffResult, nameof(folderDiffResult));
            IsNotNull(folderDiffResult.FolderDiff, nameof(folderDiffResult.FolderDiff));
            IsNotNull(folderDiffResult.FolderDiff.Source, nameof(folderDiffResult.FolderDiff.Source));
            IsNotNull(folderDiffResult.FolderDiff.Destination, nameof(folderDiffResult.FolderDiff.Destination));

            var source = folderDiffResult.FolderDiff.Source.FullName;
            var destination = folderDiffResult.FolderDiff.Destination.FullName;

            if (folderDiffResult.SyncPair.Level == SyncLevel.Parent)
                destination = Path.Combine(destination, folderDiffResult.FolderDiff.Source.Name);

            return new DiffInfo
            {
                Source = new KoreFolderInfo(source),
                Destination = new KoreFolderInfo(destination)
            };
        }
    }
}