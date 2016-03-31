using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BoardConstruction.UnitTest
{
    public class AlphabetsTest
    {
        [Fact]
        public void LoadAlphabets() {
            Alphabets.LoadAlphabets("./BoardData/alphabets.json");
            Assert.Equal(1, Alphabets.AlphabetList.Count);
        }
    }
}
