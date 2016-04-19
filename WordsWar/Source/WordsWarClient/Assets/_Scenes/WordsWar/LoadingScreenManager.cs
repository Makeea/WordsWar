using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// The loading screen will connect to Photon at startup -
/// If the connection is not available, will wait for a few seconds before moving on
/// When it moves on, then the game is in offline mode, only practice is available
/// To connect again, a user has to restart the game.
/// If a user 
/// </summary>
public class LoadingScreenManager : Photon.PunBehaviour {

    public Text ConnectionStatus;


    #region Custom Actions=============================================
    private void LoadMainScreen()
    {
        SceneManager.LoadScene("MainScreen");
    }
    #endregion
    
    #region Unity Events*******************************************************
    private void Awake()
    {
        PhotonNetwork.offlineMode = false;
    }

    // Use this for initialization
    void Start () {
        // connect to Photon With Version Number
        PhotonNetwork.PhotonServerSettings.EnableLobbyStatistics = true;
#if DEBUG
        // DEBUG - getting longer timeouts to debug
        PhotonNetwork.networkingPeer.DisconnectTimeout = 1000; // 0 * 1000;
#endif

        PhotonNetwork.ConnectUsingSettings("0.1");
    }
    // Update is called once per frame
    void Update () {
        ConnectionStatus.text = "Connection status: " + PhotonNetwork.connectionStateDetailed.ToString();
    }
    #endregion

    #region Photon Events------------------------------------------------------
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server");
        LoadMainScreen();
    }

    public override void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.LogError("Failed to connected to Master Server: " + cause.ToString());
    }

    public override void OnDisconnectedFromPhoton()
    {
        Debug.LogError("Disconnected from Photon");
        // Go to the main screen in offline mode this time.
        PhotonNetwork.offlineMode = true;
        LoadMainScreen();
    }
    #endregion
}
