using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JUMP
{
    public class JUMPCommand
    {
        public byte CommandEventCode { get; protected set; }
        public int SenderID = -1;

        public object[] CommandData;

        public JUMPCommand(object[] data, byte eventcode)
        {
            CommandData = data;
            CommandEventCode = eventcode;
        }
    }

    public class JUMPCommand_Connect : JUMPCommand
    {
        public const byte JUMPCommand_Connect_EventCode = 191;

        public int PlayerID { get { return (int)CommandData[0]; } set { CommandData[0] = value; } }

        public JUMPCommand_Connect(int playerID) : base(new object[1], JUMPCommand_Connect_EventCode)
        {
            PlayerID = playerID;
        }

        public JUMPCommand_Connect(object[] data, byte eventcode) : base(data, eventcode)
        {
        }

        public static JUMPCommand_Connect FromData(object[] data)
        {
            return new JUMPCommand_Connect(data, JUMPCommand_Connect_EventCode);
        }
    }
}


