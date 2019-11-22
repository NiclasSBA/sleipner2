using System;
using System.Collections.Generic;
using System.Text;

namespace Sleipner.Core.Netcore
{
    public class SleipnerProxy<TInterface> where TInterface : class
    {
        private readonly TInterface _implementation;

        public SleipnerProxy(TInterface implementation)
        {
            _implementation = implementation;
        }

        public TInterface WrapWith(IProxyHandler<TInterface> handler)
        {
            var proxyDelegatorType = IlGeneratorProxyGenerator.CreateProxyFor<TInterface>();
            var proxyInstance = (TInterface)Activator.CreateInstance(proxyDelegatorType, _implementation, handler);
            return proxyInstance;
        }
    }
}
