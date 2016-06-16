using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kore.Dev.Util;
using Kore.IO.Retrievers;
using Kore.IO.Scanners;
using Kore.IO.Sync;
using Kore.IO.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SyncMaester.Core.AcceptanceTests
{
    [TestClass]
    public class SyncMaesterEndToEnd
    {
        private string _testFolder;
        private string _destinationFolder;
        private string _sourceFolder;

        [TestMethod]
        public void AllowDefiningOfOneSyncPairRetrieveDiffAndProcessDiffWithOneNewFile()
        {
            _testFolder = Path.Combine(IoUtil.TestRoot, DateTime.Now.Ticks.ToString());

            IoUtil.EnsureFolderExits(IoUtil.TestRoot);
            IoUtil.EnsureFolderExits(_testFolder);

            _sourceFolder = Path.Combine(_testFolder, "src");
            _destinationFolder = Path.Combine(_testFolder, "dest");

            IKoreFileInfo sourceFileInfo = new KoreFileInfo(Path.Combine(_sourceFolder, "source_new.txt"));
            sourceFileInfo.EnsureExists();

            IoUtil.EnsureFolderExits(_sourceFolder);
            IoUtil.EnsureFolderExits(_destinationFolder);

            IDiffBuilder diffBuilder = new DiffBuilder(new FileScanner(new FileRetriever(new FileInfoProvider())), new FolderDiffer());
            IFolderDiffProcessor folderDiffProcessor = new FolderDiffProcessor(new DiffProcessor());

            IKontrol kontrol = new Kontrol(new Settings(), diffBuilder, folderDiffProcessor);
            ISyncPair syncPair = new SyncPair();

            syncPair.Source = _sourceFolder;
            syncPair.Destination = _destinationFolder;

            kontrol.AddSyncPair(syncPair);

            IFolderDiff folderDiff = kontrol.BuildDiff();

            kontrol.ProcessFolderDiff(folderDiff);

            IKoreFileInfo destiinationFileInfo = new KoreFileInfo(Path.Combine(_destinationFolder, "source_new.txt"));

            Assert.IsTrue(destiinationFileInfo.Exists);
        }
    }
}
