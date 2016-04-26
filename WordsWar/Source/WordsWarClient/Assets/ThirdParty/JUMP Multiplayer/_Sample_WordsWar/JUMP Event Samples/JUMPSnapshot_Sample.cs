using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JUMP
{
    // Example of use:
    public class mysnapshotdata_sample : JUMPSnapshotData
    {
        public int MyData;
    }

    public class mysnapshot_sample
    {
        public void send_snapshot_sample()
        {
            // Send Command:
            mysnapshotdata_sample mysnap = new mysnapshotdata_sample();
            JUMPSnapshot<mysnapshotdata_sample> c = new JUMPSnapshot<mysnapshotdata_sample>(mysnap);
            PhotonNetwork.RaiseEvent(c.CommandEventCode, c.CommandData, true, null);
        }

        private static void OnPhotonEventCall(byte eventCode, object content, int senderId)
        {
            // We are only receiving events for the Master Client
            if (eventCode == JUMPSnapshot<mysnapshotdata_sample>.JUMPSnapshot_EventCode)
            {
                mysnapshotdata_sample snap = JUMPSnapshot<mysnapshotdata_sample>.FromData((object[])content).SnapshotData;
                int val = snap.MyData;
                val++;
            }
        }
    }

}
