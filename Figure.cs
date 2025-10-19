namespace Program;

public class Figure
{
    public int x { get; set; }
    public int y { get; set; }
    public ConsoleColor color { get; set; }
    public bool isHaveEaten = false;

    private bool isDamka = false;

    public Figure(int x, int y, ConsoleColor color)
    {
        this.x = x;
        this.y = y;
        this.color = color;
    }

    public virtual void DrawFigure(bool highlight = false)
    {
        {
            Console.SetCursorPosition(x * 3, y);
            Console.BackgroundColor = highlight ? ConsoleColor.Green : ConsoleColor.DarkYellow;
            if (color == ConsoleColor.White)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write((isDamka ? " ⛂ " : " ⛀ "));
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write((isDamka ? " ⛂ " : " ⛀ "));
                Console.ResetColor();
            }
        }
    }

    public virtual Figure? GetFigureToEat(int newX, int newY, ChessBoard board)
    {
        if (isDamka)
        {
            if (Math.Abs(newX - x) != Math.Abs(newY - y))
                return null;

            int stepX = (newX - x) / Math.Abs(newX - x);
            int stepY = (newY - y) / Math.Abs(newY - y);
            int currX = x + stepX;
            int currY = y + stepY;

            Figure? figureToEat = null;
            while (currX != newX || currY != newY)
            {
                Figure? figureAtPos = board.GetFigure(currX, currY);
                if (figureAtPos != null)
                {
                    if (figureAtPos.color == this.color)
                        return null;
                    if (figureToEat != null)
                        return null;
                    figureToEat = figureAtPos;
                }
                currX += stepX;
                currY += stepY;
            }
            return figureToEat;
        }
        else
        {
            int midX = (x + newX) / 2;
            int midY = (y + newY) / 2;
            if (Math.Abs(newX - x) == 2 && Math.Abs(newY - y) == 2)
            {
                Figure? middleFigure = board.GetFigure(midX, midY);
                if (middleFigure != null && middleFigure.color != this.color)
                {
                    return middleFigure;
                }
            }
            return null;
        }
    }

    public virtual bool IsDamka() { return isDamka; }

    public virtual void Move(int newX, int newY)
    {
        x = newX;
        y = newY;
        if ((color == ConsoleColor.White && y == 0) || (color == ConsoleColor.Black && y == 7))
        {
            isDamka = true;
        }
    }

    public virtual bool IsPossibleEatingBySimpleFigure(int newX, int newY, ChessBoard board)
    {
        int midX = (x + newX) / 2;
        int midY = (y + newY) / 2;
        if (Math.Abs(newX - x) == 2 && Math.Abs(newY - y) == 2)
        {
            Figure? middleFigure = board.GetFigure(midX, midY);
            if (middleFigure != null && middleFigure.color != this.color)
            {
                return true;
            }
        }
        return false;
    }

    public virtual bool IsPossibleEating(int newX, int newY, ChessBoard board)
    {
        if (isDamka)
        {
            if (Math.Abs(newX - x) != Math.Abs(newY - y))
                return false;
            if (newX == x || newY == y)
                return false;
            int stepX = (newX - x) / Math.Abs(newX - x);
            int stepY = (newY - y) / Math.Abs(newY - y);
            int currX = x + stepX;
            int currY = y + stepY;

            Figure? figureAtEnd = board.GetFigure(newX, newY);
            if (figureAtEnd != null)
                return false;

            List<Figure> figuresOnWay = new List<Figure>();
            while (currX != newX || currY != newY)
            {
                Figure? figureAtPos = board.GetFigure(currX, currY);
                if (figureAtPos != null)
                {
                    figuresOnWay.Add(figureAtPos);
                }
                currX += stepX;
                currY += stepY;
            }
            if (figuresOnWay.Count == 0)
            {
                return false;
            }
            else if (figuresOnWay.Count == 1)
            {
                Figure middleFigure = figuresOnWay[0];
                if (middleFigure.color != this.color)
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            int midX = (x + newX) / 2;
            int midY = (y + newY) / 2;
            if (board.GetFigure(newX, newY) != null)
                return false;
            if (Math.Abs(newX - x) == 2 && Math.Abs(newY - y) == 2)
            {
                Figure? middleFigure = board.GetFigure(midX, midY);
                if (middleFigure != null && middleFigure.color != this.color)
                {
                    return true;
                }
            }
            return false;
        }
    }


    public virtual bool IsPossibleMoveDamka(int newX, int newY, ChessBoard board)
    {
        if (Math.Abs(newX - x) != Math.Abs(newY - y))
            return false;

        int stepX = (newX - x) / Math.Abs(newX - x);
        int stepY = (newY - y) / Math.Abs(newY - y);
        int currX = x + stepX;
        int currY = y + stepY;

        List<Figure> figuresOnWay = new List<Figure>();
        while (currX != newX || currY != newY)
        {
            Figure? figureAtPos = board.GetFigure(currX, currY);
            if (figureAtPos != null)
            {
                figuresOnWay.Add(figureAtPos);
            }
            currX += stepX;
            currY += stepY;
        }
        if (figuresOnWay.Count == 0)
        {
            return true;
        }
        else if (figuresOnWay.Count == 1)
        {
            Figure middleFigure = figuresOnWay[0];
            if (middleFigure.color != this.color)
            {
                return true;
            }
        }
        return false;
    }

    public virtual bool IsPossibleMove(int newX, int newY, ChessBoard board)
    {
        Figure? figureAtNewCoords = board.GetFigure(newX, newY);
        if (figureAtNewCoords != null) return false;

        isHaveEaten = false;

        if (color == ConsoleColor.White)
        {
            if (newY < y && y - newY == 1 && Math.Abs(newX - x) == 1)
            {
                return true;
            }
            else
            {
                if (isDamka)
                {
                    return IsPossibleMoveDamka(newX, newY, board);
                }
                else
                {
                    if (IsPossibleEating(newX, newY, board))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        else
        {
            if (newY > y && newY - y == 1 && Math.Abs(newX - x) == 1)
            {
                return true;
            }
            else
            {
                if (isDamka)
                {
                    return IsPossibleMoveDamka(newX, newY, board);
                }
                else
                {
                    if (IsPossibleEating(newX, newY, board))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
    }
}