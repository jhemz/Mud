using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CP
{
    public class Object : UserControl
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool CanMove { get; set; }
    }
}
