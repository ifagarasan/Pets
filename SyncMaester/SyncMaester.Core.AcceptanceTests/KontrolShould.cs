using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SyncMaester.Core.AcceptanceTests
{
    [TestClass]
    public class KontrolShould
    {
        [TestMethod]
        public void AllowAddingOfSyncPairs()
        {
            Settings settings = new Settings();
            Kontrol control = new Kontrol(settings);

            SyncPair syncPair = new SyncPair();
            syncPair.SourceFolder = "abc";

            syncPair.DestinationFolders.Add("def");
            syncPair.DestinationFolders.Add("ghi");

            control.AddSyncPair(syncPair);

            Assert.AreEqual(1, settings.SyncPairs.Count);
            Assert.AreSame(syncPair, settings.SyncPairs[0]);
        }
    }
}
