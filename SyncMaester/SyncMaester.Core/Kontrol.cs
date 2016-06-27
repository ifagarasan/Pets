using System;
using System.Collections.Generic;
using System.Linq;
using Kore.IO;
using Kore.Settings;
using static Kore.Validation.ObjectValidation;

namespace SyncMaester.Core
{
    public class Kontrol : IKontrol
    {
        private readonly ISettingsManager<ISettings> _settingsManager;
        private readonly ISyncManager _syncManager;

        public Kontrol(ISettingsManager<ISettings> settingsManager, ISyncManager syncManager)
        {
            IsNotNull(settingsManager);
            IsNotNull(syncManager);

            _settingsManager = settingsManager;
            _syncManager = syncManager;
        }

        public ISettings Settings => _settingsManager.Data;

        public IReadOnlyCollection<ISyncPair> SyncPairs => _settingsManager.Data.SyncPairs.AsReadOnly();

        public void AddSyncPair(ISyncPair syncPair)
        {
            IsNotNull(syncPair);

            _settingsManager.Data.SyncPairs.Add(syncPair);
        }

        public void Sync()
        {
            _syncManager.Sync(Settings);
        }

        //public IDiffResult BuildDiff()
        //{
        //    IsNotNull(_settingsManager.Data.SyncPairs);

        //    var folderDiffResults = _settingsManager.Data.SyncPairs.Select(s => BuildFolderDiffResult(s)).ToList();

        //    return new DiffResult(folderDiffResults);
        //}

        

        //public void ProcessFolderDiff(IDiffResult diffResult)
        //{
        //    IsNotNull(diffResult);

        //    foreach (var folderDiffResult in diffResult.FolderDiffResults)
        //        _folderDiffProcessor.Process(folderDiffResult);
        //}

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