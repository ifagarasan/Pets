using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SyncMaester.Core.AcceptanceTests
{
    [TestClass]
    public class KontrolShould
    {
        Settings _settings;
        Kontrol _control;

        [TestInitialize]
        public void Setup()
        {
            _settings = new Settings();
            _control = new Kontrol(_settings);
        }

        [TestMethod]
        public void AllowAddingOfSyncPairs()
        {
            SyncPair syncPair = new SyncPair {SourceFolder = "abc"};

            syncPair.DestinationFolders.Add("def");
            syncPair.DestinationFolders.Add("ghi");

            _control.AddSyncPair(syncPair);

            Assert.AreEqual(1, _settings.SyncPairs.Count);
            Assert.AreSame(syncPair, _settings.SyncPairs[0]);
        }

        [TestMethod]
        public void AllowRemovingOfSyncPairs()
        {
            SyncPair syncPair = new SyncPair { SourceFolder = "abc" };

            syncPair.DestinationFolders.Add("def");
            syncPair.DestinationFolders.Add("ghi");

            _settings.SyncPairs.Add(syncPair);

            _control.RemoveSyncPair(syncPair);

            Assert.AreEqual(0, _settings.SyncPairs.Count);
        }
    }
}
