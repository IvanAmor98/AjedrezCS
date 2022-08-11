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
                    Coordinates location = new(i, 7 - j);
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
                if (piece.Color != this.Turn) return;
                button.IsMoving = true;
                piece.GetValidMoves().ForEach(move => ButtonList.FindAll(button => button.Coords == move).ForEach(square => square.SetPossibleMove()));
            }
        }

        void RemovePosibleMoves()
        {
            ButtonList.FindAll(sqaure => sqaure.CanMoveHere).ForEach(square => square.RemovePossibleMove());
        }

        void SetNotMoving()
        {
            TableButton? button = ButtonList.Find(button => button.IsMoving);
            if (button != null) button.IsMoving = false;
        }

        void SetNotMoving(TableButton button)
        {
            button.IsMoving = false;
        }

        void MovePiece(TableButton target)
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
            CheckMate();
        }

        void CheckPawnPromotion(Piece piece, TableButton target)
        {
            if (!(piece.Type == Pieces.Type.Pawn && (target.Coords.Y == 7 || target.Coords.Y == 0))) return;
            Board.PieceList.Remove(piece);
            switch(new Window1().ShowCustomDialog())
            {
                case Pieces.Type.Queen:
                    piece = new Queen(piece.Coords, piece.Color);
                    break;
                case Pieces.Type.Bishop:
                    piece = new Bishop(piece.Coords, piece.Color);
                    break;
                case Pieces.Type.Knight:
                    piece = new Knight(piece.Coords, piece.Color);
                    break;
                case Pieces.Type.Rook:
                    piece = new Rook(piece.Coords, piece.Color);
                    break;
            }
            Board.PieceList.Add(piece);

            ((StackPanel)target.Content).Children.RemoveAt(0);
            ((StackPanel)target.Content).Children.Add(piece.Image);
        }

        void CheckMate()
        {

        }

        void CheckCheckMate()
        {

        }
    }
}