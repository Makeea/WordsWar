using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class ProfileCache
{
    public static void CleanLocalPlayerCache()
    {
        if (Debug.isDebugBuild)
        {
            Logger.Verbose("PlayerManager.CleanLocalPlayerCache");
            PlayerPrefs.DeleteAll();
            ProfileUtilities.DeleteTextureFile("ProfilePicture");
        }
    }

    public static void LoadPlayerFromLocalCache()
    {
        Logger.Verbose("PlayerManager.LoadPlayerFromLocalCache");
        ProfileManager player = ProfileManager.Current;

        player.Nickname = PlayerPrefs.GetString("Nickname", "");
        player.Email = PlayerPrefs.GetString("Email", "");
        player.Level = PlayerPrefs.GetInt("Level", 1);
        player.LifetimePoints = PlayerPrefsUtil.GetLong("LifetimePoints", 0);
        player.PictureTexture = ProfileUtilities.LoadTextureFile("ProfilePicture");
        if (player.PictureTexture != null)
        {
            player.RefreshPictureTexture = true;
        }
    }

    public static void SavePlayerToLocalCache()
    {
        Logger.Verbose("PlayerManager.SavePlayerToLocalCache");
        ProfileManager player = ProfileManager.Current;

        PlayerPrefs.SetString("Nickname", player.Nickname);
        PlayerPrefs.SetString("Email", player.Email);
        PlayerPrefs.SetInt("Level", player.Level);
        PlayerPrefs.SetString("LifetimePoints", player.LifetimePoints.ToString());
        ProfileUtilities.SaveTextureToFile("ProfilePicture", player.PictureTexture);
    }
}
