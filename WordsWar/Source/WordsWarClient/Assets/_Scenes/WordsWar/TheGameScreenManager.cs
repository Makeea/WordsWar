using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using WordsWar.Multiplayer;
using System;
using UnityEngine.UI;

public class TheGameScreenManager : Photon.PunBehaviour
{
    public Text ConnectionStatus;

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
        // Ensure that we are in a room with 2 players
        if ((PhotonNetwork.connectionStateDetailed == PeerState.Joined) && (PhotonNetwork.room.playerCount == 2))
        {
            GameClient.SendCommand(new CommandConnect(PhotonNetwork.player.ID));
        }
        // Otherwise, things are bad, need to bail out
        else
        {
            GoToMainScreen();
        }
    }

    // Update is called once per frame
    void Update () {
        ConnectionStatus.text = "Connection status: " + PhotonNetwork.connectionStateDetailed.ToString();

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
