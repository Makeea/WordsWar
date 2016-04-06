using BoardConstruction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

public class AlphabetsTest
{
    [Fact]
    public void LoadAlphabets() {
        // Load the Alphabet
        Alphabet alphabet = Alphabets.Load("en-us");
        Assert.Equal(26, alphabet.letters.Count);
    }
}
