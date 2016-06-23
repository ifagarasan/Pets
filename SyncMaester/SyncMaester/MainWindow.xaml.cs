using System.Windows;
using Kore.IO;
using Kore.IO.Management;
using Kore.IO.Retrievers;
using Kore.IO.Scanners;
using Kore.IO.Sync;
using Kore.Settings;
using Kore.Settings.Serializers;
using SyncMaester.Core;

namespace SyncMaester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Kontrol _kontrol;
        private readonly IKoreFileInfo _settingsFileInfo;

        public MainWindow()
        {
            _settingsFileInfo = new KoreFileInfo("settings.bin");

            _kontrol = new Kontrol(new SettingsManager<ISettings>(new BinarySerializer<ISettings>()),
                new DiffBuilder(new DiffInfoBuilder(), new FileScanner(new FileRetriever()), new FolderDiffer()),
                new FolderDiffProcessor(new DiffProcessor(new FileCopier())));

            _kontrol.ReadSettings(_settingsFileInfo);

            InitializeComponent();

            Closing += (sender, args) => { _kontrol.WriteSettings(_settingsFileInfo); };
        }

        private void sync_Click(object sender, RoutedEventArgs e)
        {
            if (_kontrol.SyncPairs == null || _kontrol.SyncPairs.Count == 0)
            {
                MessageBox.Show("Nothing to sync", Title);
                return;
            }

            var diffResult = _kontrol.BuildDiff();

            _kontrol.ProcessFolderDiff(diffResult);

            MessageBox.Show("Processed diffs", Title);
        }

        private void syncManager_Click(object sender, RoutedEventArgs e)
        {
            var sm = new SyncManager(_kontrol.Settings);

            sm.ShowDialog();
        }
    }
}
