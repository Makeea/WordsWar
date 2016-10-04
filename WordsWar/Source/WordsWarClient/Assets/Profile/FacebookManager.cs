using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facebook.Unity;

[FlagsAttribute]
public enum FacebookStatuses
{
    None = 0,
    Initializing = 1 << 0,
    InitializedOk = 1 << 1,
    InitializedError = 1 << 2,
    LoggingIn = 1 << 3,
    LogInOk = 1 << 4,
    LogInError = 1 << 5,
}

public class FacebookManager
{
    public static FacebookStatuses FacebookStatus = FacebookStatuses.None;
    public static bool IsUserLoggedIn { get { return FB.IsLoggedIn; } }

    internal static void Init()
    {
        Logger.Verbose("FacebookManager.Init");
        // Initialize Facebook
        if (!FB.IsInitialized)
        {
            FacebookStatus = FacebookStatuses.Initializing;
            FB.Init(() => {
                if (FB.IsInitialized)
                {
                    FacebookStatus = FacebookStatuses.InitializedOk;
                }
                else
                {
                    FacebookStatus = FacebookStatuses.InitializedError;
                    Logger.Error("Failed to Inizialize the Facebook SDK");
                }
            });
        }
        else
        {
            FacebookStatus = FacebookStatuses.InitializedOk;
        }
    }

    public static void DebugLoginToFacebook()
    {
        if (Debug.isDebugBuild)
        {
            FacebookStatus = FacebookStatuses.LoggingIn;
            var perms = new List<string>() { "public_profile", "email", "user_friends" };
            FB.LogInWithReadPermissions(perms,
                (ILoginResult result) =>
                {
                    if (FB.IsLoggedIn)
                    {
                            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
                            Logger.Verbose("Facebook sign in complete: user id : {0}", aToken.UserId);
                    }
                    else
                    {
                        if (result.Cancelled)
                        {
                            Logger.Verbose("FB cancelled login");
                        }
                        else
                        {
                            Logger.Error("FB Login Error: {0}", result.Error);
                        }
                    }

                });
        }
    }

internal static void LoginToFacebook()
    {
        Logger.Verbose("FacebookManager.LoginToFacebook");

        // Login to FB without creating account
        // Must be already signed in with PF and FB must be ok
        if (!FB.IsInitialized)
        {
            Logger.Error("Facebook is not intialized!");
        }
        else if (FacebookStatus != FacebookStatuses.InitializedOk)
        {
            Logger.Error("FacebookStatus {0} is not Initialize Ok", FacebookStatus.ToString());
        }
        else if (PlayFabManager.PlayFabStatus != PlayFabStatuses.LogInOk)
        {
            Logger.Error("PlayFabStatus: {0} is not Login OK", PlayFabManager.PlayFabStatus.ToString());
        }
        else 
        {
            FacebookStatus = FacebookStatuses.LoggingIn;
            var perms = new List<string>() { "public_profile", "email", "user_friends" };
            FB.LogInWithReadPermissions(perms, 
                (ILoginResult result) =>
                {
                    if (FB.IsLoggedIn)
                    {
                        FacebookStatus = FacebookStatuses.LogInOk;
                        // AccessToken class will have session details
                        var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
                        // Print current access token's User ID
                        Logger.Verbose("Facebook sign in complete: user id : {0}", aToken.UserId);
                        // Connect Facebook account to PlayFab
                        PlayFabManager.ConnectOrRetrieveFacebookProfile();
                    }
                    else {
                        if (result.Cancelled)
                        {
                            // Go back to Initialized status
                            FacebookStatus = FacebookStatuses.InitializedOk;
                            Logger.Verbose("FB cancelled login");
                        }
                        else
                        {
                            FacebookStatus = FacebookStatuses.LogInError;
                            Logger.Error("FB Login Error: {0}", result.Error);
                        }
                    }

                });
        }
    }

    internal static void DownloadPlayerPicture()
    {
        Logger.Verbose("FacebookManager.DownloadPlayerPicture");

        if (FB.IsLoggedIn)
        {
            // Make a Graph API call to get email address
            FB.API("/me/picture?type=small", HttpMethod.GET, 
                (graphResult) =>
                {
                    if (string.IsNullOrEmpty(graphResult.Error) == false)
                    {
                        Logger.Error("Could not get Facebook picture: {0}", graphResult.Error);
                    }
                    else {
                        Logger.Verbose("FB Picture retrieved");
                        ProfileManager.SetNewPicture(graphResult.Texture);
                    }
                });
        }
        else
        {
            Logger.Error("User not logged in");
        }
    }

    internal static void DownloadPlayerEmail()
    {
        Logger.Verbose("FacebookManager.DownloadPlayerPicture");

        if (FB.IsLoggedIn)
        {
            // Make a Graph API call to get email address
            FB.API("/me?fields=email,name", HttpMethod.GET,
                (graphResult) =>
                {
                    if (string.IsNullOrEmpty(graphResult.Error) == false)
                    {
                        Logger.Error("could not get email address: {0}", graphResult.Error);
                    }
                    else
                    {
                        Logger.Verbose("Email retrieved");
                        string email = graphResult.ResultDictionary["email"] as string;
                        string name = graphResult.ResultDictionary["name"] as string;
                        ProfileManager.SetNewEmail(email);
                        ProfileManager.SetNewNickname(name);
                    }
                });
        }
        else
        {
            Debug.Log("User not logged in");
        }
    }
}
