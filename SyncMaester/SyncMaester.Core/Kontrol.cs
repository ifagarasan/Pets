using System;
using System.Collections.Generic;
using Kore.IO.Exceptions;
using Kore.IO.Sync;
using Kore.IO.Util;
using Kore.Settings;
using static Kore.Validation.ObjectValidation;

namespace SyncMaester.Core
{
    public class Kontrol : IKontrol
    {
        private readonly IDiffBuilder _diffBuilder;
        private readonly IFolderDiffProcessor _folderDiffProcessor;
        private readonly ISettingsManager<ISettings> _settingsManager;

        public Kontrol(ISettingsManager<ISettings> settingsManager, IDiffBuilder diffBuilder, IFolderDiffProcessor folderDiffProcessor)
        {
            IsNotNull(settingsManager, nameof(settingsManager));
            IsNotNull(diffBuilder, nameof(diffBuilder));
            IsNotNull(folderDiffProcessor, nameof(folderDiffProcessor));

            _settingsManager = settingsManager;
            _diffBuilder = diffBuilder;
            _folderDiffProcessor = folderDiffProcessor;
        }

        public ISyncPair SyncPair => _settingsManager.Data.SyncPair;

        public void AddSyncPair(ISyncPair syncPair)
        {
            IsNotNull(syncPair, nameof(syncPair));

            _settingsManager.Data.SyncPair = syncPair;
        }

        public IFolderDiff BuildDiff()
        {
            IsNotNull(_settingsManager.Data.SyncPair);
            _settingsManager.Data.SyncPair.Destination.EnsureExists();

            if (!_settingsManager.Data.SyncPair.Source.Exists)
                throw new NodeNotFoundException();

            return _diffBuilder.Build(_settingsManager.Data.SyncPair);
        }

        public void ProcessFolderDiff(IFolderDiff folderDiff)
        {
            IsNotNull(folderDiff);

            _folderDiffProcessor.Process(folderDiff);
        }

        public void WriteSettings(IKoreFileInfo destination)
        {
            IsNotNull(destination);

            _settingsManager.Write(destination);
        }

        public void ReadSettings(IKoreFileInfo source)
        {
            IsNotNull(source);

            if (!source.Exists)
                _settingsManager.Data = new Settings();
            else
                _settingsManager.Read(source);
        }
    }
}