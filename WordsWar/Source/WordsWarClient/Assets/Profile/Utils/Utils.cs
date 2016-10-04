using System.Collections.Generic;
using PlayFab.ClientModels;
using UnityEngine;
using System.IO;

public static class Utils
{
    public static string GetValueOrDefault(this Dictionary<string, UserDataRecord> dic, string key, string def = "")
    {
        if (dic.ContainsKey(key))
        {
            return dic[key].Value;
        }
        else
        {
            return def;
        }
    } 

    public static int GetValueOrDefault(this Dictionary<string, UserDataRecord> dic, string key, int def = 0)
    {
        string res = dic.GetValueOrDefault(key, def.ToString());
        int result = 0;
        int.TryParse(res, out result);

        return result;
    }

    public static long GetValueOrDefault(this Dictionary<string, UserDataRecord> dic, string key, long def = 0L)
    {
        string res = dic.GetValueOrDefault(key, def.ToString());
        long result = 0;
        long.TryParse(res, out result);

        return result;
    }
}

public static class PlayerPrefsUtil
{
    public static long GetLong(string Key, long defaultValue)
    {
        long result;
        if (long.TryParse(PlayerPrefs.GetString(Key, defaultValue.ToString()), out result))
        {
            return result;
        }
        else
        {
            return defaultValue;
        }
    }
}

public class ProfileUtilities
{
    public static void SaveTextureToFile(string fileName, Texture2D texture)
    {
        if (texture != null)
        {
            byte[] bytes = texture.EncodeToPNG();
            FileStream file = File.Open(Application.dataPath + "/" + fileName + ".png", FileMode.Create);
            BinaryWriter binary = new BinaryWriter(file);
            binary.Write(bytes);
            file.Close();
        }
    }

    internal static Texture2D LoadTextureFile(string fileName)
    {
        Texture2D tex = null;
        byte[] fileData;
        string filePath = Application.dataPath + "/" + fileName + ".png";

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }

    internal static void DeleteTextureFile(string fileName)
    {
        string filePath = Application.dataPath + "/" + fileName + ".png";

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}