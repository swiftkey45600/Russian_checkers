namespace Program;
public class Figure
{
    public int x { get; set; }
    public int y { get; set; }
    public ConsoleColor color { get; set; }

    public bool isDamka { get; set; }

    public Figure(int x, int y, ConsoleColor color, bool isDamka)
    {
        this.x = x;
        this.y = y;
        this.color = color;
        this.isDamka = isDamka;
    }

    public virtual void DrawFigure()
    {
        {
            Console.SetCursorPosition(x * 3, y);
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            if (color == ConsoleColor.White)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" ⛂ ");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(" ⛂ ");
                Console.ResetColor();
            }
        }
    }

    public virtual void Move(int deltaX, int deltaY)
    {
        x += deltaX;
        y += deltaY;
    }

    public virtual bool IsPossibleMove(int newX, int newY)
    {
        return true;
    }
}