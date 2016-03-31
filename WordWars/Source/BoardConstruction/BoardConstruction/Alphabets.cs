using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardConstruction
{
    // generated with http://json2csharp.com/
    public class Letter
    {
        public string character { get; set; }
        public string frequency { get; set; }
        public string score { get; set; }
    }

    public class Alphabet
    {
        public string id { get; set; }
        public string characterstring { get; set; }
        public List<Letter> letters { get; set; }
    }

    class AlphabetsData
    {
        public List<Alphabet> alphabets { get; set; }
    }

    public class Alphabets
    {
        public static string EnUs = "abcdefghijklmnopqrstuvwxyz";
        public static string ItIt = "abcdefghiklmnopqrstuvz";

        public static List<Alphabet> AlphabetList { get; private set; }

        //TODO: Add error management with file system and file contents
        public static void LoadAlphabets(string filepath)
        {
            string jsoncontent = File.ReadAllText(filepath);
            AlphabetsData container = JsonConvert.DeserializeObject<AlphabetsData>(jsoncontent);
            AlphabetList = container.alphabets;
        }
    }
    // TODO: Must be accent insensitive(for example, map e' to e and so on)
}
