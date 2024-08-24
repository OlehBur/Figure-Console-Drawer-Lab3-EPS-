using static Lab3.part2.ConsoleInfo;
using static ConsoleInfo;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
SetConsoleMaximazed();


StartProg:
const string START_COMMAND = " Enter command (circle|rectangle|square|exit): ";
string consoleCommandBuff;
Commands currentCommand = Commands.none;

GetCommand();
switch (currentCommand)//complete command and get the result
{
    case Commands.circle:
        Circle c = new Circle(Circle.InputStr());
        c.Draw();
        break;

    case Commands.square:
        Square s = new Square(Square.InputStr());
        s.Draw();
        break;

    case Commands.rectangle:
        float h = 0f, w = 0f;
        Rectangle.InputStr(out h, out w);
        Rectangle r = new Rectangle(h, w);
        r.Draw();
        break;

    case Commands.none:
        Figure.DrawNone();
        break;

    case Commands.exit:
        goto EndProg;
}

WaitingNextCommand();
goto StartProg;

EndProg:
Console.WriteLine("\t\tExit the program...");


void GetCommand()
{
    Console.Write(START_COMMAND);
    consoleCommandBuff = Console.ReadLine();
    ClearConsoleLine(START_COMMAND.Length + consoleCommandBuff.Length); // without -2, because '\n'

    //for (Commands i = Commands.none; i <= Commands.exit; i++)
    //    if (consoleCommandBuff == nameof(i))
    //    {
    //        currentCommand = i;
    //        return;
    //    }
    foreach (string commandName in Enum.GetNames(typeof(Commands)))
    {
        if (consoleCommandBuff == commandName)
        {
            currentCommand = (Commands)Enum.Parse(typeof(Commands), commandName);
            return;
        }
    }

    currentCommand = Commands.none;
    Console.SetCursorPosition(0, 0);
}

void WaitingNextCommand()
{
    const string ENTER_TIP = "< Press \"Enter\" key to enter a new command >";

    DrawCenterText(ENTER_TIP, GetConsoleHeight() + WAIT_COMMAND_LINE_OFFSET);
    Console.ReadLine();
    Console.Clear();
}

void ClearConsoleLine(int length, int consoleRow = 0)
{
    string clearBuffer = new string(' ', length);// str fulll spaces
    Console.SetCursorPosition(0, consoleRow);
    Console.Write(clearBuffer);
}

enum Commands { none, circle, rectangle, square, exit };




abstract public class Figure
{
    abstract public float GetArea();
    abstract public void Draw();
    static public void DrawNone()
    {
        Console.WriteLine("The command was entered incorrectly, please try again\r\n");
    }

    protected void DrawAreaTxt()
    {
        string figureArea = $"S2 = {GetArea()}";

        DrawCenterText(figureArea);
    }

    protected void SetCursorToFigureBorder(int x, int y)
    {
        Console.SetCursorPosition(GetConsoleCenterX() + x, GetConsoleCenterY() + y);
        //Math.Clamp(GetConsoleCenterX() + x, 0, GetConsoleWidth()),
        //Math.Clamp(GetConsoleCenterY() + y, COMMAND_LINE_OFFSET, GetConsoleHeight()));
    }
}

public class Circle : Figure
{
    public float radius { get; private set; }

    public Circle(float r)
    {
        radius = r;
    }

    public override float GetArea()
    {
        return MathF.PI * MathF.Pow(radius, 2);
    }

    public override void Draw()
    {
        const int CIRCLE_STROKE = 2;
        const char STROKE_SYM = '○';
        int bufferRadius = (int)Math.Clamp(radius, 1, GetConsoleCenterY() - COMMAND_LINE_OFFSET - WAIT_COMMAND_LINE_OFFSET);

        for (int y = -bufferRadius; y <= bufferRadius; y++)
            for (int x = (int)(-bufferRadius * CONSOLE_X_OFFSET); x <= (int)(CONSOLE_X_OFFSET * bufferRadius); x++)
            {
                //if (x * x + y * y <= radius * VIEW_COEFF * radius)
                //{
                int circleArea = (int)(MathF.Pow(x / CONSOLE_X_OFFSET, 2) + y * y);
                if (circleArea > bufferRadius * (bufferRadius - CIRCLE_STROKE) && circleArea <= bufferRadius * bufferRadius)
                {
                    SetCursorToFigureBorder(x, y);
                    Console.Write(STROKE_SYM);
                }
            }

        DrawAreaTxt();
    }

    public override string ToString()
    {
        return $"r = {radius}, S2 = {GetArea()}";
    }

    public static float InputStr()
    {
        Console.SetCursorPosition(0, 0);
        Console.Write("r = ____.");
        Console.SetCursorPosition(4, 0);
        return float.Parse(Console.ReadLine());
    }
}

public class Rectangle : Figure
{
    public float height { get; private set; }
    public float width { get; private set; }

    public Rectangle(float h, float w)
    {
        height = h;
        width = w;
    }

    public override float GetArea()
    {
        return height * width;
    }

    public override void Draw()
    {
        const char STROKE_SYM = '▭';
        int bufferHeight, bufferWidth;//= (int)Math.Clamp(height, 1, GetConsoleCenterY() - COMMAND_LINE_OFFSET - WAIT_COMMAND_LINE_OFFSET);
        // = (int)Math.Clamp(width, 1, GetConsoleCenterX() / CONSOLE_X_OFFSET);
        GetSizeByRatioInLimit(out bufferHeight, out bufferWidth,
            (int)GetConsoleCenterY() - COMMAND_LINE_OFFSET - WAIT_COMMAND_LINE_OFFSET);

        for (int y = -bufferHeight; y <= bufferHeight; y++)
            for (int x = (int)(-bufferWidth * CONSOLE_X_OFFSET); x <= (int)(bufferWidth * CONSOLE_X_OFFSET); x++)
                if (MathF.Abs(y) == bufferHeight || MathF.Abs(x) == (int)(bufferWidth * CONSOLE_X_OFFSET))
                {
                    SetCursorToFigureBorder(x, y);
                    Console.Write(STROKE_SYM);
                }

        DrawAreaTxt();
    }

    private void GetSizeByRatioInLimit(out int h, out int w, int yLimit)
    {
        float sidesRatio = width / height;

        if (width > height)
        {
            w = (int)Math.Clamp(width, 1, yLimit);
            h = (int)(w / sidesRatio);
        }
        else
        {
            sidesRatio = height / width;
            h = (int)Math.Clamp(height, 1, yLimit);
            w = (int)(h / sidesRatio);
        }
    }

    public override string ToString()
    {
        return $"h = {height}, w = {width}, S2 = {GetArea()}";
    }

    public static void InputStr(out float h, out float w)
    {
        Console.SetCursorPosition(0, 0);
        Console.Write("h = ____, w = ____.");
        Console.SetCursorPosition(4, 0);
        h = float.Parse(Console.ReadLine());
        Console.SetCursorPosition(14, 0);
        w = float.Parse(Console.ReadLine());
    }
}

public class Square : Figure
{
    public float width { get; private set; }

    public Square(float w)
    {
        width = w;
    }

    public override float GetArea()
    {
        return MathF.Pow(width, 2);
    }

    public override void Draw()
    {
        const char STROKE_SYM = '□';
        int bufferWidth = (int)Math.Clamp(width, 1, GetConsoleCenterY() - COMMAND_LINE_OFFSET - WAIT_COMMAND_LINE_OFFSET);

        for (int y = -bufferWidth; y <= bufferWidth; y++)
            for (int x = (int)(-bufferWidth * CONSOLE_X_OFFSET); x <= (int)(bufferWidth * CONSOLE_X_OFFSET); x++)
                if (MathF.Abs(x) == (int)(bufferWidth * CONSOLE_X_OFFSET) || MathF.Abs(y) == bufferWidth)
                {
                    SetCursorToFigureBorder(x, y);
                    Console.Write(STROKE_SYM);
                }

        DrawAreaTxt();
    }

    public override string ToString()
    {
        return $"w = {width}, S2 = {GetArea()}";
    }

    public static float InputStr()
    {
        Console.SetCursorPosition(0, 0);
        Console.Write("w = ____.");
        Console.SetCursorPosition(4, 0);
        return float.Parse(Console.ReadLine());
    }
}

public static partial class ConsoleInfo
{
    public static readonly int COMMAND_LINE_OFFSET = 2;
    public static readonly int WAIT_COMMAND_LINE_OFFSET = 1;

    public static int GetConsoleCenterX()
    {
        return GetConsoleWidth() / 2;
    }

    public static int GetConsoleCenterY()
    {
        return GetConsoleHeight() / 2 + COMMAND_LINE_OFFSET;
    }

    public static void DrawCenterText(string text, int yPos = 0)
    {
        yPos = (yPos == 0) ?
            GetConsoleCenterY() : yPos;

        Console.SetCursorPosition(GetConsoleCenterX() - (text.Length - 1) / 2, yPos);
        Console.Write(text);
    }
}