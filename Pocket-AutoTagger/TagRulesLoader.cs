using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace Pocket_AutoTagger
{
    class TagRulesLoader
    {
        private const string REGEX = "(?<tag>.+):((?<match_type>.+)=(?<value>.+))";
        //android-apps: url=play.google.com
        internal TagRule LoadInstruction(string line)
        {
            var regexMatch = Regex.Match(line, REGEX);
            if (!regexMatch.Success)
            {
                throw new ArgumentException(String.Format("Invalid instruction line '{0}'.",line));
            }
            
            var instruction = new TagRule();
            var tag = regexMatch.Groups["tag"].Value;
            instruction.Tag = tag;

            var match = GetMatch(regexMatch);
            instruction.Matches.Add(match);
            return instruction;
            
            
        }

        private Match GetMatch(System.Text.RegularExpressions.Match match)
        {
            
            if (match.Groups["match_type"].Value == "url")
            {
                return new Match(MatchTypes.URL, match.Groups["value"].Value);
                
            }
            else if (match.Groups["match_type"].Value == "title")
            {
                return new Match(MatchTypes.Title, match.Groups["value"].Value);
            }
            else
            {
                throw new ArgumentException(String.Format("Invalid instruction line '{0}'.",match));
            }
        }

        internal IEnumerable<TagRule> LoadInstruction(string[] lines)
        {
            var tagRules = new List<TagRule>();
            foreach (var line in lines)
            {
                var tagRule = LoadInstruction(line);
                var existingTagRule = tagRules.Where(tr => tr.Tag == tagRule.Tag).FirstOrDefault();
                if (existingTagRule != null)
                {
                    existingTagRule.Matches.AddRange(tagRule.Matches);
                }
                else
                {
                    tagRules.Add(tagRule);
                }
            }
            return tagRules;
        }



        internal IEnumerable<TagRule> LoadInstructionFile(string filename)
        {
            return LoadInstruction(File.ReadAllLines(filename).Where(line=>!String.IsNullOrWhiteSpace(line)).ToArray());
        }
    }
}
