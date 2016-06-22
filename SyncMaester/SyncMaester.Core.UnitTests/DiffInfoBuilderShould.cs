using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kore.IO.Scanners;
using Kore.IO.Sync;
using Kore.IO.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;

namespace SyncMaester.Core.UnitTests
{
    [TestClass]
    public class DiffInfoBuilderShould
    {
        string source = "D:\\source\\Music";
        string destination = "D:\\destination";

        Mock<ISyncPair> _mockSyncPair;
        IDiffInfoBuilder _diffInfoBuilder;
        Mock<IFolderDiff> _mockFolderDiff;
        Mock<IKoreFolderInfo> _mockSourceFolderInfo;
        Mock<IFolderDiffResult> _mockFolderDiffResult;
        Mock<IKoreFolderInfo> _mockDestinationFolderInfo;

        [TestInitialize]
        public void Setup()
        {
            _mockSourceFolderInfo = new Mock<IKoreFolderInfo>();
            _mockSourceFolderInfo.Setup(m => m.FullName).Returns(source);
            _mockSourceFolderInfo.Setup(m => m.Name).Returns("Music");

            _mockDestinationFolderInfo = new Mock<IKoreFolderInfo>();
            _mockDestinationFolderInfo.Setup(m => m.FullName).Returns(destination);

            _mockFolderDiff = new Mock<IFolderDiff>();
            _mockFolderDiff.Setup(m => m.Source).Returns(_mockSourceFolderInfo.Object);
            _mockFolderDiff.Setup(m => m.Destination).Returns(_mockDestinationFolderInfo.Object);

            _mockSyncPair = new Mock<ISyncPair>();

            _diffInfoBuilder = new DiffInfoBuilder();

            _mockFolderDiffResult = new Mock<IFolderDiffResult>();
            _mockFolderDiffResult.Setup(m => m.FolderDiff).Returns(_mockFolderDiff.Object);
            _mockFolderDiffResult.Setup(m => m.SyncPair).Returns(_mockSyncPair.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidatesFolderDiffResultOnBuild()
        {
            _diffInfoBuilder.BuildInfo(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidatesFolderDiffResultFolderDiffOnBuild()
        {
            _mockFolderDiffResult = new Mock<IFolderDiffResult>();

            _diffInfoBuilder.BuildInfo(_mockFolderDiffResult.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidatesFolderDiffResultSyncPairOnBuild()
        {
            _mockFolderDiffResult = new Mock<IFolderDiffResult>();

            _diffInfoBuilder.BuildInfo(_mockFolderDiffResult.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidatesFolderDiffResultFolderDiffSourceOnBuild()
        {
            _mockFolderDiff = new Mock<IFolderDiff>();

            _mockFolderDiffResult = new Mock<IFolderDiffResult>();
            _mockFolderDiffResult.Setup(m => m.FolderDiff).Returns(_mockFolderDiff.Object);

            _diffInfoBuilder.BuildInfo(_mockFolderDiffResult.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidatesFolderDiffResultFolderDiffDestinationOnBuild()
        {
            _mockFolderDiff = new Mock<IFolderDiff>();
            _mockFolderDiff.Setup(m => m.Source).Returns(_mockSourceFolderInfo.Object);

            _mockFolderDiffResult = new Mock<IFolderDiffResult>();
            _mockFolderDiffResult.Setup(m => m.FolderDiff).Returns(_mockFolderDiff.Object);

            _diffInfoBuilder.BuildInfo(_mockFolderDiffResult.Object);
        }

        [TestMethod]
        public void LeaveSourceAndDestinationUnalteredWhenLevelIsFlat()
        {
            _mockSyncPair.Setup(m => m.Level).Returns(SyncLevel.Flat);

            var diffInfo = _diffInfoBuilder.BuildInfo(_mockFolderDiffResult.Object);

            Assert.AreEqual(source, diffInfo.Source.FullName);
            Assert.AreEqual(destination, diffInfo.Destination.FullName);
        }

        [TestMethod]
        public void AddSourceParentDirectoryNameToDestinationUnalteredWhenLevelIsParent()
        {
            var expectedDestination = Path.Combine(_mockDestinationFolderInfo.Object.FullName, _mockSourceFolderInfo.Object.Name);

            _mockSyncPair.Setup(m => m.Level).Returns(SyncLevel.Parent);

            var diffInfo = _diffInfoBuilder.BuildInfo(_mockFolderDiffResult.Object);
            
            Assert.AreEqual(source, diffInfo.Source.FullName);
            Assert.AreEqual(expectedDestination, diffInfo.Destination.FullName);
        }
    }
}
