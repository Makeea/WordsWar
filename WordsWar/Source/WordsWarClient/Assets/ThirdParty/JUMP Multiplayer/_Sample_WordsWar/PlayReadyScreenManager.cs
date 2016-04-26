using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using WordsWar.Multiplayer;

public class PlayReadyScreenManager : Photon.PunBehaviour
{
    public Text ConnectionStatus;
    public Text NumPlayersInRoom;

    #region Custom Actions=============================================
    public void Action_Cancel()
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

    void GoToTheGameScreen()
    {
        SceneManager.LoadScene("TheGameScreen");
    }

    private void StartGameIfRoomFull()
    {
        // when two players join the room, then we create the client and the server
        if (PhotonNetwork.room.playerCount == 2)
        {
            // The server is created on the master client
            if (PhotonNetwork.isMasterClient)
            {
                PhotonPlayer[] list = PhotonNetwork.playerList;
                if ((list != null) && (list.Length == 2))
                {
                    GameServer.StartGame(list[0].ID, list[1].ID);
                }
                else
                {
                    Debug.LogError("PLayer list is not available");
                }
            }

            GoToTheGameScreen();
        }
    }
    #endregion

    #region Unity Events*******************************************************
    // Use this for initialization
    void Start () {
        // If we are joined to a room, then we can check
        if ((PhotonNetwork.connectionStateDetailed == PeerState.Joined) && !(PhotonNetwork.offlineMode))
        {
            StartGameIfRoomFull();
        }
        // If we are not connected to a Lobby or we are in offline mode, need to go to the main screen, we cannot matchmake
        else
        {
            GoToMainScreen();
        }
    }

    // Update is called once per frame
    void Update()
    {
        ConnectionStatus.text = "Connection status: " + PhotonNetwork.connectionStateDetailed.ToString();

        // if we went offline, we have to go back to the main screen, sorry
        if (PhotonNetwork.offlineMode)
        {
            GoToMainScreen();
        }
        else
        {
            if (PhotonNetwork.inRoom)
            {
                NumPlayersInRoom.text = "Players in room : " + PhotonNetwork.room.playerCount;
            }
        }
    }
    #endregion

    #region Photon Events------------------------------------------------------
    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        StartGameIfRoomFull();
    }

    public override void OnDisconnectedFromPhoton()
    {
        Debug.LogError("Disconnected from Photon");
        PhotonNetwork.offlineMode = true;
    }
    #endregion
}
