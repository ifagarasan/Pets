using System;
using System.Collections.Generic;
using Kore.IO.Scanners;
using Kore.IO.Sync;
using Kore.IO.Util;
using static Kore.Validation.ObjectValidation;

namespace SyncMaester.Core
{
    public class DiffBuilder : IDiffBuilder
    {
        private readonly IDiffInfoBuilder _diffInfoBuilder;
        private readonly IFileScanner _fileScanner;
        private readonly IFolderDiffer _folderDiffer;

        public DiffBuilder(IDiffInfoBuilder diffInfoBuilder, IFileScanner fileScanner, IFolderDiffer folderDiffer)
        {
            IsNotNull(fileScanner, nameof(fileScanner));
            IsNotNull(folderDiffer, nameof(folderDiffer));

            _diffInfoBuilder = diffInfoBuilder;
            _fileScanner = fileScanner;
            _folderDiffer = folderDiffer;
        }

        public IFolderDiff Build(ISyncPair syncPair)
        {
            IsNotNull(syncPair, nameof(syncPair));

            var diffInfo = _diffInfoBuilder.BuildInfo(syncPair);

            diffInfo.Destination.EnsureExists();

            //TODO: throw exception if source file doesn't exist

            var sourceScan = _fileScanner.Scan(diffInfo.Source.FullName);
            var destinationScan = _fileScanner.Scan(diffInfo.Destination.FullName);

            return _folderDiffer.BuildDiff(sourceScan, destinationScan);
        }
    }
}