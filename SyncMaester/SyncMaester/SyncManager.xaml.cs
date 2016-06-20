using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using SyncMaester.Core;

namespace SyncMaester
{
    /// <summary>
    /// Interaction logic for SyncManager.xaml
    /// </summary>
    public partial class SyncManager : Window
    {
        private readonly ISettings _settings;
        ObservableCollection<ISyncPair> _syncPairs;

        public SyncManager(ISettings settings)
        {
            _settings = settings;
            _syncPairs = new ObservableCollection<ISyncPair>(_settings.SyncPairs);

            InitializeComponent();

            this.syncPairs.ItemsSource = _syncPairs;
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            _syncPairs.Add(new SyncPair { Source = "source", Destination = "destination"});
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            _settings.SyncPairs = _syncPairs.ToList();
        }
    }
}
