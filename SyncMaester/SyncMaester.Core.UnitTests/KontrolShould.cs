using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections;
using System.Collections.Generic;

namespace SyncMaester.Core.UnitTests
{
    [TestClass]
    public class KontrolShould
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowArgumentNullExceptionIfSettingsIsNull()
        {
            Kontrol kontrol = new Kontrol(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowArgumentNullExceptionIfSyncPairIsNull()
        {
            Mock<ISettings> mockSettings = new Mock<ISettings>();
            
            Kontrol kontrol = new Kontrol(mockSettings.Object);

            kontrol.AddSyncPair(null);
        }

        [TestMethod]
        public void AddTheSyncPairToSyncPairs()
        {
            Mock<ISettings> mockSettings = new Mock<ISettings>();
            Mock<IList<ISyncPair>> mockList = new Mock<IList<ISyncPair>>();
            Mock<ISyncPair> mockSyncPair = new Mock<ISyncPair>();
            Kontrol kontrol = new Kontrol(mockSettings.Object);

            mockList.Setup(m => m.Add(It.IsAny<ISyncPair>()));
            mockSettings.Setup(m => m.SyncPairs).Returns(mockList.Object);

            kontrol.AddSyncPair(mockSyncPair.Object);

            mockList.Verify(m => m.Add(mockSyncPair.Object));
        }
    }
}
