using System.Diagnostics;
using System.IO.Compression;
using System.Security.Cryptography.X509Certificates;

namespace Program;

public class ChessBoard
{
    public int size = 8;
    public List<List<string>> board = new List<List<string>>();
    public List<Figure> figures = new List<Figure>();

    public bool IsColorsSame(Figure chosenFigure, Player currentPlayer)
    {
        return chosenFigure.color == currentPlayer.color;
    }

    public bool IsAnyMoveExists(ConsoleColor color)
    {
        bool output = false;
        foreach (var figure in figures)
        {
            if (figure.color != color) continue;
            for (int newY = 0; newY < size; newY++)
            {
                for (int newX = 0; newX < size; newX++)
                {
                    if (figure.IsPossibleMove(newX, newY, this))
                    {
                        output = true;
                        break;
                    }
                }
                if (output) break;
            }
        }
        return output;
    }

    public bool IsAnyEatingMoveExistsForColor(ConsoleColor color)
    {
        foreach (var figure in figures)
        {
            if (figure.color == color)
            {
                for (int newY = 0; newY < size; newY++)
                {
                    for (int newX = 0; newX < size; newX++)
                    {
                        if (figure.IsPossibleEating(newX, newY, this))
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public bool IsAnyEatingMoveExistsForFigure(Figure figure)
    {
        for (int newY = 0; newY < size; newY++)
        {
            for (int newX = 0; newX < size; newX++)
            {
                if (figure.IsPossibleEating(newX, newY, this))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsMoveEating(Figure selectedFigure)
    {
        return true;
    }

    public bool IsMoveLegal(Figure selectedFigure)
    {
        bool isAnyFigureCanMove = true;
        foreach (var figure in figures)
        {
            if (figure.color != selectedFigure.color) continue;
            for (int newY = 0; newY < size; newY++)
            {
                for (int newX = 0; newX < size; newX++)
                {
                    if (figure.IsPossibleMove(newX, newY, this))
                    {
                        if (figure.isHaveEaten && (figure.x != selectedFigure.x || figure.y != selectedFigure.y))
                        {
                            isAnyFigureCanMove = false;
                        }
                    }
                    if (isAnyFigureCanMove) break;
                }
                if (isAnyFigureCanMove) break;
            }
            if (isAnyFigureCanMove) break;
        }
        return isAnyFigureCanMove;
    }

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
                        Figure blackFigure = new Figure(x, y, ConsoleColor.Black);
                        figures.Add(blackFigure);
                    }
                    else if (y > 4)
                    {
                        Figure whiteFigure = new Figure(x, y, ConsoleColor.White);
                        figures.Add(whiteFigure);
                    }
                }
            }
            else
            {
                for (int x = 0; x < 8; x += 2)
                {
                    if (y < 3)
                    {
                        Figure blackFigure = new Figure(x, y, ConsoleColor.Black);
                        figures.Add(blackFigure);
                    }
                    else if (y > 4)
                    {
                        Figure whiteFigure = new Figure(x, y, ConsoleColor.White);
                        figures.Add(whiteFigure);
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
        Program.DrawNotification("Figures drawn");
    }

    public Figure? GetFigure(int x, int y)
    {
        foreach (var figure in figures)
        {
            if (figure.x == x && figure.y == y)
            {
                return figure;
            }
        }
        return null;
    }

    public void RemoveFigure(Figure? figure)
    {
        if (figure != null) figures.Remove(figure);
    }

    public bool IsFigureExists(int x, int y)
    {
        if (board[y][x] == "white") return false;
        return GetFigure(x, y) != null;
    }
}
