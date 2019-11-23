using System;
using System.Collections.Generic;
using System.Text;

namespace Sleipner.Cache.Netcore.RedisSharp
{
   public class RedisObject<TObject>
    {
        public TObject Object;
        public bool IsException;
        public DateTime Created;
        public Exception Exception;
    }
}
