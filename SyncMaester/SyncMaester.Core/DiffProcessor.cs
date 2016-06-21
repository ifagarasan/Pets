using System.IO;
using Kore.IO.Sync;
using Kore.IO.Util;
using static Kore.Validation.ObjectValidation;

namespace SyncMaester.Core
{
    public class DiffProcessor : IDiffProcessor
    {
        public void Process(IDiff diff, IDiffInfo diffInfo)
        {
            IsNotNull(diff, nameof(diff));
            IsNotNull(diffInfo, nameof(diffInfo));

            var source = diffInfo.Source;
            var destination = diffInfo.Destination;

            IsNotNull(source, nameof(source));
            IsNotNull(destination, nameof(destination));

            if (diff.Type == DiffType.SourceNew)
            {
                var sourceInnerFullPath = BuildRelativePath(diff.SourceFileInfo, source);
                var target = new KoreFileInfo(Path.Combine(destination.FullName, sourceInnerFullPath));

                diff.SourceFileInfo.Copy(target);
            }
            else if (diff.Type == DiffType.SourceNewer)
                diff.SourceFileInfo.Copy(diff.DestinationFileInfo);
            else if (diff.Type == DiffType.SourceOlder)
                diff.DestinationFileInfo.Copy(diff.SourceFileInfo);
            else if (diff.Type == DiffType.DestinationOrphan)
                diff.DestinationFileInfo.Delete();
        }

        private static string BuildRelativePath(IKoreFileInfo folderInfo, IKoreFolderInfo parent)
        {
            return folderInfo.FullName.Substring(parent.FullName.Length).TrimStart('\\');
        }
    }
}