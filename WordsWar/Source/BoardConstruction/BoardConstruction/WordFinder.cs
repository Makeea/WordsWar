using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardConstruction
{
    public class WordFinder
    {
        // Note: it will return words in lower case, as input is normalized to be lower case
        static public HashSet<string> FindWords(GameBoard board, Dictionary dictionary)
        {
            HashSet<string> result = new HashSet<string>();

            // fill up the map with the info in the dictionary
            DictionaryLookupInfo lookup = DictionaryLookupInfo.BuildLookupInfo(dictionary);

            // for each cell in the board
            for (int i = 0; i < board.Size; i++)
            {
                for (int j = 0; j < board.Size; j++)
                {
                    bool[,] boardVisitedInfo = InitVisitedInfo(board.Size);
                    result.UnionWith(FindWordsFromCell(board, lookup, i, j, boardVisitedInfo, new StringBuilder()));
                }
            }

            return result;
        }

        private static bool[,] InitVisitedInfo(int size)
        {
            bool[,] result = new bool[size,size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    result[i, j] = false;
                }
            }

            return result;
        }

        // main function: depth first search in the board, using the lookup map and a temp string
        private static List<string> FindWordsFromCell(GameBoard board, DictionaryLookupInfo lookup, int i, int j, bool[,] boardVisitedInfo, StringBuilder tempword)
        {
            List<string> result = new List<string>();

            // we need to look if the char concatenated with the optional string in the recursive call has an entry. 
            char cellChar = board.Board[i,j];
            tempword.Append(cellChar);
            string tempwordstring = tempword.ToString();

            // if the word is in the lookup map
            if (lookup.LookupInfo.ContainsKey(tempwordstring))
            {
                WordLookupInfoData map_entry = lookup.LookupInfo[tempwordstring];

                // if so, if it is a word, add it to the colletion. 
                if (map_entry.IsWord)
                {
                    // we have found a word!
                    result.Add(tempwordstring);
                }

                // if it has possible children
                if (map_entry.HasChildren)
                {
                    // mark the cell as used.
                    boardVisitedInfo[i,j] = true;

                    // for all the non marked children
                    foreach (var neigh in board.Neighbors[i,j])
                    {
                        if (!boardVisitedInfo[neigh.Item1, neigh.Item2])
                        {
                            // recursion call
                            result.AddRange(FindWordsFromCell(board, lookup, neigh.Item1, neigh.Item2, boardVisitedInfo, tempword));
                        }
                    }
                    // unmark the current cell
                    boardVisitedInfo[i, j] = false;
                }
            }

            // backtrack the temp string 
            tempword.Remove(tempword.Length - 1, 1);
            return result;
        }
    }
}
