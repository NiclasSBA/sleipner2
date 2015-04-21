﻿namespace Sleipner.Cache.Policies
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