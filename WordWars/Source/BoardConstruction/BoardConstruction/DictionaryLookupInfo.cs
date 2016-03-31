using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardConstruction
{
    public class WordLookupInfoData
    {
        public bool IsWord = false;
        public bool HasChildren = false;

        public override bool Equals(object obj)
        {
            if (obj is WordLookupInfoData)
            {
                WordLookupInfoData other = (WordLookupInfoData) obj;
                return ((other.IsWord == IsWord) && (other.HasChildren == HasChildren));
            }
            else
                return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class DictionaryLookupInfo
    {
        public Dictionary<string, WordLookupInfoData> LookupInfo = new Dictionary<string, WordLookupInfoData>();
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (var item in LookupInfo)
            {
                result.AppendFormat("Key: {0} \t\tIsWord: {1} \t\tHasChildren: {2}", item.Key, item.Value.IsWord, item.Value.HasChildren);
                result.AppendLine();
            }
            return result.ToString();
        }

        // To make things fast, I should break down all the words in the dictionary in sequence of letters, as an indication there is a word I want, for example:
        // each entry should tellme whether there is a word that ends at that entry and whether there are subwords
        // if the dictionary contains the word: bread, then I need the following:
        //	entry	| IsWord	| HasChildren
        //	b		  n			  y
        //	br		  n			  y
        //	bre		  n			  y
        //	brea	  n			  y
        //	bread	  y			  y

        // if the dictionary contains bread and bred then:
        //	entry	| IsWord	| HasChildren
        //	b		  n			  y
        //	br		  n			  y
        //	bre		  n			  y
        //	brea	  n			  y
        //	bred	  y			  y
        //	bread	  y			  y
        public static DictionaryLookupInfo BuildLookupInfo(Dictionary dictionary)
        {
            DictionaryLookupInfo result = new DictionaryLookupInfo();

            foreach (string w in dictionary.Words)
            {
                // only words MinWordLength char or more.
                if (w.Length < dictionary.MinWordLength) break;

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < w.Length; i++)
                {
                    sb.Append(w[i]);

                    WordLookupInfoData id = new WordLookupInfoData();
                    string key = sb.ToString();
                    if (result.LookupInfo.ContainsKey(key))
                    {
                        id = result.LookupInfo[key];
                    }

                    id.IsWord |= (i == (w.Length - 1));
                    id.HasChildren |= (i < (w.Length - 1));

                    result.LookupInfo[key] = id;
                }
            }

            return result;
        }

    }
}
