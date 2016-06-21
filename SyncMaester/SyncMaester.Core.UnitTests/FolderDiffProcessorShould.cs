using System;
using System.Collections.Generic;
using Kore.IO.Sync;
using Kore.IO.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SyncMaester.Core.UnitTests
{
    [TestClass]
    public class FolderDiffProcessorShould
    {
        private FolderDiffProcessor _folderDiffProcessor;
        Mock<IDiffProcessor> _mockDiffProcessor;
        Mock<IFolderDiff> _mockFolderDiff;
        private Mock<IFolderDiffResult> _mockFolderDiffResult;
        Mock<IDiffInfoBuilder> _mockDiffInfoBuilder;

        [TestInitialize]
        public void Setup()
        {
            _mockDiffInfoBuilder = new Mock<IDiffInfoBuilder>();

            _mockDiffProcessor = new Mock<IDiffProcessor>();
            _folderDiffProcessor = new FolderDiffProcessor(_mockDiffProcessor.Object, _mockDiffInfoBuilder.Object);

            _mockFolderDiff = new Mock<IFolderDiff>();
            _mockFolderDiffResult = new Mock<IFolderDiffResult>();

            _mockFolderDiffResult.Setup(m => m.FolderDiff).Returns(new Mock<IFolderDiff>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidatefDiffProcessorOnInit()
        {
            _folderDiffProcessor = new FolderDiffProcessor(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidatefDiffInfoBuilderOnInit()
        {
            _folderDiffProcessor = new FolderDiffProcessor(_mockDiffProcessor.Object, null);
        }

        #region Process

        [TestMethod]
        public void CallsDiffInfoBuilder()
        {
            _mockDiffInfoBuilder.Setup(m => m.BuildInfo(It.IsAny<IFolderDiffResult>()));

            _mockFolderDiffResult.Setup(m => m.SyncPair).Returns(new Mock<ISyncPair>().Object);

            _mockFolderDiff.Setup(m => m.Diffs).Returns(new List<IDiff>());

            _mockFolderDiffResult.Setup(m => m.FolderDiff).Returns(_mockFolderDiff.Object);

            _folderDiffProcessor.Process(_mockFolderDiffResult.Object);

            _mockDiffInfoBuilder.Verify(m => m.BuildInfo(_mockFolderDiffResult.Object));
        }

        [TestMethod]
        public void CallProcessorOnEachElement()
        {
            var diffs = new List<IDiff>
            {
                new Mock<IDiff>().Object,
                new Mock<IDiff>().Object,
                new Mock<IDiff>().Object
            };

            var index = 0;
            var mockDiffInfo = new Mock<IDiffInfo>();

            _mockDiffInfoBuilder.Setup(m => m.BuildInfo(It.IsAny<IFolderDiffResult>())).Returns(mockDiffInfo.Object);

            var mockFolderDiffResult = new Mock<IFolderDiffResult>();

            mockFolderDiffResult.Setup(m => m.FolderDiff).Returns(_mockFolderDiff.Object);
            mockFolderDiffResult.Setup(m => m.SyncPair).Returns(new Mock<ISyncPair>().Object);

            _mockDiffProcessor.Setup(m => m.Process(It.IsAny<IDiff>(), It.IsAny<IDiffInfo>()))
                .Callback((IDiff x, IDiffInfo y) =>
            {
                Assert.AreSame(diffs[index++], x);
            });

            _mockFolderDiff.Setup(m => m.Diffs).Returns(diffs);

            _folderDiffProcessor.Process(mockFolderDiffResult.Object);

            _mockDiffProcessor.Verify(m => m.Process(It.IsAny<IDiff>(), mockDiffInfo.Object), Times.Exactly(3));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidatesFolderDiffResultOnProcess()
        {
            _folderDiffProcessor.Process(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidatesFolderDiffOnProcess()
        {
            _folderDiffProcessor.Process(_mockFolderDiffResult.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidatesFolderDiffDiffsOnProcess()
        {
            _mockFolderDiffResult.Setup(m => m.FolderDiff).Returns(new Mock<IFolderDiff>().Object);

            _folderDiffProcessor.Process(_mockFolderDiffResult.Object);
        }

        #endregion
    }
}
