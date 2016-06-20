using System;
using System.Collections.Generic;
using System.Linq;
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

        public IReadOnlyCollection<ISyncPair> SyncPairs => _settingsManager.Data.SyncPairs.AsReadOnly();

        public void AddSyncPair(ISyncPair syncPair)
        {
            IsNotNull(syncPair, nameof(syncPair));

            _settingsManager.Data.SyncPairs.Add(syncPair);
        }

        public void ProcessFolderDiff(IList<IFolderDiff> folderDiffs)
        {
            IsNotNull(folderDiffs);

            foreach (var folderDiff in folderDiffs)
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

        public ISettings Settings => _settingsManager.Data;

        public IList<IFolderDiff> BuildDiff()
        {
            IsNotNull(_settingsManager.Data.SyncPairs);

            return _settingsManager.Data.SyncPairs.Select(s => _diffBuilder.Build(s)).ToList();
        }
    }
}