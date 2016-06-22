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
        DiffProcessor _diffProcessor;
        Mock<IDiff> _mockDiff;

        private Mock<IKoreFileInfo> _mockSourceFileInfo;
        Mock<IKoreFolderInfo> _mockSourceFolderInfo;

        Mock<IKoreFolderInfo> _mockDestinationFolderInfo;
        Mock<IKoreFileInfo> _mockDestinationFileInfo;

        Mock<IDiffInfo> _mockDiffInfo;

        readonly string _sourceTopFolder = "D:\\stuff\\original";
        readonly string _fileName = "file1.txt";
        readonly string _destinationTopFolder = "C:\\bak";

        [TestInitialize]
        public void Setup()
        {
            _diffProcessor = new DiffProcessor();

            _mockDiff = new Mock<IDiff>();

            _mockSourceFileInfo = new Mock<IKoreFileInfo>();
            _mockSourceFileInfo.Setup(m => m.Copy(It.IsAny<IKoreIoNodeInfo>()));

            _mockSourceFolderInfo = new Mock<IKoreFolderInfo>();

            _mockDestinationFolderInfo = new Mock<IKoreFolderInfo>();
            _mockDestinationFileInfo = new Mock<IKoreFileInfo>();

            _mockSourceFolderInfo.Setup(m => m.FullName).Returns(_sourceTopFolder);
            _mockDestinationFolderInfo.Setup(m => m.FullName).Returns(_destinationTopFolder);

            _mockDiff.Setup(m => m.SourceFileInfo).Returns(_mockSourceFileInfo.Object);
            _mockDiff.Setup(m => m.DestinationFileInfo).Returns(_mockDestinationFileInfo.Object);

            _mockDiffInfo = new Mock<IDiffInfo>();
            _mockDiffInfo.Setup(m => m.Source).Returns(_mockSourceFolderInfo.Object);
            _mockDiffInfo.Setup(m => m.Destination).Returns(_mockDestinationFolderInfo.Object);
        }

        [TestMethod]
        public void CopySourceToDestinationOnSourceNew()
        {
            var sourceFile = Path.Combine(_sourceTopFolder, _fileName);
            var destinationFile = Path.Combine(_destinationTopFolder, _fileName);

            _mockSourceFileInfo.Setup(m => m.FullName).Returns(sourceFile);
            _mockDestinationFileInfo.Setup(m => m.FullName).Returns(destinationFile);

            _mockSourceFileInfo.Setup(m => m.Copy(It.IsAny<IKoreIoNodeInfo>()))
                .Callback((IKoreIoNodeInfo fileInfo) =>
                {
                    Assert.AreEqual(destinationFile, fileInfo.FullName);
                });

            _mockDiff.Setup(m => m.Type).Returns(DiffType.SourceNew);

            _diffProcessor.Process(_mockDiff.Object, _mockSourceFolderInfo.Object, _mockDestinationFolderInfo.Object);

            _mockSourceFileInfo.Verify(m => m.Copy(It.IsAny<IKoreIoNodeInfo>()), Times.Exactly(1));
        }

        [TestMethod]
        public void CopySourceToDestinationOnSourceNewer()
        {
            var sourceFile = Path.Combine(_sourceTopFolder, _fileName);

            _mockSourceFileInfo.Setup(m => m.FullName).Returns(sourceFile);            

            var destinationFile = Path.Combine(_destinationTopFolder, _fileName);
            
            _mockDestinationFileInfo.Setup(m => m.FullName).Returns(destinationFile);

            _mockDiff.Setup(m => m.Type).Returns(DiffType.SourceNewer);

            _diffProcessor.Process(_mockDiff.Object, _mockSourceFolderInfo.Object, _mockDestinationFolderInfo.Object);

            _mockSourceFileInfo.Verify(m => m.Copy(_mockDestinationFileInfo.Object));
        }

        [TestMethod]
        public void CopyDestinationToSourceOnSourceOlder()
        {
            var sourceFile = Path.Combine(_sourceTopFolder, _fileName);

            _mockSourceFileInfo.Setup(m => m.FullName).Returns(sourceFile);

            var destinationFile = Path.Combine(_destinationTopFolder, _fileName);

            _mockDestinationFileInfo.Setup(m => m.FullName).Returns(destinationFile);

            _mockDiff.Setup(m => m.Type).Returns(DiffType.SourceOlder);

            _diffProcessor.Process(_mockDiff.Object, _mockSourceFolderInfo.Object, _mockDestinationFolderInfo.Object);

            _mockDestinationFileInfo.Verify(m => m.Copy(_mockSourceFileInfo.Object));
        }

        [TestMethod]
        public void DeleteDestinationOnDestinationOrphan()
        {
            var destinationFile = Path.Combine(_destinationTopFolder, _fileName);

            _mockDestinationFileInfo.Setup(m => m.FullName).Returns(destinationFile);
            _mockDestinationFolderInfo.Setup(m => m.Delete());

            _mockDiff.Setup(m => m.Type).Returns(DiffType.DestinationOrphan);

            _diffProcessor.Process(_mockDiff.Object, _mockSourceFolderInfo.Object, _mockDestinationFolderInfo.Object);

            _mockDestinationFileInfo.Verify(m => m.Delete());
        }

        [TestMethod]
        public void DoNothingOnIdentical()
        {
            var sourceFile = Path.Combine(_sourceTopFolder, _fileName);
            var destinationFile = Path.Combine(_destinationTopFolder, _fileName);

            _mockSourceFileInfo.Setup(m => m.FullName).Returns(sourceFile);
            _mockSourceFileInfo.Setup(m => m.Delete());

            _mockDestinationFileInfo.Setup(m => m.FullName).Returns(destinationFile);
            _mockDestinationFileInfo.Setup(m => m.Delete());

            _mockDiff.Setup(m => m.Type).Returns(DiffType.Identical);

            _diffProcessor.Process(_mockDiff.Object, _mockSourceFolderInfo.Object, _mockDestinationFolderInfo.Object);

            _mockSourceFileInfo.Verify(m => m.Copy(It.IsAny<IKoreFileInfo>()), Times.Never);
            _mockDestinationFileInfo.Verify(m => m.Copy(It.IsAny<IKoreFileInfo>()), Times.Never);
            _mockSourceFileInfo.Verify(m => m.Delete(), Times.Never);
            _mockDestinationFileInfo.Verify(m => m.Delete(), Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidatesDiffOnProcess()
        {
            _diffProcessor.Process(null, _mockSourceFolderInfo.Object, _mockDestinationFolderInfo.Object);
        }
    }
}
