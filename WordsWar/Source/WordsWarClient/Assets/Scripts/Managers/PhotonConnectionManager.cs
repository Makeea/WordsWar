using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using WordsWar.Multiplayer;
using UnityEngine.SceneManagement;
using System;

public class PhotonConnectionManager : Photon.PunBehaviour
{
    public Text ConnectionStatus;
    public Text EventsStatus;
    public Button PlayButton;

    private Text PlayButtonText;
    
    public 

	// Use this for initialization
	void OnEnable () {
        PlayButtonText = PlayButton.GetComponent<Text>();
        PlayButtonText.text = "Connecting...";
        PlayButton.interactable = false;
        PhotonNetwork.ConnectUsingSettings("0.1");

#if DEBUG
        // DEBUG - getting longer timeouts to debug
        PhotonNetwork.networkingPeer.DisconnectTimeout = 100 * 1000;
#endif
    }

    public void QuitMultiplayer()
    {
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }


    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server");
        EventsStatus.text = "Connected to Master Server";
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        EventsStatus.text = "Joined Lobby";
        PlayButtonText.text = "Find Game";
        PlayButton.interactable = true;
        base.OnJoinedLobby();
    }

    public void JoinGameRoom()
    {
        if (PhotonNetwork.connectionStateDetailed == PeerState.JoinedLobby)
        {
            PlayButton.interactable = false;
            PlayButtonText.text = "Joining...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.LogFormat("Can't call Game Room when not in lobby");
            EventsStatus.text = "Error: not in a lobby";
        }
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.LogFormat("No random room, create one! Code: {0}, message: {1}", codeAndMsg[0], codeAndMsg[1]);
        EventsStatus.text = "Creating a room...";
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnCreatedRoom()
    {
        Debug.LogFormat("Created room: " + PhotonNetwork.room.name);
        EventsStatus.text = "Room Created";
    }

    public override void OnJoinedRoom()
    {
        Debug.LogFormat("Joined room: " + PhotonNetwork.room.name);
        EventsStatus.text = "Room Joined, player count: " + PhotonNetwork.room.playerCount;
        PlayButtonText.text = "Waiting...";

        // both clients have a GameClient
        GameClient.Start();

        StartGameIfRoomFull();
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        StartGameIfRoomFull();
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
                    EventsStatus.text = "PLayer list not available!";
                }
            }

            // Now we can load the Game Scene
            SceneManager.LoadScene("Game Scene");
        }
    }

    // Update is called once per frame
    void Update () {
        ConnectionStatus.text = "Connection status: " + PhotonNetwork.connectionStateDetailed.ToString();
    }

    // FAILURE cases
    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        Debug.LogFormat("Can't join room! Code: {0}, message: {1}", codeAndMsg[0], codeAndMsg[1]);
        EventsStatus.text = "Join room failed!";
    }

    public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        Debug.LogFormat("Can't create random room! Code: {0}, message: {1}", codeAndMsg[0], codeAndMsg[1]);
        EventsStatus.text = "Failed to Create Room";
    }
}
