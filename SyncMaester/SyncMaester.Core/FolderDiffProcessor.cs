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
            ObjectValidation.IsNotNull(folderDiffResult);

            foreach (var diff in folderDiffResult.FolderDiff.Diffs)
                _diffProcessor.Process(diff, folderDiffResult.Source, folderDiffResult.Destination);
        }
    }
}