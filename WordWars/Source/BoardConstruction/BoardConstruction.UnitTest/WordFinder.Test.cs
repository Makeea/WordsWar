using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using BoardConstruction;
using Xunit.Extensions;

namespace BoardConstruction.UnitTest
{
    public class WordFinderTest
    {
        public static List<object[]> ThreeByThreeSample()
        {
            GameBoard ttsample = new GameBoard(new char[3, 3]
            {
                { 'y', 'o', 'x' },
                { 'r', 'b', 'a' },
                { 'v', 'e', 'd' }
            });

            GameBoard onecharboard = new GameBoard(new char[1, 1] {{ 'c' }});

			GameBoard mixedCase = new GameBoard(new char[3, 3]
            {
                { 'y', 'O', 'x' },
                { 'r', 'B', 'a' },
                { 'V', 'e', 'd' }
            });

            List<object[]> res = new List<object[]>()
            {
			    // find the most simple word, top left
                new object[] { ttsample, 1,
                    new List<string>() { "y" },
                    new List<string>() { "y" } },

			    // ok, use the second cell now
                new object[] { ttsample, 1,
                    new List<string>() { "y", "yo" },
                    new List<string>() { "y", "yo" } },

			    // mix of words that are there and are not, use the top left corner
                new object[] { ttsample, 1,
                    new List<string>() { "y", "yo", "yox", "yor", "yoyo", "od" },
                    new List<string>() { "y", "yo", "yox", "yor" } },

			    // should not find words that are not there, repetitions on the dictionary should not affect
                new object[] { ttsample, 1,
                    new List<string>() { "balla", "balla", "balerino", "tutto", "il", "giorno", "fino", "al", "mattino" },
                    new List<string>() { } },

				// should not find word that use repetitions
                new object[] { ttsample, 1,
                    new List<string>() { "bob", "boxo", "roboa", "vede" },
                    new List<string>() { } },

				// find some words in the middle
				new object[] { ttsample, 1,
                    new List<string>() { "b", "boa", "bead", "beaver", "brox","bar" },
                    new List<string>() { "b", "boa", "bead", "brox" } },

				// find some more words in the middle
                new object[] { ttsample, 1,
                    new List<string>() { "yo", "yox", "b", "bad" },
                    new List<string>() { "yo", "yox", "b", "bad" } },

				// empty dictionary
                new object[] { ttsample, 1,
                    new List<string>() { },
                    new List<string>() { } },

				// main test case
                new object[] { ttsample, 1,
                    new List<string>() {"bred", "bread", "yore", "byre", "abed", "oread", "bore", "orby", "robed", "broad", "byroad", "robe", "bored", "derby", "bade", "aero", "read", "orbed", "verb", "aery", "bead", "very", "road", "robbed", "robber", "board", "dove", "babe", "rober" },
                    new List<string>() {"bred", "bread", "yore", "byre", "abed", "oread", "bore", "orby", "robed", "broad", "byroad", "robe", "bored", "derby", "bade", "aero", "read", "orbed", "verb", "aery", "bead", "very", "road"}  },

				// test cases: empty board
				new object[] { onecharboard, 1,
                    new List<string>() { "bred", "bread", "yore", "byre", "abed", "oread", "bore", "orby", "robed", "broad", "byroad", "robe", "bored", "derby", "bade", "aero", "read", "orbed", "verb", "aery", "bead", "very", "road", "robbed", "robber", "board", "dove", "babe", "rober" },
                    new List<string>() { } },

                new object[] { onecharboard, 1,
                    new List<string>() { "c", "bred", "bread", "yore", "byre", "abed", "oread", "bore", "orby", "robed", "broad", "byroad", "robe", "bored", "derby", "bade", "aero", "read", "orbed", "verb", "aery", "bead", "very", "road", "robbed", "robber", "board", "dove", "babe", "rober" },
                    new List<string>() { "c" } },

				// mixed case:
                new object[] { mixedCase, 1,
                    new List<string>() {"BRED", "bread", "yOre", "BYRe", "aBEd", "oreaD", "boRE", "orby", "robed", "broad", "byroad", "robe", "bored", "derby", "bade", "aero", "read", "orbed", "verb", "aery", "bead", "very", "road", "robbed", "robber", "board", "dove", "babe", "rober" },
                    new List<string>() {"bred", "bread", "yore", "byre", "abed", "oread", "bore", "orby", "robed", "broad", "byroad", "robe", "bored", "derby", "bade", "aero", "read", "orbed", "verb", "aery", "bead", "very", "road"}  },
            };

            return res;
        }

        [Theory, MemberData("ThreeByThreeSample")]
        public void MyFirstTheory(GameBoard board, int minWordLength, List<string> dictionary, List<string> expected)
        {
            WordDictionary wordDictionary = new WordDictionary(dictionary, minWordLength);

            List<string> result = WordFinder.FindWords(board, wordDictionary);
            result.Sort();
            expected.Sort();

            Assert.Equal(result, expected);
        }
    }
}
