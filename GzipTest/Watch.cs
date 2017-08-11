using System;
using System.Diagnostics;

namespace GzipTest
{
    class Watch
    {
        private static Stopwatch stopWatch = new Stopwatch();

        public static void Start()
        {
            stopWatch.Start();
        }

        public static void Stop()
        {
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
        }
    }
}
