using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardConstruction
{
    public struct AlphabetInfo
    {
        char character;
        double frequency;
        int score;
    }

    public class Alphabets
    {
        List<AlphabetInfo> Elements = new List<AlphabetInfo>();

        public static string EnUs = "abcdefghijklmnopqrstuvwxyz";
        public static string ItIt = "abcdefghiklmnopqrstuvz";
    }

    // TODO: Generation should be weighted on the frequency of each letter per language
    // TODO: The score should be adjusted to reuse letters that 
    // TODO: Must be accent insensitive(for example, map e' to e and so on)
}
