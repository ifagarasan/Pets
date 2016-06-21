using Kore.IO.Sync;

namespace SyncMaester.Core
{
    public interface IDiffInfoBuilder
    {
        IDiffInfo BuildInfo(IFolderDiffResult folderDiffResult);
    }
}