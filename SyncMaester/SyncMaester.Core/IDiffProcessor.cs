using Kore.IO.Sync;

namespace SyncMaester.Core
{
    public interface IDiffProcessor
    {
        void Process(IDiff diff);
    }
}