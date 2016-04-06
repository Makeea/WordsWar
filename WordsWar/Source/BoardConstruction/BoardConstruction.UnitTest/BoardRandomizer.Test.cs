using BoardConstruction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

public class BoardRandomizerTest
{
    [Fact]
    void TestBoardRandomizer()
    {
        string debug;
        GameBoard board = null;
        board = BoardRandomizer.GenerateRandom(4, Alphabets.Load("en-us"));
        Assert.Equal(4, board.Size);
        debug = board.ToString();
    }
}
