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
using System.Windows.Shapes;

namespace TestCS
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private Pieces.Type SelectedType { get; set; }
        private bool CanClose { get; set; } = false;

        public Window1()
        {
            InitializeComponent();
            
        }

        public Pieces.Type ShowCustomDialog()
        {
            this.ShowDialog();
            return SelectedType;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch(Grid.GetColumn((Button)sender))
            {
                case 0:
                    this.SelectedType = Pieces.Type.Queen;
                    break;
                case 1:
                    this.SelectedType = Pieces.Type.Bishop;
                    break;
                case 2:
                    this.SelectedType = Pieces.Type.Knight;
                    break;
                case 3:
                    this.SelectedType = Pieces.Type.Rook;
                    break;
            }
            this.CanClose = true;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !this.CanClose;
        }
    }
}
