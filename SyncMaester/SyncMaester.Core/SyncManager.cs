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

        public SyncManager(IDiffBuilder diffBuilder, IFolderDiffProcessor folderDiffProcessor)
        {
            IsNotNull(diffBuilder);
            IsNotNull(folderDiffProcessor);

            _diffBuilder = diffBuilder;
            _folderDiffProcessor = folderDiffProcessor;
        }

        public void Sync(ISettings settings)
        {
            IsNotNull(settings);
            IsNotNull(settings.SyncPairs);

            foreach (var diff in settings.SyncPairs.Select(BuildFolderDiffResult))
                _folderDiffProcessor.Process(diff);
        }

        private IFolderDiffResult BuildFolderDiffResult(ISyncPair syncPair)
        {
            return new FolderDiffResult(syncPair, _diffBuilder.Build(syncPair));
        }
    }
}