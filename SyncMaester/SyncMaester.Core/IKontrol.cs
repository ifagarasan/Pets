using System.Threading.Tasks;
using Kore.IO;

namespace SyncMaester.Core
{
    public interface IKontrol
    {
        ISettings Settings { get; }
        IScanInfo ScanInfo { get; }

        void Sync();

        void WriteSettings(IKoreFileInfo destination);
        void ReadSettings(IKoreFileInfo source);
    }
}