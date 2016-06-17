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

        public void Process(IFolderDiff folderDiff)
        {
            ObjectValidation.IsNotNull(folderDiff);
            ObjectValidation.IsNotNull(folderDiff.Diffs);

            foreach (var diff in folderDiff.Diffs)
                _diffProcessor.Process(diff, folderDiff.Source, folderDiff.Destination);
        }
    }
}