using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SyncMaester.Core
{
    public interface ISettings
    {
        IList<ISyncPair> SyncPairs { get; }
    }
}