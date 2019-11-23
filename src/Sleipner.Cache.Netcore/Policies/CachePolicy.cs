using System;
using System.Collections.Generic;
using System.Text;

namespace Sleipner.Cache.Netcore.Policies
{
    public class CachePolicy
    {
        public int CacheDuration;
        public int MaxAge;
        public int ExceptionCacheDuration = 10;
        public bool BubbleExceptions;
        public bool DiscardStale;
    }
}
