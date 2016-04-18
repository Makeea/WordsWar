using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardConstruction
{
    public class GameBoard
    {
        public enum BoardStringFormat {
            None,
            NewLines,
            Formatted
        };

        Tuple<int, int> Left(int x, int y) { return new Tuple<int, int>(x , y - 1); }
        Tuple<int, int> Right(int x, int y) { return new Tuple<int, int>(x, y + 1); }
        Tuple<int, int> Top(int x, int y) { return new Tuple<int, int>(x - 1, y); }
        Tuple<int, int> Bottom(int x, int y) { return new Tuple<int, int>(x + 1, y); }
        Tuple<int, int> TopLeft(int x, int y) { return new Tuple<int, int>(x - 1, y - 1); }
        Tuple<int, int> TopRight(int x, int y) { return new Tuple<int, int>(x - 1, y + 1); }
        Tuple<int, int> BottomLeft(int x, int y) { return new Tuple<int, int>(x + 1, y - 1); }
        Tuple<int, int> BottomRight(int x, int y) { return new Tuple<int, int>(x + 1, y + 1); }

        public int Size = 0;
        public char[,] Board = null;
        [JsonIgnore]
        public List<Tuple<int, int>>[,] Neighbors = null;

        // board will work lower case, no matter the init data
        public GameBoard(char [,] initData)
        {
            if (initData == null) throw new ArgumentException("Must pass a valid non-null board");
            if (initData.GetLength(0) < 1) throw new ArgumentException("Must pass a valid non-empty board");
            if (initData.GetLength(0) != initData.GetLength(1)) throw new ArgumentException("Board must be squared");

            Size = initData.GetLength(0);
            Board = initData;
            Neighbors = new List<Tuple<int, int>>[Size, Size];

            // init neighbors and lower case the board
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Board[i, j] = Char.ToLower(Board[i, j]);
                    Neighbors[i, j] = new List<Tuple<int, int>>();

                    // fill neighbors
                    // example:
                    // (i,j)
                    // (0,0) (0,1) (0,2)
                    // (1,0) (1,1) (1,2)
                    // (2,0) (2,1) (2,2)

                    bool hastop = (i > 0);         // does the cell has another cell on top?
                    bool hasleft = (j > 0);        // does the cell has another cell on the left?
                    bool hasbottom = (i < Size - 1);   // does the cell has another cell on the bottom?
                    bool hasright = (j < Size - 1);    // does the cell has another cell on the right?

                    if (hastop && hasleft) Neighbors[i, j].Add(TopLeft(i, j));
                    if (hastop) Neighbors[i, j].Add(Top(i, j));
                    if (hastop && hasright) Neighbors[i, j].Add(TopRight(i, j));
                    if (hasleft) Neighbors[i, j].Add(Left(i, j));
                    if (hasright) Neighbors[i, j].Add(Right(i, j));
                    if (hasbottom && hasleft) Neighbors[i, j].Add(BottomLeft(i, j));
                    if (hasbottom) Neighbors[i, j].Add(Bottom(i, j));
                    if (hasbottom && hasright) Neighbors[i, j].Add(BottomRight(i, j));
                }
            }
        }

        private void FindNeighbours()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                }
            }
        }

        public string ToString(BoardStringFormat format)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < Size; i++)
            {
                if (format == BoardStringFormat.Formatted)
                {
                    result.AppendFormat("row {0} :", i);
                }
                for (int j = 0; j < Size; j++)
                {
                    if (format == BoardStringFormat.Formatted)
                    {
                        if (j == 0) result.Append("|");
                        result.AppendFormat(" {0} ", Board[i, j]);
                        result.Append("|");
                    }
                    else
                    {
                        result.Append(Board[i, j]);
                    }
                }
                if (format != BoardStringFormat.None)
                {
                    result.AppendLine();
                }
            }

            return result.ToString();
        }

        public override string ToString()
        {
            return ToString(BoardStringFormat.NewLines);
        }
    }
}
