using System;
using Kore.IO;
using Kore.IO.Scanners;
using Kore.IO.Sync;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Kore.IO.Exceptions;

namespace SyncMaester.Core.UnitTests
{
    [TestClass]
    public class DiffBuilderShould
    {
        readonly string source = "src";
        readonly string destination = "dest";

        Mock<IFolderDiffer> _mockFolderDiffer;
        Mock<IFileScanner> _mockFileScanner;
        Mock<IDiffInfoBuilder> _mockDiffInfoBuilder;
        DiffBuilder _builder;
        Mock<ISyncPair> _mockSyncPair;
        Mock<IDiffInfo> _mockDiffInfo;
        Mock<IKoreFolderInfo> _mockSourceFolderInfo;
        Mock<IKoreFolderInfo> _mockDestinationFolderInfo;

        [TestInitialize]
        public void Setup()
        {
            _mockSyncPair = new Mock<ISyncPair>();

            _mockSyncPair.Setup(m => m.Source).Returns(source);
            _mockSyncPair.Setup(m => m.Destination).Returns(destination);

            _mockSourceFolderInfo = new Mock<IKoreFolderInfo>();
            _mockSourceFolderInfo.Setup(m => m.FullName).Returns(source);
            _mockSourceFolderInfo.Setup(m => m.Exists).Returns(true);

            _mockDestinationFolderInfo = new Mock<IKoreFolderInfo>();
            _mockDestinationFolderInfo.Setup(m => m.FullName).Returns(destination);
            _mockDestinationFolderInfo.Setup(m => m.EnsureExists());

            _mockDiffInfo = new Mock<IDiffInfo>();
            _mockDiffInfo.Setup(m => m.Source).Returns(_mockSourceFolderInfo.Object);
            _mockDiffInfo.Setup(m => m.Destination).Returns(_mockDestinationFolderInfo.Object);

            _mockDiffInfoBuilder = new Mock<IDiffInfoBuilder>();
            _mockDiffInfoBuilder.Setup(m => m.BuildInfo(It.IsAny<ISyncPair>()));
            _mockDiffInfoBuilder.Setup(m => m.BuildInfo(It.IsAny<ISyncPair>())).Returns(_mockDiffInfo.Object);

            _mockFolderDiffer = new Mock<IFolderDiffer>();

            _mockFileScanner = new Mock<IFileScanner>();
            _mockFileScanner.Setup(m => m.Scan(It.IsAny<IKoreFolderInfo>()));

            _builder = new DiffBuilder(_mockDiffInfoBuilder.Object, _mockFileScanner.Object, _mockFolderDiffer.Object);
        }

        #region Init

        //TODO: validity of diff info builder

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsArgumentNullExceptionIfFileScannerIsNull()
        {
            _builder = new DiffBuilder(_mockDiffInfoBuilder.Object, null, _mockFolderDiffer.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsArgumentNullExceptionIfFolderDifferIsNull()
        {
            _builder = new DiffBuilder(_mockDiffInfoBuilder.Object, _mockFileScanner.Object, null);
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
        public void CallsDiffInfoBuilder()
        {
            _builder.Build(_mockSyncPair.Object);

            _mockDiffInfoBuilder.Verify(m => m.BuildInfo(_mockSyncPair.Object));

        }

        [TestMethod]
        public void CreatesDestinationFolderIfNotExists()
        {
            _builder.Build(_mockSyncPair.Object);

            _mockDestinationFolderInfo.Verify(m => m.EnsureExists());
        }

        [TestMethod]
        [ExpectedException(typeof(NodeNotFoundException))]
        public void ValidatesSourceFolderExists()
        {
            _mockSourceFolderInfo.Setup(m => m.Exists).Returns(false);

            _builder.Build(_mockSyncPair.Object);
        }

        [TestMethod]
        public void BuildDiffInfoOnBuild()
        {
            _builder.Build(_mockSyncPair.Object);

            _mockDiffInfoBuilder.Verify(m => m.BuildInfo(_mockSyncPair.Object));
        }

        [TestMethod]
        public void ScanSourceOnBuild()
        {
            _builder.Build(_mockSyncPair.Object);

            _mockDiffInfoBuilder.Verify(m => m.BuildInfo(_mockSyncPair.Object));

            _mockFileScanner.Verify(m => m.Scan(_mockDiffInfo.Object.Source));
        }

        [TestMethod]
        public void ScanDestinationOnBuild()
        {
            _builder.Build(_mockSyncPair.Object);

            _mockDiffInfoBuilder.Verify(m => m.BuildInfo(_mockSyncPair.Object));

            _mockFileScanner.Verify(m => m.Scan(_mockDiffInfo.Object.Destination));
        }

        //[TestMethod]
        //public void ReturnedTheDiffList()
        //{
        //    var sourceScanResult = new Mock<IFileScanResult>();
        //    var destinationScanResult = new Mock<IFileScanResult>();

        //    var mockSourceFolderInfo = new Mock<IKoreFolderInfo>();
        //    mockSourceFolderInfo.Setup(m => m.FullName).Returns(source);

        //    var mockDestinationFolderInfo = new Mock<IKoreFolderInfo>();
        //    mockDestinationFolderInfo.Setup(m => m.FullName).Returns(destination);

        //    var mockDiffInfo = new Mock<IDiffInfo>();
        //    mockDiffInfo.Setup(m => m.Source).Returns(mockSourceFolderInfo.Object);
        //    mockDiffInfo.Setup(m => m.Destination).Returns(mockDestinationFolderInfo.Object);

        //    var mockDiffInfoBuilder = new Mock<IDiffInfoBuilder>();
        //    mockDiffInfoBuilder.Setup(m => m.BuildInfo(It.IsAny<ISyncPair>())).Returns(mockDiffInfo.Object);

        //    _mockFileScanner.Setup(m => m.Scan(source)).Returns(sourceScanResult.Object);
        //    _mockFileScanner.Setup(m => m.Scan(destination)).Returns(destinationScanResult.Object);

        //    var mockSyncPair = new Mock<ISyncPair>();

        //    mockSyncPair.Setup(m => m.Source).Returns(source);
        //    mockSyncPair.Setup(m => m.Destination).Returns(destination);

        //    var mockFolderDIff = new Mock<IFolderDiff>();
        //    _mockFolderDiffer.Setup(m => m.BuildDiff(It.IsAny<IFileScanResult>(), It.IsAny<IFileScanResult>())).Returns(mockFolderDIff.Object);

        //    var diff =  _builder.Build(mockSyncPair.Object);

        //    _mockFileScanner.VerifyAll();
        //    _mockFolderDiffer.Verify(m => m.BuildDiff(sourceScanResult.Object, destinationScanResult.Object));

        //    Assert.AreSame(mockFolderDIff.Object, diff);
        //}

        #endregion
    }
}
