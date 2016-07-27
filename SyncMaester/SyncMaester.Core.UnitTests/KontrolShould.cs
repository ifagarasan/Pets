using System;
using System.Collections.Generic;
using System.Linq;
using Kore.Exceptions;
using Kore.IO;
using Kore.IO.Sync;
using Kore.Settings;
using Kore.Settings.Serializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SyncMaester.Core.UnitTests
{
    [TestClass]
    public class KontrolShould
    {
        ISettings _settings;
        IKontrol _kontrol;
        Mock<ISettingsManager<ISettings>> _mockSettingsManager;
        private Mock<ISyncInfo> _mockScanInfo;

        readonly string _sourceFolder = "abc";
        readonly string _destinationFolder = "efg";

        Mock<ISyncPair> _mockSyncPair;

        Mock<IKoreFileInfo> _mockFileInfo;

        private Mock<ISyncManager> _mockSyncManager;

        [TestInitialize]
        public void Setup()
        {
            _mockScanInfo = new Mock<ISyncInfo>();

            _mockFileInfo = new Mock<IKoreFileInfo>();
            _mockFileInfo.Setup(m => m.Exists).Returns(true);

            _mockSyncPair = new Mock<ISyncPair>();
            _mockSyncPair.Setup(m => m.Source).Returns(_sourceFolder);
            _mockSyncPair.Setup(m => m.Destination).Returns(_destinationFolder);

            _settings = new Settings();

            _mockSettingsManager = new Mock<ISettingsManager<ISettings>>();
            _mockSettingsManager.Setup(m => m.Data).Returns(_settings);
            _mockSettingsManager.Setup(m => m.Write(It.IsAny<IKoreFileInfo>()));

            _mockSyncManager = new Mock<ISyncManager>();
            _mockSyncManager.Setup(m => m.Sync(It.IsAny<ISettings>()));
            _mockSyncManager.Setup(m => m.SyncInfo).Returns(_mockScanInfo.Object);

            _kontrol = new Kontrol(_mockSettingsManager.Object, _mockSyncManager.Object);
        }

        #region Init

        [TestMethod]
        [ExpectedException(typeof(NullException))]
        public void ThrowArgumentNullExceptionIfSettingsManagerNull()
        {
            _kontrol = new Kontrol(null, _mockSyncManager.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(NullException))]
        public void ValidatesSyncManagerOnInit()
        {
            _kontrol = new Kontrol(_mockSettingsManager.Object, null);
        }

        #endregion

        #region AddSyncPair

        [TestMethod]
        public void AddSyncPairToList()
        {
            ISyncPair syncPair = new SyncPair
            {
                Source = _sourceFolder,
                Destination = _destinationFolder
            };

            _kontrol.Settings.SyncPairs.Add(syncPair);

            Assert.AreSame(syncPair, _settings.SyncPairs.Last());
        }

        #endregion

        #region Sync

        [TestMethod]
        public void MakeSyncManagerScanInfoAvailable()
        {
            Assert.AreSame(_mockScanInfo.Object, _kontrol.SyncInfo);
        }

        [TestMethod]
        public void CallsSyncManager()
        {
            _kontrol.Sync();

            _mockSyncManager.Verify(m => m.Sync(_kontrol.Settings));
        }

        #endregion

        #region Settings

        [TestMethod]
        public void ExposeSettings()
        {
            Assert.AreSame(_settings, _kontrol.Settings);
        }

        [TestMethod]
        public void WriteSettings()
        {
            _kontrol.WriteSettings(_mockFileInfo.Object);

            _mockSettingsManager.Verify(m => m.Write(_mockFileInfo.Object));
        }

        [TestMethod]
        [ExpectedException(typeof(NullException))]
        public void ValidateFileInfoOnWriteSettings()
        {
            _kontrol.WriteSettings(null);
        }

        [TestMethod]
        public void ReadSettings()
        {
            _kontrol.ReadSettings(_mockFileInfo.Object);

            _mockSettingsManager.Verify(m => m.Read(_mockFileInfo.Object));
        }

        [TestMethod]
        [ExpectedException(typeof(NullException))]
        public void ValidateFileInfoOnReadSettings()
        {
            _kontrol.ReadSettings(null);
        }

        [TestMethod]
        public void CreateNewInstanceOfSettingsIfFileDoesNotExistOnReadSettings()
        {
            ISettingsManager<ISettings> settingsManager = new SettingsManager<ISettings>(new BinarySerializer<ISettings>());
            
            _kontrol = new Kontrol(settingsManager, _mockSyncManager.Object);

            _mockFileInfo.Setup(m => m.Exists).Returns(false);

            _kontrol.ReadSettings(_mockFileInfo.Object);

            _mockSettingsManager.Verify(m => m.Read(It.IsAny<IKoreFileInfo>()), Times.Never);

            Assert.IsNotNull(settingsManager.Data);
        }

        #endregion
    }
}