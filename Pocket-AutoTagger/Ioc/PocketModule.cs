using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net.SuddenElfilio.RilSharp;

namespace Pocket_AutoTagger.Ioc
{
    class PocketModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<PocketTagger>().To<PocketTagger>();
            Bind<IRilClient>().To<RilClient>();
            Bind<Credentials>().ToProvider<CredentialProvider>();
        }

    }
}
