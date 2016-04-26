using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JUMP
{
    // Example Command
    public class mycommand_sample : JUMPCommand
    {
        public const byte MYCommand_EventCode = 0;

        public int Property { get { return (int)CommandData[0]; } set { CommandData[0] = value; } }

        // Create a command to send with this initializer
        public mycommand_sample(int propertyValue) : base(new object[1], MYCommand_EventCode)
        {
            Property = propertyValue;
        }

        // Create a command when receiving it from Photon
        public static mycommand_sample FromData(object[] data)
        {
            return (mycommand_sample)new JUMPCommand(data, MYCommand_EventCode);
        }
    }

    public class hot_to_use_command_sample
    {
        public void sendcommand_sample()
        {
            // Send Command:
            mycommand_sample c = new mycommand_sample(3);
            PhotonNetwork.RaiseEvent(c.CommandEventCode, c.CommandData, true, null);
        }

        private static void OnPhotonEventCall(byte eventCode, object content, int senderId)
        {
            // We are only receiving events for the Master Client
            if (eventCode == mycommand_sample.MYCommand_EventCode)
            {
                mycommand_sample c = mycommand_sample.FromData((object[])content);
                int val = c.Property;
                val++;
            }
        }
    }
}
