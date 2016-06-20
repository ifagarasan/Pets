using System.Collections.Generic;
using System.Collections.ObjectModel;
using Kore.IO.Sync;
using Kore.IO.Util;

namespace SyncMaester.Core
{
    public interface IKontrol
    {
        ISettings Settings { get; }
        IList<IFolderDiff> BuildDiff();
        void ProcessFolderDiff(IList<IFolderDiff> folderDiff);
        void WriteSettings(IKoreFileInfo destination);
        void ReadSettings(IKoreFileInfo source);
    }
}