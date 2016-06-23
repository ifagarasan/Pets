﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        Mock<IDiffBuilder> _mockDiffBuilder;
        Mock<IFolderDiffProcessor> _mockFolderDiffProcessor;
        IKontrol _kontrol;
        Mock<ISettingsManager<ISettings>> _mockSettingsManager;

        readonly string _sourceFolder = "abc";
        readonly string _destinationFolder = "efg";

        Mock<ISyncPair> _mockSyncPair;

        Mock<IKoreFileInfo> _mockFileInfo;

        [TestInitialize]
        public void Setup()
        {
            _mockFileInfo = new Mock<IKoreFileInfo>();
            _mockFileInfo.Setup(m => m.Exists).Returns(true);

            _mockSyncPair = new Mock<ISyncPair>();
            _mockSyncPair.Setup(m => m.Source).Returns(_sourceFolder);
            _mockSyncPair.Setup(m => m.Destination).Returns(_destinationFolder);

            _settings = new Settings();

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

        #region BuildDiff

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidateSettingsSyncPairOnBuildDiff()
        {
            _settings.SyncPairs = null;

            _kontrol.BuildDiff();
        }

        [TestMethod]
        public void BuildDiff()
        {
            var syncPair1 = new SyncPair { Source = _sourceFolder, Destination = _destinationFolder };
            var syncPair2 = new SyncPair { Source = _sourceFolder, Destination = _destinationFolder };

            _settings.SyncPairs.Add(syncPair1);
            _settings.SyncPairs.Add(syncPair2);

            var mockFolderDiff1 = new Mock<IFolderDiff>();
            var mockFolderDiff2 = new Mock<IFolderDiff>();

            _mockDiffBuilder.Setup(m => m.Build(syncPair1)).Returns(mockFolderDiff1.Object);
            _mockDiffBuilder.Setup(m => m.Build(syncPair2)).Returns(mockFolderDiff2.Object);

            IDiffResult diffResult = _kontrol.BuildDiff();

            _mockDiffBuilder.Verify(m => m.Build(syncPair1));
            _mockDiffBuilder.Verify(m => m.Build(syncPair2));

            Assert.AreEqual(2, diffResult.FolderDiffResults.Count);

            Assert.AreSame(mockFolderDiff1.Object, diffResult.FolderDiffResults[0].FolderDiff);
            Assert.AreSame(syncPair1, diffResult.FolderDiffResults[0].SyncPair);

            Assert.AreSame(mockFolderDiff2.Object, diffResult.FolderDiffResults[1].FolderDiff);
            Assert.AreSame(syncPair2, diffResult.FolderDiffResults[1].SyncPair);
        }

        #endregion

        #region ProcessFolderDiff

        [TestMethod]
        public void ProcessFolderDiff()
        {
            var mockFolderDiff1 = new Mock<IFolderDiff>();
            var mockFolderDiff2 = new Mock<IFolderDiff>();

            var mockFolderDiffResult1 = new Mock<IFolderDiffResult>();
            mockFolderDiffResult1.Setup(m => m.FolderDiff).Returns(mockFolderDiff1.Object);

            var mockFolderDiffResult2 = new Mock<IFolderDiffResult>();
            mockFolderDiffResult2.Setup(m => m.FolderDiff).Returns(mockFolderDiff2.Object);

            var mockDiffResult = new Mock<IDiffResult>();

            mockDiffResult.Setup(m => m.FolderDiffResults).Returns(new List<IFolderDiffResult> { mockFolderDiffResult1.Object, mockFolderDiffResult2.Object });

            _kontrol.ProcessFolderDiff(mockDiffResult.Object);

            _mockFolderDiffProcessor.Verify(m => m.Process(mockFolderDiffResult1.Object), Times.Once);
            _mockFolderDiffProcessor.Verify(m => m.Process(mockFolderDiffResult2.Object), Times.Once);
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