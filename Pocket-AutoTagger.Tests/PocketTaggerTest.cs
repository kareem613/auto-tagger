using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Net.SuddenElfilio.RilSharp;
using Ninject;
using SemanticProxy;
using Pocket_AutoTagger.Ioc;
using SemanticProxy.Ioc;

namespace Pocket_AutoTagger.Tests
{
    
    
    /// <summary>
    ///This is a test class for PocketTaggerTest and is intended
    ///to contain all PocketTaggerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PocketTaggerTest
    {
        IKernel kernel;
        [TestInitialize]
        public void BeginTests()
        {
            kernel = new StandardKernel(new MockPocketModule(), new MockSemanticProxyModule());
        }

        [TestMethod]
        public void LoadInstructions()
        {
            var pt = kernel.Get<PocketTagger>();
            string[] lines = new string[] { "test-tag1:url=test1.com", "test-tag2:url=test2.com" };
            var numLoaded = pt.LoadInstructions(lines);

            Assert.AreEqual(lines.Length, numLoaded);
            
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NoTagRulesLoadedUpdateUntaggedItemsThrowsTest()
        {
            var pt = kernel.Get<PocketTagger>();
            pt.UpdateUntaggedItems();
        }

        [TestMethod()]
        public void UpdateUntaggedItemsWithoutSemanticLookup()
        {
            var pt = kernel.Get<PocketTagger>();
            pt.SemanticProxyLookupEnabled = false;
            string[] lines = new string[] { "test-tag1:url=www.web1.com", "test-tag2:url=www.web2.com", "test-tag3:url=www.web3.com" };
            pt.LoadInstructions(lines);
            var modifiedItems = pt.UpdateUntaggedItems(DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)));
            Assert.AreEqual(0,modifiedItems.Count(i => i.Tags.Contains("Business_Finance")));
        }

        [TestMethod()]
        [DeploymentItem(@"Pocket-AutoTagger.Tests\sample_response.xml")]
        public void UpdateUntaggedItemsWithSemanticLookup()
        {
            var pt = kernel.Get<PocketTagger>();
            pt.SemanticProxyLookupEnabled = true;
            string[] lines = new string[] { "test-tag1:url=www.web1.com", "test-tag2:url=www.web2.com", "test-tag3:url=www.web3.com" };
            pt.LoadInstructions(lines);
            var modifiedItems = pt.UpdateUntaggedItems(DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)));
            Assert.IsTrue(modifiedItems.First().Tags.Contains("Business_Finance"));
        }

        [TestMethod()]
        public void AddFirstTag()
        {
            var pt = kernel.Get<PocketTagger>();
            var item = new RilListItem { Url = "http://www.google.be?2" };

            string newTag = "newTag";
            pt.AddTag(item, newTag);

            Assert.IsTrue(item.Tags.Split(',').Contains(newTag));
        }

        [TestMethod()]
        public void AddNextTag()
        {
            var pt = kernel.Get<PocketTagger>();
            var item = new RilListItem { Url = "http://www.google.be?2", Tags = "test"} ;
            
            string newTag = "newTag";
            pt.AddTag(item, newTag);

            Assert.IsTrue(item.Tags.Split(',').Contains(newTag));
        }

        [TestMethod()]
        public void AddDuplicateTagDoesNothing()
        {
            var pt = kernel.Get<PocketTagger>();
            var item = new RilListItem { Url = "http://www.google.be?2", Tags = "test" };

            string newTag = "test";
            bool tagModified = pt.AddTag(item, newTag);

            Assert.IsFalse(tagModified);
            Assert.AreEqual(1, item.Tags.Split(',').Count());
        }

        [TestMethod()]
        public void AddExistingTagDoesNotDuplicate()
        {
            var pt = kernel.Get<PocketTagger>();
            var item = new RilListItem { Url = "http://www.google.be?2", Tags = "test" };

            string newTag = "test";
            pt.AddTag(item, newTag);

            Assert.AreEqual(1,item.Tags.Split(',').Count(t=>t == newTag));
        }

        [TestMethod]
        [DeploymentItem(@"Pocket-AutoTagger.Tests\testrules.tgr")]
        public void LoadFile()
        {
            var pt = kernel.Get<PocketTagger>();
            string filename = "testrules.tgr";
            int tagRulesLoaded = pt.LoadInstructions(filename);

            int expectedTagRules = File.ReadAllLines(filename).Where(line => !String.IsNullOrWhiteSpace(line)).Count();
            Assert.AreEqual(expectedTagRules, tagRulesLoaded);
        }
    }
}
