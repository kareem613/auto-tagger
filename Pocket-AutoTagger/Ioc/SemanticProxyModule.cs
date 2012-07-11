using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using SemanticProxy;
using SemanticProxy.Ioc;

namespace Pocket_AutoTagger.Ioc
{
    class SemanticProxyModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISemanticProxyClient>().To<SemanticProxyClient>();
            Bind<ISemanticProxyProvider>().ToProvider<SemanticProxyProviderProvider>();
        }

    }
}
