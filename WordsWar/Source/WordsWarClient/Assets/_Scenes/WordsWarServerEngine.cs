using UnityEngine;
using System.Collections;
using JUMP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WordsWarEngine
{
    /// <summary>
    /// The game will start by getting a board from the repository and communicating it to the clients with the SendBoardIDs state
    /// The clients will ack with the WordsWarCommand_AckBoard command
    /// When all players ack, we will do a 5 seconds countdown
    /// </summary>
    public enum WordsWarGameStages
    {
        None,  // Waiting for players to connect to the room
        SendBoardID,        // Communicating the board to players
        CountingDown,       // Count down to help players visualize and get ready
        Playing,            // Players find the words, game time counts down
        GameOver            // Game is over!
    }

    public class WordsWarPlayer : JUMPPlayer
    {
        public int Score = 0;
        public List<string> WordsFound = new List<string>();
        public bool BoardReceived = false;
    }

    public class WordsWarGameState
    {
        public Dictionary<int, WordsWarPlayer> Players = new Dictionary<int, WordsWarPlayer>();
        public WordsWarGameStages Stage = WordsWarGameStages.None;
        public int BoardID = -1;
        public double CountingDownRemaining = 0;
        public double PlayTimeRemaining = 0;
        public BoardSolution Board = null;
        public int WinnerPlayerID = -1;
    }

    public class WordsWar_Snapshot : JUMPCommand_Snapshot
    {
        // ForPlayerID is at CommandData[0]
        public int BoardID { get { return (int)CommandData[1]; } set { CommandData[1] = value; } }
        public WordsWarGameStages Stage { get { return (WordsWarGameStages)CommandData[2]; } set { CommandData[2] = value; } }
        public double CountingDownRemaining { get { return (double)CommandData[3]; } set { CommandData[3] = value; } }
        public double PlayTimeRemaining { get { return (double)CommandData[4]; } set { CommandData[4] = value; } }
        public int MyScore { get { return (int)CommandData[5]; } set { CommandData[5] = value; } }
        public int OpponentScore { get { return (int)CommandData[6]; } set { CommandData[6] = value; } }
        public int WinnerPlayerID { get { return (int)CommandData[7]; } set { CommandData[7] = value; } }

        // Create a command to send with this initializer
        public WordsWar_Snapshot() : base(new object[8])
        {
        }

        // Create a command when receiving it from Photon
        public WordsWar_Snapshot(object[] data) : base(data)
        {
        }
    }

    public class WordsWarServerEngine : IJUMPGameServerEngine
    {
        private WordsWarGameState GameState;
        public const int CountDownSeconds = 5;
        public const int PlayTimeSeconds = 30;

        public WordsWarServerEngine()
        {
            GameState = new WordsWarGameState();
        }

        public JUMPCommand CommandFromEvent(byte eventCode, object content)
        {
            if (eventCode == WordsWarCommand_BoardReceived.BoardReceived_EventCode)
            {
                return new WordsWarCommand_BoardReceived((object[])content);
            }
            else if (eventCode == WordsWarCommand_WordFound.WordFound_EventCode)
            {
                return new WordsWarCommand_WordFound((object[])content);
            }

            return null;
        }

        public void ProcessCommand(JUMPCommand command)
        {
            // Players received the board id and sends AckBoard back
            if ((command.CommandEventCode == WordsWarCommand_BoardReceived.BoardReceived_EventCode) && (GameState.Stage == WordsWarGameStages.SendBoardID))
            {
                WordsWarCommand_BoardReceived ackBoard = command as WordsWarCommand_BoardReceived;
                WordsWarPlayer player;

                if (GameState.Players.TryGetValue(ackBoard.PlayerID, out player))
                {
                    player.BoardReceived = true;
                }

                // how many players received the board id?
                int NumPlayersAcked = GameState.Players.Count(p => p.Value.IsConnected);
                if (NumPlayersAcked == JUMPOptions.NumPlayers)
                {
                    // move on to the next stage, the countdown
                    GameState.Stage = WordsWarGameStages.CountingDown;
                    GameState.CountingDownRemaining = CountDownSeconds;
                }
            }
            else if ((command.CommandEventCode == WordsWarCommand_WordFound.WordFound_EventCode) && (GameState.Stage == WordsWarGameStages.Playing))
            {
                WordsWarCommand_WordFound wordFound = command as WordsWarCommand_WordFound;
                WordsWarPlayer player;

                if (GameState.Players.TryGetValue(wordFound.PlayerID, out player))
                {
                    string found = wordFound.WordFound.ToLower();
                    if (!player.WordsFound.Contains(wordFound.WordFound))
                    {
                        player.Score += found.Length;
                        player.WordsFound.Add(found);
                    }
                }
            }
        }

        public void StartGame(List<JUMPPlayer> Players)
        {
            GameState = new WordsWarGameState();
            GameState.Stage = WordsWarGameStages.SendBoardID;
            GameState.BoardID = BoardManager.GetNewBoard().BoardID;

            foreach (var pl in Players)
            {
                WordsWarPlayer player = new WordsWarPlayer();
                player.PlayerID = pl.PlayerID;
                player.IsConnected = pl.IsConnected;

                GameState.Players.Add(player.PlayerID, player);
            }
        }

        public JUMPCommand_Snapshot TakeSnapshot(int ForPlayerID)
        {
            WordsWar_Snapshot snap = new WordsWar_Snapshot();
            snap.ForPlayerID = ForPlayerID;
            snap.MyScore = 0;
            snap.OpponentScore = 0;
            snap.BoardID = GameState.BoardID;
            snap.Stage = GameState.Stage;
            snap.CountingDownRemaining = GameState.CountingDownRemaining;
            snap.PlayTimeRemaining = GameState.PlayTimeRemaining;
            snap.WinnerPlayerID = GameState.WinnerPlayerID;

            foreach (var item in GameState.Players)
            {
                if (item.Value.PlayerID == ForPlayerID)
                {
                    snap.MyScore = item.Value.Score;
                }
                else
                {
                    snap.OpponentScore = item.Value.Score;
                }
            }

            return (JUMPCommand_Snapshot) snap;
        }

        public void Tick(double ElapsedSeconds)
        {
            if (GameState.Stage == WordsWarGameStages.CountingDown)
            {
                GameState.CountingDownRemaining -= (float)ElapsedSeconds;
                if (GameState.CountingDownRemaining <= 0)
                {
                    GameState.Stage = WordsWarGameStages.Playing;
                    GameState.PlayTimeRemaining = PlayTimeSeconds;
                }
            }
            if (GameState.Stage == WordsWarGameStages.Playing)
            {
                GameState.PlayTimeRemaining -= (float)ElapsedSeconds;
                if (GameState.PlayTimeRemaining <= 0)
                {
                    GameState.Stage = WordsWarGameStages.GameOver;
                    int maxscore = 0;
                    int winner = -1;
                    foreach (var item in GameState.Players)
                    {
                        if (item.Value.Score > maxscore)
                        {
                            maxscore = item.Value.Score;
                            winner = item.Key;
                        }
                    }
                    GameState.WinnerPlayerID = winner;
                }
            }
        }
    }
}
