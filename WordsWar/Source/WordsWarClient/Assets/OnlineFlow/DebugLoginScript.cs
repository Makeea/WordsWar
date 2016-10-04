using UnityEngine;
using System.Collections;

public class DebugLoginScript : MonoBehaviour {

    void Awake()
    {
        FacebookManager.Init();
    }

    public void CleanupLocalProfile()
    {
        ProfileCache.CleanLocalPlayerCache();
    }

    public void DebugLoginToFacebook()
    {
        FacebookManager.DebugLoginToFacebook();
    }

    public void GoToLoginPage()
    {
        DebugManager.GoToLoginPage();
    }
}
