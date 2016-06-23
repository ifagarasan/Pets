using Kore.IO;

namespace SyncMaester.Core
{
    public interface IKontrol
    {
        ISettings Settings { get; }
        IDiffResult BuildDiff();
        void ProcessFolderDiff(IDiffResult folderDiff);
        void WriteSettings(IKoreFileInfo destination);
        void ReadSettings(IKoreFileInfo source);
    }
}