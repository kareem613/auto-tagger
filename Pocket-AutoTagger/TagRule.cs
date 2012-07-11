using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net.SuddenElfilio.RilSharp;

namespace Pocket_AutoTagger
{
    public enum MatchTypes { URL, Title, Unsupported };
    class TagRule
    {
        virtual public string Tag { get; set; }
        public List<Match> Matches { get; set; }

        public TagRule()
        {
            Matches = new List<Match>();
        }
        virtual internal bool HasMatch(RilListItem item)
        {
            return Matches.Any(m => m.MatchesItem(item));
        }
    }
}
