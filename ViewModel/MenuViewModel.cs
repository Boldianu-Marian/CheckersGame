using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVP_labs2.Core;

namespace MVP_labs2.ViewModel
{

    internal class MenuViewModel : ObservableObject
    {
        public RelayCommand PlayCommand { get; set; }

        public RelayCommand JumpCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }

        private string _jumpText = "Enable Multi Jump";
        public string JumpText
        { 
            get { return _jumpText; }
            set { _jumpText = value; OnPropertyChanged("JumpText"); }
        }
       
        public MenuViewModel( MainViewModel parentVM) 
        {      
            
            PlayCommand = new RelayCommand(o=>{

                parentVM.CurrentPage = new GameViewModel(parentVM);
            });

            JumpCommand = new RelayCommand(o => {
                parentVM.MultiJp = !parentVM.MultiJp;
                JumpText = parentVM.MultiJp ?  "Disable Multi Jump": "Enable Multi Jump";
                });
            
        }  
    }
}
