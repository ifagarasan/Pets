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
        IScanInfo _scanInfo;

        [TestInitialize]
        public void Setup()
        {
            _scanInfo = new ScanInfo();
        }

        [TestMethod]
        public void IncrementSourceFilesOnNewSourceFileFound()
        {
            _scanInfo.NewSourceFileFound(null);

            Assert.AreEqual(1u, _scanInfo.SourceFiles);
        }

        [TestMethod]
        public void IncrementDestinationFilesOnNewDestinationFileFound()
        {
            _scanInfo.NewDestinationFileFound(null);

            Assert.AreEqual(1u, _scanInfo.DestinationFiles);
        }

        [TestMethod]
        public void ClearResetsToZero()
        {
            _scanInfo.NewDestinationFileFound(null);
            _scanInfo.NewSourceFileFound(null);

            _scanInfo.Clear();

            Assert.AreEqual(0u, _scanInfo.SourceFiles);
            Assert.AreEqual(0u, _scanInfo.DestinationFiles);
        }

        [TestMethod]
        public void CompleteSetsStatusToCompleted()
        {
            _scanInfo.Complete();

            Assert.AreEqual(ScanStatus.Complete, _scanInfo.Status);
        }
    }
}