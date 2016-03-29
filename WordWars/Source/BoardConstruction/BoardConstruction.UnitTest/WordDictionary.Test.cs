using BoardConstruction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BoardConstructionUnitTest
{
    public class WordDictionaryTest
    {
        [Fact]
        void TestDictLookup()
        {
            // bread and bred
            WordDictionary dict = new WordDictionary(new List<string> () { "bre", "bread", "bred" });
            DictionaryLookupInfo result = DictionaryLookupInfo.BuildLookupInfo(dict);
            Assert.Equal(6, result.LookupInfo.Count);
            Assert.Equal(result.LookupInfo["b"], new WordLookupInfoData() { HasChildren = true, IsWord = false });
            Assert.Equal(result.LookupInfo["br"], new WordLookupInfoData() { HasChildren = true, IsWord = false });
            Assert.Equal(result.LookupInfo["bre"], new WordLookupInfoData() { HasChildren = true, IsWord = true } );
            Assert.Equal(result.LookupInfo["brea"], new WordLookupInfoData() { HasChildren = true, IsWord = false });
            Assert.Equal(result.LookupInfo["bred"], new WordLookupInfoData() { HasChildren = false, IsWord = true });
            Assert.Equal(result.LookupInfo["bread"], new WordLookupInfoData() { HasChildren = false, IsWord = true });
        }

        [Fact]
        void FiveKWords()
        {
            var reader = new StreamReader(File.OpenRead(@".\5000Words.csv"));
            List<string> listA = new List<string>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                listA.Add(line);
            }

            WordDictionary dict = new WordDictionary(listA);
            DictionaryLookupInfo result = DictionaryLookupInfo.BuildLookupInfo(dict);

        }
    }
}
