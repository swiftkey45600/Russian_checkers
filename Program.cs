using System.Drawing;
using System;
using System.Runtime.InteropServices;

namespace Program
{
    class Program()
    {
        static public int coordOfNotificationLine = 10;
        static List<string> symbols = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H" };
        static ConsoleColor turn = ConsoleColor.White;

        static public void DrawLastFiveMoves(ChessBoard board)
        {
            Console.SetCursorPosition(30, 2);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Last 5 Moves:");
            Console.ResetColor();
            int line = 3;
            if (board.moves.Count < 5)
            {
                for (int i = 0; i < board.moves.Count; i++)
                {
                    Console.SetCursorPosition(30, line);
                    Console.WriteLine(board.moves[i]);
                    line++;
                }
            }
            else
            {
                foreach (var move in board.moves[(board.moves.Count - 5)..])
                {
                    Console.SetCursorPosition(30, line);
                    Console.WriteLine(move);
                    line++;
                }
            }
        }
        static public void DrawTurn(ConsoleColor turn)
        {
            Console.SetCursorPosition(30, 0);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write((turn == ConsoleColor.Black) ? "Current turn: Black" : "Current turn: White");
            Console.ResetColor();
            Console.SetCursorPosition(0, coordOfNotificationLine);
        }

        static public void DrawPixel(int x, int y, ConsoleColor color)
        {
            Console.SetCursorPosition(x, y);
            Console.BackgroundColor = color;
            Console.Write("   ");
            Console.ResetColor();
        }

        static public void DrawNotification(string message, ConsoleColor color = ConsoleColor.Yellow)
        {
            Console.SetCursorPosition(0, coordOfNotificationLine);
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
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
                if (!board.IsAnyMoveExists(turn))
                {
                    Program.DrawNotification(new string(' ', 40));
                    Program.DrawNotification((turn == ConsoleColor.Black) ? "Whites won! Thanks for playing.." : "Blacks won! Thanks for playing..", ConsoleColor.Yellow);
                    Console.WriteLine();
                    Console.CursorVisible = true;
                    return;
                }

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
                                    Program.DrawNotification("It's not your turn!", ConsoleColor.Red);
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
                                            Program.DrawNotification($"Selected ({symbols[cursorX]}, {8 - cursorY})", ConsoleColor.Green);
                                        }
                                        else
                                        {
                                            Program.DrawNotification(new string(' ', 40));
                                            Program.DrawNotification("You must choose figure, which can eat!", ConsoleColor.Red);
                                        }
                                    }
                                    else
                                    {
                                        selecting = true;
                                        selectedFigure.DrawFigure(highlight: true);
                                        Program.DrawNotification(new string(' ', 40));
                                        Program.DrawNotification($"Selected ({symbols[cursorX]}, {8 - cursorY})", ConsoleColor.Green);
                                    }
                                }

                            }
                            else
                            {
                                Program.DrawNotification(new string(' ', 40));
                                Program.DrawNotification("No figure here", ConsoleColor.Red);
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
                                            int begincoordX = selectedFigure.x;
                                            int begincoordY = selectedFigure.y;
                                            selectedFigure.Move(cursorX, cursorY);
                                            Console.Clear();
                                            board.DrawChessBoard(width, height);
                                            board.DrawFigures();
                                            Program.DrawNotification(new string(' ', 40));
                                            Program.DrawNotification("Move successful", ConsoleColor.Green);
                                            board.AddMoveToList($"{symbols[begincoordX]}, {8 - begincoordY} -> {symbols[cursorX]}, {8 - cursorY}");
                                            Program.DrawLastFiveMoves(board);
                                            if (!board.IsAnyEatingMoveExistsForFigure(selectedFigure))
                                            {
                                                turn = (turn == ConsoleColor.White) ? ConsoleColor.Black : ConsoleColor.White;
                                                Program.DrawTurn(turn);
                                            }
                                            else
                                            {
                                                Program.DrawNotification(new string(' ', 40));
                                                Program.DrawNotification("You have another eating!", ConsoleColor.Cyan);
                                                Program.DrawTurn(turn);
                                            }
                                        }
                                        else
                                        {
                                            Program.DrawNotification(new string(' ', 40));
                                            Program.DrawNotification("You must eat!", ConsoleColor.Red);
                                        }
                                    }
                                    else
                                    {
                                        int begincoordX = selectedFigure.x;
                                        int begincoordY = selectedFigure.y;
                                        selectedFigure.Move(cursorX, cursorY);
                                        Console.Clear();
                                        board.DrawChessBoard(width, height);
                                        board.DrawFigures();
                                        Program.DrawNotification(new string(' ', 40));
                                        Program.DrawNotification("Move successful", ConsoleColor.Green);
                                        board.AddMoveToList($"{symbols[begincoordX]}, {8 - begincoordY} -> {symbols[cursorX]}, {8 - cursorY}");
                                        Program.DrawLastFiveMoves(board);
                                        turn = (turn == ConsoleColor.White) ? ConsoleColor.Black : ConsoleColor.White;
                                        Program.DrawTurn(turn);
                                    }
                                }
                                else
                                {
                                    Program.DrawNotification(new string(' ', 40));
                                    Program.DrawNotification("Invalid move", ConsoleColor.Red);
                                }
                                selecting = false;
                                needToEat = false;
                                selectedFigure = null;
                            }
                        }
                        break;
                    case ConsoleKey.Escape:
                        Program.DrawNotification(new string(' ', 40));
                        Program.DrawNotification("Exiting game... Goodbye!", ConsoleColor.Yellow);
                        Console.WriteLine();
                        Console.CursorVisible = true;
                        return;
                }
            }
        }
    }
}