using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using WordsWar.Multiplayer;
using System;
using UnityEngine.UI;

public class TheGameScreenManager : Photon.PunBehaviour
{
    public Text ConnectionStatus;
    public Text MyScore;
    public Text TheirScore;
    public Text TimeLeft;
    public Text GameStatus;

    #region Game Events @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public void Action_FoundWord()
    {
        int score = UnityEngine.Random.Range(0,4);
        string[] words = new string[5] { "cat", "dog", "annie", "tarzan", "cheeta" };
        GameClient.SendCommandPhotonEvent(new CommandWordFound(words[score]));
    }
    #endregion

    #region Custom Actions=============================================
    public void Action_Quit()
    {
        GoToMainScreen();
    }

    void GoToMainScreen()
    {
        if (PhotonNetwork.inRoom)
        {
            // TODO: should wait for this to complete...
            PhotonNetwork.LeaveRoom();
        }

        if (PhotonNetwork.insideLobby)
        {
            // TODO: it does not work, should return to ConnectedToMaster and it does not do it.
            PhotonNetwork.LeaveLobby();
        }
        SceneManager.LoadScene("MainScreen");
    }
    #endregion

    #region Unity Events*******************************************************
    // Use this for initialization
    void Start () {
        GameClient.OnSnapshotReceived += GameClient_OnSnapshotReceived;

        MyScore.text = "0";
        TheirScore.text = "0";
        TimeLeft.text = "0";
        GameStatus.text = "Not Started";

        // Ensure that we are in a room with 2 players
        if ((PhotonNetwork.connectionStateDetailed == PeerState.Joined) && (PhotonNetwork.room.playerCount == 2))
        {
            GameClient.SendCommandPhotonEvent(new CommandConnect(PhotonNetwork.player.ID));
        }
        // Otherwise, things are bad, need to bail out
        else
        {
            GoToMainScreen();
        }
    }

    void OnDisable()
    {
        GameClient.OnSnapshotReceived -= GameClient_OnSnapshotReceived;
    }

    private void GameClient_OnSnapshotReceived(Snapshot snapshot)
    {
        MyScore.text = snapshot.Player1Score.ToString();
        TheirScore.text = snapshot.Player2Score.ToString();
        TimeLeft.text = snapshot.SecondsLeft.ToString();
        GameStatus.text = snapshot.GameState.ToString();
    }

    // Update is called once per frame
    void Update () {
        ConnectionStatus.text = "Connection status: " + PhotonNetwork.connectionStateDetailed.ToString();

        if (PhotonNetwork.isMasterClient)
        {
            GameServer.Tick(Time.deltaTime);
        }
        
        // if we went offline, we have to go back to the main screen, sorry
        if (PhotonNetwork.offlineMode)
        {
            GoToMainScreen();
        }
    }
    #endregion

    #region Photon Events------------------------------------------------------
    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Action_Quit();
    }

    public override void OnDisconnectedFromPhoton()
    {
        Debug.LogError("Disconnected from Photon");
        PhotonNetwork.offlineMode = true;
    }
    #endregion
}
