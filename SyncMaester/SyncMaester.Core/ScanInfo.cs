using System.ComponentModel;
using System.Runtime.CompilerServices;
using Kore.IO;
using SyncMaester.Core.Annotations;

namespace SyncMaester.Core
{
    public class ScanInfo : IScanInfo, INotifyPropertyChanged
    {
        private uint _sourceFiles;
        private uint _destinationFiles;

        public uint SourceFiles
        {
            get { return _sourceFiles; }
            set { _sourceFiles = value; OnPropertyChanged(nameof(SourceFiles)); }
        }

        public uint DestinationFiles
        {
            get { return _destinationFiles; }
            set { _destinationFiles = value; OnPropertyChanged(nameof(DestinationFiles)); }
        }

        public void Clear()
        {
            SourceFiles = DestinationFiles = 0;
        }

        public void NewSourceFileFound(IKoreFileInfo file)
        {
            SourceFiles++;
        }

        public void NewDestinationFileFound(IKoreFileInfo file)
        {
            DestinationFiles++;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}