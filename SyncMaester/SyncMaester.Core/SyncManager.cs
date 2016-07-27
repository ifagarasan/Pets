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

        public SyncManager(IDiffBuilder diffBuilder, IFolderDiffProcessor folderDiffProcessor, ISyncInfo syncInfo)
        {
            IsNotNull(diffBuilder);
            IsNotNull(folderDiffProcessor);
            IsNotNull(syncInfo);

            _diffBuilder = diffBuilder;

            _diffBuilder.SourceFileFound += file => SyncInfo.NewSourceFileFound(file);
            _diffBuilder.DestinationFileFound += file => SyncInfo.NewDestinationFileFound(file);

            _folderDiffProcessor = folderDiffProcessor;

            SyncInfo = syncInfo;
        }

        public ISyncInfo SyncInfo { get; }

        public void Sync(ISettings settings)
        {
            IsNotNull(settings);
            IsNotNull(settings.SyncPairs);

            SyncInfo.Clear();

            foreach (var diff in settings.SyncPairs.Select(BuildFolderDiffResult))
                _folderDiffProcessor.Process(diff);

            SyncInfo.Complete();
        }

        private IFolderDiffResult BuildFolderDiffResult(ISyncPair syncPair)
        {
            return new FolderDiffResult(syncPair, _diffBuilder.Build(syncPair));
        }
    }
}