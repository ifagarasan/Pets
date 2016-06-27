using Kore.IO;

namespace SyncMaester.Core
{
    public interface IKontrol
    {
        ISettings Settings { get; }
        
        void Sync();

        void WriteSettings(IKoreFileInfo destination);
        void ReadSettings(IKoreFileInfo source);
    }
}