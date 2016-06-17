using System.IO;
using Kore.IO.Sync;
using Kore.IO.Util;

namespace SyncMaester.Core
{
    public class DiffProcessor : IDiffProcessor
    {
        public void Process(IDiff diff, IKoreFolderInfo source, IKoreFolderInfo destination)
        {
            diff.SourceFileInfo.Copy(SelectTarget(diff, source, destination));
        }

        private static IKoreFileInfo SelectTarget(IDiff diff, IKoreFolderInfo source, IKoreFolderInfo destination)
        {
            IKoreFileInfo target = diff.DestinationFileInfo;

            if (diff.Type == DiffType.SourceNew)
            {
                string sourceInnerFullPath = BuildRelativePath(diff.SourceFileInfo, source);
                target = new KoreFileInfo(Path.Combine(destination.FullName, sourceInnerFullPath));
            }

            return target;
        }

        private static string BuildRelativePath(IKoreFileInfo folderInfo, IKoreFolderInfo parent)
        {
            return folderInfo.FullName.Substring(parent.FullName.Length).TrimStart('\\');
        }
    }
}