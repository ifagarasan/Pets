using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Kore.IO.Retrievers;
using Kore.IO.Scanners;
using Kore.IO.Sync;
using Kore.IO.Util;
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
                new DiffBuilder(new FileScanner(new FileRetriever(new FileInfoProvider())), new FolderDiffer()),
                new FolderDiffProcessor(new DiffProcessor(), new DiffInfoBuilder()));

            _kontrol.ReadSettings(_settingsFileInfo);

            InitializeComponent();

            Closing += (sender, args) => { _kontrol.WriteSettings(_settingsFileInfo); };
        }

        private void sync_Click(object sender, RoutedEventArgs e)
        {
            if (_kontrol.SyncPairs == null || _kontrol.SyncPairs.Count == 0)
            {
                MessageBox.Show("Nothing to sync");
                return;
            }

            var diffResult = _kontrol.BuildDiff();

            _kontrol.ProcessFolderDiff(diffResult);

            MessageBox.Show("Processed diffs");
        }

        private void syncManager_Click(object sender, RoutedEventArgs e)
        {
            var sm = new SyncManager(_kontrol.Settings);

            sm.ShowDialog();
        }
    }
}
