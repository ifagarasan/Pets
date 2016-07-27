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
    public class SyncManagerShould
    {
        ISyncManager _syncManager;
        Mock<IDiffBuilder> _mockDiffBuilder;
        List<ISyncPair> _syncPairs;
        Mock<ISettings> _mockSettings;
        Mock<IFolderDiffProcessor> _mockFolderDiffProcessor;
        private Mock<ISyncInfo> _mockScanInfo;
        Mock<IKoreFileInfo> _mockFileInfo;

        [TestInitialize]
        public void Setup()
        {
            _mockFileInfo = new Mock<IKoreFileInfo>();

            _mockScanInfo = new Mock<ISyncInfo>();

            _mockFolderDiffProcessor = new Mock<IFolderDiffProcessor>();
            _mockFolderDiffProcessor.Setup(m => m.Process(It.IsAny<IFolderDiffResult>()));

            _syncPairs = new List<ISyncPair>
            {
                new Mock<ISyncPair>().Object,
                new Mock<ISyncPair>().Object,
                new Mock<ISyncPair>().Object
            };

            _mockSettings = new Mock<ISettings>();
            _mockSettings.Setup(m => m.SyncPairs).Returns(_syncPairs);

            _mockDiffBuilder = new Mock<IDiffBuilder>();
            _mockDiffBuilder.Setup(m => m.Build(It.IsAny<ISyncPair>()));

            _syncManager = new SyncManager(_mockDiffBuilder.Object, _mockFolderDiffProcessor.Object, _mockScanInfo.Object);
        }

        #region Init

        [TestMethod]
        [ExpectedException(typeof(NullException))]
        public void ValidateDiffBuilderOnInit()
        {
            _syncManager = new SyncManager(null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullException))]
        public void ValidateFolderDiffProcessorOnInit()
        {
            _syncManager = new SyncManager(_mockDiffBuilder.Object, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullException))]
        public void ValidateScanInfoOnInit()
        {
            _syncManager = new SyncManager(_mockDiffBuilder.Object, _mockFolderDiffProcessor.Object, null);
        }

        #endregion

        #region Sync

        [TestMethod]
        [ExpectedException(typeof(NullException))]
        public void ValidateSettingsOnSync()
        {
            _syncManager.Sync(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullException))]
        public void ValidateSyncPairsnSync()
        {
            _syncManager.Sync(new Mock<ISettings>().Object);
        }

        [TestMethod]
        public void CallsScanInfoSourceFileFound()
        {
            _mockDiffBuilder.Setup(m => m.Build(It.IsAny<ISyncPair>())).Returns(new Mock<IFolderDiff>().Object)
                .Raises(m => m.SourceFileFound += null, _mockFileInfo.Object);

            _syncManager.Sync(_mockSettings.Object);

            _mockScanInfo.Verify(m => m.NewSourceFileFound(_mockFileInfo.Object));
        }

        [TestMethod]
        public void ClearsScanInfoOnSync()
        {
            _syncManager.Sync(_mockSettings.Object);

            _mockScanInfo.Verify(m => m.Clear());
        }

        [TestMethod]
        public void CallsBuildDiffOnSync()
        {
            var index = 0;
            var mockFolderDiff = new Mock<IFolderDiff>();

            _mockDiffBuilder.Setup(m => m.Build(It.IsAny<ISyncPair>())).Callback<ISyncPair>(p => Assert.AreSame(_syncPairs[index++], p))
                .Returns(mockFolderDiff.Object);

            _syncManager.Sync(_mockSettings.Object);

            _mockDiffBuilder.Verify(m => m.Build(It.IsAny<ISyncPair>()), Times.Exactly(_syncPairs.Count));
        }

        [TestMethod]
        public void CallDIffProcessorOnSync()
        {
            _syncManager.Sync(_mockSettings.Object);

            _mockFolderDiffProcessor.Verify(m => m.Process(It.IsAny<IFolderDiffResult>()), Times.Exactly(_syncPairs.Count));
        }

        #endregion
    }
}