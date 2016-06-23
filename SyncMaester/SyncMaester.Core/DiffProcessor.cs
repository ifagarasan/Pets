using System.IO;
using Kore.IO;
using Kore.IO.Management;
using Kore.IO.Sync;
using static Kore.Validation.ObjectValidation;

namespace SyncMaester.Core
{
    public class DiffProcessor : IDiffProcessor
    {
        private readonly IFileCopier _fileCopier;

        public DiffProcessor(IFileCopier fileCopier)
        {
            IsNotNull(fileCopier);

            _fileCopier = fileCopier;
        }

        public void Process(IDiff diff, IKoreFolderInfo source, IKoreFolderInfo destination)
        {
            IsNotNull(diff, nameof(diff));

            if (diff.Type == DiffType.Identical)
                return;

            if (diff.Type == DiffType.DestinationOrphan)
            {
                diff.DestinationFileInfo.Delete();
                return;
            }

            IsNotNull(source, nameof(source));
            IsNotNull(destination, nameof(destination));

            var sourceFileInfo = diff.SourceFileInfo;
            var destinationFileInfo = diff.DestinationFileInfo;

            if (diff.Type == DiffType.SourceNew)
            {
                var sourceInnerFullPath = BuildRelativePath(diff.SourceFileInfo, source);
                destinationFileInfo = new KoreFileInfo(Path.Combine(destination.FullName, sourceInnerFullPath));
            }
            else if (diff.Type == DiffType.SourceOlder)
            {
                sourceFileInfo = destinationFileInfo;
                destinationFileInfo = diff.SourceFileInfo;
            }

            _fileCopier.Copy(sourceFileInfo, destinationFileInfo);
        }

        private static string BuildRelativePath(IKoreFileInfo folderInfo, IKoreFolderInfo parent)
        {
            return folderInfo.FullName.Substring(parent.FullName.Length).TrimStart('\\');
        }
    }
}