using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Net.SuddenElfilio.RilSharp;

namespace Pocket_AutoTagger.Tests
{
    [TestClass]
    public class TagRulesTests
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SingleLineInvalidFormatThrowsException()
        {
            TagRulesLoader trl = new TagRulesLoader();
            TagRule tagRule = trl.LoadInstruction("thiswillfail");
        }

        [TestMethod]
        public void SingleLineParsesTag()
        {
            string line = "android-apps:url=test1.com";
            TagRulesLoader trl = new TagRulesLoader();
            TagRule tagRule = trl.LoadInstruction(line);

            Assert.AreEqual("android-apps", tagRule.Tag);

        }

        [TestMethod]
        public void SingleUrlParsesUrl()
        {
            string line = "android-apps:url=test1";
            TagRulesLoader trl = new TagRulesLoader();
            TagRule tagRule = trl.LoadInstruction(line);
            var item = new RilListItem { Title = "test1", Url = "test1.com" };
            Assert.AreEqual(MatchTypes.URL, tagRule.Matches.First().MatchType);

        }

        [TestMethod]
        public void SingleTitleParsesTitle()
        {
            string line = "title-test:title=title1";
            TagRulesLoader trl = new TagRulesLoader();
            TagRule tagRule = trl.LoadInstruction(line);
            var item = new RilListItem { Title = "title1", Url = "test1.com" };
            Assert.AreEqual(MatchTypes.Title, tagRule.Matches.First().MatchType);

        }

        [TestMethod]
        public void SingleUrlMatchesUrl()
        {
            string line = "android-apps:url=test1";
            TagRulesLoader trl = new TagRulesLoader();
            TagRule tagRule = trl.LoadInstruction(line);
            var item = new RilListItem { Title = "test1", Url = "test1.com" };
            Assert.IsTrue(tagRule.HasMatch(item));

        }

        [TestMethod]
        public void SingleTitleMatchTitle()
        {
            string line = "title-test:title=title1";
            TagRulesLoader trl = new TagRulesLoader();
            TagRule tagRule = trl.LoadInstruction(line);
            var item = new RilListItem { Title = "title1", Url = "test1.com" };
            Assert.IsTrue(tagRule.HasMatch(item));

        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SingleUnsupportedMatchTypeThrowsException()
        {
            string line = "title-test:notvalid=title1";
            TagRulesLoader trl = new TagRulesLoader();
            TagRule tagRule = trl.LoadInstruction(line);
        }

        [TestMethod]
        public void MultipleLinesParsesAll()
        {
            string[] lines = new string[] { "test-tag1:url=test1.com", "test-tag2:url=test2.com" };
            TagRulesLoader trl = new TagRulesLoader();
            IEnumerable<TagRule> tagRule = trl.LoadInstruction(lines);

            Assert.AreEqual(lines.Length, tagRule.Count());

        }

        [TestMethod]
        public void DuplicateTagsCollapsesToOneTagRuleTwoMatches()
        {
            string[] lines = new string[] { "test-tag:url=test1.com", "test-tag:url=test2.com" };
            TagRulesLoader trl = new TagRulesLoader();
            IEnumerable<TagRule> tagRule = trl.LoadInstruction(lines);

            Assert.AreEqual(1, tagRule.Count());
            Assert.AreEqual(2, tagRule.First().Matches.Count);

        }

        [TestMethod]
        [DeploymentItem(@"Pocket-AutoTagger.Tests\testrules.tgr")]
        public void LoadFile()
        {
            TagRulesLoader trl = new TagRulesLoader();
            string filename = "testrules.tgr";
            IEnumerable<TagRule> tagRules = trl.LoadInstructionFile(filename);

            int expectedTagRules = File.ReadAllLines(filename).Where(line=>!String.IsNullOrWhiteSpace(line)).Count();
            Assert.AreEqual(expectedTagRules, tagRules.Count());
        }

    }
}
