using System;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Threading;


namespace GzipTest
{
    class Gzip
    {
        private int threadNumber;

        private byte[][] dataArray;
        private byte[][] compressedDataArray;
        private int dataPortionSize;

        //private object locker = new object();

        private Parser pars = new Parser();
        private string pattern = "";
        private Regex regExp;
        private string parameters = "";

        public Gzip()
        {
            threadNumber = Environment.ProcessorCount;
            dataArray = new byte[threadNumber][];
            compressedDataArray = new byte[threadNumber][];
            dataPortionSize = 1048576;

        }

        public void Compress()
        {
            Console.WriteLine("START");
            string directory = Directory.GetCurrentDirectory();
            Console.WriteLine(directory);
            string inFileName = directory + "\\" + pars.FirstName;
            string outFileName = directory + "\\" + pars.SecondName;


            try
            {
                Watch.Start();

                FileStream inFile = new FileStream(inFileName, FileMode.Open);
                FileStream outFile = new FileStream(outFileName + "." + pars.Extension + ".gz", FileMode.Append);
                int _dataPortionSize;
                Thread[] tPool;
                byte[][] block_size_before_compression = new byte[threadNumber][];
                

                byte[][] result = new byte[threadNumber][];
                Console.Write("Compressing\n");
                while (inFile.Position < inFile.Length)
                {
                    tPool = new Thread[threadNumber];
                    for (int portionCount = 0; (portionCount < threadNumber) && (inFile.Position < inFile.Length);
                         portionCount++)
                    {
                        if (inFile.Length - inFile.Position <= dataPortionSize)
                        {
                            _dataPortionSize = (int)(inFile.Length - inFile.Position);
                        }
                        else
                        {
                            _dataPortionSize = dataPortionSize;
                        }

                        block_size_before_compression[portionCount] = BitConverter.GetBytes(_dataPortionSize);
                       
                        dataArray[portionCount] = new byte[_dataPortionSize];
                        inFile.Read(dataArray[portionCount], 0, _dataPortionSize);

                        tPool[portionCount] = new Thread(CompressBlock);
                        tPool[portionCount].Start(portionCount);

                    }

                    for (int portionCount = 0; (portionCount < threadNumber) && (tPool[portionCount] != null);)
                    {
                        if (tPool[portionCount].ThreadState == System.Threading.ThreadState.Stopped)
                        {
                            BitConverter.GetBytes(compressedDataArray[portionCount].Length)
                                        .CopyTo(compressedDataArray[portionCount], 4);

                            block_size_before_compression[portionCount].CopyTo(compressedDataArray[portionCount], compressedDataArray[portionCount].Length-4);

                           
                            outFile.Write(compressedDataArray[portionCount], 0, compressedDataArray[portionCount].Length);
                            portionCount++;
                        }
                    }

                    ProgressBar.drawTextProgressBar((double)inFile.Position , (double)inFile.Length);
                }

                outFile.Close();
                inFile.Close();

                Watch.Stop();

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR:" + ex.Message);
            }
        }

        private void CompressBlock(object i)
        {
            //lock (locker)
            //{
            using (MemoryStream output = new MemoryStream(dataArray[(int)i].Length))
            {
                using (GZipStream cs = new GZipStream(output, CompressionMode.Compress))
                {
                    cs.Write(dataArray[(int)i], 0, dataArray[(int)i].Length);
                }
                compressedDataArray[(int)i] = output.ToArray();
            }
            //}  
        }

        public void Decompress()
        {
            Console.WriteLine("START");
            string directory = Directory.GetCurrentDirectory();
            Console.WriteLine(directory);
            string inFileName = directory + "\\" + pars.FirstName;
            string outFileName = directory + "\\" + pars.SecondName;

            try
            {
                Watch.Start();
                FileStream inFile = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
                FileStream outFile = new FileStream(outFileName, FileMode.Create, FileAccess.Write);

                int _dataPortionSize;
                int compressedBlockLength;
                Thread[] tPool;
                Console.Write("Decompressing\n");

                byte[] buffer = new byte[8];
                while (inFile.Position < inFile.Length)
                {
                    tPool = new Thread[threadNumber];
                    for (int portionCount = 0;
                         (portionCount < threadNumber) && (inFile.Position < inFile.Length);
                         portionCount++)
                    {
                       

                        inFile.Read(buffer, 0, buffer.Length);
                        compressedBlockLength = BitConverter.ToInt32(buffer, 4);
                        compressedDataArray[portionCount] = new byte[compressedBlockLength];
                        buffer.CopyTo(compressedDataArray[portionCount], 0);
                        inFile.Read(compressedDataArray[portionCount], buffer.Length, compressedBlockLength - buffer.Length);
                        _dataPortionSize = BitConverter.ToInt32(compressedDataArray[portionCount], compressedBlockLength - 4);
                        dataArray[portionCount] = new byte[_dataPortionSize];

                        tPool[portionCount] = new Thread(DecompressBlock);
                        tPool[portionCount].Start(portionCount);
                    }

                    for (int portionCount = 0; (portionCount < threadNumber) && (tPool[portionCount] != null);)
                    {
                        if (tPool[portionCount].ThreadState == System.Threading.ThreadState.Stopped)
                        {
                            outFile.Write(dataArray[portionCount], 0, dataArray[portionCount].Length);
                            portionCount++;
                        }
                    }
                    ProgressBar.drawTextProgressBar((double)inFile.Position, (double)inFile.Length);
                }

                outFile.Close();
                inFile.Close();
                Watch.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }

        }

        private void DecompressBlock(object i)
        {
            try
            {
                using (MemoryStream input = new MemoryStream(compressedDataArray[(int)i]))
                {

                    using (GZipStream ds = new GZipStream(input, CompressionMode.Decompress))
                    {
                        ds.Read(dataArray[(int)i], 0, dataArray[(int)i].Length);
                    }

                }
            }
            catch (InvalidDataException ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }

        public void Start(string[] args)
        {
            int i = 1;
            if (args.Length != 0)
                foreach (string s in args)
                {
                    if(i < args.Length)
                    {
                        Parameters += s + " ";
                        i++;
                    }
                    else
                    {
                        Parameters += s;
                    }
                   
                }
                   

            pattern = @"^(\b((c|dec)ompress)\b\s[^\[\]]{1,50}\s[^\[\]]{1,50})$";
            try
            {
                regExp = new Regex(pattern);

                if (regExp.IsMatch(Parameters) || Parameters == "help")
                {

                    pars.ParserParameters(Parameters);

                    if (pars.Operation == "compress")
                    {
                        Compress();
                    }
                    if (pars.Operation == "decompress")
                    {
                        Decompress();
                    }
                    if (Parameters == "help")
                    {
                        Reference();
                    }


                }
                else
                    throw new Exception("Неверно задана строка параметров!!!\n");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public void Reference()
        {
            string helpMsg = string.Format("Для архивации: GZipTest.exe compress [имя исходного файла] [имя архива] \n"
                        + "Для разархивации: GZipTest.exe decompress  [имя архива] [имя распакованного файла]");
            string helpExample = string.Format("ПРИМЕР : GZipTest.exe compress test.doc result");
            Console.WriteLine(helpMsg);
            Console.WriteLine(helpExample);
        }

        public string Parameters { get => parameters; set => parameters = value; }

    }
}
