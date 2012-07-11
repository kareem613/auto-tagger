using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using SemanticProxy;
using SemanticProxy.Mocks;

namespace SemanticProxy.Ioc
{
    public class MockSemanticProxyModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISemanticProxyClient>().To<SemanticProxyClient>();
            Bind<ISemanticProxyProvider>().To<MockSemanticProxyProvider>();
        }

    }
}
