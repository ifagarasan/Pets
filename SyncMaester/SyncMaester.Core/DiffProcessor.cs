using Kore.IO.Sync;

namespace SyncMaester.Core
{
    public class DiffProcessor : IDiffProcessor
    {
        public void Process(IDiff folderDiff)
        {
            folderDiff.SourceFileInfo.Copy(folderDiff.DestinationFileInfo);
        }
    }
}