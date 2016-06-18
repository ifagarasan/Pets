using System;
using Kore.IO.Sync;
using static Kore.Validation.ObjectValidation;

namespace SyncMaester.Core
{
    public class Kontrol : IKontrol
    {
        private readonly IDiffBuilder _diffBuilder;
        private readonly IFolderDiffProcessor _folderDiffProcessor;
        private readonly ISettings _settings;

        public Kontrol(ISettings settings, IDiffBuilder diffBuilder, IFolderDiffProcessor folderDiffProcessor)
        {
            IsNotNull(settings, nameof(settings));
            IsNotNull(diffBuilder, nameof(diffBuilder));
            IsNotNull(folderDiffProcessor, nameof(folderDiffProcessor));

            _settings = settings;
            _diffBuilder = diffBuilder;
            _folderDiffProcessor = folderDiffProcessor;
        }

        public void AddSyncPair(ISyncPair syncPair)
        {
            IsNotNull(syncPair, nameof(syncPair));

            _settings.SyncPair = syncPair;
        }

        public IFolderDiff BuildDiff()
        {
            IsNotNull(_settings.SyncPair);
            _settings.SyncPair.Destination.EnsureExists();

            return _diffBuilder.Build(_settings.SyncPair);
        }

        public void ProcessFolderDiff(IFolderDiff folderDiff)
        {
            IsNotNull(folderDiff);

            _folderDiffProcessor.Process(folderDiff);
        }
    }
}