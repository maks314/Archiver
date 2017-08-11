using System;//
using System.Collections.Generic;//
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;//
using System.Text;//
using System.Threading;


namespace GzipTest
{
    class Program
    {


        static void Main(string[] args)
        {
            Gzip gzip = new Gzip();
            string[] s = { "compress", "new.txt", "m00" };
            gzip.Start(s);
            Console.WriteLine("Complate");
            Console.ReadKey();

        }

    }
}
