using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kore.IO.Scanners;
using Kore.IO.Sync;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SyncMaester.Core.UnitTests
{
    [TestClass]
    public class DiffBuilderShould
    {
        Mock<IFolderDiffer> _mockFolderDiffer;
        Mock<IFileScanner> _mockFileScanner;
        DiffBuilder _builder;

        [TestInitialize]
        public void Setup()
        {
            _mockFolderDiffer = new Mock<IFolderDiffer>();
            _mockFileScanner = new Mock<IFileScanner>();

            _builder = new DiffBuilder(_mockFileScanner.Object, _mockFolderDiffer.Object);
        }

        #region Init

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsArgumentNullExceptionIfFileScannerIsNull()
        {
            _builder = new DiffBuilder(null, _mockFolderDiffer.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsArgumentNullExceptionIfFolderDifferIsNull()
        {
            _builder = new DiffBuilder(_mockFileScanner.Object, null);
        }

        #endregion

        #region BuildDiff

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsArgumentNullExceptionIfISyncPairIsNull()
        {
            _builder.Build(null);
        }

        [TestMethod]
        public void ReturnedTheDiffList()
        {
            string source = "src";
            string destination = "dest";

            Mock<IFileScanResult> sourceScanResult = new Mock<IFileScanResult>();
            Mock<IFileScanResult> destinationScanResult = new Mock<IFileScanResult>();

            _mockFileScanner.Setup(m => m.Scan(source)).Returns(sourceScanResult.Object);
            _mockFileScanner.Setup(m => m.Scan(destination)).Returns(destinationScanResult.Object);

            Mock<ISyncPair> mockSyncPair = new Mock<ISyncPair>();

            mockSyncPair.Setup(m => m.Source).Returns(source);
            mockSyncPair.Setup(m => m.Destination).Returns(destination);

            Mock<IFolderDiff> mockFolderDIff = new Mock<IFolderDiff>();
            _mockFolderDiffer.Setup(m => m.BuildDiff(It.IsAny<IFileScanResult>(), It.IsAny<IFileScanResult>())).Returns(mockFolderDIff.Object);

            IFolderDiff diff =  _builder.Build(mockSyncPair.Object);

            _mockFileScanner.VerifyAll();
            _mockFolderDiffer.Verify(m => m.BuildDiff(sourceScanResult.Object, destinationScanResult.Object));

            Assert.AreSame(mockFolderDIff.Object, diff);
        }

        #endregion
    }
}
