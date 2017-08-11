using System;//
using System.Collections.Generic;//
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;//
using System.Text;//
using System.Threading;
using System.Threading.Tasks;//

namespace GzipTest
{
    class Program
    {


        static void Main(string[] args)
        {
            Gzip gzip = new Gzip();
            //string[] s = { "decompress", "m00.iso.gz", "m00.iso" };
            gzip.Start(args);
            Console.WriteLine("Complate");
            Console.ReadKey();

        }

    }
}
