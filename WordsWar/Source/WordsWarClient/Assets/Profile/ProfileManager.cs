using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlayFab.ClientModels;

[FlagsAttribute]
public enum ProfileStatuses
{
    None = 0,
    Loading = 1 << 0,
    Saving = 1 << 1,
    Ready = 1 << 2,
    Error = 1 << 5,
}

public class ProfileManager
{
    public string   Nickname;
    public string   Email;
    public int      Level;
    public long     LifetimePoints;
    public Texture2D PictureTexture = null;
    public bool RefreshPictureTexture = false;  // set when you want to reload the PictureTexture above

    public static ProfileStatuses ProfileStatus = ProfileStatuses.None;

    private static ProfileManager _player = new ProfileManager();
    public static ProfileManager Current { get { return _player; } }

    internal static void Init()
    {
        Logger.Verbose("PlayerManager.Init");
        // Load from the local cache, then refresh when connected
        ProfileCache.LoadPlayerFromLocalCache();
    }

    internal static void CreateBrandNewPlayer(string playFabId)
    {
        Logger.Verbose("PlayerManager.CreateBrandNewPlayer");

        int randomPlayerID = Math.Abs(SystemInfo.deviceUniqueIdentifier.GetHashCode()) % 1000000000;
        string randomPlayerIDStr = randomPlayerID.ToString("D6");

        _player.Nickname = "Player" + randomPlayerIDStr;
        _player.Email = "";
        _player.Level = 1;
        _player.LifetimePoints = 0L;

        SaveProfile();
    }

    internal static void DownloadProfile()
    {
        // Retrieve the local account with what was on the cloud.
        ProfileStatus = ProfileStatuses.Loading;
        PlayFabManager.GetLocalPlayerProfileFromPlayFab();

        //// Now Refresh Facebook properties to your profile
        //FacebookManager.DownloadPlayerPicture();
        //FacebookManager.DownloadPlayerEmail();
    }

    internal static void SaveProfile()
    {
        // Save the player locally
        ProfileCache.SavePlayerToLocalCache();
        // And to the cloud
        ProfileStatus = ProfileStatuses.Saving;
        PlayFabManager.SavePlayerProfileToPlayFab();
    }

    internal static Dictionary<string, string> GetUserDataForPFAPI(string[] fields = null)
    {
        Logger.Verbose("PlayerManager.GetUserDataForPFAPI");
        Dictionary<string, string> result = new Dictionary<string, string>();

        if (fields == null || fields.Contains("Nickname", StringComparer.OrdinalIgnoreCase)) result.Add("Nickname", _player.Nickname);
        if (fields == null || fields.Contains("Email", StringComparer.OrdinalIgnoreCase)) result.Add("Email", _player.Email);
        if (fields == null || fields.Contains("Level", StringComparer.OrdinalIgnoreCase)) result.Add("Level", _player.Level.ToString());
        if (fields == null || fields.Contains("LifetimePoints", StringComparer.OrdinalIgnoreCase)) result.Add("LifetimePoints", _player.LifetimePoints.ToString());
        return result;
    }

    internal static void UpdateUserDataFromPFAPI(Dictionary<string, UserDataRecord> userData)
    {
        Logger.Verbose("PlayerManager.UpdateUserDataFromPFAPI");

        _player.Nickname = userData.GetValueOrDefault("Nickname", _player.Nickname);
        _player.Email = userData.GetValueOrDefault("Email", _player.Nickname);
        _player.Level = userData.GetValueOrDefault("Level", _player.Level);
        _player.LifetimePoints = userData.GetValueOrDefault("LifetimePoints", _player.LifetimePoints);

        // Now save the player locally
        ProfileCache.SavePlayerToLocalCache();
    }

    internal static void SetNewPicture(Texture2D texture)
    {
        Logger.Verbose("PlayerManager.SetNewPicture");
        _player.PictureTexture = texture;
        _player.RefreshPictureTexture = true;
        // Now save the player locally
        ProfileCache.SavePlayerToLocalCache();
    }

    internal static void SetNewEmail(string email)
    {
        _player.Email = email;
        // Now save the player locally
        ProfileCache.SavePlayerToLocalCache();
        // Update Playfab:
        PlayFabManager.SavePlayerProfileToPlayFab(new string[] { "Email" });
    }

    internal static void SetNewNickname(string name)
    {
        _player.Nickname = name;
        // Now save the player locally
        ProfileCache.SavePlayerToLocalCache();
        // Update Playfab:
        PlayFabManager.SavePlayerProfileToPlayFab(new string[] { "Nickname" });
    }
}

