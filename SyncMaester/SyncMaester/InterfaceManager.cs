using System.Windows.Forms;
using SyncMaester.Core;

namespace SyncMaester
{
    public class InterfaceManager : IInterfaceManager
    {
        public void DisplaySyncPairs(Kontrol kontrol, TextBox sourcePath, TextBox destinationPath)
        {
            sourcePath.Text = kontrol.SyncPair.Source.FullName;
            destinationPath.Text = kontrol.SyncPair.Destination.FullName;
        }
    }
}