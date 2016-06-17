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

        IDiffBuilder _diffBuilder;
        IFolderDiffProcessor _folderDiffProcessor;
        IKontrol _kontrol;

        [TestInitialize]
        public void Setup()
        {
            EnsureFolderExits(TestRoot);
            EnsureFolderExits(_currentWorkingFolder);

            _diffBuilder = new DiffBuilder(new FileScanner(new FileRetriever(new FileInfoProvider())), new FolderDiffer());
            _folderDiffProcessor = new FolderDiffProcessor(new DiffProcessor());
            _kontrol = new Kontrol(new Settings(), _diffBuilder, _folderDiffProcessor);
        }

        [TestMethod]
        public void CopyANewSourceFile()
        {
            var currentTest = Path.Combine(_currentWorkingFolder, "test1");

            _sourceFolder = Path.Combine(currentTest, "src");
            _destinationFolder = Path.Combine(currentTest, "dest");

            EnsureFolderExits(_destinationFolder);

            var fileName = "file1.txt";
            var sourceFileInfo = new KoreFileInfo(Path.Combine(_sourceFolder, fileName));
            sourceFileInfo.EnsureExists();

            var syncPair = new SyncPair
            {
                Source = new KoreFolderInfo(_sourceFolder),
                Destination = new KoreFolderInfo(_destinationFolder)
            };

            _kontrol.AddSyncPair(syncPair);

            var folderDiff = _kontrol.BuildDiff();

            _kontrol.ProcessFolderDiff(folderDiff);

            var destinationFileInfo = new KoreFileInfo(Path.Combine(_destinationFolder, fileName));

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
            var fileName = "file1.txt";

            var sourceFileInfo = new KoreFileInfo(Path.Combine(_sourceFolder, fileName));
            sourceFileInfo.EnsureExists();
            sourceFileInfo.LastWriteTime = now;

            var destinationFileInfo = new KoreFileInfo(Path.Combine(_destinationFolder, fileName));
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
    }
}
