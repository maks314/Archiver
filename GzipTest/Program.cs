using System;//


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
