using System;
using System.Collections.Generic;
using Kore.IO.Scanners;
using Kore.IO.Sync;
using Kore.Validation;

namespace SyncMaester.Core
{
    public class DiffBuilder : IDiffBuilder
    {
        private readonly IFileScanner _fileScanner;
        private readonly IFolderDiffer _folderDiffer;

        public DiffBuilder(IFileScanner fileScanner, IFolderDiffer folderDiffer)
        {
            ObjectValidation.IsNotNull(fileScanner, nameof(fileScanner));
            ObjectValidation.IsNotNull(folderDiffer, nameof(folderDiffer));

            _fileScanner = fileScanner;
            _folderDiffer = folderDiffer;
        }

        public IFolderDiff Build(ISyncPair syncPair)
        {
            ObjectValidation.IsNotNull(syncPair, nameof(syncPair));

            IFileScanResult sourceScan = _fileScanner.Scan(syncPair.Source);
            IFileScanResult destinationScan = _fileScanner.Scan(syncPair.Destination);

            return _folderDiffer.BuildDiff(sourceScan, destinationScan);
        }
    }
}