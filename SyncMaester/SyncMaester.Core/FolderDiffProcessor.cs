using Kore.IO.Sync;
using static Kore.Validation.ObjectValidation;

namespace SyncMaester.Core
{
    public class FolderDiffProcessor : IFolderDiffProcessor
    {
        private readonly IDiffInfoBuilder _diffInfoBuilder;
        private readonly IDiffProcessor _diffProcessor;

        public FolderDiffProcessor(IDiffProcessor diffProcessor, IDiffInfoBuilder diffInfoBuilder)
        {
            IsNotNull(diffProcessor, nameof(diffProcessor));
            IsNotNull(diffInfoBuilder, nameof(diffInfoBuilder));

            _diffProcessor = diffProcessor;
            _diffInfoBuilder = diffInfoBuilder;
        }

        public void Process(IFolderDiffResult folderDiffResult)
        {
            IsNotNull(folderDiffResult, nameof(folderDiffResult));
            IsNotNull(folderDiffResult.FolderDiff, nameof(folderDiffResult.FolderDiff));
            IsNotNull(folderDiffResult.FolderDiff.Diffs, nameof(folderDiffResult.FolderDiff.Diffs));

            var diffInfo = _diffInfoBuilder.BuildInfo(folderDiffResult);

            foreach (var diff in folderDiffResult.FolderDiff.Diffs)
                _diffProcessor.Process(diff, diffInfo);
        }
    }
}