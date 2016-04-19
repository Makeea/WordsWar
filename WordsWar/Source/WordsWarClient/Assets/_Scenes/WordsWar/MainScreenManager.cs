using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainScreenManager : Photon.PunBehaviour {

    public Text ConnectionStatus;
    public Text NumPlayers;
    public Text PlayButton;

    #region Custom Actions=============================================
    public void Play()
    {
        // When the user press play, we try to join the lobby, when the lobby is loaded, we move to the matchmaking screen
        PhotonNetwork.JoinLobby();
    }

    public void GoToLoadingScreen()
    {
        SceneManager.LoadScene("LoadingScreen");
    }
    void GoToMatchmakingScreen()
    {
        SceneManager.LoadScene("MatchmakingScreen");
    }
    #endregion
    
    #region Unity Events*******************************************************
    // Use this for initialization
    void Start ()
    {
        // If we are not connected and we have not decided this is offline mode, let's go back to the Loading Screen to connect
        if ((PhotonNetwork.connectionStateDetailed != PeerState.ConnectedToMaster) && !(PhotonNetwork.offlineMode))
        {
            GoToLoadingScreen();
        }
    }

    // Update is called once per frame
    void Update()
    {
        ConnectionStatus.text = "Connection status: " + PhotonNetwork.connectionStateDetailed.ToString();
        if (PhotonNetwork.offlineMode)
        {
            ConnectionStatus.text = "Connection status: OFFLINE";
            PlayButton.GetComponent<Button>().interactable = false;
            NumPlayers.text = "Offline.";
        }
        else
        {
            // If we are not connected with the Master, we need to disable the play button, only practice is enabled
            if (PhotonNetwork.connectionStateDetailed != PeerState.ConnectedToMaster)
            {
                PlayButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                PlayButton.GetComponent<Button>().interactable = true;
            }

            if (PhotonNetwork.EnableLobbyStatistics)
            {
                List<TypedLobbyInfo> info = PhotonNetwork.LobbyStatistics;
                TypedLobbyInfo lobby = info.Find(l => l.IsDefault == true);
                if (lobby != null)
                {
                    NumPlayers.text = "Online players: " + lobby.PlayerCount.ToString();
                }
            }
        }
    }
    #endregion

    #region Photon Events------------------------------------------------------
    public override void OnJoinedLobby()
    {
        Debug.Log("Connected to Lobby");
        // We are in the lobby, go to matchmaking
        GoToMatchmakingScreen();
    }

    public override void OnConnectionFail(DisconnectCause cause)
    {
        Debug.LogError("Failed to connected : " + cause.ToString());
    }

    public override void OnDisconnectedFromPhoton()
    {
        Debug.LogError("Disconnected from Photon");
        PhotonNetwork.offlineMode = true;
    }
    #endregion
}
