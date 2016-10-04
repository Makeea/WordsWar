using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using JUMP;
using System;
using PlayFab.ClientModels;
using PlayFab;

public enum JUMPConnectionStatus
{
    None,
    RetrievingAuthToken,
    AuthTokenFailure,
    Connecting,
    Online,
    Offline
}

public class MainPageScript : MonoBehaviour {

    public Button OnlinePlayButton;
    public Button OfflinePlayButton;
    public Text MessageText;

    public Text AccountStatusText;
    public Text ProfileStatusText;
    public Text MultiplayerStatusText;
    public Text FacebookManagerStatusText;
    public Text FacebookLoginStatusText;

    // Player
    public Text NicknameText;
    public Text EmailText;
    public Text LevelText;
    public Text ScoreText;
    public Image ProfilePicture;
    
    public Button FacebookLoginButton;

    // JUMP Multiplayer
    public JUMPMultiplayer JUMPConnection;
    private JUMPConnectionStatus JUMPConnectionStatus = JUMPConnectionStatus.None;
    public const string PhotonAppID = "29efb939-1fe1-4500-b184-2325d8526c34";
    public const string AppID = "0.1";

    void Awake()
    {
        JUMPConnection.gameObject.SetActive(false);
        if (Debug.isDebugBuild && !DebugManager.DebugInitialized)
        {
            Logger.LogLevel = Logger.LogLevels.Verbose;
            // Load Debug Screen to do optional pre-login with Facebook
            DebugManager.LoadDebugIntroScreen();
        }
        else
        {
            ProfileManager.Init();
            PlayFabManager.Init();
            FacebookManager.Init();
        }
    }

    // Use this for initialization
    void Start()
    {
        AutoLogin();
    }

    public void AutoLogin()
    {
        Logger.Verbose("LoginManager.AutoLogin");
        // If you are logged in with Facebook already 
        if (FacebookManager.IsUserLoggedIn)
        {
            // Sign in to PlayFab using your Facebook Identity (auto create the account)
            PlayFabManager.LoginUsingFacebookId();
        }
        // Otherwise 
        else
        {
            // Sign In to PlayFab using the local device identifier (auto create the account)
            PlayFabManager.LoginUsingDeviceId();
        }
    }

    public void LoginWithFacebook()
    {
        FacebookManager.LoginToFacebook();
    }

    // Update is called once per frame
    void Update() {
        // Player
        ProfileManager player = ProfileManager.Current;

        if (NicknameText != null) NicknameText.text = player.Nickname;
        if (EmailText != null) EmailText.text = player.Email;
        if (LevelText != null) LevelText.text = "Level : " + player.Level.ToString();
        if (ScoreText != null) ScoreText.text = "Score : " + player.LifetimePoints.ToString();

        if (player.RefreshPictureTexture && (ProfilePicture != null))
        {
            ProfilePicture.sprite = Sprite.Create(player.PictureTexture, new Rect(0, 0, 50, 47), new Vector2());
            player.RefreshPictureTexture = false;
        }


        // Account Status
        AccountStatusText.text = "Account : " + PlayFabManager.PlayFabStatus.ToString();
        // Proile Status
        ProfileStatusText.text = "Profile : " + ProfileManager.ProfileStatus.ToString();
        // Facebook Manager Status
        FacebookManagerStatusText.text = "FB Mgr : " + FacebookManager.FacebookStatus.ToString();
        // Facebook Login Status
        FacebookLoginStatusText.text = "FB Login : " + FacebookManager.IsUserLoggedIn.ToString();

        // Message Text
        MessageText.gameObject.SetActive(false);
        if (PlayFabManager.PlayFabStatus == PlayFabStatuses.LoggingIn)
        {
            MessageText.gameObject.SetActive(true);
            MessageText.text = "Logging in...";
        }

        if (JUMPConnectionStatus == JUMPConnectionStatus.RetrievingAuthToken)
        {
            MessageText.gameObject.SetActive(true);
            MessageText.text = "Authenticating...";
        }

        if (JUMPConnectionStatus == JUMPConnectionStatus.Connecting)
        {
            MessageText.gameObject.SetActive(true);
            MessageText.text = "Connecting to Photon...";
        }

        if (ProfileManager.ProfileStatus == ProfileStatuses.Loading)
        {
            MessageText.gameObject.SetActive(true);
            MessageText.text = "Loading Profile...";
        }

        if (ProfileManager.ProfileStatus == ProfileStatuses.Saving)
        {
            MessageText.gameObject.SetActive(true);
            MessageText.text = "Saving Profile...";
        }

        // Connect to Photon when PlayFab is connected
        if (PlayFabManager.PlayFabStatus == PlayFabStatuses.LogInOk && JUMPConnectionStatus == JUMPConnectionStatus.None)
        {
            if ((JUMPConnection != null) && (!JUMPConnection.gameObject.activeSelf))
            {
                ConnectToPhoton();
            }
        }

            // Online Play Buttons
            OnlinePlayButton.gameObject.SetActive(false);
        if (   PlayFabManager.PlayFabStatus == PlayFabStatuses.LogInOk 
            && ProfileManager.ProfileStatus != ProfileStatuses.Loading
            && ProfileManager.ProfileStatus != ProfileStatuses.Saving
            && JUMPConnectionStatus == JUMPConnectionStatus.Online)
        {
            OnlinePlayButton.gameObject.SetActive(true);
        }

        OfflinePlayButton.gameObject.SetActive(false);
        if (   PlayFabManager.PlayFabStatus == PlayFabStatuses.LogInError 
            || PlayFabManager.PlayFabStatus == PlayFabStatuses.InitializedError
            || JUMPConnectionStatus == JUMPConnectionStatus.Offline
            || JUMPConnectionStatus == JUMPConnectionStatus.AuthTokenFailure)
        {
            OfflinePlayButton.gameObject.SetActive(true);
        }

        // Facebook Button
        FacebookLoginButton.gameObject.SetActive(false);
        if (   PlayFabManager.PlayFabStatus == PlayFabStatuses.LogInOk
            && !FacebookManager.IsUserLoggedIn
            && (FacebookManager.FacebookStatus == FacebookStatuses.InitializedOk || FacebookManager.FacebookStatus == FacebookStatuses.LogInOk))
        {
            FacebookLoginButton.gameObject.SetActive(true);
        }
    }

    private void ConnectToPhoton()
    {
        Logger.Verbose("MainPageScript.ConnectToPhoton");
        // Retrieve the Photon Auth Token
        JUMPConnectionStatus = JUMPConnectionStatus.RetrievingAuthToken;

        GetPhotonAuthenticationTokenRequest request = new GetPhotonAuthenticationTokenRequest();
        request.PhotonApplicationId = PhotonAppID;

        // get an authentication ticket to pass on to Photon
        PlayFabClientAPI.GetPhotonAuthenticationToken(request, OnPhotonAuthenticationSuccess, OnPlayFabError);

    }

    private void OnPlayFabError(PlayFabError obj)
    {
        Logger.Verbose("MainPageScript.OnPlayFabError");
        // Something wend bad retrieving the auth key
        JUMPConnectionStatus = JUMPConnectionStatus.AuthTokenFailure;
    }

    private void OnPhotonAuthenticationSuccess(GetPhotonAuthenticationTokenResult obj)
    {
        Logger.Verbose("MainPageScript.OnPhotonAuthenticationSuccess");

        // Set the Photon Connection Settings
        // See https://api.playfab.com/docs/using-photon-with-playfab/
        PhotonNetwork.PhotonServerSettings.AppID = PhotonAppID;
        PhotonNetwork.PhotonServerSettings.HostType = ServerSettings.HostingOption.PhotonCloud;
        PhotonNetwork.PhotonServerSettings.PreferredRegion = CloudRegionCode.us;
        PhotonNetwork.PhotonServerSettings.Protocol = ExitGames.Client.Photon.ConnectionProtocol.Udp;

        string authToken = obj.PhotonCustomAuthenticationToken;

        AuthenticationValues customAuth = new AuthenticationValues();
        customAuth.AuthType = CustomAuthenticationType.Custom;
        customAuth.AddAuthParameter("username", PlayFabManager.PlayfabId);    // expected by PlayFab custom auth service
        customAuth.AddAuthParameter("token", authToken);                      // expected by PlayFab custom auth service

        JUMPOptions.GameVersion = AppID;
        JUMPOptions.CustomAuth = customAuth;

        // Now Activate the JUMP Connection object that will trigger login to Photon.
        JUMPConnectionStatus = JUMPConnectionStatus.Connecting;
        JUMPConnection.gameObject.SetActive(true);
    }

    public void JUMPMasterConnect()
    {
        Logger.Verbose("MainPageScript.JUMPMasterConnect");
        if (JUMPConnection != null && JUMPConnectionStatus == JUMPConnectionStatus.Connecting)
        {
            JUMPConnectionStatus = JUMPMultiplayer.IsOffline ? JUMPConnectionStatus.Offline : JUMPConnectionStatus.Online;
        }
    }

    public void GoToMatchmake()
    {
        // Load the debug screen to allow pre-sign in with Facebook
        SceneManager.LoadScene("Matchmake");
    }
}
