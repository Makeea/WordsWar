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
        public DiceRollerCommand_RollDice(int rolledDiceValue) : base(new object[2], RollDice_EventCode)
        {
            RolledDiceValue = rolledDiceValue;
        }

        // Create a command when receiving it from Photon
        public static DiceRollerCommand_RollDice FromData(object[] data)
        {
            return (DiceRollerCommand_RollDice)new JUMPCommand(data, RollDice_EventCode);
        }
    }

    // Custom Player
    public class DiceRollerPlayer : JUMPPlayer
    {
        public int Score = 0;
    }

    // Custom Snapshot
    public class DiceRollerSnapshotData : JUMPSnapshotData
    {
        public int ForPlayerID;
        public int MyScore;
        public int OpponentScore;
        public double SecondsRemaining;
        public DiceRollerGameStages Stage;
        public int WinnerPlayerID;
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
        public double SecondsRemaining;
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
                return DiceRollerCommand_RollDice.FromData((object[]) content);
            }
            return null;
        }

        public void ProcessCommand(JUMPCommand command)
        {
            if (command.CommandEventCode == DiceRollerCommand_RollDice.RollDice_EventCode)
            {
                DiceRollerCommand_RollDice rollDiceCommand = command as DiceRollerCommand_RollDice;

                DiceRollerPlayer player;
                if (GameState.Players.TryGetValue(rollDiceCommand.PlayerID, out player))
                {
                    player.Score += rollDiceCommand.RolledDiceValue;
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
                GameState.SecondsRemaining -= ElapsedSeconds;
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

        public JUMPSnapshotData TakeSnapshot(int ForPlayerID)
        {
            DiceRollerSnapshotData snap = new DiceRollerSnapshotData();
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

            return (JUMPSnapshotData) snap;
        }
    }
}
