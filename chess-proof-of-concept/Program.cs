using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess_proof_of_concept
{
    //TODO connect square to piece
    //TODO should squares know their coordinates?
    class Square
    {
        bool has_piece;
        Piece piece;
        public Square()
        {
            has_piece = false;
        }
        public Square(Piece piece)
        {
            this.piece = piece;
            has_piece = true;
        }
        public Piece getPiece()
        {
            if (has_piece)
            {
                return piece ;
            }
            else
            {
                return null;
            }
        }
        public void changePiece(Piece piece)
        {
            has_piece = true;
            this.piece = piece;
        }
        public void removePiece()
        {
            has_piece = false;
            piece = null;
        }
    }
    //TODO should pieces know their coordinates?
    abstract class Piece
    {
        protected string symbol;
        protected string color;
        public string getSymbol()
        {
            return symbol;
        }
    }
    class Pawn : Piece
    {
        bool first_move = true;
        public Pawn(string color)
        {
            this.color = color;
            symbol = color + "P";
        }
    }
    class Chessboard
    {
        private Square[,] board = new Square[8,8];
        //TODO on start there should be all of the pieces present, both white and black
        //TODO the x and y coords are swapped i think
        public Chessboard()
        {
            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    Square empty_square = new Square();
                    board[i,j] = empty_square;
                }
            }
            for (int i = 0; i < 8; i++)
            {
                Piece p = new Pawn("b");
                board[i,1].changePiece(p);
            }
            for (int i = 0; i < 8; i++)
            {
                Piece p = new Pawn("w");
                board[i,6].changePiece(p);
            }

        }
        public void drawChessboard()
        {
            int line_len = 0;
            int k = 7;
            Console.Write("8| ");
            for(int i = 0;i < 8;i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    string piece = "  ";
                    if (board[j,i].getPiece() != null)
                    {
                        piece = board[j,i].getPiece().getSymbol();
                    }
                    line_len += piece.Length + 3;
                    Console.Write(piece + " | ");
                }
                string line = "\n";
                for (int j = 0; j <= line_len; j++)
                {
                    line = line + "-";
                }
                Console.WriteLine(line);
                if(k < 1) break;
                Console.Write("{0}| ", k);
                k--;
                line_len = 0;
            }

            Console.Write(" | ");
            string letters = "abcdefgh";
            for (int j = 0; j < 8; j++)
            {
                char letter = letters[j];
                line_len += 5;
                Console.Write(" {0} | ",letter);
            }
            string line1 = "\n";
            for (int j = 0; j <= line_len; j++)
            {
                line1 = line1 + "-";
            }
            Console.WriteLine();
            Console.WriteLine();
        }
        Piece getPiece(int x, int y)
        {
            return board[x, y].getPiece();
        }
        //TODO parse chess notation (string), like Pe4 ect.
        public void movePiece(int x, int y, int target_x, int target_y)
        {
            Piece p = getPiece(x,y);
            //if (getPiece(target_x,target_y) == null)
            {
                board[x, y].removePiece();
                board[target_x, target_y].changePiece(p);
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Chessboard chessboard1 = new Chessboard();
            
            chessboard1.drawChessboard();
            chessboard1.movePiece(6, 2, 4, 2);
            chessboard1.drawChessboard();
        }
    }
}
