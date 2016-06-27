using static Kore.Validation.ObjectValidation;

namespace SyncMaester.Core
{
    public class FolderDiffProcessor : IFolderDiffProcessor
    {
        private readonly IDiffProcessor _diffProcessor;

        public FolderDiffProcessor(IDiffProcessor diffProcessor)
        {
            IsNotNull(diffProcessor);

            _diffProcessor = diffProcessor;
        }

        public void Process(IFolderDiffResult folderDiffResult)
        {
            IsNotNull(folderDiffResult);
            IsNotNull(folderDiffResult.FolderDiff);
            IsNotNull(folderDiffResult.FolderDiff.Diffs);

            foreach (var diff in folderDiffResult.FolderDiff.Diffs)
                _diffProcessor.Process(diff, folderDiffResult.FolderDiff.Source, folderDiffResult.FolderDiff.Destination);
        }
    }
}