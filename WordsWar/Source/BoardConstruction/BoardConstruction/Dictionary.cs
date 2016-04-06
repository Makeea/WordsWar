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

    public class Dictionary
    {
        public string id { get; set; } = "";
        public readonly List<string> Words;
        public readonly int MinWordLength = 3;

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
    }

    /// <summary>
    /// Retrievest list of dictionary file names from the configuration, then loads them dynamically
    /// </summary>
    public class Dictionaries
    {
        public class DictionaryInfo
        {
            public string id { get; set; }
            public string alphabetid { get; set; }
            public string filename { get; set; }
            [System.Xml.Serialization.XmlIgnore()]
            public Dictionary dict { get; set; } = null;
        }

        // Internal class used to serialize the json
        private static List<DictionaryInfo> DictionaryList = new List<DictionaryInfo>();

        
        public static Dictionary Load(string dictionaryId)
        {
            DictionaryInfo info = DictionaryList.First((it) => it.id == dictionaryId);
            Dictionary result = null;
            if (info != null)
            {
                if (info.dict == null)
                {
                    try
                    {
                        var reader = new StreamReader(File.OpenRead(info.filename));
                        List<string> listA = new List<string>();
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            listA.Add(line);
                        }
                        info.dict = new Dictionary(listA);
                    }
                    catch
                    {
                        info.dict = new Dictionary(new List<string>() { });
                    }
                }
                result = info.dict;
                result.id = info.id;
            }
            return result;
        }

        // Loads list of alphabets from json file path given by config file
        static Dictionaries()
        {
            try
            {
                string filepath = Properties.Settings.Default.DictionaryInfoPath;
                string jsoncontent = File.ReadAllText(filepath);
                List<DictionaryInfo> list = JsonConvert.DeserializeObject<List<DictionaryInfo>>(jsoncontent);
                DictionaryList = list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
