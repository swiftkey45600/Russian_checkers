using System.Drawing;

namespace Program
{
    class Program()
    {
        static public void DrawPixel(int x, int y, ConsoleColor color)
        {
            Console.SetCursorPosition(x, y);
            Console.BackgroundColor = color;
            Console.Write("   ");
            Console.ResetColor();
        }

        static bool IsBorder(int x, int y, int width, int height)
        {
            if (x == 0 || x == width - 1)
            {
                return true;
            }

            if (y == 0 || y == height - 1)
            {
                return true;
            }


            return false;
        }

        static void Main()
        {

            Console.CursorVisible = false;
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            Console.Clear();

            ChessBoard chessBoard = new ChessBoard();
            chessBoard.InitChessBoard();
            chessBoard.DrawChessBoard(width, height);
            chessBoard.InitFigures();
            chessBoard.DrawFigures();
            
        }
    }
}