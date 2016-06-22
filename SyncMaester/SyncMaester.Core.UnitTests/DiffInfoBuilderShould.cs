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
        readonly string source = "D:\\source\\Music";
        readonly string destination = "D:\\destination";

        Mock<ISyncPair> _mockSyncPair;
        private IDiffInfoBuilder _diffInfoBuilder;

        [TestInitialize]
        public void Setup()
        {
            _mockSyncPair = new Mock<ISyncPair>();
            _mockSyncPair.Setup(m => m.Source).Returns(source);
            _mockSyncPair.Setup(m => m.Destination).Returns(destination);

            _diffInfoBuilder = new DiffInfoBuilder();
        }

        [TestMethod]
        public void LeaveSourceAndDestinationUnalteredWhenLevelIsFlat()
        {
            _mockSyncPair.Setup(m => m.Level).Returns(SyncLevel.Flat);

            var diffInfo = _diffInfoBuilder.BuildInfo(_mockSyncPair.Object);

            Assert.AreEqual(source, diffInfo.Source.FullName);
            Assert.AreEqual(destination, diffInfo.Destination.FullName);
        }

        [TestMethod]
        public void AddSourceParentDirectoryNameToDestinationUnalteredWhenLevelIsParent()
        {
            var expectedDestination = Path.Combine(destination, "Music");

            _mockSyncPair.Setup(m => m.Level).Returns(SyncLevel.Parent);

            var diffInfo = _diffInfoBuilder.BuildInfo(_mockSyncPair.Object);
            
            Assert.AreEqual(source, diffInfo.Source.FullName);
            Assert.AreEqual(expectedDestination, diffInfo.Destination.FullName);
        }
    }
}
