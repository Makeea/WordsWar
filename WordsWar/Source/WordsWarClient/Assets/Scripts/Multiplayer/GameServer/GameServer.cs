using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordsWar.Multiplayer
{
    public enum GameServerStates
    {
        NotStarted,
        Started,
        Playing,
        Complete
    }

    public delegate void SendSnapshotDelegate(Snapshot snapshot);

    /// <summary>
    /// The Game Server
    /// </summary>
    public static class GameServer
    {
        public static event SendSnapshotDelegate OnSendSnapshot;

        #region /// Send Snapshot Event
        public static void SendSnapshotEvent(Snapshot e)
        {
            if (OnSendSnapshot != null)
            {
                OnSendSnapshot(e); 
            }
        }
        #endregion

        static Player[] Players = new Player[2];
        static GameServerStates GameState = GameServerStates.NotStarted;
        static TimeSpan GameTimer = TimeSpan.Zero;
        static TimeSpan SnapshotTimer = TimeSpan.Zero;

        const int SnapshotsPerSecond = 3;
        static TimeSpan SnapshotFrequency = TimeSpan.FromMilliseconds(1000 / SnapshotsPerSecond);
        static TimeSpan PlayingTime = TimeSpan.FromSeconds(60);  // Seconds to complete the game

        public static void StartGame(int Player1ID, int Player2ID)
        {
            Players[0] = new Player();
            Players[1] = new Player();

            Players[0].PlayerID = Player1ID;
            Players[1].PlayerID = Player2ID;

            GameState = GameServerStates.Started;
            PhotonNetwork.OnEventCall += OnPhotonEventCall;
        }

        /// Move the clock of the server 
        public static void Tick(double seconds)
        {
            GameTimer += TimeSpan.FromSeconds(seconds);
            SnapshotTimer += TimeSpan.FromSeconds(seconds);
            if (SnapshotTimer > SnapshotFrequency)
            {
                SnapshotTimer = TimeSpan.Zero;
                foreach (var player in Players)
                {
                    SendSnapshotEvent(GetSnapshot(player));
                }
            }

            switch (GameState)
            {
                case GameServerStates.Playing:
                    if (GameTimer >= PlayingTime)
                        GameState = GameServerStates.Complete;
                    break;
                default:
                    break;
            }

        }

        public static void ReceiveCommand(Command command, int playerID, params object[] paramList)
        {
            switch (command.CommandType)
            {
                case CommandType.Connect:
                    var player = Players.FirstOrDefault(x => x.PlayerID == playerID);
                    if (player != null)
                    {
                        player.Connected = true;
                    }
                    int connPlayers = Players.Where(x => x.Connected == true).Count();
                    if ((connPlayers == 2) && (GameState == GameServerStates.Started))
                    {
                        GameState = GameServerStates.Playing;
                    }
                    break;
                case CommandType.WordFound:
                    string wordFound = paramList[0] as string;
                    player = Players.FirstOrDefault(x => x.PlayerID == playerID);
                    if (player != null)
                    {
                        player.Score += wordFound.Length;
                    }
                    break;
                case CommandType.Disconnect:
                    break;
                default:
                    break;
            }

        }

        private static Snapshot GetSnapshot(Player player)
        {
            Snapshot s = new Snapshot();

            s.GameState = GameState;
            s.SecondsLeft = GameTimer;
            s.Player1Score = Players[0].Score;
            s.Player2Score = Players[1].Score;

            return s;
        }

        private static void OnPhotonEventCall(byte eventCode, object content, int senderId)
        {
            // We are only receiving events for the Master Client
            if (PhotonNetwork.isMasterClient)
            {
                ReceiveCommand(Command.FromPhotonEvent(eventCode, content), senderId);
            }
        }
    }
}
