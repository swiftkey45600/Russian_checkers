using System.Drawing;
using System;
using System.Runtime.InteropServices;

namespace Program
{
    class Program()
    {
        static public int coordOfNotificationLine = 11;
        static int cursorX = 0;
        static int cursorY = 0;
        static bool selecting = false;
        static bool needToEat = false;
        static int selectedX = -1;
        static int selectedY = -1;
        static List<string> symbols = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H" };
        static ConsoleColor turn = ConsoleColor.White;
        static public void DrawTurn(ConsoleColor turn)
        {
            Console.SetCursorPosition(30, 0);
            Console.Write((turn == ConsoleColor.Black) ? "Current turn: Black" : "Current turn: White");
            Console.SetCursorPosition(0, coordOfNotificationLine);
        }

        static public void DrawPixel(int x, int y, ConsoleColor color)
        {
            Console.SetCursorPosition(x, y);
            Console.BackgroundColor = color;
            Console.Write("   ");
            Console.ResetColor();
        }

        static public void DrawNotification(string message)
        {
            Console.SetCursorPosition(0, coordOfNotificationLine);
            Console.WriteLine(message);
        }

        static public void HighlightCell(int x, int y, ConsoleColor color)
        {
            Console.SetCursorPosition(x * 3, y);
            Console.BackgroundColor = color;
            Console.Write("   ");
            Console.ResetColor();
        }

        static void Main()
        {
            Console.CursorVisible = false;
            Console.Clear();

            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            ChessBoard board = new ChessBoard();
            board.InitChessBoard();
            board.DrawChessBoard(width, height);
            board.InitFigures();
            board.DrawFigures();
            DrawTurn(turn);

            int cursorX = 0;
            int cursorY = 0;
            bool selecting = false;
            bool needToEat = false;
            Figure? selectedFigure = null;

            while (true)
            {
                Console.SetCursorPosition(cursorX * 3, cursorY);
                var figureAtNow = board.GetFigure(cursorX, cursorY);
                if (figureAtNow != null)
                {
                    figureAtNow.DrawFigure(highlight: true);
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.Write("   ");
                    Console.ResetColor();
                }

                var key = Console.ReadKey(true);

                var figureAtOld = board.GetFigure(cursorX, cursorY);
                if (figureAtOld != null)
                {
                    figureAtOld.DrawFigure();
                }
                else
                {
                    string cellColor = board.board[cursorY][cursorX];
                    Program.DrawPixel(cursorX * 3, cursorY, cellColor == "white" ? ConsoleColor.White : ConsoleColor.DarkYellow);
                }
                switch (key.Key)
                {
                    case ConsoleKey.W:
                        if (cursorY > 0) cursorY--;
                        break;
                    case ConsoleKey.S:
                        if (cursorY < 7) cursorY++;
                        break;
                    case ConsoleKey.A:
                        if (cursorX > 0) cursorX--;
                        break;
                    case ConsoleKey.D:
                        if (cursorX < 7) cursorX++;
                        break;
                    case ConsoleKey.Enter:
                        if (!selecting)
                        {
                            selectedFigure = board.GetFigure(cursorX, cursorY);
                            if (selectedFigure != null)
                            {
                                if (selectedFigure.color != turn)
                                {
                                    Program.DrawNotification(new string(' ', 40));
                                    Program.DrawNotification("It's not your turn!");
                                }
                                else
                                {

                                    if (board.IsAnyEatingMoveExistsForColor(turn))
                                    {
                                        if (board.IsAnyEatingMoveExistsForFigure(selectedFigure))
                                        {
                                            selecting = true;
                                            needToEat = true;
                                            selectedFigure.DrawFigure(highlight: true);
                                            Program.DrawNotification(new string(' ', 40));
                                            Program.DrawNotification($"Selected ({cursorX}, {cursorY})");
                                        }
                                        else
                                        {
                                            Program.DrawNotification(new string(' ', 40));
                                            Program.DrawNotification("You must choose figure, which can eat!");
                                        }
                                    }
                                    else
                                    {
                                        selecting = true;
                                        selectedFigure.DrawFigure(highlight: true);
                                        Program.DrawNotification(new string(' ', 40));
                                        Program.DrawNotification($"Selected ({cursorX}, {cursorY})");
                                    }
                                }
                                
                            }
                            else
                            {
                                Program.DrawNotification(new string(' ', 40));
                                Program.DrawNotification("No figure here");
                            }
                        }
                        else
                        {
                            if (selectedFigure != null)
                            {
                                if (selectedFigure.IsPossibleMove(cursorX, cursorY, board))
                                {
                                    if (needToEat)
                                    {
                                        if (selectedFigure.IsPossibleEating(cursorX, cursorY, board))
                                        {
                                            board.RemoveFigure(selectedFigure.GetFigureToEat(cursorX, cursorY, board));
                                            selectedFigure.Move(cursorX, cursorY);
                                            Console.Clear();
                                            board.DrawChessBoard(width, height);
                                            board.DrawFigures();
                                            Program.DrawNotification(new string(' ', 40));
                                            Program.DrawNotification("Move successful");
                                            if (!board.IsAnyEatingMoveExistsForFigure(selectedFigure))
                                            {
                                                turn = (turn == ConsoleColor.White) ? ConsoleColor.Black : ConsoleColor.White;
                                                Program.DrawTurn(turn);
                                            }
                                            else
                                            {
                                                Program.DrawNotification(new string(' ', 40));
                                                Program.DrawNotification("You have another eating!");
                                                Program.DrawTurn(turn);
                                            }
                                        }
                                        else
                                        {
                                            Program.DrawNotification(new string(' ', 40));
                                            Program.DrawNotification("You must eat!");
                                        }
                                    }
                                    else
                                    {
                                        selectedFigure.Move(cursorX, cursorY);
                                        Console.Clear();
                                        board.DrawChessBoard(width, height);
                                        board.DrawFigures();
                                        Program.DrawNotification(new string(' ', 40));
                                        Program.DrawNotification("Move successful");
                                        turn = (turn == ConsoleColor.White) ? ConsoleColor.Black : ConsoleColor.White;
                                        Program.DrawTurn(turn);
                                    }
                                }
                                else
                                {
                                    Program.DrawNotification(new string(' ', 40));
                                    Program.DrawNotification("Invalid move");
                                }
                                selecting = false;
                                needToEat = false;
                                selectedFigure = null;
                            }
                        }
                        break;
                    case ConsoleKey.Escape:
                        Program.DrawNotification(new string(' ', 40));
                        Program.DrawNotification("Exiting game... Goodbye!");
                        Console.WriteLine();
                        Console.CursorVisible = true;
                        return;
                }
            }
        }
    }
}