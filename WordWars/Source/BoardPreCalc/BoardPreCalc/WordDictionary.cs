using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardPreCalc
{
    public class WordDictionary
    {
        public List<string> Words;
        public const int MinWordLength = 3;

        public WordDictionary(List<string> words)
        {
            Words = words;
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
        public WordLookupInfo BuildLookupInfo()
        {
            WordLookupInfo result = new WordLookupInfo();

            foreach (string w in Words)
            {
                // only words MinWordLength char or more.
                if (w.Length < MinWordLength) break;

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
