using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {
    static MusicPlayer instance = null;

	// Use this for initialization
	void Awake () {
        if (instance != null)
        {
            Destroy(gameObject);
            print("duplicate music player, self-destruct");
        }
        else
        {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
	}
}
