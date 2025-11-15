using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess_proof_of_concept
{
    public struct PieceAndCoords
    {
        public int X { get; }
        public int Y { get; }
        public Piece Piece { get; }
        public PieceAndCoords(int x, int y, Piece piece)
        {
            X = x;
            Y = y;
            this.Piece = piece;
        }
    }
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
    public abstract class Piece
    {
        protected char symbol;
        protected char color;
        protected string displaySymbol;
        public char getSymbol()
        {
            return symbol;
        }
        public char getColor()
        {
            return color;
        }
        public string getDisplaySymbol()
        {
            string result;
            if (displaySymbol == null)
            {
                result = color.ToString() + symbol;
                displaySymbol = result;
                
            }
            else
            {
                result = displaySymbol;
            }
            return result;
        }
    }
    class Pawn : Piece
    {
        bool first_move = true;
        public Pawn(char color)
        {
            this.color = color;
            symbol = 'P';
        }
        public bool hasMoved()
        {
            return first_move;
        }
    }
    class Chessboard
    {
        private Square[,] board = new Square[8,8];
        //TODO on start there should be all of the pieces present, both white and black
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
                Piece p = new Pawn('b');
                board[i,1].changePiece(p);
            }
            for (int i = 0; i < 8; i++)
            {
                Piece p = new Pawn('w');
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
                        piece = board[j,i].getPiece().getDisplaySymbol();
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
        
        PieceAndCoords findPiece(string notation, char current_color)
        {
            char symbol = notation[0];
            int column = notation[1] - 97;
            Piece piece = null;
            int found_row = -1;
            //TODO add cases for other than pawn
            switch(symbol)
            {
                case 'P':
                    for (int i = 0; i < 8; i++)
                    {
                        if (getPiece(column, i) != null)
                        {
                            if (getPiece(column, i).getSymbol() == symbol && getPiece(column, i).getColor() == current_color)
                            {
                                piece = board[column, i].getPiece();
                                found_row = i;
                                break;
                            }
                        }
                    }
                    break;
                default:
                    Console.WriteLine("Wrong symbol input!");break;
            }

            if (found_row != -1)
            {
                PieceAndCoords result = new PieceAndCoords(column, found_row, piece);
                return result;
            }
            else
            {
                throw (new Exception("FindPiece() exception!"));
            }
        }
        //TODO parse chess notation (string), like Pe4 ect.
        public void movePiece(int x, int y, int target_x, int target_y)
        {
            Piece p = getPiece(x,y);
            if (getPiece(target_x,target_y) == null)
            {
                board[x, y].removePiece();
                board[target_x, target_y].changePiece(p);
            }
        }
        public void movePiece(string notation, char current_color)
        {
            if (notation.Length == 3)
            {
                char symbol = notation[0];
                int column = notation[1] - 97;
                int row = notation[2] - 48;

                PieceAndCoords piece = findPiece(notation, current_color);

                //moving pawn
                if (piece.Piece != null)
                {
                    movePiece(piece.X,piece.Y,column,row);
                }
                else
                {
                    Console.WriteLine("Piece not found!");
                }
            }
            else
            {
                Console.WriteLine("Wrong notation syntax!");
            }

            /*Console.WriteLine(symbol);
            Console.WriteLine(row);
            Console.WriteLine(column);
            Console.WriteLine();*/
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Chessboard chessboard1 = new Chessboard();
            string read = "";
            chessboard1.drawChessboard();
            while (read != "end")
            {
                Console.WriteLine("Podaj komendę: ");
                read = Console.ReadLine();
                Console.WriteLine();
                chessboard1.movePiece(read, 'w');
                chessboard1.drawChessboard(); 
            }
        }
    }
}
