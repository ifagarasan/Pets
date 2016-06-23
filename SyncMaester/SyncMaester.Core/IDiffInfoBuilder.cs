namespace SyncMaester.Core
{
    public interface IDiffInfoBuilder
    {
        IDiffInfo BuildInfo(ISyncPair syncPair);
    }
}