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

        Mock<ISyncPair> mockSyncPair;
        IDiffInfoBuilder diffInfoBuilder;
        Mock<IFolderDiff> mockFolderDiff;
        Mock<IKoreFolderInfo> mockSourceFolderInfo;
        Mock<IFolderDiffResult> mockFolderDiffResult;
        Mock<IKoreFolderInfo> mockDestinationFolderInfo;

        [TestInitialize]
        public void Setup()
        {
            mockSourceFolderInfo = new Mock<IKoreFolderInfo>();
            mockSourceFolderInfo.Setup(m => m.FullName).Returns(source);
            mockSourceFolderInfo.Setup(m => m.Name).Returns("Music");

            mockDestinationFolderInfo = new Mock<IKoreFolderInfo>();
            mockDestinationFolderInfo.Setup(m => m.FullName).Returns(destination);

            mockFolderDiff = new Mock<IFolderDiff>();
            mockFolderDiff.Setup(m => m.Source).Returns(mockSourceFolderInfo.Object);
            mockFolderDiff.Setup(m => m.Destination).Returns(mockDestinationFolderInfo.Object);

            mockSyncPair = new Mock<ISyncPair>();

            diffInfoBuilder = new DiffInfoBuilder();

            mockFolderDiffResult = new Mock<IFolderDiffResult>();
            mockFolderDiffResult.Setup(m => m.FolderDiff).Returns(mockFolderDiff.Object);
            mockFolderDiffResult.Setup(m => m.SyncPair).Returns(mockSyncPair.Object);
        }

        [TestMethod]
        public void LeaveSourceAndDestinationUnalteredWhenLevelIsFlat()
        {
            mockSyncPair.Setup(m => m.Level).Returns(SyncLevel.Flat);

            var diffInfo = diffInfoBuilder.BuildInfo(mockFolderDiffResult.Object);

            Assert.AreEqual(source, diffInfo.Source.FullName);
            Assert.AreEqual(destination, diffInfo.Destination.FullName);
        }

        [TestMethod]
        public void AddSourceParentDirectoryNameToDestinationUnalteredWhenLevelIsParent()
        {
            var expectedDestination = Path.Combine(mockDestinationFolderInfo.Object.FullName, mockSourceFolderInfo.Object.Name);

            mockSyncPair.Setup(m => m.Level).Returns(SyncLevel.Parent);

            var diffInfo = diffInfoBuilder.BuildInfo(mockFolderDiffResult.Object);
            
            Assert.AreEqual(source, diffInfo.Source.FullName);
            Assert.AreEqual(expectedDestination, diffInfo.Destination.FullName);
        }
    }
}
