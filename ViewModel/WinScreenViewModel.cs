using MVP_labs2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVP_labs2.ViewModel;
using MVP_labs2.View;

namespace MVP_labs2.ViewModel
{
    internal class WinScreenViewModel:ObservableObject
    {
        private MainViewModel MainVM { get; set; }

        private string _winner;
        public string Winner
        {
            get { return _winner; }
            set { _winner = value; OnPropertyChanged(nameof(Winner)); } 
        }

        public RelayCommand MenuCommand { get; set; }
        public RelayCommand PlayCommand { get; set; }

        public WinScreenViewModel(string winner, MainViewModel mvm) {
            MainVM = mvm;
            Winner = winner;

            MenuCommand = new RelayCommand(o =>
            {
                MainVM.CurrentPage = new MenuViewModel(mvm);
            });

            PlayCommand = new RelayCommand(o => {
                MainVM.CurrentPage = new GameViewModel(mvm);
            });
        }
    }
}
