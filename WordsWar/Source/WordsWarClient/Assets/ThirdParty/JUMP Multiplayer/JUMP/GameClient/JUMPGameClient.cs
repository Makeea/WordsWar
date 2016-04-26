using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace JUMP
{
    public class JUMPSnapshotReceivedUnityEvent : UnityEvent<JUMPSnapshotData> { }

    public class JUMPGameClient: MonoBehaviour
    {
        public JUMPSnapshotReceivedUnityEvent OnSnapshotReceived;

        public void Awake()
        {
            PhotonNetwork.OnEventCall += OnPhotonEventCall;
        }

        public void OnDestroy()
        {
            PhotonNetwork.OnEventCall -= OnPhotonEventCall;
        }

        // receive snapshots from server - you can cast JUMPSnapshotData to your own custom type
        private void OnPhotonEventCall(byte eventCode, object content, int senderId)
        {
            if (eventCode == JUMPSnapshot.JUMPSnapshot_EventCode)
            {
                JUMPSnapshotData snap = JUMPSnapshot.FromData((object[])content).SnapshotData;

                if (OnSnapshotReceived != null)
                {
                    OnSnapshotReceived.Invoke(snap);
                }
            }
        }

        // The client needs to do the sending to the server, because with reliability turned on, it needs to send all the commands until the server acks them
        public void SendCommandToServer(JUMPCommand c)
        {
            RaiseEventOptions options = new RaiseEventOptions();
            options.Receivers = ExitGames.Client.Photon.ReceiverGroup.MasterClient;

            PhotonNetwork.RaiseEvent(c.CommandEventCode, c.CommandData, true, options);
        }

        // Send a connect command to the server with the photon client id
        public void ConnectToServer()
        {
            JUMPCommand_Connect c = new JUMPCommand_Connect(PhotonNetwork.player.ID);
            SendCommandToServer(c);
        }
    }
}
