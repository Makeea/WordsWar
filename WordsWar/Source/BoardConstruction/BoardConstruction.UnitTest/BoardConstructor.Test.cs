using BoardConstruction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

public class BoardConstructorTest
{
    [Fact]
    public void ConstructABoard()
    {
        BoardSolution sol = null;
        string solboard = "";
        int words = 0;

        sol = BoardConstructor.Build_Solve(4, "en-us", "english-words");
        solboard = sol.SolvedBoard.ToString();
        words = sol.SolutionWords.Count;

        Assert.InRange(words, 1, int.MaxValue);
        Assert.Equal(24, solboard.Length);
        Assert.Equal(4, sol.SolvedBoard.Size);
        Assert.Equal("en-us", sol.AlphabetId);
        Assert.Equal("english-words", sol.DictionaryId);
    }

    [Fact]
    public void ExistingBoard()
    {
        GameBoard board = new GameBoard(new char[4, 4]
        {
                { 'y', 'o', 'x', 'f' },
                { 'y', 'o', 'x', 'u' },
                { 'r', 'b', 'a', 'n' },
                { 'v', 'e', 'd', 'd' }
        });

        // Load the Alphabet
        Alphabet alphabet = Alphabets.Load("en-us");

        // Load the Dictionary
        Dictionary dict = Dictionaries.Load("english-words");

        BoardSolution sol = BoardConstructor.Solve(alphabet, dict, board);
        string solboard = sol.SolvedBoard.ToString();
        int words = sol.SolutionWords.Count;

        Assert.InRange(words, 1, int.MaxValue);
        Assert.Equal(24, solboard.Length);
        Assert.Equal(4, sol.SolvedBoard.Size);
        Assert.Equal("en-us", sol.AlphabetId);
        Assert.Equal("english-words", sol.DictionaryId);
    }
}
