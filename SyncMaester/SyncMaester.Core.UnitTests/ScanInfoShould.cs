using System;
using System.Collections.Generic;
using System.Linq;
using Kore.Exceptions;
using Kore.IO;
using Kore.IO.Sync;
using Kore.Settings;
using Kore.Settings.Serializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SyncMaester.Core.UnitTests
{
    [TestClass]
    public class ScanInfoShould
    {
        ISyncInfo _syncInfo;

        [TestInitialize]
        public void Setup()
        {
            _syncInfo = new SyncInfo();
        }

        [TestMethod]
        public void IncrementSourceFilesOnNewSourceFileFound()
        {
            _syncInfo.NewSourceFileFound(null);

            Assert.AreEqual(1u, _syncInfo.SourceFiles);
        }

        [TestMethod]
        public void IncrementDestinationFilesOnNewDestinationFileFound()
        {
            _syncInfo.NewDestinationFileFound(null);

            Assert.AreEqual(1u, _syncInfo.DestinationFiles);
        }

        [TestMethod]
        public void ClearResetsToZero()
        {
            _syncInfo.NewDestinationFileFound(null);
            _syncInfo.NewSourceFileFound(null);

            _syncInfo.Clear();

            Assert.AreEqual(0u, _syncInfo.SourceFiles);
            Assert.AreEqual(0u, _syncInfo.DestinationFiles);
        }

        [TestMethod]
        public void CompleteSetsStatusToCompleted()
        {
            _syncInfo.Complete();

            Assert.AreEqual(SyncStatus.Complete, _syncInfo.Status);
        }
    }
}