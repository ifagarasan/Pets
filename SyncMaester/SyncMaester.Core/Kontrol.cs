using System;
using Kore.IO.Sync;
using Kore.Validation;

namespace SyncMaester.Core
{
    public class Kontrol : IKontrol
    {
        private readonly IDiffBuilder _diffBuilder;
        private readonly IFolderDiffProcessor _folderDiffProcessor;
        private readonly ISettings _settings;

        public Kontrol(ISettings settings, IDiffBuilder diffBuilder, IFolderDiffProcessor folderDiffProcessor)
        {
            ObjectValidation.IsNotNull(settings, nameof(settings));
            ObjectValidation.IsNotNull(diffBuilder, nameof(diffBuilder));
            ObjectValidation.IsNotNull(folderDiffProcessor, nameof(folderDiffProcessor));

            _settings = settings;
            _diffBuilder = diffBuilder;
            _folderDiffProcessor = folderDiffProcessor;
        }

        public void AddSyncPair(ISyncPair syncPair)
        {
            ObjectValidation.IsNotNull(syncPair, nameof(syncPair));

            _settings.SyncPair = syncPair;
        }

        public IFolderDiff BuildDiff()
        {
            return _diffBuilder.Build(_settings.SyncPair);
        }

        public void ProcessFolderDiff(IFolderDiff folderDiff)
        {
            ObjectValidation.IsNotNull(folderDiff);

            _folderDiffProcessor.Process(folderDiff);
        }
    }
}