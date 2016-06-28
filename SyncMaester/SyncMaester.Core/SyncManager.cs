using System;
using System.Collections.Generic;
using System.Linq;
using Kore.IO.Sync;
using Kore.Validation;
using SyncMaester.Core;
using static Kore.Validation.ObjectValidation;

namespace SyncMaester.Core
{
    public class SyncManager : ISyncManager
    {
        private readonly IDiffBuilder _diffBuilder;
        private readonly IFolderDiffProcessor _folderDiffProcessor;

        public SyncManager(IDiffBuilder diffBuilder, IFolderDiffProcessor folderDiffProcessor, IScanInfo scanInfo)
        {
            IsNotNull(diffBuilder);
            IsNotNull(folderDiffProcessor);
            IsNotNull(scanInfo);

            _diffBuilder = diffBuilder;

            _diffBuilder.SourceFileFound += file => ScanInfo.NewSourceFileFound(file);
            _diffBuilder.DestinationFileFound += file => ScanInfo.NewDestinationFileFound(file);

            _folderDiffProcessor = folderDiffProcessor;

            ScanInfo = scanInfo;
        }

        public IScanInfo ScanInfo { get; }

        public void Sync(ISettings settings)
        {
            IsNotNull(settings);
            IsNotNull(settings.SyncPairs);

            ScanInfo.Clear();

            foreach (var diff in settings.SyncPairs.Select(BuildFolderDiffResult))
                _folderDiffProcessor.Process(diff);
        }

        private IFolderDiffResult BuildFolderDiffResult(ISyncPair syncPair)
        {
            return new FolderDiffResult(syncPair, _diffBuilder.Build(syncPair));
        }
    }
}