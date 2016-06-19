using System.Windows.Controls;
using SyncMaester.Core;

namespace SyncMaester
{
    internal interface IInterfaceManager
    {
        void DisplaySyncPairs(Kontrol kontrol, TextBox sourcePath, TextBox destinationPath);
    }
}