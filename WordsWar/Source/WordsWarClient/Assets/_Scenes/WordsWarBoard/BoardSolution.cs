using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WordsWarEngine
{
    [Serializable]
    public class GameBoard
    {
        public int Size = 0;
        public char[,] Board = new char[4,4];

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    result.Append(Board[i, j]);
                }
                result.AppendLine();
            }

            return result.ToString();
        }
    }

    [Serializable]
    // Contains a Board, an Alphabet and the Board solution
    public class BoardSolution
    {
        public int BoardID;
        public GameBoard SolvedBoard;
        public List<string> SolutionWords;
        public string AlphabetId;
        public string DictionaryId;
    }

    public static class BoardManager
    {
        public static string jsonSample = "{\"BoardID\" :123,  \"AlphabetId\": \"en-us\",  \"DictionaryId\": \"english-words\",  \"SolvedBoard\": {    \"Size\": 4,    \"Board\": [     [ \"n\", \"a\", \"t\", \"e\" ],      [ \"i\", \"r\", \"r\", \"a\" ],      [ \"n\", \"w\", \"w\", \"s\" ],      [ \"h\", \"n\", \"o\", \"d\" ]    ]  },  \"SolutionWords\": [   \"aer\",    \"aet\",    \"ain\",    \"air\",    \"airn\",    \"airt\",    \"ani\",    \"ara\",    \"arain\",    \"arar\",    \"are\",    \"area\",    \"areas\",    \"areason\",    \"aret\",    \"arn\",    \"arni\",    \"arr\",    \"arras\",    \"arret\",    \"arri\",    \"ars\",    \"arson\",    \"art\",    \"artar\",    \"artarin\",    \"arte\",    \"arter\",    \"atar\",    \"ate\",    \"atria\",    \"awd\",    \"awn\",    \"don\",    \"dos\",    \"dosa\",    \"dow\",    \"down\",    \"dows\",    \"dsr\",    \"ear\",    \"ears\",    \"eat\",    \"era\",    \"eras\",    \"erat\",    \"err\",    \"errata\",    \"erratas\",    \"ers\",    \"eta\",    \"etas\",    \"iare\",    \"inn\",    \"ira\",    \"iran\",    \"irate\",    \"irater\",    \"irrate\",    \"nain\",    \"nar\",    \"nare\",    \"narr\",    \"narra\",    \"narras\",    \"narrate\",    \"narw\",    \"nat\",    \"natr\",    \"niata\",    \"nod\",    \"nods\",    \"nos\",    \"now\",    \"nows\",    \"ods\",    \"osar\",    \"owd\",    \"own\",    \"rain\",    \"ran\",    \"rani\",    \"rara\",    \"rare\",    \"ras\",    \"rason\",    \"rat\",    \"rata\",    \"ratan\",    \"rate\",    \"rater\",    \"raters\",    \"raw\",    \"raws\",    \"rea\",    \"reason\",    \"reata\",    \"ret\",    \"retain\",    \"retan\",    \"retar\",    \"retrain\",    \"ria\",    \"riata\",    \"riatas\",    \"rin\",    \"rte\",    \"rwd\",    \"sae\",    \"saeta\",    \"sar\",    \"saran\",    \"sare\",    \"sarra\",    \"sart\",    \"sartain\",    \"sat\",    \"sata\",    \"satai\",    \"satan\",    \"sate\",    \"saw\",    \"sawn\",    \"sod\",    \"son\",    \"sow\",    \"sowar\",    \"sown\",    \"swa\",    \"sware\",    \"swart\",    \"swat\",    \"swow\",    \"tae\",    \"tai\",    \"tain\",    \"tairn\",    \"tan\",    \"tar\",    \"tara\",    \"tare\",    \"tarea\",    \"tari\",    \"tarin\",    \"tarn\",    \"tarr\",    \"tarras\",    \"tarre\",    \"tarri\",    \"tars\",    \"tas\",    \"taw\",    \"tawn\",    \"taws\",    \"tea\",    \"tear\",    \"tears\",    \"teas\",    \"ter\",    \"tera\",    \"terai\",    \"teras\",    \"terr\",    \"terra\",    \"terrain\",    \"terran\",    \"tra\",    \"train\",    \"treas\",    \"treason\",    \"tri\",    \"trin\",    \"trina\",    \"trs\",    \"wae\",    \"waer\",    \"war\",    \"ware\",    \"warran\",    \"warrin\",    \"wars\",    \"wart\",    \"was\",    \"wat\",    \"water\",    \"waters\",    \"win\",    \"winare\",    \"winnow\",    \"winnows\",    \"wir\",    \"wirr\",    \"wirra\",    \"wod\",    \"won\",    \"wos\",    \"wow\",    \"wows\",    \"wran\",    \"wraw\",    \"wreat\"  ]}";

       
        public static BoardSolution GetBoard(int boardID)
        {


            string json = jsonSample;
            BoardSolution sol = JsonUtility.FromJson<BoardSolution>(json);

            sol.SolvedBoard.Board[0, 0] = 'n';
            sol.SolvedBoard.Board[0, 1] = 'a';
            sol.SolvedBoard.Board[0, 2] = 't';
            sol.SolvedBoard.Board[0, 3] = 'e';
            sol.SolvedBoard.Board[1, 0] = 'i';
            sol.SolvedBoard.Board[1, 1] = 'r';
            sol.SolvedBoard.Board[1, 2] = 'r';
            sol.SolvedBoard.Board[1, 3] = 'a';
            sol.SolvedBoard.Board[2, 0] = 'n';
            sol.SolvedBoard.Board[2, 1] = 'w';
            sol.SolvedBoard.Board[2, 2] = 'w';
            sol.SolvedBoard.Board[2, 3] = 's';
            sol.SolvedBoard.Board[3, 0] = 'h';
            sol.SolvedBoard.Board[3, 1] = 'n';
            sol.SolvedBoard.Board[3, 2] = 'o';
            sol.SolvedBoard.Board[3, 3] = 'd';

            return sol;
        }

        public static BoardSolution GetNewBoard()
        {
            return GetBoard(123);
        }
    }
}
