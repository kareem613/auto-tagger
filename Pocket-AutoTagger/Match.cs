using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net.SuddenElfilio.RilSharp;

namespace Pocket_AutoTagger
{
    class Match
    {
        public MatchTypes MatchType { get; set; }
        public string MatchValue { get; set; }

        public Match(MatchTypes matchTypes, string matchValue)
        {
            this.MatchType = matchTypes;
            this.MatchValue = matchValue;
        }

        internal bool MatchesItem(RilListItem item)
        {
            switch (MatchType)
            {
                case MatchTypes.URL:
                    return MatchesUrl(item.Url);
                case MatchTypes.Title:
                    return MatchesTitle(item.Title);
                default:
                    throw new NotSupportedException(String.Format("Match type {0} not supported.", MatchType));
            }
        }

        private bool MatchesTitle(string title)
        {
            return title.ToLower().Contains(MatchValue.ToLower());
        }

        
        private bool MatchesUrl(string url)
        {
            return url.ToLower().Contains(MatchValue.ToLower());
        }
    }
}
