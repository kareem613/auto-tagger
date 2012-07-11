using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net.SuddenElfilio.RilSharp;
using System.Web;
using SemanticProxy;

namespace Pocket_AutoTagger
{
    class PocketTagger
    {
        public Credentials UserCredentials { get; set; }
        public IRilClient Client { get; set; }
        public ISemanticProxyClient SemanticProxyClient { get; set; }
        public IEnumerable<TagRule> TagRules { get; set; }
        public bool SemanticProxyLookupEnabled { get; set; }

        public PocketTagger(IRilClient client, ISemanticProxyClient semanticProxyClient)
        {
            this.Client = client;
            this.SemanticProxyClient = semanticProxyClient;
        }


        public List<RilListItem> UpdateUntaggedItems()
        {
            return UpdateUntaggedItems(null);
        }

        public List<RilListItem> UpdateUntaggedItems(DateTime? since)
        {
            if (TagRules == null)
            {
                throw new InvalidOperationException("No instructions loaded.");
            }

            var items = GetItemsToTag(since);
            
            List<RilListItem> modifiedItems = new List<RilListItem>();
            Console.WriteLine(String.Format("Retreived {0} items.", items.Items.Count));
            foreach (var item in items.Items)
            {
                
                var applicableTagRules = TagRules.Where(i => i.HasMatch(item.Value));
                foreach(var tagRule in applicableTagRules)
                {
                    bool itemModified = AddTag(item.Value,tagRule.Tag);

                    if (itemModified)
                    {
                        modifiedItems.Add(item.Value);
                        Console.WriteLine(String.Format("{0}: {1}", tagRule.Tag, item.Value.Url));
                    }
                }

                if (SemanticProxyLookupEnabled)
                {
                    try{
                    IEnumerable<string> cats = SemanticProxyClient.GetCategory(item.Value.Url);
                    foreach (string cat in cats)
                    {
                        if (cat.ToLower() != "other")
                        {
                            if (AddTag(item.Value, cat))
                            {
                                modifiedItems.Add(item.Value);
                                Console.WriteLine(String.Format("s-{0}: {1}", cat, item.Value.Url));
                            }
                        }
                    }
                    }
                    catch (System.Net.WebException)
                    {
                        //log this
                    }
                }
               
            }
            var result = Client.Send(SendType.Update_tags, modifiedItems);
            return modifiedItems;
        }

        public bool AddTag(RilListItem item, string newTag)
        {
            if (item.Tags!= null && item.Tags.Split(',').Contains(newTag))
            {
                return false;
            }
            if (!string.IsNullOrEmpty(item.Tags))
            {
                item.Tags = item.Tags + ",";
            }
            item.Tags = item.Tags + newTag;
            return true;
        }

        private RilList GetItemsToTag(DateTime? since)
        {
            return Client.Get(ReadState.All,since,null, null, false, true);
        }


        internal int LoadInstructions(string[] lines)
        {
            TagRulesLoader trl = new TagRulesLoader();
            TagRules = trl.LoadInstruction(lines);
            return TagRules.Count();
        }

        internal int LoadInstructions(string filename)
        {
            TagRulesLoader trl = new TagRulesLoader();
            TagRules = trl.LoadInstructionFile(filename);
            return TagRules.Count();
        }

        
    }
}
