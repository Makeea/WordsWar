using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardConstruction
{
    // generates a random board
    public class BoardRandomizer
    {
        // Generates a random board
        public static GameBoard GenerateRandom(int size, Alphabet alphabet)
        {
            char[,] boardData = new char[size, size];
            var random = new Random(Guid.NewGuid().GetHashCode());

            double alphabetTotalDistribution = alphabet.letters.Sum((it) => it.frequency);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    double randomDouble = random.NextDouble() * alphabetTotalDistribution;
                    double distributionHit = 0;
                    char nextrandom = ' ';

                    for (int chindex = 0; chindex < alphabet.letters.Count; chindex++)
                    {
                        distributionHit += alphabet.letters[chindex].frequency;
                        if (distributionHit >= randomDouble)
                        {
                            nextrandom = alphabet.letters[chindex].character;
                            break;
                        }
                    }

                    boardData[i, j] = nextrandom;
                }
            }

            return new GameBoard(boardData);
        }
    }
}
