using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JUMP;

namespace DiceRollerSample
{
    // Custom Commands
    public class DiceRollerCommand_RollDice : JUMPCommand
    {
        public const byte RollDice_EventCode = 100;

        public int PlayerID { get { return (int)CommandData[0]; } set { CommandData[0] = value; } }
        public int RolledDiceValue { get { return (int)CommandData[1]; } set { CommandData[1] = value; } }

        // Create a command to send with this initializer
        public DiceRollerCommand_RollDice(int playerID, int rolledDiceValue) : base(new object[2], RollDice_EventCode)
        {
            PlayerID = playerID;
            RolledDiceValue = rolledDiceValue;
        }

        // Create a command when receiving it from Photon
        public DiceRollerCommand_RollDice(object[] data) : base(data, RollDice_EventCode)
        {
        }
    }

    // Custom Player
    public class DiceRollerPlayer : JUMPPlayer
    {
        public int Score = 0;
    }

    // Custom Snapshot
    public class DiceRoller_Snapshot : JUMPCommand_Snapshot
    {
        // ForPlayerID is at CommandData[0]
        public int MyScore { get { return (int)CommandData[1]; } set { CommandData[1] = value; } }
        public int OpponentScore { get { return (int)CommandData[2]; } set { CommandData[2] = value; } }
        public float SecondsRemaining { get { return (float)CommandData[3]; } set { CommandData[3] = value; } }
        public DiceRollerGameStages Stage { get { return (DiceRollerGameStages)CommandData[4]; } set { CommandData[4] = value; } }
        public int WinnerPlayerID { get { return (int)CommandData[5]; } set { CommandData[5] = value; } }

        // Create a command to send with this initializer
        public DiceRoller_Snapshot() : base(new object[6])
        {
        }

        // Create a command when receiving it from Photon
        public DiceRoller_Snapshot(object[] data) : base(data)
        {
        }
    }

    // Custom Engine
    public enum DiceRollerGameStages
    {
        WaitingForPlayers,
        Playing,
        Complete
    }

    public class DiceRollerGameState
    {
        public Dictionary<int, DiceRollerPlayer> Players = new Dictionary<int, DiceRollerPlayer>();
        public float SecondsRemaining;
        public DiceRollerGameStages Stage;
        public int WinnerPlayerID;
    }

    public class DiceRollerEngine : IJUMPGameServerEngine
    {
        private DiceRollerGameState GameState;

        public DiceRollerEngine()
        {
            GameState = new DiceRollerGameState();
            GameState.Stage = DiceRollerGameStages.WaitingForPlayers;
        }

        public JUMPCommand CommandFromEvent(byte eventCode, object content)
        {
            if (eventCode == DiceRollerCommand_RollDice.RollDice_EventCode)
            {
                return new DiceRollerCommand_RollDice((object[]) content);
            }
            return null;
        }

        public void ProcessCommand(JUMPCommand command)
        {
            if (command.CommandEventCode == DiceRollerCommand_RollDice.RollDice_EventCode)
            {
                DiceRollerCommand_RollDice rollDiceCommand = command as DiceRollerCommand_RollDice;

                DiceRollerPlayer player;
                if (GameState.Stage == DiceRollerGameStages.Playing)
                {
                    if (GameState.Players.TryGetValue(rollDiceCommand.PlayerID, out player))
                    {
                        player.Score += rollDiceCommand.RolledDiceValue;
                    }
                }
            }
        }

        public void StartGame(List<JUMPPlayer> Players)
        {
            GameState = new DiceRollerGameState();
            GameState.SecondsRemaining = 30;
            GameState.Stage = DiceRollerGameStages.Playing;

            foreach (var pl in Players)
            {
                DiceRollerPlayer player = new DiceRollerPlayer();
                player.PlayerID = pl.PlayerID;
                player.Connected = pl.Connected;
                player.Score = 0;

                GameState.Players.Add(player.PlayerID, player);
            }
        }

        public void Tick(double ElapsedSeconds)
        {
            if (GameState.Stage == DiceRollerGameStages.Playing)
            {
                GameState.SecondsRemaining -= (float) ElapsedSeconds;
                if (GameState.SecondsRemaining <= 0)
                {
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
                    GameState.Stage = DiceRollerGameStages.Complete;
                    GameState.WinnerPlayerID = winner;
                }
            }
        }

        public JUMPCommand_Snapshot TakeSnapshot(int ForPlayerID)
        {
            DiceRoller_Snapshot snap = new DiceRoller_Snapshot();
            snap.ForPlayerID = ForPlayerID;
            snap.MyScore = 0;
            snap.OpponentScore = 0;
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
            snap.SecondsRemaining = GameState.SecondsRemaining;
            snap.Stage = GameState.Stage;
            snap.WinnerPlayerID = GameState.WinnerPlayerID;

            return (JUMPCommand_Snapshot) snap;
        }
    }
}
