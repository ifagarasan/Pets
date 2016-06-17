using System.IO;
using Kore.IO.Sync;
using Kore.IO.Util;

namespace SyncMaester.Core
{
    public class DiffProcessor : IDiffProcessor
    {
        public void Process(IDiff diff, IKoreFolderInfo source, IKoreFolderInfo destination)
        {
            if (diff.Type == DiffType.SourceNew)
            {
                string sourceInnerFullPath = BuildRelativePath(diff.SourceFileInfo, source);
                IKoreFileInfo target = new KoreFileInfo(Path.Combine(destination.FullName, sourceInnerFullPath));

                diff.SourceFileInfo.Copy(target);
            }
            else if (diff.Type == DiffType.SourceNewer)
            {
                diff.SourceFileInfo.Copy(diff.DestinationFileInfo);
            }
        }

        private static string BuildRelativePath(IKoreFileInfo folderInfo, IKoreFolderInfo parent)
        {
            return folderInfo.FullName.Substring(parent.FullName.Length).TrimStart('\\');
        }
    }
}