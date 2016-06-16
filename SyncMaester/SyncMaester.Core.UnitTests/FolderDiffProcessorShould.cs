using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using Kore.IO.Scanners;
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

        [TestInitialize]
        public void Setup()
        {
            _mockDiffProcessor = new Mock<IDiffProcessor>();
            _folderDiffProcessor = new FolderDiffProcessor(_mockDiffProcessor.Object);

            _mockFolderDiff = new Mock<IFolderDiff>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidatefDiffProcessorOnInit()
        {
            _folderDiffProcessor = new FolderDiffProcessor(null);
        }

        #region Process

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

            _mockDiffProcessor.Setup(m => m.Process(It.IsAny<IDiff>())).Callback((IDiff x) =>
            {
                Assert.AreSame(diffs[index++], x);
            });

            _mockFolderDiff.Setup(m => m.Diffs).Returns(diffs);

            _folderDiffProcessor.Process(_mockFolderDiff.Object);

            _mockDiffProcessor.Verify(m => m.Process(It.IsAny<IDiff>()), Times.Exactly(3));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidatesFolderDiffOnProcess()
        {
            _folderDiffProcessor.Process(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidatesFolderDiffDiffsOnProcess()
        {
            _folderDiffProcessor.Process(_mockFolderDiff.Object);
        }

        #endregion
    }
}
