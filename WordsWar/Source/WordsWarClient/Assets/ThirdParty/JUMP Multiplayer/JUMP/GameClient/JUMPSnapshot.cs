using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JUMP
{
     public class JUMPSnapshotData
    {
    }

    public class JUMPSnapshot : JUMPSnapshot<JUMPSnapshotData>
    {
        public JUMPSnapshot(JUMPSnapshotData snapshotData) : base(snapshotData)
        {
        }
    }

    public class JUMPSnapshot<T> : JUMPCommand where T : JUMPSnapshotData
    {
        public const byte JUMPSnapshot_EventCode = 190;

        public T SnapshotData { get { return (T)CommandData[0]; } set { CommandData[0] = value; } }

        // Create a command to send with this initializer
        public JUMPSnapshot(T snapshotData) : base(new object[1], JUMPSnapshot_EventCode)
        {
            SnapshotData = snapshotData;
        }

        // Create a command when receiving it from Photon
        public static JUMPSnapshot<T> FromData(object[] data)
        {
            return (JUMPSnapshot<T>)new JUMPCommand(data, JUMPSnapshot_EventCode);
        }
    }
}

