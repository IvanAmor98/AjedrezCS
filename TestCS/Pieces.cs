using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using TestCS;

namespace Pieces
{

    public class Coordinates
    {
        public static Coordinates operator + (Coordinates a, Coordinates b) => new(a.X + b.X, a.Y + b.Y);
        public static Coordinates operator - (Coordinates a, Coordinates b) => new(a.X - b.X, a.Y - b.Y);
        public static Coordinates operator * (Coordinates a, Coordinates b) => new(a.X * b.X, a.Y * b.Y);
        public static Coordinates operator * (Coordinates a, int b) => new(a.X * b, a.Y * b);
        public static bool operator == (Coordinates a, Coordinates b) => a.X == b.X && a.Y == b.Y;
        public static bool operator != (Coordinates a, Coordinates b) => a.X != b.X || a.Y != b.Y;

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

        public bool IsValid()
        {
            return X > -1 && X < 8 && Y > -1 && Y < 8;  
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
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

    public abstract class Piece
    {
        public Type Type { get; set; }
        public Color Color { get; set; }
        public Coordinates Coords { get; set; } = new(0, 0);
        public bool HorizontalMove { get; set; }
        public bool DiagonalMove { get; set; }
        public bool KnightMove { get; set; }
        public int Range { get; set; }
        public bool IsFirtMove { get; set; } = true;

        public Piece(Coordinates coords, Color color)
        {
            this.Coords = coords;
            this.Color = color;
        }

        public List<Coordinates> GetValidMoves()
        {
            List<Coordinates> result = new();
            result.AddRange(GetHorizontalMoves());
            result.AddRange(GetDiagonalMoves());
            result.AddRange(GetKnightMoves());
            return result;
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
            if (!KnightMove) return result;

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
            bool isObstructed = false;
            for (int i = 1; i <= this.Range && !isObstructed; i++)
            {
                Coordinates move = this.Coords + (step * i);
                if (!move.IsValid()) return;
                if (IsObstructed(move))
                {
                    isObstructed = true;
                    return;
                }
                 result.Add(move);
            }
        }

        public bool IsObstructed(Coordinates space)
        {
            if (BoardNS.Board.PieceGrid[space.X, space.Y] == 0) return false;
            if (BoardNS.Board.PieceList.Find(piece => piece.Coords == space)?.Color != Color) return false;
            return true;
        }

    }

    class King : Piece
    {
        public King(Coordinates coords, Color color) : base(coords, color)
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
        public Queen(Coordinates coords, Color color) : base(coords, color)
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
        public Knight(Coordinates coords, Color color) : base(coords, color)
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
        public Bishop(Coordinates coords, Color color) : base(coords, color)
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
        public Rook(Coordinates coords, Color color) : base(coords, color)
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
        public Pawn(Coordinates coords, Color color) : base(coords, color)
        {
            this.Type = Type.Pawn;
            this.HorizontalMove = true;
            this.DiagonalMove = true;
            this.KnightMove = false;
            this.Range = 1;
        }
    }
}
