using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardConstruction
{
    // Main class that coordinates all the components
    public class BoardConstructor
    {

        public static BoardSolution Build_Solve_Publish(int dimension, string alphabetId, string dictionaryId)
        {
            BoardSolution sol = Build_Solve(4, alphabetId, dictionaryId);

            if (BoardEvaluator.EvaluateBoard(sol))
            {
                BoardPublisher.Publish(sol);
                return sol;
            }
            else
            {
                return null;
            }
        }

        public static BoardSolution Build_Solve(int dimension, string alphabetId, string dictionaryId)
        {
            // Load the Alphabet
            Alphabet alphabet = Alphabets.Load(alphabetId);

            // Load the Dictionary
            Dictionary dict = Dictionaries.Load(dictionaryId);

            // Build a RandomBoard based on the Alphabet
            GameBoard board = BoardRandomizer.GenerateRandom(dimension, alphabet);

            // Solve the Board using the Dictionary
            return Solve(alphabet, dict, board);
        }

        public static BoardSolution Solve(Alphabet alphabet, Dictionary dict, GameBoard board)
        {
            // Solve the Board using the Dictionary
            List<string> wordsfound = WordFinder.FindWords(board, dict).ToList();
            wordsfound.Sort();

            return new BoardSolution(board, wordsfound, alphabet.id, dict.id);
        }
    }
}
