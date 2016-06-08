using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections;
using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;

namespace SyncMaester.Core.UnitTests
{
    [TestClass]
    public class KontrolShould
    {
        private Kontrol _kontrol;
        private Mock<ISettings> _mockSettings;
        private Mock<IList<ISyncPair>> _mockList;
        private Mock<ISyncPair> _mockSyncPair;

        [TestInitialize]
        public void Setup()
        {
            _mockList = new Mock<IList<ISyncPair>>();
            _mockSyncPair = new Mock<ISyncPair>();

            _mockSettings = new Mock<ISettings>();
            _kontrol = new Kontrol(_mockSettings.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowArgumentNullExceptionIfSettingsIsNull()
        {
            _kontrol = new Kontrol(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowArgumentNullExceptionIfSyncPairIsNullOnAdd()
        {
            _kontrol.AddSyncPair(null);
        }

        [TestMethod]
        public void AddTheSyncPairToSyncPairs()
        {
            _mockList.Setup(m => m.Add(It.IsAny<ISyncPair>()));
            _mockSettings.Setup(m => m.SyncPairs).Returns(_mockList.Object);

            _kontrol.AddSyncPair(_mockSyncPair.Object);

            _mockList.Verify(m => m.Add(_mockSyncPair.Object));
        }

        [TestMethod]
        public void RemoveTheSyncPairFromSyncPairs()
        {
            _mockList.Setup(m => m.Remove(It.IsAny<ISyncPair>()));
            _mockSettings.Setup(m => m.SyncPairs).Returns(_mockList.Object);

            _kontrol.RemoveSyncPair(_mockSyncPair.Object);

            _mockList.Verify(m => m.Remove(_mockSyncPair.Object));
        }

        [TestMethod]
        public void ReturnsTrueWhenRemovalSucceeds()
        {
            TestRemoveReturnValue(true);
        }

        [TestMethod]
        public void ReturnsFalseWhenRemovalFalse()
        {
            TestRemoveReturnValue(false);
        }

        public void TestRemoveReturnValue(bool returnValue)
        {
            _mockList.Setup(m => m.Remove(It.IsAny<ISyncPair>())).Returns(returnValue);
            _mockSettings.Setup(m => m.SyncPairs).Returns(_mockList.Object);

            Assert.AreEqual(returnValue, _kontrol.RemoveSyncPair(_mockSyncPair.Object));
        }
    }
}
