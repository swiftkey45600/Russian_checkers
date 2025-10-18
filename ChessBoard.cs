using System.Diagnostics;
using System.IO.Compression;

namespace Program;

public class ChessBoard
{
    public int size = 8;
    public List<List<string>> board = new List<List<string>>();
    public List<Figure> figures = new List<Figure>();

    public void InitChessBoard()
    {
        for (int y = 0; y < size; y++)
        {
            List<string> row = new List<string>();
            for (int x = 0; x < size; x++)
            {
                if ((x + y) % 2 == 0)
                    row.Add("white");
                else
                    row.Add("brown");
            }
            board.Add(row);
        }
    }

    public void InitFigures()
    {
        for (int y = 0; y < 8; y++)
        {
            if (y % 2 == 0)
            {
                for (int x = 1; x < 8; x += 2)
                {
                    if (y < 3)
                    {
                        Figure whiteFigure = new Figure(x, y, ConsoleColor.White, false);                        
                        figures.Add(whiteFigure);
                    }
                    else if (y > 4)
                    {
                        Figure blackFigure = new Figure(x, y, ConsoleColor.Black, false);                        
                        figures.Add(blackFigure);
                    }
                }
            } else
            {
                for (int x = 0; x < 8; x += 2)
                {
                    if (y < 3)
                    {
                        Figure whiteFigure = new Figure(x, y, ConsoleColor.White, false);                        
                        figures.Add(whiteFigure);
                    }
                    else if (y > 4)
                    {
                        Figure blackFigure = new Figure(x, y, ConsoleColor.Black, false);                        
                        figures.Add(blackFigure);
                    }
                }
            }
        }
    }

    public void DrawChessBoard(int width, int height)
    {
        int startX = 0;
        int startY = 0;
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size * 3; x += 3)
            {
                if (board[y][x / 3] == "white")
                {
                    Program.DrawPixel(x, y, ConsoleColor.White);
                    startX = x;
                    startY = y;
                }
                else
                {
                    Program.DrawPixel(x, y, ConsoleColor.DarkYellow);
                    startX = x;
                    startY = y;
                }
            }
        }
        for (int dy = 0; dy < size; dy++)
        {
            Console.SetCursorPosition(startX + 3, dy);
            Console.Write("  ");
            Console.Write((8 - dy).ToString());
        }

        Console.SetCursorPosition(0, startY + 1);
        Console.Write(" a  b  c  d  e  f  g  h ");
        Console.ResetColor();
    }
    
    public void DrawFigures()
    {
        foreach (var figure in figures)
        {
            figure.DrawFigure();
        }
        Console.SetCursorPosition(0, 12);
        Console.WriteLine("Figures drawn");
    }
}