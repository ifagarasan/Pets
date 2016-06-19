using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kore.IO.Retrievers;
using Kore.IO.Scanners;
using Kore.IO.Sync;
using Kore.IO.Util;
using Kore.Settings;
using Kore.Settings.Serializers;
using SyncMaester.Core;

namespace SyncMaester
{
    public partial class MainWindow : Form
    {
        private readonly Kontrol _kontrol;
        private readonly IKoreFileInfo _settingsFileInfo;
        private readonly IInterfaceManager _interfaceManager;

        public MainWindow()
        {
            _settingsFileInfo = new KoreFileInfo("settings.bin");

            _kontrol = new Kontrol(new SettingsManager<ISettings>(new BinarySerializer<ISettings>()), 
                new DiffBuilder(new FileScanner(new FileRetriever(new FileInfoProvider())), new FolderDiffer()),
                new FolderDiffProcessor(new DiffProcessor()));

            _kontrol.ReadSettings(_settingsFileInfo);

            _interfaceManager = new InterfaceManager();

            FormClosing += (sender, args) => { _kontrol.WriteSettings(_settingsFileInfo); };

            InitializeComponent();

            _interfaceManager.DisplaySyncPairs(_kontrol, sourcePath, destinationPath);
        }

        private void bSync_Click(object sender, EventArgs e)
        {
            _kontrol.AddSyncPair(new SyncPair
            {
                Source = new KoreFolderInfo(sourcePath.Text),
                Destination = new KoreFolderInfo(destinationPath.Text)
            });

            var diff = _kontrol.BuildDiff();

            _kontrol.ProcessFolderDiff(diff);

            MessageBox.Show($"Processed {diff.Diffs.Count} diffs");
        }
    }
}
