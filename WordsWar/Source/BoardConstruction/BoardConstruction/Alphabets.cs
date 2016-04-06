using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardConstruction
{
    // Class generated with http://json2csharp.com/

    /// <summary>
    /// An alphabet letter, with frequency and score modifier
    /// </summary>
    [Serializable()]
    public class Letter
    {
        public char character { get; set; }
        public double frequency { get; set; }
        public double score { get; set; }
    }

    /// <summary>
    /// An alphabet with a list of letters
    /// </summary>
    [Serializable()]
    public class Alphabet
    {
        public string id { get; set; }
        public List<Letter> letters { get; set; }
    }

    /// <summary>
    /// Retrieves alphabets from the alphabets.json config file in BoardData
    /// </summary>
    public class Alphabets
    {
        // Internal class used to serialize the json
        private static List<Alphabet> AlphabetList = new List<Alphabet>();

        public static Alphabet Load(string alphabetId)
        {
            return AlphabetList.FirstOrDefault((it) => it.id == alphabetId);
        }

        // Loads list of alphabets from json file path given by config file
        static Alphabets()
        {
            try
            {
                string filepath = Properties.Settings.Default.AlphabetsPath;
                string jsoncontent = File.ReadAllText(filepath);
                List<Alphabet> list = JsonConvert.DeserializeObject< List<Alphabet>>(jsoncontent);
                AlphabetList = list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
