using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Net.SuddenElfilio.RilSharp;
using Ninject;
using Ninject.Activation;
using Pocket_AutoTagger.Ioc;
using log4net;
using System.Reflection;

namespace Pocket_AutoTagger
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            Log.Info("Starting up.");
            IKernel kernel = new StandardKernel(new PocketModule(), new SemanticProxyModule());
            var pt = kernel.Get<PocketTagger>();

            pt.SemanticProxyLookupEnabled = false;

            Log.InfoFormat("SemanticProxyLookupEnabled: {0}", pt.SemanticProxyLookupEnabled);

            int tagRuleCount = pt.LoadInstructions("sample_tags.tgr");

            Log.InfoFormat("Loaded {0} tag rules.", tagRuleCount);

            var searchSinceDate = DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0));

            Log.InfoFormat("AutoTagging articles since {0}.", searchSinceDate.ToShortDateString());

            var modifiedItems = pt.UpdateUntaggedItems(searchSinceDate);
            Log.InfoFormat("Tagged {0} items.",modifiedItems.Count());
            Console.ReadLine();
        }
    }

    
    

    

    
}



