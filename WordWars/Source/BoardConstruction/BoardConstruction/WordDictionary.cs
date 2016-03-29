using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardConstruction
{
    public class WordDictionary
    {
        public readonly List<string> Words;
        public readonly int MinWordLength = 3;

        // dictionary will work lower case - no matter the parameters passed.
        public WordDictionary(List<string> inputWords)
        {
            Words = new List<string>();
            foreach (var word in inputWords)
            {
                Words.Add(word.ToLower());
            }
        }

        public WordDictionary(List<string> inputWords, int minwordlength) : this(inputWords)
        {
            MinWordLength = minwordlength;
        }
    }
}
