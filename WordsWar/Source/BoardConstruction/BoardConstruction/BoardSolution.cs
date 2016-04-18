using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardConstruction
{
    // Contains a Board, an Alphabet and the Board solution
    public class BoardSolution
    {
        public string GameBoardKey { get; private set; }
        public GameBoard SolvedBoard { get; private set; }
        public List<string> SolutionWords { get; private set; }
        public string AlphabetId { get; private set; }
        public string DictionaryId { get; private set; }

        public BoardSolution(GameBoard board, List<string> boardWords, string alphabetId, string dictionaryId)
        {
            SolvedBoard = board;
            GameBoardKey = board.ToString(GameBoard.BoardStringFormat.None);
            SolutionWords = boardWords;
            AlphabetId = alphabetId;
            DictionaryId = dictionaryId;
        }

        public BoardSolution(GameBoard board, HashSet<string> boardWords, string alphabetId, string dictionaryId)
            : this(board, boardWords.ToList(), alphabetId, dictionaryId)
        {            
        }
    }
}
