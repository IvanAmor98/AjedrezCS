using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Pieces;
using BoardNS;
using Color = Pieces.Color;
using Type = Pieces.Type;
using System.Diagnostics;

namespace TestCS
{

    public class TableButton : Button
    {
        public Coordinates Coords { get; }
        public bool IsMoving { get; set; } = false;
        public bool CanMoveHere { get; set; } = false;
        private Brush? OriginalBackGround { get; set; }

        public TableButton(Coordinates coords)
        {
            Coords = coords;
        }

        public void SetPossibleMove()
        {
            CanMoveHere = true;
            OriginalBackGround = Background;
            Background = new SolidColorBrush(Colors.Yellow);
        }

        public void RemovePossibleMove()
        {
            CanMoveHere = false;
            Background = OriginalBackGround;
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool Moving { get; set; } = false;
        public TableButton? PieceMoving { get; set; }
        public Color Turn { get; set; } = Color.White;
        public List<TableButton> ButtonList { get; set; } = new List<TableButton>();

        public MainWindow()
        {
            InitializeComponent();
            Board.InitializeGrid();
            Board.CreatePieces();
            FillBoard();
        }

        public void FillBoard()
        {
            for (int i = 0; i < 8; i++) {
                for (int j = 0; j < 8; j++)
                {
                    Coordinates location = new(i, j);
                    StackPanel stackPnl = new()
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness(10)
                    };
                    Piece? piece = Board.PieceList.Find(piece => piece.Coords == location);
                    if (piece != null) stackPnl.Children.Add(piece.Image);

                    TableButton button = new(location)
                    {
                        Background = new SolidColorBrush(j % 2 == 0 ? (i % 2 == 0 ? Colors.DarkGray : Colors.White) : (i % 2 == 0 ? Colors.White : Colors.DarkGray)),
                        Content = stackPnl
                    };
                    button.Click += OnButtonClick;
                    ButtonList.Add(button);
                    GameTable.Children.Add(button);
                    Grid.SetColumn(button, button.Coords.X);
                    Grid.SetRow(button, button.Coords.Y);

                }
            }
        }

        void OnButtonClick(object sender, RoutedEventArgs e)
        {
            TableButton button = (TableButton)sender;

            
            if (button.IsMoving)
            {
                SetNotMoving(button);
                RemovePosibleMoves();
                return;
            }

            
            if (button.CanMoveHere)
            {
                MovePiece(button);
                RemovePosibleMoves();
                SetNotMoving();
                return;
            }
            
            SetNotMoving();
            RemovePosibleMoves();

            Piece? piece = Board.PieceList.Find(piece => piece.Coords == button.Coords);
            if (piece != null)
            {
                if (piece.Color != Turn) return;
                button.IsMoving = true;
                CheckForCheck(piece).ForEach(move => ButtonList.FindAll(button => button.Coords == move).ForEach(square => square.SetPossibleMove()));
            }
        }

        private List<Coordinates> CheckForCheck(Piece piece)
        {
            List<Coordinates> validMoveList = new();
            piece.GetValidMoves().ForEach(move =>
            {
                int[,] tempGrid = (int[,])Board.PieceGrid.Clone();
                Board.PieceGrid[piece.Coords.X, piece.Coords.Y] = 0;
                Board.PieceGrid[move.X, move.Y] = 1;

                bool isCheck = false;
                List<Piece> enemyPieces = Board.PieceList.FindAll(enemyPiece => enemyPiece.Color != piece.Color);
                enemyPieces.ForEach(enemyPiece => enemyPiece.GetValidMoves().ForEach(enemyMove =>
                    {
                        Piece? piece = Board.PieceList.Find(king => king.Type == Type.King && king.Color != enemyPiece.Color);
                        isCheck = piece != null && piece.Coords == enemyMove || isCheck;
                    }
                ));
                Board.PieceGrid = (int[,])tempGrid.Clone();
                if (!isCheck) validMoveList.Add(move);
            });
            return validMoveList;
        }

        private void RemovePosibleMoves()
        {
            ButtonList.FindAll(sqaure => sqaure.CanMoveHere).ForEach(square => square.RemovePossibleMove());
        }

        private void SetNotMoving()
        {
            TableButton? button = ButtonList.Find(button => button.IsMoving);
            if (button != null) button.IsMoving = false;
        }

        private void SetNotMoving(TableButton button)
        {
            button.IsMoving = false;
        }

        private void MovePiece(TableButton target)
        {
            TableButton? origin = ButtonList.Find(button => button.IsMoving);
            if (origin == null) throw new NullReferenceException();

            Piece? piece = Board.PieceList.Find(piece => piece.Coords == origin.Coords);
            if (piece == null) throw new NullReferenceException();

            Piece? eatenPiece = Board.PieceList.Find(piece => piece.Coords == target.Coords);
            if (eatenPiece != null)
            {
                ((StackPanel)target.Content).Children.RemoveAt(0);
                Board.PieceList.Remove(eatenPiece);
            }

            ((StackPanel)origin.Content).Children.RemoveAt(0);
            ((StackPanel)target.Content).Children.Add(piece.Image);

            piece.Coords = target.Coords;
            piece.IsFirstMove = false;
            Board.PieceGrid[origin.Coords.X, origin.Coords.Y] = 0;
            Board.PieceGrid[target.Coords.X, target.Coords.Y] = 1;

            Turn = Turn == Color.White ? Color.Black : Color.White;
            CheckPawnPromotion(piece, target);
        }

        private void CheckPawnPromotion(Piece piece, TableButton target)
        {
            if (!(piece.Type == Type.Pawn && (target.Coords.Y == 7 || target.Coords.Y == 0))) return;
            Board.PieceList.Remove(piece);
            switch(new SelectionWindow().ShowCustomDialog())
            {
                case Type.Queen:
                    piece = new Queen(piece.Coords, piece.Color);
                    break;
                case Type.Bishop:
                    piece = new Bishop(piece.Coords, piece.Color);
                    break;
                case Type.Knight:
                    piece = new Knight(piece.Coords, piece.Color);
                    break;
                case Type.Rook:
                    piece = new Rook(piece.Coords, piece.Color);
                    break;
            }
            Board.PieceList.Add(piece);

            ((StackPanel)target.Content).Children.RemoveAt(0);
            ((StackPanel)target.Content).Children.Add(piece.Image);
        }
    }
}