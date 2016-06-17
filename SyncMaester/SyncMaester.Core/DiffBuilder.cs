using System;
using System.Collections.Generic;
using Kore.IO.Scanners;
using Kore.IO.Sync;
using static Kore.Validation.ObjectValidation;

namespace SyncMaester.Core
{
    public class DiffBuilder : IDiffBuilder
    {
        private readonly IFileScanner _fileScanner;
        private readonly IFolderDiffer _folderDiffer;

        public DiffBuilder(IFileScanner fileScanner, IFolderDiffer folderDiffer)
        {
            IsNotNull(fileScanner, nameof(fileScanner));
            IsNotNull(folderDiffer, nameof(folderDiffer));

            _fileScanner = fileScanner;
            _folderDiffer = folderDiffer;
        }

        public IFolderDiff Build(ISyncPair syncPair)
        {
            IsNotNull(syncPair, nameof(syncPair));

            var sourceScan = _fileScanner.Scan(syncPair.Source.FullName);
            var destinationScan = _fileScanner.Scan(syncPair.Destination.FullName);

            return _folderDiffer.BuildDiff(sourceScan, destinationScan);
        }
    }
}