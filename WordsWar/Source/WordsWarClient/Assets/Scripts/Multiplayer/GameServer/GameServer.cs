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

    /// <summary>
    /// The Game Server
    /// </summary>
    public static class GameServer
    {
        public static int SnapshotsPerSecond = 3;
        public static int MatchSecondsDuration = 60;

        public static GameServerStates GameState = GameServerStates.NotStarted;
        public static Player[] Players = new Player[2];

        public static TimeSpan GameTimer = TimeSpan.Zero;
        private static TimeSpan SnapshotTimer = TimeSpan.Zero;

        private static TimeSpan SnapshotFrequency = TimeSpan.FromMilliseconds(1000 / SnapshotsPerSecond);
        private static TimeSpan PlayingTime = TimeSpan.FromSeconds(MatchSecondsDuration);  // Seconds to complete the game

        static GameServer()
        {
            PhotonNetwork.OnEventCall += OnPhotonEventCall;
        }

        #region GAME MANAGEMENT ===============================================
        // Start the game
        public static void StartGame(int Player1ID, int Player2ID)
        {
            Players[0] = new Player();
            Players[1] = new Player();

            Players[0].PlayerID = Player1ID;
            Players[1].PlayerID = Player2ID;

            GameState = GameServerStates.Started;
            GameTimer = TimeSpan.Zero;
            SnapshotTimer = TimeSpan.Zero;
        }

        // Move the clock of the server 
        public static void Tick(double seconds)
        {
            SnapshotTimer += TimeSpan.FromSeconds(seconds);
            if (SnapshotTimer > SnapshotFrequency)
            {
                SnapshotTimer = TimeSpan.Zero;
                foreach (var player in Players)
                {
                    SendSnapshotPhotonEvent(TakeSnapshot(player));
                }
            }

            if (GameState == GameServerStates.Playing)
            {
                GameTimer += TimeSpan.FromSeconds(seconds);
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

        // Process the command
        public static void ProcessCommand(int playerID, Command command)
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
                    if (GameState == GameServerStates.Playing)
                    {
                        string wordFound = ((CommandWordFound)command).Word;

                        //always give the points to the first player f
                        player = Players.FirstOrDefault(x => x.PlayerID == playerID);
                        if (player != null)
                        {
                            player.Score += wordFound.Length;
                        }
                    }
                    break;
                case CommandType.Disconnect:
                    break;
                default:
                    break;
            }

        }

        // Build the snapshot for a specific player
        public static Snapshot TakeSnapshot(Player player)
        {
            Snapshot s = new Snapshot();

            s.GameState = GameState;
            s.SecondsLeft = GameTimer.TotalSeconds;
            s.Player1Score = Players[0].Score;
            s.Player2Score = Players[1].Score;

            return s;
        }
        #endregion

        #region SEND SNAPSHOTS=================================================
        // Send Snapshot to client
        private static void SendSnapshotPhotonEvent(Snapshot snapshot)
        {
            RaiseEventOptions options = new RaiseEventOptions();
            options.Receivers = ExitGames.Client.Photon.ReceiverGroup.All;

            PhotonNetwork.RaiseEvent(Snapshot.SnapshotEventID, snapshot.CommandData, true, options);
        }
        #endregion

        #region RECEIVE COMMANDS===============================================
        // Check Photon events for Game Commands
        private static void OnPhotonEventCall(byte eventCode, object content, int senderId)
        {
            // We are only receiving events for the Master Client
            if (PhotonNetwork.isMasterClient)
            {
                Command c = Command.FromPhotonEvent(eventCode, content);
                if (c != null)
                {
                    ProcessCommand(senderId, c);
                }
            }
        }
        #endregion
    }
}
