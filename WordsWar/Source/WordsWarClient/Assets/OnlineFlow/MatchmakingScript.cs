using UnityEngine;
using System.Collections;
using JUMP;
using UnityEngine.UI;

public class MatchmakingScript : MonoBehaviour {

    public JUMPMultiplayer JUMPMaster;
    public JUMPMultiplayer JUMPGameRoom;
    public Text MatchmakingStatusText;

    // Use this for initialization
    void Start () {
        StartMatchmake();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartMatchmake()
    {
        if (JUMPMaster != null)
        {
            MatchmakingStatusText.text = "Joining Random Room";
            JUMPMaster.Matchmake();
        }
    }

    public void GameRoomConnect()
    {
        if (JUMPGameRoom != null)
        {
            MatchmakingStatusText.text = "In the room";
            JUMPMaster.gameObject.SetActive(false);
            JUMPGameRoom.gameObject.SetActive(true);
            // Wait in the room for 30 seconds for an opponent, then go out and try to reconnect.
            StartCoroutine(Wait30SecondsInTheGameRoom());
        }
    }

    public IEnumerator Wait30SecondsInTheGameRoom()
    {
        int countdown = 10;
        while (countdown > 0)
        {
            MatchmakingStatusText.text = string.Format("In the room: ({0})", countdown);
            yield return new WaitForSeconds(1.0f);
            MatchmakingStatusText.text = string.Format("In the room: ({0})", countdown);
            countdown--;
        }
        if (JUMPGameRoom != null)
        {
            JUMPGameRoom.CancelGameRoom();
        }
    }

    public void GameRoomDisconnect()
    {
        MatchmakingStatusText.text = "In the room";
        JUMPMaster.gameObject.SetActive(true);
        JUMPGameRoom.gameObject.SetActive(false);
        StartMatchmake();
    }

}
