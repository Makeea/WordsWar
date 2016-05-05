using JUMP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class WordsWarCommand_BoardReceived : JUMPCommand
{
    public const byte BoardReceived_EventCode = 100;

    public int PlayerID { get { return (int)CommandData[0]; } set { CommandData[0] = value; } }
    public int BoardID { get { return (int)CommandData[1]; } set { CommandData[1] = value; } }

    // Create a command to send with this initializer
    public WordsWarCommand_BoardReceived(int playerID, int boardID) : base(new object[2], BoardReceived_EventCode)
    {
        PlayerID = playerID;
        BoardID = boardID;
    }

    // Create a command when receiving it from Photon
    public WordsWarCommand_BoardReceived(object[] data) : base(data, BoardReceived_EventCode)
    {
    }
}

public class WordsWarCommand_WordFound : JUMPCommand
{
    public const byte WordFound_EventCode = 101;

    public int PlayerID { get { return (int)CommandData[0]; } set { CommandData[0] = value; } }
    public string WordFound { get { return (string)CommandData[1]; } set { CommandData[1] = value; } }

    // Create a command to send with this initializer
    public WordsWarCommand_WordFound(int playerID, string wordFound) : base(new object[2], WordFound_EventCode)
    {
        PlayerID = playerID;
        WordFound = wordFound;
    }

    // Create a command when receiving it from Photon
    public WordsWarCommand_WordFound(object[] data) : base(data, WordFound_EventCode)
    {
    }
}
