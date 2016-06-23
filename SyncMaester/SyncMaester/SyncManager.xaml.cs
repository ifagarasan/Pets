using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SyncMaester.Core;

namespace SyncMaester
{
    /// <summary>
    /// Interaction logic for SyncManager.xaml
    /// </summary>
    public partial class SyncManager : Window
    {
        private readonly ISettings _settings;
        readonly ObservableCollection<ISyncPair> _syncPairs;

        public SyncManager(ISettings settings)
        {
            _settings = settings;
            _syncPairs = new ObservableCollection<ISyncPair>(_settings.SyncPairs);

            InitializeComponent();

            syncPairs.ItemsSource = _syncPairs;

            Closing += (sender, args) =>
            {
                _settings.SyncPairs = _syncPairs.ToList();
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            _syncPairs.Add(new SyncPair { Source = "source", Destination = "destination"});
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Delete selected sync pair?", Title, MessageBoxButton.YesNoCancel) != MessageBoxResult.Yes)
                return;

            var button = sender as Button;
            var syncPair = button.DataContext as SyncPair;

            _syncPairs.Remove(syncPair);
        }
    }
}
