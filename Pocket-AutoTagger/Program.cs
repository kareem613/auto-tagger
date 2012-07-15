using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Net.SuddenElfilio.RilSharp;
using Ninject;
using Ninject.Activation;
using Pocket_AutoTagger.Ioc;

namespace Pocket_AutoTagger
{
    class Program
    {
        static void Main(string[] args)
        {
            IKernel kernel = new StandardKernel(new PocketModule(), new SemanticProxyModule());


            var pt = kernel.Get<PocketTagger>();
            
            pt.SemanticProxyLookupEnabled = true;
            pt.LoadInstructions("sample_tags.tgr");
            var modifiedItems = pt.UpdateUntaggedItems(DateTime.Now.Subtract(new TimeSpan(5,0,0,0)));
            Console.WriteLine(String.Format("Tagged {0} items.",modifiedItems.Count()));
            Console.ReadLine();
        }
    }

    
    

    

    
}



