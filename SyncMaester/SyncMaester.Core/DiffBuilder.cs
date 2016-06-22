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

            //TODO: throw exception if source file doesn't exist
            //TODO: create file if doesn't exist
            //TODO: create diffinfo here

            var sourceScan = _fileScanner.Scan(syncPair.Source);
            var destinationScan = _fileScanner.Scan(syncPair.Destination);

            return _folderDiffer.BuildDiff(sourceScan, destinationScan);
        }
    }
}