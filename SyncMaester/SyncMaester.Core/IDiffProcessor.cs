using Kore.IO.Sync;
using Kore.IO.Util;

namespace SyncMaester.Core
{
    public interface IDiffProcessor
    {
        void Process(IDiff diff, IDiffInfo diffInfo);
    }
}