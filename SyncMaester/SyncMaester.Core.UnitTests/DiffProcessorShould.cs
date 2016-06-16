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

namespace SyncMaester.Core.UnitTests
{
    [TestClass]
    public class DiffProcessorShould
    {
        [TestMethod]
        public void CopySourceToDestinationIfSourceIsNewer()
        {
            IDiffProcessor diffProcessor = new DiffProcessor();

            Mock<IDiff> mockDiff = new Mock<IDiff>();

            Mock<IKoreFileInfo> mockSourceFileInfo = new Mock<IKoreFileInfo>();
            mockSourceFileInfo.Setup(m => m.Copy(It.IsAny<IKoreFileInfo>()));

            Mock<IKoreFileInfo> mockDestinationFileInfo = new Mock<IKoreFileInfo>();

            mockDiff.Setup(m => m.Type).Returns(DiffType.SourceNewer);
            mockDiff.Setup(m => m.SourceFileInfo).Returns(mockSourceFileInfo.Object);
            mockDiff.Setup(m => m.DestinationFileInfo).Returns(mockDestinationFileInfo.Object);

            diffProcessor.Process(mockDiff.Object);

            mockSourceFileInfo.Verify(m => m.Copy(mockDestinationFileInfo.Object));
        }
    }
}
