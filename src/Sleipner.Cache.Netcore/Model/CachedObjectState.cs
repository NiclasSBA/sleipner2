using System;
using System.Collections.Generic;
using System.Text;

namespace Sleipner.Cache.Netcore.Model
{
    public enum CachedObjectState
    {
        Fresh,
        Stale,
        Exception,
        None
    }
}