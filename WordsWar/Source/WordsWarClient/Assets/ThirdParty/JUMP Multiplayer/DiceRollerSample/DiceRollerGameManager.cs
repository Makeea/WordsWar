using UnityEngine;
using System.Collections;
using JUMP;
using DiceRollerSample;
using UnityEngine.UI;

public class DiceRollerGameManager : MonoBehaviour {

    public Text MyScore;
    public Text TheirScore;
    public Text GameStatus;
    public Text TimeLeft;
    public Text Result;
    public Text DiceResult;
    public Button RollDice;
    DiceRollerGameStages UIStage = DiceRollerGameStages.WaitingForPlayers;

    public void OnSnapshotReceived(JUMPCommand_Snapshot data)
    {
        DiceRoller_Snapshot snap = new DiceRoller_Snapshot(data.CommandData);
        GameStatus.text = snap.Stage.ToString();
        MyScore.text = "My score: " + snap.MyScore.ToString();
        TheirScore.text = "Opponent's score: " + snap.OpponentScore.ToString();
        TimeLeft.text = "Time left: " + snap.SecondsRemaining.ToString("0.");
        UIStage = snap.Stage;
        if (UIStage == DiceRollerGameStages.Complete)
        {
            Result.text = (snap.MyScore > snap.OpponentScore) ? "You Won :)" : ((snap.MyScore == snap.OpponentScore) ? "Tied!" : "You Lost :(");
        }
    }

    public void RollADice()
    {
        int score = Random.Range(1, 7);

        if (RollDice != null)
        {
            // RollDice.GetComponent<Text>().text = "Rolled a " + score + " \nroll again.."; 

            DiceResult.text = "Rolled a " + score;
        }
        Singleton<JUMPGameClient>.Instance.SendCommandToServer(new DiceRollerCommand_RollDice(JUMPMultiplayer.PlayerID, score));
    }

    // Use this for initialization
    void Start()
    {
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
    }

    // Update is called once per frame
    void Update()
    {
        RollDice.interactable = (UIStage == DiceRollerGameStages.Playing);
	}
}
