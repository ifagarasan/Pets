using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kore.IO.Retrievers;
using Kore.IO.Scanners;
using Kore.IO.Sync;
using Kore.IO.Util;
using Kore.Settings;
using Kore.Settings.Serializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Kore.Dev.Util.IoUtil;
using Kore.Dev.Util;

namespace SyncMaester.Core.AcceptanceTests
{
    [TestClass]
    public class SyncMaesterShould
    {
        private static readonly string _currentWorkingFolder = Path.Combine(TestRoot, DateTime.Now.Ticks.ToString());
        private string _destinationFolder;
        private string _sourceFolder;
        readonly string _primaryTestFileName = "file1.txt";

        IDiffBuilder _diffBuilder;
        IFolderDiffProcessor _folderDiffProcessor;
        IKontrol _kontrol;
        private ISettingsManager<ISettings> _settingsManager;

        [TestInitialize]
        public void Setup()
        {
            EnsureFolderExists(TestRoot);
            EnsureFolderExists(_currentWorkingFolder);

            _diffBuilder = new DiffBuilder(new FileScanner(new FileRetriever(new FileInfoProvider())), new FolderDiffer());
            _folderDiffProcessor = new FolderDiffProcessor(new DiffProcessor());
            _settingsManager = new SettingsManager<ISettings>(new BinarySerializer<ISettings>())
            {
                Data = new Settings()
            };
            _kontrol = new Kontrol(_settingsManager, _diffBuilder, _folderDiffProcessor);
        }

        #region File Moving

        [TestMethod]
        public void CopyANewSourceFile()
        {
            var currentTest = Path.Combine(_currentWorkingFolder, "test1");

            _sourceFolder = Path.Combine(currentTest, "src");
            _destinationFolder = Path.Combine(currentTest, "dest");

            EnsureFolderExists(_destinationFolder);

            var sourceFileInfo = new KoreFileInfo(Path.Combine(_sourceFolder, _primaryTestFileName));
            sourceFileInfo.EnsureExists();

            var syncPair = new SyncPair
            {
                Source = _sourceFolder,
                Destination = _destinationFolder
            };

            _kontrol.Settings.SyncPairs.Add(syncPair);

            var folderDiffs = _kontrol.BuildDiff();

            _kontrol.ProcessFolderDiff(folderDiffs);

            var destinationFileInfo = new KoreFileInfo(Path.Combine(_destinationFolder, _primaryTestFileName));

            Assert.AreEqual(1, folderDiffs.Count);
            Assert.AreEqual(DiffType.SourceNew, folderDiffs[0].Diffs[0].Type);
            Assert.IsTrue(destinationFileInfo.Exists);
        }

        [TestMethod]
        public void CopyANewerSourceFile()
        {
            var currentTest = Path.Combine(_currentWorkingFolder, "test2");

            _sourceFolder = Path.Combine(currentTest, "src");
            _destinationFolder = Path.Combine(currentTest, "dest");

            var now = DateTime.Now;

            var sourceFileInfo = new KoreFileInfo(Path.Combine(_sourceFolder, _primaryTestFileName));
            sourceFileInfo.EnsureExists();
            sourceFileInfo.LastWriteTime = now;

            var destinationFileInfo = new KoreFileInfo(Path.Combine(_destinationFolder, _primaryTestFileName));
            destinationFileInfo.EnsureExists();
            destinationFileInfo.LastWriteTime = now.AddSeconds(-1);

            var syncPair = new SyncPair
            {
                Source = _sourceFolder,
                Destination = _destinationFolder
            };

            _kontrol.Settings.SyncPairs.Add(syncPair);

            var folderDiffs = _kontrol.BuildDiff();

            _kontrol.ProcessFolderDiff(folderDiffs);

            Assert.AreEqual(1, folderDiffs.Count);
            Assert.AreEqual(DiffType.SourceNewer, folderDiffs[0].Diffs[0].Type);
            Assert.AreEqual(now, destinationFileInfo.LastWriteTime);
        }

        [TestMethod]
        public void CopyANewerDestinationFile()
        {
            var currentTest = Path.Combine(_currentWorkingFolder, "test3");

            _sourceFolder = Path.Combine(currentTest, "src");
            _destinationFolder = Path.Combine(currentTest, "dest");

            var now = DateTime.Now;

            var sourceFileInfo = new KoreFileInfo(Path.Combine(_sourceFolder, _primaryTestFileName));
            sourceFileInfo.EnsureExists();
            sourceFileInfo.LastWriteTime = now.AddSeconds(-1);

            var destinationFileInfo = new KoreFileInfo(Path.Combine(_destinationFolder, _primaryTestFileName));
            destinationFileInfo.EnsureExists();
            destinationFileInfo.LastWriteTime = now;

            var syncPair = new SyncPair
            {
                Source = _sourceFolder,
                Destination = _destinationFolder
            };

            _kontrol.Settings.SyncPairs.Add(syncPair);

            var folderDiffs = _kontrol.BuildDiff();

            _kontrol.ProcessFolderDiff(folderDiffs);

            Assert.AreEqual(1, folderDiffs.Count);
            Assert.AreEqual(DiffType.SourceOlder, folderDiffs[0].Diffs[0].Type);
            Assert.AreEqual(now, sourceFileInfo.LastWriteTime);
        }

        [TestMethod]
        public void DeleteAnOrphanDestinationFile()
        {
            var currentTest = Path.Combine(_currentWorkingFolder, "test4");

            _sourceFolder = Path.Combine(currentTest, "src");
            EnsureFolderExists(_sourceFolder);

            _destinationFolder = Path.Combine(currentTest, "dest");
            EnsureFolderExists(_destinationFolder);

            var destinationFileInfo = new KoreFileInfo(Path.Combine(_destinationFolder, _primaryTestFileName));
            destinationFileInfo.EnsureExists();

            var syncPair = new SyncPair
            {
                Source = _sourceFolder,
                Destination = _destinationFolder
            };

            _kontrol.Settings.SyncPairs.Add(syncPair);

            var folderDiffs = _kontrol.BuildDiff();

            _kontrol.ProcessFolderDiff(folderDiffs);

            Assert.AreEqual(1, folderDiffs.Count);
            Assert.AreEqual(DiffType.DestinationOrphan, folderDiffs[0].Diffs[0].Type);
            Assert.IsFalse(destinationFileInfo.Exists);
        }

        [TestMethod]
        public void SupportMultipleSyncPairs()
        {
            var currentTest = Path.Combine(_currentWorkingFolder, "test-multiple-sync-pairs");

            var sourceFolder1 = Path.Combine(currentTest, "src1");
            EnsureFolderExists(sourceFolder1);

            var sourceFolder2 = Path.Combine(currentTest, "src2");
            EnsureFolderExists(sourceFolder2);

            var destinationFolder1 = Path.Combine(currentTest, "dest1");
            EnsureFolderExists(destinationFolder1);

            var destinationFolder2 = Path.Combine(currentTest, "dest2");
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
                Destination = destinationFolder1
            });

            _kontrol.Settings.SyncPairs.Add(new SyncPair
            {
                Source = sourceFolder2,
                Destination = destinationFolder2
            });

            var folderDiffs = _kontrol.BuildDiff();

            _kontrol.ProcessFolderDiff(folderDiffs);

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
            var currentTest = Path.Combine(_currentWorkingFolder, "test-sync-options-copy-content");

            _sourceFolder = Path.Combine(currentTest, "src");
            _destinationFolder = Path.Combine(currentTest, "dest");

            EnsureFolderExists(_destinationFolder);

            var sourceFileInfo = new KoreFileInfo(Path.Combine(_sourceFolder, _primaryTestFileName));
            sourceFileInfo.EnsureExists();

            var syncPair = new SyncPair
            {
                Source = _sourceFolder,
                Destination = _destinationFolder,
                Level = SyncLevel.Parent
            };

            _kontrol.Settings.SyncPairs.Add(syncPair);

            var folderDiffs = _kontrol.BuildDiff();

            _kontrol.ProcessFolderDiff(folderDiffs);

            var destinationFileInfo = new KoreFileInfo(Path.Combine(_destinationFolder, "src", _primaryTestFileName));

            Assert.AreEqual(1, folderDiffs.Count);
            Assert.AreEqual(DiffType.SourceNew, folderDiffs[0].Diffs[0].Type);
            Assert.IsTrue(destinationFileInfo.Exists);
        }

        #endregion

        #region Settings

        [TestMethod]
        public void WriteSettings()
        {
            var currentTest = Path.Combine(_currentWorkingFolder, "test5");

            EnsureFolderExists(currentTest);

            _settingsManager.Data.SyncPairs.Add(new SyncPair { Source = currentTest, Destination = currentTest});

            var settingsFile = new KoreFileInfo(Path.Combine(currentTest, "settings.bin"));

            _kontrol.WriteSettings(settingsFile);

            Assert.IsTrue(settingsFile.Exists);
        }

        public void ReadSettings()
        {
            var currentTest = Path.Combine(_currentWorkingFolder, "test6");

            EnsureFolderExists(currentTest);

            var sourceFolder = "C:\\Music";
            var destinationFolder = "D:\\Backups\\Music";

            _settingsManager.Data.SyncPairs.Add(new SyncPair { Source = currentTest, Destination = currentTest });

            var settingsFile = new KoreFileInfo(Path.Combine(currentTest, "settings.bin"));

            _kontrol.WriteSettings(settingsFile);

            var settingsManager = new SettingsManager<ISettings>(new BinarySerializer<ISettings>());

            var kontrol = new Kontrol(settingsManager,
                new DiffBuilder(new FileScanner(new FileRetriever(new FileInfoProvider())), new FolderDiffer()),
                new FolderDiffProcessor(new DiffProcessor()));

            kontrol.ReadSettings(settingsFile);

            Assert.IsNotNull(settingsManager.Data);
            Assert.IsNotNull(settingsManager.Data.SyncPairs);
            Assert.AreEqual(1, settingsManager.Data.SyncPairs.Count);
            Assert.AreEqual(sourceFolder, settingsManager.Data.SyncPairs[0].Source);
            Assert.AreEqual(destinationFolder, settingsManager.Data.SyncPairs[0].Destination);

            Assert.IsTrue(settingsFile.Exists);
        }

        #endregion
    }
}
