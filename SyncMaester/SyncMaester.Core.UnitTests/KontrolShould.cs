using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kore.IO.Sync;
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

        [TestInitialize]
        public void Setup()
        {
            _settings = new Settings();
            _mockDiffBuilder = new Mock<IDiffBuilder>();
            _mockFolderDiffProcessor = new Mock<IFolderDiffProcessor>();
            _kontrol = new Kontrol(_settings, _mockDiffBuilder.Object, _mockFolderDiffProcessor.Object);
        }

        #region AddSyncPair

        [TestMethod]
        public void SetSyncPairToProvided()
        {
            ISyncPair syncPair = new SyncPair {Source = "abc", Destination = "efg"};

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
            _settings.SyncPair = new SyncPair { Source = "abc", Destination = "efg" };

            IFolderDiff expectedDiff = new FolderDiff(new List<IDiff>());

            _mockDiffBuilder.Setup(m => m.Build(It.IsAny<ISyncPair>())).Returns(expectedDiff);

            IFolderDiff actualDiff = _kontrol.BuildDiff();

            _mockDiffBuilder.Verify(m => m.Build(_settings.SyncPair));

            Assert.AreSame(expectedDiff, actualDiff);
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
