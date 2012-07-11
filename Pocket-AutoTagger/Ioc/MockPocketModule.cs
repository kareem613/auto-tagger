using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net.SuddenElfilio.RilSharp;
using Ninject.Modules;
using Pocket_AutoTagger.Ioc;
using Pocket_AutoTagger.Mocks;

namespace Pocket_AutoTagger.Ioc
{
    class MockPocketModule : NinjectModule
    {
        public override void Load()
        {
            Bind<PocketTagger>().To<PocketTagger>();
            Bind<IRilClient>().To<MockRilClient>();
            Bind<Credentials>().ToProvider<CredentialProvider>();
        }

    }
}
