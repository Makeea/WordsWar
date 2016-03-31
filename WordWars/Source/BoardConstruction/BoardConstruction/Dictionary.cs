using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardConstruction
{


    public class DictionaryInfo
    {
        public string id { get; set; }
        public string alphabetid { get; set; }
        public string filename { get; set; }
    }

    public class DictionaryInfoData
    {
        public List<DictionaryInfo> dictionaryinfo { get; set; }
    }

    public class Dictionary
    {
        public readonly List<string> Words;
        public readonly int MinWordLength = 3;
        private static List<DictionaryInfo> DictionaryInfoList { get; set; }

        // dictionary will work lower case - no matter the parameters passed.
        public Dictionary(List<string> inputWords)
        {
            Words = new List<string>();
            foreach (var word in inputWords)
            {
                Words.Add(word.ToLower());
            }
        }

        public Dictionary(List<string> inputWords, int minwordlength) : this(inputWords)
        {
            MinWordLength = minwordlength;
        }

        //TODO: Add error management with file system and file contents
        public static Dictionary LoadFromInfo(string infoid)
        {
            DictionaryInfo di = DictionaryInfoList.First((it) => it.id == infoid);
            if (di != null)
            {
                var reader = new StreamReader(File.OpenRead(di.filename));
                List<string> listA = new List<string>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    listA.Add(line);
                }
                Dictionary dict = new Dictionary(listA);
                return dict;
            }
            return null;
        }

        //TODO: Add error management with file system and file contents
        public static void LoadDictionaryInfo(string filepath)
        {
            string jsoncontent = File.ReadAllText(filepath);
            DictionaryInfoData container = JsonConvert.DeserializeObject<DictionaryInfoData>(jsoncontent);
            DictionaryInfoList = container.dictionaryinfo;

        }
    }
}
