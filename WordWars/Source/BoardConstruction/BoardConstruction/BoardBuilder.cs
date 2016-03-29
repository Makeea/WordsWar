using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardConstruction
{
    // generates a random board
    public class BoardBuilder
    {
        // Generates a random board
        public static GameBoard GenerateRandom(int size, string alphabet)
        {
            char[,] boardData = new char[size, size];
            var random = new Random(Guid.NewGuid().GetHashCode());

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    boardData[i, j] = alphabet[random.Next(alphabet.Length)];
                }
            }

            return new GameBoard(boardData);
        }
    }
}
