using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Pieces;

namespace BoardNS
{
	public class Board
	{
        public static List<Piece> PieceList { get; } = new List<Piece>();
        public static int[,] PieceGrid { get; } = new int[8, 8];

        public Board()
		{
            InitializeGrid();
            CreatePieces();
		}

        public void InitializeGrid()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    PieceGrid[i, j] = 0;
                }
            }
            for (int i = 0; i < 8; i++)
            {
                PieceGrid[i, 0] = 1;
                PieceGrid[i, 1] = 1;
                PieceGrid[i, 6] = 1;
                PieceGrid[i, 7] = 1;
            }
        }

        public void CreatePieces()
        {
            PieceList.Add(new King(new(3, 0), Color.White));
            PieceList.Add(new Queen(new(4, 0), Color.White));
            PieceList.Add(new Bishop(new(2, 0), Color.White));
            PieceList.Add(new Bishop(new(5, 0), Color.White));
            PieceList.Add(new Knight(new(1, 0), Color.White));
            PieceList.Add(new Knight(new(6, 0), Color.White));
            PieceList.Add(new Rook(new(0, 0), Color.White));
            PieceList.Add(new Rook(new(7, 0), Color.White));

            PieceList.Add(new King(new(3, 7), Color.Black));
            PieceList.Add(new Queen(new(4, 7), Color.Black));
            PieceList.Add(new Bishop(new(2, 7), Color.Black));
            PieceList.Add(new Bishop(new(5, 7), Color.Black));
            PieceList.Add(new Knight(new(1, 7), Color.Black));
            PieceList.Add(new Knight(new(6, 7), Color.Black));
            PieceList.Add(new Rook(new(0, 7), Color.Black));
            PieceList.Add(new Rook(new(7, 7), Color.Black));

            for (int i = 0; i < 8; i++)
            {
                PieceList.Add(new Pawn(new(i, 1), Color.White));
                PieceList.Add(new Pawn(new(i, 6), Color.Black));
            }
        }
    }
}

