using System;
using System.IO;
using Kore.IO.Retrievers;
using Kore.IO.Scanners;
using Kore.IO.Sync;
using Kore.Settings;
using Kore.Settings.Serializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Kore.Dev.Util.IoUtil;
using Kore.IO;
using Kore.IO.Management;

namespace SyncMaester.Core.AcceptanceTests
{
    [TestClass]
    public class SyncMaesterShould
    {
        private static readonly string CurrentWorkingFolder = Path.Combine(TestRoot, DateTime.Now.Ticks.ToString());
        private string _destinationFolder;
        private string _sourceFolder;
        readonly string _primaryTestFileName = "file1.txt";

        ISyncManager _syncManager;
        IKontrol _kontrol;
        private ISettingsManager<ISettings> _settingsManager;
        private string _currentTest;
        private ISyncPair _syncPair;

        [TestInitialize]
        public void Setup()
        {
            EnsureFolderExists(TestRoot);
            EnsureFolderExists(CurrentWorkingFolder);

            _settingsManager = new SettingsManager<ISettings>(new BinarySerializer<ISettings>())
            {
                Data = new Settings()
            };

            _syncManager = new SyncManager(new DiffBuilder(new DiffInfoBuilder(), new FileScanner(new FileRetriever()),
                new FolderDiffer(new IdentityProvider())), new FolderDiffProcessor(new DiffProcessor(new FileCopier())),
                new ScanInfo());

            _kontrol = new Kontrol(_settingsManager, _syncManager);
        }

        #region File Moving

        [TestMethod]
        public void CopyANewSourceFile()
        {
            SetupCurrentTestFolder("test-new-source-file");

            var sourceFileInfo = new KoreFileInfo(Path.Combine(_sourceFolder, _primaryTestFileName));
            sourceFileInfo.EnsureExists();

            _kontrol.Sync();

            var destinationFileInfo = new KoreFileInfo(Path.Combine(_destinationFolder, _primaryTestFileName));

            Assert.IsTrue(destinationFileInfo.Exists);
        }

        [TestMethod]
        public void CopyANewerSourceFile()
        {
            SetupCurrentTestFolder("test-newer-source-file");

            var now = DateTime.Now;

            var sourceFileInfo = new KoreFileInfo(Path.Combine(_sourceFolder, _primaryTestFileName));
            sourceFileInfo.EnsureExists();
            sourceFileInfo.LastWriteTime = now;

            var destinationFileInfo = new KoreFileInfo(Path.Combine(_destinationFolder, _primaryTestFileName));
            destinationFileInfo.EnsureExists();
            destinationFileInfo.LastWriteTime = now.AddSeconds(-1);

            _kontrol.Sync();

            Assert.AreEqual(now, destinationFileInfo.LastWriteTime);
        }

        [TestMethod]
        public void CopyANewerDestinationFile()
        {
            SetupCurrentTestFolder("test-newer-destination-file");

            var now = DateTime.Now;

            var sourceFileInfo = new KoreFileInfo(Path.Combine(_sourceFolder, _primaryTestFileName));
            sourceFileInfo.EnsureExists();
            sourceFileInfo.LastWriteTime = now.AddSeconds(-1);

            var destinationFileInfo = new KoreFileInfo(Path.Combine(_destinationFolder, _primaryTestFileName));
            destinationFileInfo.EnsureExists();
            destinationFileInfo.LastWriteTime = now;

            _kontrol.Sync();

            Assert.AreEqual(now, sourceFileInfo.LastWriteTime);
        }

        [TestMethod]
        public void DeleteAnOrphanDestinationFile()
        {
            SetupCurrentTestFolder("test-oprhanr-destination-file");

            var destinationFileInfo = new KoreFileInfo(Path.Combine(_destinationFolder, _primaryTestFileName));
            destinationFileInfo.EnsureExists();

            _kontrol.Sync();

            Assert.IsFalse(destinationFileInfo.Exists);
        }

        [TestMethod]
        public void NotCopyAnIdenticalFile()
        {
            SetupCurrentTestFolder("test-identical-files");

            var sourceFileInfo = new KoreFileInfo(Path.Combine(_sourceFolder, _primaryTestFileName));
            sourceFileInfo.EnsureExists();

            using (var streamWriter = new StreamWriter(sourceFileInfo.FullName))
            {
                streamWriter.Write('c');
            }

            var destinationFileInfo = new KoreFileInfo(Path.Combine(_destinationFolder, _primaryTestFileName));
            destinationFileInfo.EnsureExists();

            var now = DateTime.Now;

            sourceFileInfo.LastWriteTime = now;
            destinationFileInfo.LastWriteTime = now;


            _kontrol.Sync();

            Assert.IsTrue(destinationFileInfo.Exists);
            Assert.IsTrue(sourceFileInfo.Exists);
            Assert.AreEqual(0, destinationFileInfo.Size);
        }

        [TestMethod]
        public void SupportMultipleSyncPairs()
        {
            _currentTest = Path.Combine(CurrentWorkingFolder, "test-multiple-sync-pairs");

            var sourceFolder1 = Path.Combine(_currentTest, "src1");
            EnsureFolderExists(sourceFolder1);

            var sourceFolder2 = Path.Combine(_currentTest, "src2");
            EnsureFolderExists(sourceFolder2);

            var destinationFolder1 = Path.Combine(_currentTest, "dest1");
            EnsureFolderExists(destinationFolder1);

            var destinationFolder2 = Path.Combine(_currentTest, "dest2");
            EnsureFolderExists(destinationFolder2);

            var fileName1 = "file1.txt";
            var sourceFileInfo1 = new KoreFileInfo(Path.Combine(sourceFolder1, fileName1));
            sourceFileInfo1.EnsureExists();

            var fileName2 = "file2.exe";
            var sourceFileInfo2 = new KoreFileInfo(Path.Combine(sourceFolder2, fileName2));
            sourceFileInfo2.EnsureExists();

            _kontrol.Settings.SyncPairs.Add(new SyncPair
            {
                Source = sourceFolder1,
                Destination = destinationFolder1,
                Level = SyncLevel.Flat
            });

            _kontrol.Settings.SyncPairs.Add(new SyncPair
            {
                Source = sourceFolder2,
                Destination = destinationFolder2,
                Level = SyncLevel.Flat
            });

            _kontrol.Sync();

            var destinationFileInfo1 = new KoreFileInfo(Path.Combine(destinationFolder1, fileName1));
            var destinationFileInfo2 = new KoreFileInfo(Path.Combine(destinationFolder2, fileName2));

            Assert.IsTrue(destinationFileInfo1.Exists);
            Assert.IsTrue(destinationFileInfo2.Exists);
        }

        #endregion

        #region Sync Options

        [TestMethod]
        public void CopiesContentAtDestinationUnderSourceParentIfLevelIsParent()
        {
            SetupCurrentTestFolder("test-sync-options-copy-content", SyncLevel.Parent);

            var sourceFileInfo = new KoreFileInfo(Path.Combine(_sourceFolder, _primaryTestFileName));
            sourceFileInfo.EnsureExists();

            _kontrol.Sync();

            var destinationFileInfo = new KoreFileInfo(Path.Combine(_destinationFolder, "src", _primaryTestFileName));

            Assert.IsTrue(destinationFileInfo.Exists);
        }

        #endregion

        #region Settings

        [TestMethod]
        public void WriteSettings()
        {
            _currentTest = Path.Combine(CurrentWorkingFolder, "test5");

            EnsureFolderExists(_currentTest);

            _settingsManager.Data.SyncPairs.Add(new SyncPair { Source = _currentTest, Destination = _currentTest});

            var settingsFile = new KoreFileInfo(Path.Combine(_currentTest, "settings.bin"));

            _kontrol.WriteSettings(settingsFile);

            Assert.IsTrue(settingsFile.Exists);
        }

        public void ReadSettings()
        {
            _currentTest = Path.Combine(CurrentWorkingFolder, "test6");

            EnsureFolderExists(_currentTest);

            var sourceFolder = "C:\\Music";
            var destinationFolder = "D:\\Backups\\Music";

            _settingsManager.Data.SyncPairs.Add(new SyncPair { Source = _currentTest, Destination = _currentTest });

            var settingsFile = new KoreFileInfo(Path.Combine(_currentTest, "settings.bin"));

            _kontrol.WriteSettings(settingsFile);

            var settingsManager = new SettingsManager<ISettings>(new BinarySerializer<ISettings>());

            var kontrol = new Kontrol(settingsManager, _syncManager);

            kontrol.ReadSettings(settingsFile);

            Assert.IsNotNull(settingsManager.Data);
            Assert.IsNotNull(settingsManager.Data.SyncPairs);
            Assert.AreEqual(1, settingsManager.Data.SyncPairs.Count);
            Assert.AreEqual(sourceFolder, settingsManager.Data.SyncPairs[0].Source);
            Assert.AreEqual(destinationFolder, settingsManager.Data.SyncPairs[0].Destination);

            Assert.IsTrue(settingsFile.Exists);
        }

        #endregion

        [TestMethod]
        public void MakeScanInformationAvailable()
        {
            SetupCurrentTestFolder("test-scan-info");

            var now = DateTime.Now;

            var sourceFileInfo = new KoreFileInfo(Path.Combine(_sourceFolder, _primaryTestFileName));
            sourceFileInfo.EnsureExists();
            sourceFileInfo.LastWriteTime = now;

            var destinationFileInfo = new KoreFileInfo(Path.Combine(_destinationFolder, _primaryTestFileName));
            destinationFileInfo.EnsureExists();
            destinationFileInfo.LastWriteTime = now.AddSeconds(-1);

            _kontrol.Sync();

            Assert.AreEqual(1u, _kontrol.ScanInfo.SourceFiles);
            Assert.AreEqual(1u, _kontrol.ScanInfo.DestinationFiles);
        }

        private void SetupCurrentTestFolder(string testFolder, SyncLevel syncLevel = SyncLevel.Flat)
        {
            _currentTest = Path.Combine(CurrentWorkingFolder, testFolder);

            _sourceFolder = Path.Combine(_currentTest, "src");
            _destinationFolder = Path.Combine(_currentTest, "dest");

            EnsureFolderExists(_sourceFolder);
            EnsureFolderExists(_destinationFolder);

            _syncPair = new SyncPair
            {
                Source = _sourceFolder,
                Destination = _destinationFolder,
                Level = syncLevel
            };

            _kontrol.Settings.SyncPairs.Add(_syncPair);
        }
    }
}
