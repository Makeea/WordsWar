using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using JUMP;
using System.Collections.Generic;

namespace WordsWarEngine
{
    public class WordsWarGameManager : MonoBehaviour {

        public Text MyScore;
        public Text TimeLeft;
        public Text OpponentScore;
        public Button FoundWord;
        public Text GameStatus;
        public Text Board;
        public Text Message;
        public Text FoundWordText;
        private WordsWarGameStages UIStage = WordsWarGameStages.None;
        private BoardSolution BoardSol;
        private List<string> WordsFound;

        public void OnSnapshotReceived(JUMPCommand_Snapshot data)
        {
            WordsWar_Snapshot snap = new WordsWar_Snapshot(data.CommandData);
            GameStatus.text = snap.Stage.ToString();
            MyScore.text = snap.MyScore.ToString();
            OpponentScore.text = snap.OpponentScore.ToString();
            UIStage = snap.Stage;
            if (UIStage == WordsWarGameStages.SendBoardID)
            {
                Message.text = "Receiving board .. " + snap.BoardID.ToString();
                BoardSol = BoardManager.GetBoard(snap.BoardID);

                Board.text = BoardSol.SolvedBoard.ToString().ToUpper();  // "Playing Board: " + snap.BoardID.ToString();
                WordsWarCommand_BoardReceived ack = new WordsWarCommand_BoardReceived(JUMPMultiplayer.PlayerID, snap.BoardID);
                Singleton<JUMPGameClient>.Instance.SendCommandToServer(ack);
            }
            else if (UIStage == WordsWarGameStages.CountingDown)
            {
                TimeLeft.text = snap.CountingDownRemaining.ToString("0.");
            }
            else if (UIStage == WordsWarGameStages.Playing)
            {
                TimeLeft.text = snap.PlayTimeRemaining.ToString("0.");
            }
            else if (UIStage == WordsWarGameStages.GameOver)
            {
                Message.text = (snap.WinnerPlayerID == JUMPMultiplayer.PlayerID) ? "You Won :)" : "You Lost :(";
            }
        }

        public void PlayerFoundWord()
        {
            if (UIStage == WordsWarGameStages.Playing)
            {
                string word = FoundWordText.text.ToLower();
                if (WordsFound.Contains(word))
                {
                    Message.text = "You found this already...";
                }
                else if (BoardSol.SolutionWords.Contains(word))
                {
                    Message.text = "You found the word: " + word;
                    WordsFound.Add(word);
                    Singleton<JUMPGameClient>.Instance.SendCommandToServer(new WordsWarCommand_WordFound(JUMPMultiplayer.PlayerID, word));
                    FoundWordText.text = "";
                }
                else
                {
                    Message.text = "Not a valid word, sorry";
                }
            }
        }

        // Use this for initialization
        void Start()
        {
            WordsFound = new List<string>();
        }

        // Update is called once per frame
        void Update()
        {
            FoundWord.interactable = (UIStage == WordsWarGameStages.Playing);
        }

    }
}
