using System;
using System.IO;
using Kore.IO;
using Kore.IO.Management;
using Kore.IO.Sync;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SyncMaester.Core.UnitTests
{
    [TestClass]
    public class DiffProcessorShould
    {
        DiffProcessor _diffProcessor;
        Mock<IDiff> _mockDiff;

        private Mock<IKoreFileInfo> _mockSourceFileInfo;
        Mock<IKoreFolderInfo> _mockSourceFolderInfo;

        Mock<IKoreFolderInfo> _mockDestinationFolderInfo;
        Mock<IKoreFileInfo> _mockDestinationFileInfo;

        Mock<IFileCopier> _mockFileCopier;
        Mock<IDiffInfo> _mockDiffInfo;

        readonly string _sourceTopFolder = "D:\\stuff\\original";
        readonly string _fileName = "file1.txt";
        readonly string _destinationTopFolder = "C:\\bak";

        [TestInitialize]
        public void Setup()
        {
            _mockFileCopier = new Mock<IFileCopier>();

            _diffProcessor = new DiffProcessor(_mockFileCopier.Object);

            _mockSourceFolderInfo = new Mock<IKoreFolderInfo>();
            _mockSourceFolderInfo.Setup(m => m.FullName).Returns(_sourceTopFolder);

            _mockSourceFileInfo = new Mock<IKoreFileInfo>();
            _mockSourceFileInfo.Setup(m => m.FullName).Returns(Path.Combine(_sourceTopFolder, _fileName));

            _mockDestinationFolderInfo = new Mock<IKoreFolderInfo>();
            _mockDestinationFolderInfo.Setup(m => m.FullName).Returns(_destinationTopFolder);

            _mockDestinationFileInfo = new Mock<IKoreFileInfo>();
            _mockDestinationFileInfo.Setup(m => m.FullName).Returns(Path.Combine(_destinationTopFolder, _fileName));

            _mockDiff = new Mock<IDiff>();
            _mockDiff.Setup(m => m.SourceFileInfo).Returns(_mockSourceFileInfo.Object);
            _mockDiff.Setup(m => m.DestinationFileInfo).Returns(_mockDestinationFileInfo.Object);

            _mockDiffInfo = new Mock<IDiffInfo>();
            _mockDiffInfo.Setup(m => m.Source).Returns(_mockSourceFolderInfo.Object);
            _mockDiffInfo.Setup(m => m.Destination).Returns(_mockDestinationFolderInfo.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidateFileCopierOnInit()
        {
            _diffProcessor = new DiffProcessor(null);
        }

        [TestMethod]
        public void CopySourceToDestinationOnSourceNew()
        {
            _mockDiff.Setup(m => m.Type).Returns(DiffType.SourceNew);

            TestFileWasCopied();
        }

        [TestMethod]
        public void CopySourceToDestinationOnSourceNewer()
        {
            _mockDiff.Setup(m => m.Type).Returns(DiffType.SourceNewer);

            TestFileWasCopied();
        }

        [TestMethod]
        public void CopyDestinationToSourceOnSourceOlder()
        {
            _mockDiff.Setup(m => m.Type).Returns(DiffType.SourceOlder);

            _diffProcessor.Process(_mockDiff.Object, _mockSourceFolderInfo.Object, _mockDestinationFolderInfo.Object);

            _mockFileCopier.Setup(m => m.Copy(_mockDestinationFileInfo.Object, _mockSourceFileInfo.Object));
        }

        [TestMethod]
        public void DeleteDestinationOnDestinationOrphan()
        {
            _mockDiff.Setup(m => m.Type).Returns(DiffType.DestinationOrphan);

            _diffProcessor.Process(_mockDiff.Object, _mockSourceFolderInfo.Object, _mockDestinationFolderInfo.Object);

            _mockDestinationFileInfo.Verify(m => m.Delete());

            _mockFileCopier.Verify(m => m.Copy(It.IsAny<IKoreFileInfo>(), It.IsAny<IKoreFileInfo>()), Times.Never);
        }

        [TestMethod]
        public void NotCopyOnIdentical()
        {
            _mockDiff.Setup(m => m.Type).Returns(DiffType.Identical);

            _diffProcessor.Process(_mockDiff.Object, _mockSourceFolderInfo.Object, _mockDestinationFolderInfo.Object);

            _mockFileCopier.Verify(m => m.Copy(It.IsAny<IKoreFileInfo>(), It.IsAny<IKoreFileInfo>()), Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidatesDiffOnProcess()
        {
            _diffProcessor.Process(null, _mockSourceFolderInfo.Object, _mockDestinationFolderInfo.Object);
        }

        private void TestFileWasCopied()
        {
            _mockFileCopier.Setup(m => m.Copy(It.IsAny<IKoreFileInfo>(), It.IsAny<IKoreFileInfo>())).Callback(
                            (IKoreFileInfo src, IKoreFileInfo dest) =>
                            {
                                Assert.AreEqual(dest.FullName, Path.Combine(_mockDestinationFolderInfo.Object.FullName, _fileName));
                            });

            _diffProcessor.Process(_mockDiff.Object, _mockSourceFolderInfo.Object, _mockDestinationFolderInfo.Object);

            _mockFileCopier.Verify(m => m.Copy(_mockSourceFileInfo.Object, It.IsAny<IKoreFileInfo>()));
        }
    }
}
