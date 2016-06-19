using System.Windows.Forms;
using SyncMaester.Core;

namespace SyncMaester
{
    internal interface IInterfaceManager
    {
        void DisplaySyncPairs(Kontrol kontrol, TextBox sourcePath, TextBox destinationPath);
    }
}