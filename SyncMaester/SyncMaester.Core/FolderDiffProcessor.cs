using Kore.IO.Sync;
using static Kore.Validation.ObjectValidation;

namespace SyncMaester.Core
{
    public class FolderDiffProcessor : IFolderDiffProcessor
    {
        private readonly IDiffProcessor _diffProcessor;

        public FolderDiffProcessor(IDiffProcessor diffProcessor)
        {
            IsNotNull(diffProcessor, nameof(diffProcessor));

            _diffProcessor = diffProcessor;
        }

        public void Process(IFolderDiffResult folderDiffResult)
        {
            IsNotNull(folderDiffResult, nameof(folderDiffResult));
            IsNotNull(folderDiffResult.FolderDiff, nameof(folderDiffResult.FolderDiff));
            IsNotNull(folderDiffResult.FolderDiff.Diffs, nameof(folderDiffResult.FolderDiff.Diffs));

            foreach (var diff in folderDiffResult.FolderDiff.Diffs)
                _diffProcessor.Process(diff, folderDiffResult.FolderDiff.Source, folderDiffResult.FolderDiff.Destination);
        }
    }
}