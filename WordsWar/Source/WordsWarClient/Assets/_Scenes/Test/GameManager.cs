using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using WordsWar.Multiplayer;
using UnityEngine.SceneManagement;

public class GameManager : Photon.PunBehaviour
{
    public Text ConnectionStatus;
    public Text EventsStatus;
    public Text MyScore;
    public Text TheirScore;
    public Text SecondsLeft;

	// Use this for initialization
	void Start () {
        GameClient.SendCommandPhotonEvent(new CommandConnect(PhotonNetwork.player.ID));
	}
	
	// Update is called once per frame
	void Update () {
        ConnectionStatus.text = "Connection status: " + PhotonNetwork.connectionStateDetailed.ToString();
    }

    public void QuitGame()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }

    public override void OnDisconnectedFromPhoton()
    {
        QuitGame();
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        QuitGame();
    }
}
