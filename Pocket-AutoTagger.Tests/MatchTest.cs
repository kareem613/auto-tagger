using Pocket_AutoTagger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Net.SuddenElfilio.RilSharp;

namespace Pocket_AutoTagger.Tests
{
    
    
    /// <summary>
    ///This is a test class for MatchTest and is intended
    ///to contain all MatchTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MatchTest
    {


        /// <summary>
        ///A test for Match Constructor
        ///</summary>
        [TestMethod()]
        public void MatchesUrl()
        {
            Match m = new Match(MatchTypes.URL, "test");
            var item = new RilListItem { Title = "test1", Url = "test.com" };

            Assert.IsTrue(m.MatchesItem(item));
        }


    }
}
