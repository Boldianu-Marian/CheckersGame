using MVP_labs2.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVP_labs2.Services
{
    internal class MoveLogic
    {
        private Pieces[,] matForm;
        private readonly ObservableCollection<Pieces> Ocp;
        public MoveLogic( ObservableCollection<Pieces> ocp) {
            Ocp = ocp;
        }

        public ObservableCollection<Tuple<int,int>> CheckMoves(Pieces piece, bool multi = false)
        {   //builds a list of valid moves for the chosen piece

            ObservableCollection<Tuple<int,int>> moves = [];
            matForm = new Pieces[8, 8];

            foreach (var p in Ocp)
            {
                matForm[p.y, p.x] = p;
            }
            if (piece.type != 1)//type is checked to determine orientation
            {   
                if( piece.y + 1 <= 7 && piece.x + 1 <= 7 && matForm[piece.y+1,piece.x+1]!=null){
                    //checks if the right-side square next to the piece are accupied
                    if (piece.y + 1 <=7 && piece.x + 1 <= 7 && matForm[piece.y + 1, piece.x + 1].type != piece.type%2)
                        //checks if the occupied square are enemy pieces
                        if (piece.y +2 <= 7 && piece.x +2 <= 7 && matForm[piece.y + 2, piece.x + 2] == null)
                            //checks if capture is possible
                            moves.Add(new Tuple<int,int>(piece.y + 2, piece.x + 2));
                            
                }
                else if(!multi && piece.y + 1 <= 7 && piece.x + 1 <= 7)
                {
                    //directly adds empty square into the viable moves list
                    //multi filters non-capture squares in case of chain capture
                    moves.Add(new Tuple<int,int>(piece.y+1,piece.x+1));
                }


                if (piece.y + 1 <= 7 && piece.x - 1>= 0 && matForm[piece.y + 1, piece.x - 1] != null)
                {
                    //exact same checks but for the left-side square
                    if (piece.y + 1 <= 7 && piece.x - 1 >= 0 && matForm[piece.y + 1, piece.x - 1].type != piece.type%2)
                        if (piece.y + 2 <= 7 && piece.x - 2 >= 0 && matForm[piece.y + 2, piece.x - 2] == null)
                            moves.Add(new Tuple<int, int>(piece.y + 2, piece.x - 2));
                            
                }
                else if(!multi && piece.y + 1 <= 7 && piece.x - 1 >= 0)
                {
                    moves.Add(new Tuple<int, int>(piece.y + 1, piece.x - 1));
                }

            }
            if(piece.type != 0 )
            {
                //same checks but inverted for the opposing pieces
                if (piece.y - 1 >= 0 && piece.x + 1 <= 7 && matForm[piece.y - 1, piece.x + 1] != null)
                {
                    if (piece.y - 1 >= 0 && piece.x + 1 <= 7 && matForm[piece.y - 1, piece.x + 1].type != piece.type%2)
                        if (piece.y - 2 >= 0 && piece.x + 2 <= 7 && matForm[piece.y - 2, piece.x + 2] == null)                       
                            moves.Add(new Tuple<int, int>(piece.y - 2, piece.x + 2));
                }
                else  if(!multi && piece.y - 1 >= 0 && piece.x + 1 <= 7)
                {
                    moves.Add(new Tuple<int, int>(piece.y - 1, piece.x + 1));
                }


                if (piece.y-1>=0 && piece.x-1>=0&&matForm[piece.y - 1, piece.x - 1] != null)
                {
                    if (piece.y - 1 >= 0 && piece.x - 1 >= 0 && matForm[piece.y - 1, piece.x - 1].type != piece.type%2)
                        if (piece.y - 2 >= 0 && piece.x - 2 >= 0 && matForm[piece.y - 2, piece.x - 2] == null)
                            moves.Add(new Tuple<int, int>(piece.y - 2, piece.x - 2));
                }
                else if(!multi&&piece.y - 1 >= 0 && piece.x - 1 >= 0)
                {
                    moves.Add(new Tuple<int, int>(piece.y - 1, piece.x - 1));
                }

            }
            return moves;

        }
        public bool MakeMove(Tuple<int, int> piece, Tuple<int,int> pos) {
            Pieces pi;
            pi = Ocp.FirstOrDefault(i => i.x == piece.Item2 && i.y == piece.Item1);

            if  (Math.Abs(pi.x-pos.Item2) == 2)
            {
                Ocp.Remove(Ocp.FirstOrDefault(i => i.x == (piece.Item2 + pos.Item2) / 2 && i.y == (piece.Item1 + pos.Item1) / 2));

                pi.x = pos.Item2;
                pi.y = pos.Item1;
                return true;
               
            }
            pi.x = pos.Item2;
            pi.y = pos.Item1;
         
            return false;
        }
    }
}
