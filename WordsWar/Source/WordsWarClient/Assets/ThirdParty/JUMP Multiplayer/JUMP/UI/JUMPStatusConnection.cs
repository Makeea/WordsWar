using UnityEngine;
using UnityEngine.UI;

namespace JUMP
{
    class JUMPStatusConnection : Photon.PunBehaviour
    {
        private Text ConnectionStatus = null;

        [SerializeField]
        private bool debugMode;

        void Start()
        {
            ConnectionStatus = GetComponent<Text>();

            // Do not show informations when not in editor or debug mode
            if ((!Application.isEditor) && (!debugMode))
            {
                gameObject.SetActive(false);
            }
        }

        void Update()
        {
            if (ConnectionStatus != null)
            {
                ConnectionStatus.text = "Photon connection status: " + PhotonNetwork.connectionStateDetailed.ToString();
            }
        }
    }
}
