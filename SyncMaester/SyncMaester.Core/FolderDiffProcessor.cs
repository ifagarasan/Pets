using Kore.IO.Sync;
using Kore.Validation;

namespace SyncMaester.Core
{
    public class FolderDiffProcessor : IFolderDiffProcessor
    {
        private readonly IDiffProcessor _diffProcessor;

        public FolderDiffProcessor(IDiffProcessor diffProcessor)
        {
            ObjectValidation.IsNotNull(diffProcessor);

            _diffProcessor = diffProcessor;
        }

        public void Process(IFolderDiffResult folderDiffResult)
        {
            ObjectValidation.IsNotNull(folderDiffResult, nameof(folderDiffResult));
            ObjectValidation.IsNotNull(folderDiffResult.FolderDiff, nameof(folderDiffResult.FolderDiff));

            foreach (var diff in folderDiffResult.FolderDiff.Diffs)
                _diffProcessor.Process(diff, folderDiffResult.FolderDiff.Source, folderDiffResult.FolderDiff.Destination);
        }
    }
}