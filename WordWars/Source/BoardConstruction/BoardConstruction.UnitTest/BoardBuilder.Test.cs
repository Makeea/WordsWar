using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BoardConstruction.UnitTest
{
    public class BoardBuilderTest
    {
        [Fact]
        void TestBoardBuilder()
        {
            string debug;
            GameBoard board;
            board = BoardBuilder.GenerateRandom(4, Alphabets.EnUs);
            Assert.Equal(4, board.Size);
            debug = board.ToString();

            board = BoardBuilder.GenerateRandom(6, Alphabets.ItIt);
            Assert.Equal(6, board.Size);
            debug = board.ToString();
            Assert.DoesNotContain('y',debug);
        }
    }
}
