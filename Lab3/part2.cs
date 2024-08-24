using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    internal class part2
    {
        public static partial class ConsoleInfo
        {
            public const float CONSOLE_X_OFFSET = 1.9f;


            public static void SetConsoleMaximazed()
            {
                Console.WindowWidth = Console.BufferWidth = Console.LargestWindowWidth;
                Console.WindowHeight = Console.BufferHeight = Console.LargestWindowHeight;
            }

            public static int GetConsoleHeight()
            {
                return Console.WindowHeight - 2;// + -1 because for next command tip line
            }

            public static int GetConsoleWidth()
            {
                return Console.WindowWidth - 1;
            }
        }
    }
}
