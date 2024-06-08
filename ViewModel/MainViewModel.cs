using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using MVP_labs2.Core;

namespace MVP_labs2.ViewModel
{
    internal class MainViewModel:ObservableObject
    {

        private bool _multiJp;
        public bool MultiJp
        {
            get { return _multiJp; }
            set { _multiJp = value; OnPropertyChanged("MultiJp"); }
        }


        private object _currentPage;
        public object CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; OnPropertyChanged("CurrentPage"); }
        }


        private GameViewModel _game;
        public GameViewModel Game
        {
            get { return _game; }
            set { _game = value; OnPropertyChanged(nameof(Game)); }
        }


        private MenuViewModel _menu;
        public MenuViewModel Menu
        {
            get { return _menu; }
            set { _menu = value; OnPropertyChanged(nameof(Menu)); }
        }


        public MainViewModel()
        {
            Menu = new MenuViewModel(this);
            CurrentPage = Menu;
            MultiJp = false;

        }

    }
}
