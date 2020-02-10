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

namespace CP
{
    /// <summary>
    /// Interaction logic for Block.xaml
    /// </summary>
    public partial class Block : Object
    {
        public Block(bool canMove = true)
        {
            InitializeComponent();
            CanMove = canMove;

            if (canMove)
            {
                image.Source = new BitmapImage(new Uri(@"./Images/block.png", UriKind.Relative));
            }
            else
            { 
                 image.Source = new BitmapImage(new Uri(@"./Images/borderblock.png", UriKind.Relative));
            }
        }


    }
}
