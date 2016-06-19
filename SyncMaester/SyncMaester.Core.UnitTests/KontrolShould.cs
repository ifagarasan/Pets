using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kore.IO.Exceptions;
using Kore.IO.Sync;
using Kore.IO.Util;
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
        Mock<IDiffBuilder> _mockDiffBuilder;
        Mock<IFolderDiffProcessor> _mockFolderDiffProcessor;
        IKontrol _kontrol;
        Mock<ISettingsManager<ISettings>> _mockSettingsManager;

        Mock<IKoreFolderInfo> _mockSourceFolderInfo;
        Mock<IKoreFolderInfo> _mockDestinationFolderInfo;

        readonly string _sourceFolder = "abc";
        readonly string _destinationFolder = "efg";

        Mock<ISyncPair> _mockSyncPair;

        Mock<IKoreFileInfo> _mockFileInfo;

        [TestInitialize]
        public void Setup()
        {
            _mockFileInfo = new Mock<IKoreFileInfo>();
            _mockFileInfo.Setup(m => m.Exists).Returns(true);

            _mockSourceFolderInfo = new Mock<IKoreFolderInfo>();
            _mockSourceFolderInfo.Setup(m => m.FullName).Returns(_sourceFolder);
            _mockSourceFolderInfo.Setup(m => m.Exists).Returns(true);

            _mockDestinationFolderInfo = new Mock<IKoreFolderInfo>();
            _mockDestinationFolderInfo.Setup(m => m.FullName).Returns(_destinationFolder);
            _mockDestinationFolderInfo.Setup(m => m.Exists).Returns(true);

            _mockSyncPair = new Mock<ISyncPair>();
            _mockSyncPair.Setup(m => m.Source).Returns(_mockSourceFolderInfo.Object);
            _mockSyncPair.Setup(m => m.Destination).Returns(_mockDestinationFolderInfo.Object);

            _settings = new Settings {SyncPair = _mockSyncPair.Object};

            _mockSettingsManager = new Mock<ISettingsManager<ISettings>>();
            _mockSettingsManager.Setup(m => m.Data).Returns(_settings);
            _mockSettingsManager.Setup(m => m.Write(It.IsAny<IKoreFileInfo>()));

            _mockDiffBuilder = new Mock<IDiffBuilder>();
            _mockFolderDiffProcessor = new Mock<IFolderDiffProcessor>();

            _kontrol = new Kontrol(_mockSettingsManager.Object, _mockDiffBuilder.Object, _mockFolderDiffProcessor.Object);
        }

        #region Init

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowArgumentNullExceptionIfSettingsManagerNull()
        {
            _kontrol = new Kontrol(null, _mockDiffBuilder.Object, _mockFolderDiffProcessor.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowArgumentNullExceptionIfDiffBuilderNull()
        {
            _kontrol = new Kontrol(_mockSettingsManager.Object, null, _mockFolderDiffProcessor.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowArgumentNullExceptionIfDiffProcessorNull()
        {
            _kontrol = new Kontrol(_mockSettingsManager.Object, _mockDiffBuilder.Object, null);
        }

        #endregion

        #region AddSyncPair

        [TestMethod]
        public void SetSyncPairToProvided()
        {
            ISyncPair syncPair = new SyncPair
            {
                Source = _mockSourceFolderInfo.Object,
                Destination = _mockDestinationFolderInfo.Object
            };

            _kontrol.AddSyncPair(syncPair);

            Assert.AreSame(syncPair, _settings.SyncPair);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowArgumentNullExceptionIfSyncPairIsNull()
        {
            _kontrol.AddSyncPair(null);
        }

        #endregion

        #region BuildDiff

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidateSettingsSyncPairOnBuildDiff()
        {
            _settings.SyncPair = null;

            _kontrol.BuildDiff();
        }

        [TestMethod]
        public void CreateDestinationFolderIfItDoesNotExist()
        {
            _mockDestinationFolderInfo.Setup(m => m.EnsureExists());

            _kontrol.BuildDiff();

            _mockDestinationFolderInfo.Verify(m => m.EnsureExists(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(NodeNotFoundException))]
        public void ValidateSourceFolderExistsOnBUild()
        {
            _mockSourceFolderInfo.Setup(m => m.Exists).Returns(false);

            _kontrol.BuildDiff();
        }

        [TestMethod]
        public void CallBuildDiffOnDiffBuilder()
        {
            _settings.SyncPair = new SyncPair { Source = _mockSourceFolderInfo.Object, Destination = _mockDestinationFolderInfo.Object };

            var mockFolderDiff = new Mock<IFolderDiff>();

            _mockDiffBuilder.Setup(m => m.Build(It.IsAny<ISyncPair>())).Returns(mockFolderDiff.Object);

            var actualDiff = _kontrol.BuildDiff();

            _mockDiffBuilder.Verify(m => m.Build(_settings.SyncPair));

            Assert.AreSame(mockFolderDiff.Object, actualDiff);
        }

        #endregion

        #region ProcessFolderDiff

        [TestMethod]
        public void ProcessFolderDiff()
        {
            var mockFolderDiff = new Mock<IFolderDiff>();

            _kontrol.ProcessFolderDiff(mockFolderDiff.Object);

            _mockFolderDiffProcessor.Verify(m => m.Process(mockFolderDiff.Object));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowArgumentNullExceptionIfFolderDiffIsNull()
        {
            _kontrol.ProcessFolderDiff(null);
        }

        #endregion

        #region Settings

        [TestMethod]
        public void WriteSettings()
        {
            _kontrol.WriteSettings(_mockFileInfo.Object);

            _mockSettingsManager.Verify(m => m.Write(_mockFileInfo.Object));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
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
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidateFileInfoOnReadSettings()
        {
            _kontrol.ReadSettings(null);
        }

        [TestMethod]
        public void CreateNewInstanceOfSettingsIfFileDoesNotExistOnReadSettings()
        {
            ISettingsManager<ISettings> settingsManager = new SettingsManager<ISettings>(new BinarySerializer<ISettings>());
            
            _kontrol = new Kontrol(settingsManager, _mockDiffBuilder.Object, _mockFolderDiffProcessor.Object);

            _mockFileInfo.Setup(m => m.Exists).Returns(false);

            _kontrol.ReadSettings(_mockFileInfo.Object);

            _mockSettingsManager.Verify(m => m.Read(It.IsAny<IKoreFileInfo>()), Times.Never);

            Assert.IsNotNull(settingsManager.Data);
        }

        #endregion
    }
}