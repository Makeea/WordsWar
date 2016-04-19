using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class MatchmakingScreenManager : Photon.PunBehaviour
{

    public Text ConnectionStatus;
    public Text NumPlayers;

    #region Custom Actions=============================================
    public void Action_Cancel()
    {
        GoToMainScreen();
    }

    void Matchmake()
    {
        // Try to join a random room
        PhotonNetwork.JoinRandomRoom();
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

    void GoToPlayReadyScreen()
    {
        SceneManager.LoadScene("PlayReadyScreen");
    }
    #endregion

    #region Unity Events*******************************************************
    // Use this for initialization
    void Start () {
        // If we are joined to a lobby, then we can matchmake
        if ((PhotonNetwork.connectionStateDetailed == PeerState.JoinedLobby) && !(PhotonNetwork.offlineMode))
        {
            Matchmake();
        }
        // If we are not connected to a Lobby or we are in offline mode, need to go to the main screen, we cannot matchmake
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
        else
        {
            if (PhotonNetwork.EnableLobbyStatistics)
            {
                List<TypedLobbyInfo> info = PhotonNetwork.LobbyStatistics;
                TypedLobbyInfo lobby = info.Find(l => l.Name == PhotonNetwork.lobby.Name);
                if (lobby != null)
                {
                    NumPlayers.text = "Online players: " + lobby.PlayerCount.ToString();
                }
            }
        }
    }
    #endregion

    #region Photon JOIN Room Events------------------------------------------------------
    public override void OnJoinedRoom()
    {
        Debug.LogFormat("Joined room: " + PhotonNetwork.room.name);
        GoToPlayReadyScreen();
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        // we could not join a room, let's create one and wait for players
        Debug.LogFormat("No random room, create one! Code: {0}, message: {1}", codeAndMsg[0], codeAndMsg[1]);
        RoomOptions opt = new RoomOptions();
        opt.maxPlayers = 2;
        opt.isOpen = true;
        opt.isVisible = true;
        PhotonNetwork.CreateRoom(null, opt, null);
    }
    #endregion

    #region Photon CREATED Room Events------------------------------------------------------
    public override void OnCreatedRoom()
    {
        Debug.LogFormat("Created room: " + PhotonNetwork.room.name);
        // Will also call OnJoinedRoom now, which will move us to the game scene
    }

    public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        Debug.LogErrorFormat("Unable to create room! Code: {0}, message: {1}", codeAndMsg[0], codeAndMsg[1]);
        // go back to the main scene
        GoToMainScreen();
    }

    public override void OnDisconnectedFromPhoton()
    {
        Debug.LogError("Disconnected from Photon");
        PhotonNetwork.offlineMode = true;
    }
    #endregion
}
