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
        public List<string> Words;
        public const int MinWordLength = 3;

        public WordDictionary(List<string> words)
        {
            Words = words;
        }
    }
}
