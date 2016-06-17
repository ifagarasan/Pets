using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kore.IO.Scanners;
using Kore.IO.Sync;
using Kore.IO.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SyncMaester.Core.UnitTests
{
    [TestClass]
    public class DiffProcessorShould
    {
        [TestMethod]
        public void CopySourceToDestinationOnSourceNew()
        {
            var diffProcessor = new DiffProcessor();

            var mockDiff = new Mock<IDiff>();

            var sourceTopFolder = "D:\\stuff\\original";
            var fileName = "file1.txt";
            var sourceFile = Path.Combine(sourceTopFolder, fileName);

            var mockSourceFileInfo = new Mock<IKoreFileInfo>();
            mockSourceFileInfo.Setup(m => m.FullName).Returns(sourceFile);

            var mockSourceFolderInfo = new Mock<IKoreFolderInfo>();
            mockSourceFolderInfo.Setup(m => m.FullName).Returns(sourceTopFolder);

            var destinationTopFolder = "C:\\bak";
            var mockDestinationFolderInfo = new Mock<IKoreFolderInfo>();
            mockDestinationFolderInfo.Setup(m => m.FullName).Returns(destinationTopFolder);

            var destinationFile = Path.Combine(destinationTopFolder, fileName);
            var mockDestinationFileInfo = new Mock<IKoreFileInfo>();
            mockDestinationFileInfo.Setup(m => m.FullName).Returns(destinationFile);

            mockSourceFileInfo.Setup(m => m.Copy(It.IsAny<IKoreIoNodeInfo>()))
                .Callback((IKoreIoNodeInfo fileInfo) =>
                {
                    Assert.AreEqual(destinationFile, fileInfo.FullName);
                });

            mockDiff.Setup(m => m.Type).Returns(DiffType.SourceNew);
            mockDiff.Setup(m => m.SourceFileInfo).Returns(mockSourceFileInfo.Object);
            mockDiff.Setup(m => m.DestinationFileInfo).Returns(mockDestinationFileInfo.Object);

            diffProcessor.Process(mockDiff.Object, mockSourceFolderInfo.Object, mockDestinationFolderInfo.Object);

            mockSourceFileInfo.Verify(m => m.Copy(It.IsAny<IKoreIoNodeInfo>()), Times.Exactly(1));
        }
    }
}
