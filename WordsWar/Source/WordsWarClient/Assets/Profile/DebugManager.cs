using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.SceneManagement;

public class DebugManager
{
    public static bool DebugInitialized = false;

    internal static void GoToLoginPage()
    {
        Logger.Verbose("DebugManager.GoToLoginPage");
        // You can go back to the Login page now
        DebugInitialized = true;
        SceneManager.LoadScene("MainPage");
    }

    internal static void LoadDebugIntroScreen()
    {
        Logger.Verbose("DebugManager.LoadDebugIntroScreen");
        // Load the debug screen to allow pre-sign in with Facebook
        SceneManager.LoadScene("DebugLogin");
    }
}
