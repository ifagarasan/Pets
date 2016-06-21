using System.Collections.Generic;

namespace SyncMaester.Core
{
    public interface IDiffResult
    {
        IList<IFolderDiffResult> FolderDiffResults { get; }
    }
}