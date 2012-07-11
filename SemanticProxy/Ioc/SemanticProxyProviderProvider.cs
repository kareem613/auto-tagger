using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Activation;
using System.Configuration;

namespace SemanticProxy.Ioc
{
    public class SemanticProxyProviderProvider : IProvider
    {
        protected SemanticProxyProvider CreateInstance(IContext context)
        {
            var spp = new SemanticProxyProvider() { ApiKey = ConfigurationManager.AppSettings["semanticproxy_api_key"] };
            return spp;
        }

        public object Create(IContext context)
        {
            return CreateInstance(context);
        }

        public Type Type
        {
            get { return typeof(SemanticProxyProvider); }
        }
    }
}
