using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlayFab;
using PlayFab.ClientModels;

[FlagsAttribute]
public enum PlayFabStatuses
{
    None = 0,
    Initializing = 1 << 0,
    InitializedOk = 1 << 1,
    InitializedError = 1 << 2,
    LoggingIn = 1 << 3,
    LogInOk = 1 << 4,
    LogInError = 1 << 5,
}


public class PlayFabManager
{
    public static PlayFabStatuses PlayFabStatus = PlayFabStatuses.None;
    private static string DeviceUniqueIdentifier { get
        {
#if DEBUG
            return "E173D146-C0CE-415F-AEB3-8F213AA43DFE-SECOND";
#else
            return SystemInfo.deviceUniqueIdentifier;
#endif
        }
    }

    public static string PlayfabId = "";

    internal static void Init()
    {
        PlayFabStatus = PlayFabStatuses.Initializing;
        Logger.Verbose("PlayFabManager.Init");
        PlayFabSettings.TitleId = "B2D4";
        PlayFabStatus = PlayFabStatuses.InitializedOk;
    }

    internal static void LoginUsingFacebookId()
    {
        Logger.Verbose("PlayFabManager.LoginUsingFacebook, status: {0}", PlayFabStatus);

        if (PlayFabStatus == PlayFabStatuses.InitializedOk && FacebookManager.IsUserLoggedIn)
        {
            // Login only if we are not
            if (!PlayFabClientAPI.IsClientLoggedIn())
            {
                PlayFabStatus = PlayFabStatuses.LoggingIn;

                var loginRequest = new LoginWithFacebookRequest();
                loginRequest.AccessToken = Facebook.Unity.AccessToken.CurrentAccessToken.TokenString;
                loginRequest.CreateAccount = false;
                PlayFabClientAPI.LoginWithFacebook(loginRequest,
                    (LoginResult result) =>
                    {
                        PlayFabStatus = PlayFabStatuses.LogInOk;
                        PlayfabId = result.PlayFabId;
                        bool created = result.NewlyCreated;
                        Logger.Verbose("PLAYFAB LOGIN OK, PlayFab ID: {0}, Created: {1}", PlayfabId, created);
                        ProfileManager.DownloadProfile();
                    },
                    (PlayFabError error) =>
                    {
                        PlayFabStatus = PlayFabStatuses.LogInError;
                        Logger.Verbose("PLAYFAB LOGIN ERROR! {0}", error.ErrorMessage);
                    });
            }
        }
    }

    internal static void LoginUsingDeviceId()
    {
        Logger.Verbose("PlayFabManager.SignInUsingDeviceId, status: {0}", PlayFabStatus);
        if (PlayFabStatus == PlayFabStatuses.InitializedOk)
        {
            // Login only if we are not
            if (!PlayFabClientAPI.IsClientLoggedIn())
            {
                PlayFabStatus = PlayFabStatuses.LoggingIn;

                var loginRequest = new LoginWithCustomIDRequest();
                loginRequest.CustomId = DeviceUniqueIdentifier;
                loginRequest.CreateAccount = true;
                PlayFabClientAPI.LoginWithCustomID(loginRequest,
                    (LoginResult result) =>
                    {
                        PlayfabId = result.PlayFabId;
                        bool created = result.NewlyCreated;
                        Logger.Verbose("PLAYFAB LOGIN OK, PlayFab ID: {0}, Created: {1}", PlayfabId, created);
                        if (created)
                        {
                            // Brand new player
                            PlayFabStatus = PlayFabStatuses.LogInOk;
                            ProfileManager.CreateBrandNewPlayer(PlayfabId);
                        }
                        else
                        {
                            PlayFabStatus = PlayFabStatuses.LogInOk;
                            ProfileManager.DownloadProfile();
                        }
                    },
                    (PlayFabError error) =>
                    {
                        PlayFabStatus = PlayFabStatuses.LogInError;
                        Logger.Verbose("PLAYFAB LOGIN ERROR! {0}", error.ErrorMessage);
                    });
            }
        }
    }

    internal static void SwitchToFacebookIdProfile()
    {
        Logger.Verbose("PlayFabManager.SwitchToFacebookIdProfile, status: {0}", PlayFabStatus);

        // We should already be logged in at this point
        if (PlayFabStatus == PlayFabStatuses.LogInOk && FacebookManager.IsUserLoggedIn)
        {
            PlayFabStatus = PlayFabStatuses.LoggingIn;

            var loginRequest = new LoginWithFacebookRequest();
            loginRequest.AccessToken = Facebook.Unity.AccessToken.CurrentAccessToken.TokenString;
            loginRequest.CreateAccount = false;
            PlayFabClientAPI.LoginWithFacebook(loginRequest,
                (LoginResult result) =>
                {
                    PlayFabStatus = PlayFabStatuses.LogInOk;
                    PlayfabId = result.PlayFabId;
                    bool created = result.NewlyCreated;
                    Logger.Verbose("PLAYFAB LOGIN OK, PlayFab ID: {0}, Created: {1}", PlayfabId, created);
                    // Now update the local account with what was on the cloud.
                    PlayFabManager.GetLocalPlayerProfileFromPlayFab();
                    // Now Refresh Facebook properties to your profile
                    FacebookManager.DownloadPlayerPicture();
                    FacebookManager.DownloadPlayerEmail();
                },
                (PlayFabError error) =>
                {
                    PlayFabStatus = PlayFabStatuses.LogInError;
                    Logger.Verbose("PLAYFAB LOGIN ERROR! {0}", error.ErrorMessage);
                });
        }
        else
        {
            Logger.Error("Player should be already signed in to switch to another profile based on their Facebook ID");
        }
    }

    internal static void SavePlayerProfileToPlayFab(string[] fields = null)
    {
        Logger.Verbose("PlayFabManager.SavePlayerProfile");
        var updateUserDataRequest = new UpdateUserDataRequest();

        updateUserDataRequest.Data = ProfileManager.GetUserDataForPFAPI(fields);
        PlayFabClientAPI.UpdateUserData(updateUserDataRequest, 
            (UpdateUserDataResult res) =>
            {
                Logger.Verbose("PLAYFAB UPDATE DATA OK");
                if (ProfileManager.ProfileStatus == ProfileStatuses.Saving) ProfileManager.ProfileStatus = ProfileStatuses.Ready;
            },
            (PlayFabError error) =>
            {
                Logger.Verbose("PLAYFAB LOGIN ERROR! {0}", error.ErrorMessage);
                if (ProfileManager.ProfileStatus == ProfileStatuses.Saving) ProfileManager.ProfileStatus = ProfileStatuses.Error;
            });
    }

    internal static void GetLocalPlayerProfileFromPlayFab()
    {
        Logger.Verbose("PlayerManager.UpdateLocalPlayerFromPlayFab");
        var getUserDataRequest = new GetUserDataRequest();

        PlayFabClientAPI.GetUserData(getUserDataRequest,
            (GetUserDataResult result) =>
            {
                Logger.Verbose("PLAYFAB GETUSERDATA OK");
                ProfileManager.UpdateUserDataFromPFAPI(result.Data);
                if (ProfileManager.ProfileStatus == ProfileStatuses.Loading) ProfileManager.ProfileStatus = ProfileStatuses.Ready;
            },
            (PlayFabError error) =>
            {
                Logger.Verbose("PLAYFAB ERROR! {0}", error.ErrorMessage);
                if (ProfileManager.ProfileStatus == ProfileStatuses.Loading) ProfileManager.ProfileStatus = ProfileStatuses.Error;
            });
    }

    internal static void ConnectOrRetrieveFacebookProfile()
    {
        Logger.Verbose("PlayerManager.ConnectFacebookProfile");

        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            Logger.Error("PlayFab is not logged in");
        }
        else if (PlayFabStatus != PlayFabStatuses.LogInOk)
        {
            Logger.Error("PlayFabStatus: {0} is not Login OK", PlayFabManager.PlayFabStatus.ToString());
        }
        else if (FacebookManager.FacebookStatus != FacebookStatuses.LogInOk)
        {
            Logger.Error("Facebook status: {0} is not Login OK", FacebookManager.FacebookStatus.ToString());
        }
        else
        {
            var linkFacebookAccount = new LinkFacebookAccountRequest();
            linkFacebookAccount.AccessToken = Facebook.Unity.AccessToken.CurrentAccessToken.TokenString;
            PlayFabClientAPI.LinkFacebookAccount(linkFacebookAccount, 
                (LinkFacebookAccountResult result) =>
                {
                    Logger.Verbose("Connection with Facebook Success");
                    // Now Refresh Facebook properties to your profile
                    FacebookManager.DownloadPlayerPicture();
                    FacebookManager.DownloadPlayerEmail();
                },
                (PlayFabError error) =>
                {
                    if (error.Error == PlayFabErrorCode.LinkedAccountAlreadyClaimed)
                    {
                        // Account already claimed - do switch and override local player
                        SwitchToFacebookIdProfile();
                    }
                    else
                    {
                        Logger.Error("Connection with Facebook Error: {0}", error.ErrorMessage);
                    }
                });
        }


    }
}

