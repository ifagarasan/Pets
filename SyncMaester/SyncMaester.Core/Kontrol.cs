using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public ISyncInfo SyncInfo => _syncManager.SyncInfo;

        public IReadOnlyCollection<ISyncPair> SyncPairs => _settingsManager.Data.SyncPairs.AsReadOnly();

        public void Sync() => _syncManager.Sync(Settings);

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