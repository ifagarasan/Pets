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

namespace SyncMaester.Core.AcceptanceTests
{
    [TestClass]
    public class SyncMaesterEndToEnd
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
                Source = new KoreFolderInfo(_sourceFolder),
                Destination = new KoreFolderInfo(_destinationFolder)
            };

            _kontrol.AddSyncPair(syncPair);

            var folderDiff = _kontrol.BuildDiff();

            _kontrol.ProcessFolderDiff(folderDiff);

            var destinationFileInfo = new KoreFileInfo(Path.Combine(_destinationFolder, _primaryTestFileName));

            Assert.AreEqual(DiffType.SourceNew, folderDiff.Diffs[0].Type);
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
                Source = new KoreFolderInfo(_sourceFolder),
                Destination = new KoreFolderInfo(_destinationFolder)
            };

            _kontrol.AddSyncPair(syncPair);

            var folderDiff = _kontrol.BuildDiff();

            _kontrol.ProcessFolderDiff(folderDiff);

            Assert.AreEqual(DiffType.SourceNewer, folderDiff.Diffs[0].Type);
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
                Source = new KoreFolderInfo(_sourceFolder),
                Destination = new KoreFolderInfo(_destinationFolder)
            };

            _kontrol.AddSyncPair(syncPair);

            var folderDiff = _kontrol.BuildDiff();

            _kontrol.ProcessFolderDiff(folderDiff);

            Assert.AreEqual(DiffType.SourceOlder, folderDiff.Diffs[0].Type);
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
                Source = new KoreFolderInfo(_sourceFolder),
                Destination = new KoreFolderInfo(_destinationFolder)
            };

            _kontrol.AddSyncPair(syncPair);

            var folderDiff = _kontrol.BuildDiff();

            _kontrol.ProcessFolderDiff(folderDiff);

            Assert.AreEqual(DiffType.DestinationOrphan, folderDiff.Diffs[0].Type);
            Assert.IsFalse(destinationFileInfo.Exists);
        }

        #endregion

        #region Settings

        [TestMethod]
        public void WriteSettings()
        {
            var currentTest = Path.Combine(_currentWorkingFolder, "test5");

            EnsureFolderExists(currentTest);

            _settingsManager.Data.SyncPair.Source = new KoreFolderInfo(currentTest);
            _settingsManager.Data.SyncPair.Destination = new KoreFolderInfo(currentTest);

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

            _settingsManager.Data.SyncPair.Source = new KoreFolderInfo(sourceFolder);
            _settingsManager.Data.SyncPair.Destination = new KoreFolderInfo(destinationFolder);

            var settingsFile = new KoreFileInfo(Path.Combine(currentTest, "settings.bin"));

            _kontrol.WriteSettings(settingsFile);

            var settingsManager = new SettingsManager<ISettings>(new BinarySerializer<ISettings>());

            var kontrol = new Kontrol(settingsManager,
                new DiffBuilder(new FileScanner(new FileRetriever(new FileInfoProvider())), new FolderDiffer()),
                new FolderDiffProcessor(new DiffProcessor()));

            kontrol.ReadSettings(settingsFile);

            Assert.AreEqual(sourceFolder, settingsManager.Data.SyncPair.Source);
            Assert.AreEqual(destinationFolder, settingsManager.Data.SyncPair.Destination);

            Assert.IsTrue(settingsFile.Exists);
        }

        #endregion
    }
}
