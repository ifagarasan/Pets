using Kore.IO;
using Kore.IO.Scanners;
using Kore.IO.Sync;
using static Kore.Validation.ObjectValidation;
using Kore.IO.Exceptions;
using Kore.IO.Retrievers;

namespace SyncMaester.Core
{
    public class DiffBuilder : IDiffBuilder
    {
        private readonly IDiffInfoBuilder _diffInfoBuilder;
        private readonly IFileScanner _fileScanner;
        private readonly IFolderDiffer _folderDiffer;

        public DiffBuilder(IDiffInfoBuilder diffInfoBuilder, IFileScanner fileScanner, IFolderDiffer folderDiffer)
        {
            IsNotNull(fileScanner);
            IsNotNull(folderDiffer);

            _diffInfoBuilder = diffInfoBuilder;
            _fileScanner = fileScanner;
            _folderDiffer = folderDiffer;
        }

        public event FileFoundDelegate SourceFileFound;

        public event FileFoundDelegate DestinationFileFound;

        public IFolderDiff Build(ISyncPair syncPair)
        {
            IsNotNull(syncPair);

            var diffInfo = _diffInfoBuilder.BuildInfo(syncPair);

            diffInfo.Destination.EnsureExists();

            if (!diffInfo.Source.Exists)
                throw new NodeNotFoundException();

            _fileScanner.FileFound += _fileScanner_SourceFileFound;

            var sourceScan = _fileScanner.Scan(diffInfo.Source);

            _fileScanner.FileFound -= _fileScanner_SourceFileFound;
            _fileScanner.FileFound += _fileScanner_DestinationFileFound;

            var destinationScan = _fileScanner.Scan(diffInfo.Destination);

            _fileScanner.FileFound -= _fileScanner_DestinationFileFound;

            return _folderDiffer.BuildDiff(sourceScan, destinationScan);
        }

        private void _fileScanner_DestinationFileFound(IKoreFileInfo file)
        {
            DestinationFileFound?.Invoke(file);
        }

        private void _fileScanner_SourceFileFound(IKoreFileInfo file)
        {
            SourceFileFound?.Invoke(file);
        }
    }
}