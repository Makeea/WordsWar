using BoardConstruction;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

    public class GameBoardTests
    {
        [Fact]
        void twobytwoboard()
        {
            GameBoard twobytwo = new GameBoard(new char[2,2] { { 'a', 'b' }, { 'c', 'd' } });
            Assert.Equal(twobytwo.Size, 2);
            Assert.True(twobytwo.Neighbors[0,0].Contains(new Tuple<int, int>(0,1)));
            Assert.True(twobytwo.Neighbors[0, 0].Contains(new Tuple<int, int>(1, 0)));
            Assert.True(twobytwo.Neighbors[0, 0].Contains(new Tuple<int, int>(1, 1)));
        }

        [Fact]
        void fourbyfour()
        {
            GameBoard fourbyfour = new GameBoard(new char[4, 4] 
                { 
                    { 'a', 'b', 'c', 'd' }, 
                    { 'e', 'f', 'g', 'h' },
                    { 'i', 'j', 'k', 'l' },
                    { 'm', 'n', 'o', 'p' }
                });
            Assert.Equal(fourbyfour.Size, 4);
            Assert.Equal(fourbyfour.Neighbors[0, 0].Count,3);
            Assert.True(fourbyfour.Neighbors[0, 0].Contains(new Tuple<int, int>(0, 1)));
            Assert.True(fourbyfour.Neighbors[0, 0].Contains(new Tuple<int, int>(1, 0)));
            Assert.True(fourbyfour.Neighbors[0, 0].Contains(new Tuple<int, int>(1, 1)));
            Assert.False(fourbyfour.Neighbors[0, 0].Contains(new Tuple<int, int>(1, 2)));

            Assert.Equal(fourbyfour.Neighbors[1, 1].Count, 8);
            Assert.True(fourbyfour.Neighbors[1, 1].Contains(new Tuple<int, int>(0, 0)));
            Assert.True(fourbyfour.Neighbors[1, 1].Contains(new Tuple<int, int>(0, 1)));
            Assert.True(fourbyfour.Neighbors[1, 1].Contains(new Tuple<int, int>(0, 2)));
            Assert.True(fourbyfour.Neighbors[1, 1].Contains(new Tuple<int, int>(1, 0)));
            Assert.True(fourbyfour.Neighbors[1, 1].Contains(new Tuple<int, int>(1, 2)));
            Assert.True(fourbyfour.Neighbors[1, 1].Contains(new Tuple<int, int>(2, 0)));
            Assert.True(fourbyfour.Neighbors[1, 1].Contains(new Tuple<int, int>(2, 1)));
            Assert.True(fourbyfour.Neighbors[1, 1].Contains(new Tuple<int, int>(2, 2)));

            Assert.Equal(fourbyfour.Neighbors[3, 3].Count, 3);
            Assert.True(fourbyfour.Neighbors[3, 3].Contains(new Tuple<int, int>(2, 2)));
            Assert.True(fourbyfour.Neighbors[3, 3].Contains(new Tuple<int, int>(2, 3)));
            Assert.True(fourbyfour.Neighbors[3, 3].Contains(new Tuple<int, int>(3, 2)));
            Assert.False(fourbyfour.Neighbors[3, 3].Contains(new Tuple<int, int>(1, 2)));
        }

        [Fact]
        public void edgecases()
        {
            char[,] empty = new char[0,0];
            char[,] oddshaped = new char[1, 2] { { 'a', 'b' } };
            Assert.Throws<ArgumentException>(() => new GameBoard(null));
            Assert.Throws<ArgumentException>(() => new GameBoard(empty));
            Assert.Throws<ArgumentException>(() => new GameBoard(oddshaped));
        }

        //[Theory]
        //[InlineData(3)]
        //public void MyFirstTheory(int value)
        //{
        //    Assert.True(IsOdd(value));
        //}

        //bool IsOdd(int value)
        //{
        //    return value % 2 == 1;
        //}
    }
