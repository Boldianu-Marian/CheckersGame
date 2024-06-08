using MVP_labs2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MVP_labs2.Model
{
    internal class Pieces: ObservableObject
    {
        private int _x;
        public int x { get { return _x; } set { _x = value; OnPropertyChanged("x"); } }
        private int _y;
        public int y { get { return _y; } set { _y = value;OnPropertyChanged("y"); } }
        public int type {  get; set; }

        private ImageSource _icon;
        public ImageSource Icon { get { return _icon; } set { _icon = value; OnPropertyChanged("Icon"); } }
        
    }
}
