using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net.SuddenElfilio.RilSharp;
using System.Web;
using SemanticProxy;
using log4net;
using System.Reflection;

namespace Pocket_AutoTagger
{
    class PocketTagger
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
            
            List<RilListItem> allModifiedItems = new List<RilListItem>();
            

            
            int pageSize = 10;
            int totalPages = (int)Math.Ceiling((decimal)items.Items.Count / (decimal)pageSize);

            Log.InfoFormat("Analyzing {0} items, {1} per set. Total sets: {2} ", items.Items.Count, pageSize, totalPages);


            for (int currentPage = 0; currentPage <= totalPages; currentPage++)
            {
                int currentOffset = currentPage * pageSize;

                var modifiedItemsSet = TagItems(items.Items.Skip(currentOffset).Take(pageSize));

                if (modifiedItemsSet.Count > 0)
                {
                    Log.InfoFormat("Tagging {0} articles in Pocket.", modifiedItemsSet.Count);
                    var result = Client.Send(SendType.Update_tags, modifiedItemsSet);
                    allModifiedItems.AddRange(modifiedItemsSet);
                }
                
            }
            return allModifiedItems;
        }

        private List<RilListItem> TagItems(IEnumerable<KeyValuePair<string,RilListItem>> items)
        {
            var modifiedItems = new List<RilListItem>();
            foreach (var item in items)
            {

                var applicableTagRules = TagRules.Where(i => i.HasMatch(item.Value));
                foreach (var tagRule in applicableTagRules)
                {
                    bool itemModified = AddTag(item.Value, tagRule.Tag);

                    if (itemModified)
                    {
                        modifiedItems.Add(item.Value);
                        Log.DebugFormat("{0}: {1}", tagRule.Tag, item.Value.Url);
                    }
                }

                if (SemanticProxyLookupEnabled)
                {
                    try
                    {
                        IEnumerable<string> cats = SemanticProxyClient.GetCategory(item.Value.Url);
                        foreach (string cat in cats)
                        {
                            if (cat.ToLower() != "other")
                            {
                                if (AddTag(item.Value, cat))
                                {
                                    modifiedItems.Add(item.Value);
                                    Log.DebugFormat("s-{0}: {1}", cat, item.Value.Url);
                                }
                            }
                        }
                    }
                    catch (System.Net.WebException we)
                    {
                        Log.Error("Failed Semantic Proxy lookup.", we);
                    }
                }

            }
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
