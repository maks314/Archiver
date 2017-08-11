using System;//


namespace GzipTest
{
    class Program
    {


        static void Main(string[] args)
        {
            Gzip gzip = new Gzip();
            gzip.Start(args);
            Console.WriteLine("Complate");
            Console.ReadKey();

        }

    }
}
