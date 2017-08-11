using System;

namespace GzipTest
{
    class ProgressBar
    {
        public static void drawTextProgressBar(double progress, double total)
        {
            //draw empty progress bar
            progress = progress / 1000000.0;
            total = total / 1000000.0;
            Console.CursorLeft = 0;
            Console.Write("Progress");
            Console.CursorLeft = 9;
            Console.Write("["); //start
            Console.CursorLeft = 40;
            Console.Write("]"); //end
            Console.CursorLeft = 1;
            double onechunk = 30.0 / total;

            //draw filled part
            int position = 10;
            for (int i = 0; i < onechunk * progress; i++)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw unfilled part
            for (int i = position; i <= 39; i++)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw totals
            Console.CursorLeft = 45;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(progress.ToString() + " of " + total.ToString() + "    "); //blanks at the end remove any excess
        }
    }
}
