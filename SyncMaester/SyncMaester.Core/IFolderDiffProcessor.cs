namespace SyncMaester.Core
{
    public interface IFolderDiffProcessor
    {
        void Process(IFolderDiffResult folderDiff);
    }
}