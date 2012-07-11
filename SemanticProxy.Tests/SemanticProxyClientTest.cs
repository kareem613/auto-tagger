using SemanticProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Ninject;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using SemanticProxy.Ioc;
namespace SemanticProxy.Tests
{
    
    
    /// <summary>
    ///This is a test class for SemanticProxyClientTest and is intended
    ///to contain all SemanticProxyClientTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SemanticProxyClientTest
    {

        static IKernel kernel;
        
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            kernel = new StandardKernel(new MockSemanticProxyModule());
        }
        
        
        [TestMethod()]
        [DeploymentItem(@"SemanticProxy.Tests\sample_response.xml")]
        public void QueryUrl()
        {
            ISemanticProxyClient target = kernel.Get<ISemanticProxyClient>();
            var results = target.QueryUrl("www.doesntmatter.com");
            Assert.IsNotNull(results);
            
        }

        [TestMethod()]
        [DeploymentItem(@"SemanticProxy.Tests\sample_response.xml")]
        public void GetCategory()
        {
            ISemanticProxyClient target = kernel.Get<ISemanticProxyClient>();
            var result = target.GetCategory("www.doesntmatter.com");
            Assert.AreEqual("Business_Finance",result.Single());

        }

        [TestMethod()]
        public void GetCategoryFromLiveService()
        {
            ISemanticProxyClient target = new SemanticProxyClient(
                new SemanticProxyProvider() { ApiKey = ConfigurationManager.AppSettings["semanticproxy_api_key"] }
            );

            var result = target.GetCategory("http://blogs.hbr.org/cs/2012/06/cracks_starting_to_show_at_app.html");
            Assert.IsTrue(result.Any(c=>c == "Business_Finance"));

        }
    }
}
