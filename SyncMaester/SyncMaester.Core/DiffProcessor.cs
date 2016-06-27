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
            IsNotNull(diff);

            if (diff.Relation == DiffRelation.Identical)
                return;

            if (diff.Relation == DiffRelation.DestinationOrphan)
            {
                diff.Destination.Delete();
                return;
            }

            IsNotNull(source);
            IsNotNull(destination);

            var sourceFileInfo = diff.Source;
            var destinationFileInfo = diff.Destination;

            if (diff.Relation == DiffRelation.SourceNew)
            {
                var sourceInnerFullPath = BuildRelativePath(diff.Source, source);
                destinationFileInfo = new KoreFileInfo(Path.Combine(destination.FullName, sourceInnerFullPath));
            }
            else if (diff.Relation == DiffRelation.SourceOlder)
            {
                sourceFileInfo = destinationFileInfo;
                destinationFileInfo = diff.Source;
            }

            _fileCopier.Copy(sourceFileInfo, destinationFileInfo);
        }

        private static string BuildRelativePath(IKoreFileInfo folderInfo, IKoreFolderInfo parent)
        {
            return folderInfo.FullName.Substring(parent.FullName.Length).TrimStart('\\');
        }
    }
}