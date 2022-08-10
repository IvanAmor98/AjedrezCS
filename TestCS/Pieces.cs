using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Pieces
{

    public class Coordinates
    {
        public static Coordinates operator + (Coordinates a, Coordinates b) => new(a.X + b.X, a.Y + b.Y);
        public static Coordinates operator - (Coordinates a, Coordinates b) => new(a.X - b.X, a.Y - b.Y);
        public static Coordinates operator * (Coordinates a, Coordinates b) => new(a.X * b.X, a.Y * b.Y);
        public static Coordinates operator * (Coordinates a, int b) => new(a.X * b, a.Y * b);

        public readonly static Coordinates UP = new(0, 1);
        public readonly static Coordinates DOWN = new(0, -1);
        public readonly static Coordinates LEFT = new(-1, 0);
        public readonly static Coordinates RIGHT = new(1, 0);

        public readonly static Coordinates UPLEFT = new(-1, 1);
        public readonly static Coordinates UPRIGHT = new(1, 1);
        public readonly static Coordinates DOWNLEFT = new(-1, -1);
        public readonly static Coordinates DOWNRIGHT = new(1, -1);

        public readonly static Coordinates KUPLEFT = new(-1, 2);
        public readonly static Coordinates KUPRIGHT = new(1, 2);
        public readonly static Coordinates KDOWNLEFT = new(-1, -2);
        public readonly static Coordinates KDOWNRIGHT = new(1, -2);
        public readonly static Coordinates KLEFTUP = new(-2, 1);
        public readonly static Coordinates KLEFTDOWN = new(-2, -1);
        public readonly static Coordinates KRIGHTUP = new(2, 1);
        public readonly static Coordinates KRIGHTDOWN = new(2, -1);

        public int X { get; set; }
        public int Y { get; set; }
        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public enum Type
    {
        King,
        Queen,
        Knight,
        Bishop,
        Rook,
        Pawn
    }

    public enum Color
    {
        White,
        Black
    }

    abstract class Piece
    {
        public int[][] Grid { get; set; }
        public Type Type { get; set; }
        public Color Color { get; set; } //0 white, 1 black
        public Coordinates Coords { get; set; } = new(0, 0);
        public bool HorizontalMove { get; set; }
        public bool DiagonalMove { get; set; }
        public bool KnightMove { get; set; }
        public int Range { get; set; }
        public bool IsFirtMove { get; set; } = true;

        public Piece(int[][] grid, Coordinates coords, Color color)
        {
            this.Grid = grid;
            this.Coords = coords;
            this.Color = color;
        }

        public List<Coordinates> GetHorizontalMoves()
        {
            List<Coordinates> result = new();
            if (!HorizontalMove) return result;

            CheckStep(result, Coordinates.UP);
            CheckStep(result, Coordinates.DOWN);
            CheckStep(result, Coordinates.RIGHT);
            CheckStep(result, Coordinates.LEFT);

            return result;
        }

        public  List<Coordinates> GetDiagonalMoves()
        {
            List<Coordinates> result = new();
            if (!DiagonalMove) return result;

            CheckStep(result, Coordinates.UPLEFT);
            CheckStep(result, Coordinates.UPRIGHT);
            CheckStep(result, Coordinates.DOWNLEFT);
            CheckStep(result, Coordinates.DOWNRIGHT);

            return result;
        }

        public List<Coordinates> GetKnightMoves()
        {
            List<Coordinates> result = new();
            if (!DiagonalMove) return result;

            CheckStep(result, Coordinates.KUPLEFT);
            CheckStep(result, Coordinates.KUPRIGHT);
            CheckStep(result, Coordinates.KDOWNLEFT);
            CheckStep(result, Coordinates.KDOWNRIGHT);
            CheckStep(result, Coordinates.KLEFTUP);
            CheckStep(result, Coordinates.KLEFTDOWN);
            CheckStep(result, Coordinates.KRIGHTUP);
            CheckStep(result, Coordinates.KRIGHTDOWN);

            return result;
        }

        public void CheckStep(List<Coordinates> result, Coordinates step)
        {
            Coordinates move = this.Coords;
            bool isObstructed = false;
            for (int i = 0; i < this.Range || !isObstructed; i++)
            {
                move += step * i;
                if (!IsObstructed(move))
                {
                    result.Add(move);
                } else
                {
                    isObstructed = true;
                }
            }
        }

        public bool IsObstructed(Coordinates space)
        {
            //TODO check position
            return false;
        }

    }

    class King : Piece
    {
        public King(int[][] grid, Coordinates coords, Color color) : base(grid, coords, color)
        {
            this.Type = Type.King;
            this.HorizontalMove = true;
            this.DiagonalMove = true;
            this.KnightMove = false;
            this.Range = 1;
        }
    }

    class Queen : Piece
    {
        public Queen(int[][] grid, Coordinates coords, Color color) : base(grid, coords, color)
        {
            this.Type = Type.Queen;
            this.HorizontalMove = true;
            this.DiagonalMove = true;
            this.KnightMove = false;
            this.Range = 7;
        }
    }

    class Knight : Piece
    {
        public Knight(int[][] grid, Coordinates coords, Color color) : base(grid, coords, color)
        {
            this.Type = Type.Knight;
            this.HorizontalMove = false;
            this.DiagonalMove = false;
            this.KnightMove = true;
            this.Range = 1;
        }
    }

    class Bishop : Piece
    {
        public Bishop(int[][] grid, Coordinates coords, Color color) : base(grid, coords, color)
        {
            this.Type = Type.Bishop;
            this.HorizontalMove = false;
            this.DiagonalMove = true;
            this.KnightMove = false;
            this.Range = 7;
        }
    }

    class Rook : Piece
    {
        public Rook(int[][] grid, Coordinates coords, Color color) : base(grid, coords, color)
        {
            this.Type = Type.Rook;
            this.HorizontalMove = true;
            this.DiagonalMove = false;
            this.KnightMove = false;
            this.Range = 7;
        }
    }

    class Pawn : Piece
    {
        public Pawn(int[][] grid, Coordinates coords, Color color) : base(grid, coords, color)
        {
            this.Type = Type.Pawn;
            this.HorizontalMove = true;
            this.DiagonalMove = true;
            this.KnightMove = false;
            this.Range = 1;
        }
    }
}
