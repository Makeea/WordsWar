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
            Dictionary dict = new Dictionary(new List<string> () { "bre", "bread", "bred" });
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
            Dictionary.LoadDictionaryInfo("./BoardData/dictionaryinfo.json");
            Dictionary dict = Dictionary.LoadFromInfo("5kwords");
            Assert.Equal(5000, dict.Words.Count);
            DictionaryLookupInfo result = DictionaryLookupInfo.BuildLookupInfo(dict);

        }
    }
}
