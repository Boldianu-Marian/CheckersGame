using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MVP_labs2.Core;
using MVP_labs2.Model;
using MVP_labs2.Services;

namespace MVP_labs2.ViewModel
{
    internal class GameViewModel : ObservableObject
    {

        private MoveLogic ML;

        private BitmapImage CheckRed;
        private BitmapImage CheckWhite;


        private int redP;
        public int RedP
        {
            get { return redP; }
            set { redP = value; OnPropertyChanged("RedP"); }
        }


        private int whiteP;
        public int WhiteP
        {
            get { return whiteP; }
            set { whiteP = value; OnPropertyChanged(nameof(WhiteP)); }
        }


        private ImageSource _titleSwitch;
        public ImageSource TitleSwitch
        {
            get { return _titleSwitch; }
            set
            {
                _titleSwitch = value;
                OnPropertyChanged(nameof(TitleSwitch));
            }
        }


        public BitmapImage CheckRedK;
        public BitmapImage CheckWhiteK;

        private bool _round;
        public bool Round
        {
            get { return _round;}
            set
            {
                _round = value;
                OnPropertyChanged(nameof(Round));    
            }
        }


        private Tuple<int,int> _currentPiece;
        private Tuple<int,int> CurrentPiece
        {
            get { return _currentPiece; }
            set { _currentPiece = value; OnPropertyChanged(nameof(CurrentPiece)); }
        }

        private string _txt;
        public string txt { get { return _txt; } set { _txt = value;OnPropertyChanged("txt"); } }


        private ObservableCollection<Pieces> _pieces;
        public ObservableCollection<Pieces> MyPieces
        {
            get { return _pieces; }
            set { _pieces = value; OnPropertyChanged("MyPieces"); }
        }


        private ObservableCollection<Tuple<int, int>> _highlights;
        public ObservableCollection<Tuple<int, int>> Highlights
        {
            get { return _highlights; }
            set { _highlights = value; OnPropertyChanged("Highlights"); }
        }


        private ICommand _hoverCommand;
        public ICommand HoverCommand
        {
            get
            {
                if (_hoverCommand == null)
                {
                    _hoverCommand = new RelayCommand(
                        param => this.hoverCommandExecute(param),
                        param => this.hoverCommandCanExecute(param)
                    );
                }
                return _hoverCommand;
            }
        }

        private bool hoverCommandCanExecute(object param)
        {
            
            return MyPieces.Any();
        }

        private bool _jumpInProgress = false;
        private void hoverCommandExecute(object param)
        {
            
            var p = param as Pieces;
            if (p != null && _jumpInProgress==false)
            {
                if ((p.type % 2 == 1 && Round == true) || (p.type % 2 == 0 && Round == false))
                {
                    ML = new MoveLogic(MyPieces);//gives MoveLogic the current context of the game
                    Highlights = ML.CheckMoves(p);
                    CurrentPiece = new Tuple<int, int>(p.y, p.x);
                }
            }
        }

        private ICommand _moveCommand;
        public ICommand MoveCommand
        {
            get
            {
                if (_moveCommand == null)
                {
                    _moveCommand = new RelayCommand(
                        param => this.moveCommandExecute(param),
                        param => this.moveCommandCanExecute(param)
                    );
                }
                return _moveCommand;
            }
        }

        private bool moveCommandCanExecute(object param)
        {
            return MyPieces.Any();
        }
        private void moveCommandExecute(object param)
        {

            var newPosition = param as Tuple<int, int>;

            bool movemade;

            movemade = ML.MakeMove(CurrentPiece, newPosition);//returns wether or not a piece was captured

            Pieces piece = MyPieces.FirstOrDefault(o => o.y == newPosition.Item1 && o.x == newPosition.Item2);

            if (newPosition.Item1 == 0 && piece.type == 1)
            {
                piece.type += 2;
                piece.Icon = CheckRedK;
            }
            if (newPosition.Item1 == 7 && piece.type == 0)
            {
                piece.type += 2;
                piece.Icon = CheckWhiteK;
            }

            WhiteP = MyPieces.Count(o => o.type == 0);
            RedP = MyPieces.Count(o => o.type == 1);
        
            if (WhiteP == 0)
            {
                MainVM.CurrentPage = new WinScreenViewModel("The Red Player Won", MainVM);
            }
            if (RedP == 0)
            {
                MainVM.CurrentPage = new WinScreenViewModel("The White Player Won", MainVM);
            }
            ML = new MoveLogic(MyPieces);
            if (MainVM.MultiJp == false || movemade == false || ML.CheckMoves(MyPieces.FirstOrDefault(o => o.y == newPosition.Item1 && o.x == newPosition.Item2), true).Count < 1)
            {
                //in the case that multi jump is allowed the round is only allowed to change if
                //there has been no capture or there is no more captures that can be made from the current position of the last moved piece
                Round = !Round;
                Highlights.Clear();
                _jumpInProgress = false;
            }
            else
            {
                ML = new MoveLogic(MyPieces);
                Highlights = ML.CheckMoves(MyPieces.FirstOrDefault(o => o.y == newPosition.Item1 && o.x == newPosition.Item2), true);
                _jumpInProgress = true;
                CurrentPiece = new Tuple<int, int>(newPosition.Item1, newPosition.Item2);
            }

            TitleSwitch = Round ? CheckRed : CheckWhite;//changes the round marker
        }

        private MainViewModel MainVM{ get; set; }


        public RelayCommand BackCommand { get; set; }
        public GameViewModel( MainViewModel mvm)
        {
            MainVM = mvm;   
            WhiteP = 12;
            RedP = 12;
            Round = true;
            MyPieces = new ObservableCollection<Pieces>();

            CheckWhiteK = new BitmapImage();
            CheckWhiteK.BeginInit();
            CheckWhiteK.UriSource = new Uri("/image/checkers_king_white.png", UriKind.Relative);
            CheckWhiteK.EndInit();

            CheckRedK = new BitmapImage();
            CheckRedK.BeginInit();
            CheckRedK.UriSource = new Uri("/image/checkers_king.png", UriKind.Relative);
            CheckRedK.EndInit();


            CheckWhite = new BitmapImage();
            CheckWhite.BeginInit();
            CheckWhite.UriSource = new Uri("/image/checkers_title_white.png", UriKind.Relative);
            CheckWhite.EndInit();

            CheckRed = new BitmapImage();
            CheckRed.BeginInit();
            CheckRed.UriSource = new Uri("/image/checkers_title.png", UriKind.Relative);
            CheckRed.EndInit();


            Pieces piece;
            for (int i = 0; i < 12; i++)
            {   //create the pieces
                piece = new Pieces
                {
                    x = (1 - i * 2 / 8 % 2 + i * 2) % 8, // 1 is then you subtract the offsent in case of odd rows then add the position on the x axis
                    y = i * 2 / 8,
                    type = 0,
                    Icon = CheckWhite

                };
                MyPieces.Add(piece);
                piece = new Pieces
                {
                    x = (62 - i * 2 / 8 % 2 + i * 2) % 8,
                    y = (63 - i * 2) / 8,
                    type = 1,
                    Icon = CheckRed
                };
                MyPieces.Add(piece);
            }
            TitleSwitch = CheckRed;

            BackCommand = new RelayCommand(o =>
            {
                mvm.CurrentPage = mvm.Menu ;
            });
        }

        
    }
}
