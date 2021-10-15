using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad2
{
    class MainClass
    {
        static void Main(string[] args)
        {
            if (args.Length != 4)
                Console.WriteLine("Invalid argument count.");
            else if (!File.Exists(args[0]))
                Console.WriteLine("Input file does not exist.");
            else if (!Int32.TryParse(args[2], out int m) || !Int32.TryParse(args[3], out int n) || m <= 0 || n <= 0)
                Console.WriteLine("Invalid m or n value.");
            else
            {
                try
                {
                    Random random = new Random();
                    FileStream input = File.OpenRead(args[0]);
                    FileStream output = File.OpenWrite(args[1]);
                    StreamReader sr = new StreamReader(input);
                    StreamWriter sw = new StreamWriter(output);
                    long len = input.Length / sizeof(char);
                    char[] block = new char[m];
                    for (int i = 0; i < n; i++)
                    {

                        int ran = random.Next((int)len - m);
                        input.Seek(ran, SeekOrigin.Begin);
                        sr.ReadBlock(block, 0, m);

                        sw.WriteLine(block);

                        input.Position = 0;
                        sr.DiscardBufferedData();

                        //Array.Clear(block, 0, block.Length);
                    }
                    sr.Close();
                    sw.Close();
                    input.Close();
                    output.Close();
                }
                catch(Exception e)
                {
                    Console.WriteLine("An error occured when processing the files.\nStacktrace: " + e.Message);
                }
                Console.WriteLine("Done");
            }
        }
    }
}
