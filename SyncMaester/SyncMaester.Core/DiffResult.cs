using System;
using System.Collections.Generic;

namespace SyncMaester.Core
{
    public class DiffResult : IDiffResult
    {
        public DiffResult(List<IFolderDiffResult> folderDiffResults)
        {
            FolderDiffResults = folderDiffResults.AsReadOnly();
        }

        public IList<IFolderDiffResult> FolderDiffResults { get; }
    }
}