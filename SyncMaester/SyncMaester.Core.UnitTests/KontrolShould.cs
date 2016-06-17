using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kore.IO.Sync;
using Kore.IO.Util;
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

        Mock<IKoreFolderInfo> _mockSourceFolderInfo;
        Mock<IKoreFolderInfo> _mockDestinationFolderInfo;

        readonly string _sourceFolder = "abc";
        readonly string _destinationFolder = "efg";

        [TestInitialize]
        public void Setup()
        {
            _mockSourceFolderInfo = new Mock<IKoreFolderInfo>();
            _mockSourceFolderInfo.Setup(m => m.FullName).Returns(_sourceFolder);

            _mockDestinationFolderInfo = new Mock<IKoreFolderInfo>();
            _mockSourceFolderInfo.Setup(m => m.FullName).Returns(_destinationFolder);

            _settings = new Settings();
            _mockDiffBuilder = new Mock<IDiffBuilder>();
            _mockFolderDiffProcessor = new Mock<IFolderDiffProcessor>();
            _kontrol = new Kontrol(_settings, _mockDiffBuilder.Object, _mockFolderDiffProcessor.Object);
        }

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

        [TestMethod]
        public void CallBuildDiffOnDiffBuilder()
        {
            _settings.SyncPair = new SyncPair { Source = _mockSourceFolderInfo.Object, Destination = _mockDestinationFolderInfo.Object };

            Mock<IFolderDiff> mockFolderDiff = new Mock<IFolderDiff>();

            _mockDiffBuilder.Setup(m => m.Build(It.IsAny<ISyncPair>())).Returns(mockFolderDiff.Object);

            IFolderDiff actualDiff = _kontrol.BuildDiff();

            _mockDiffBuilder.Verify(m => m.Build(_settings.SyncPair));

            Assert.AreSame(mockFolderDiff.Object, actualDiff);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowArgumentNullExceptionIfSettingsNull()
        {
            new Kontrol(null, _mockDiffBuilder.Object, _mockFolderDiffProcessor.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowArgumentNullExceptionIfDiffBuilderNull()
        {
            new Kontrol(_settings, null, _mockFolderDiffProcessor.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowArgumentNullExceptionIfDiffProcessorNull()
        {
            new Kontrol(_settings, _mockDiffBuilder.Object, null);
        }

        #region ProcessFolderDiff

        [TestMethod]
        public void ProcessFolderDiff()
        {
            Mock<IFolderDiff> mockFolderDiff = new Mock<IFolderDiff>();

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
    }
}
