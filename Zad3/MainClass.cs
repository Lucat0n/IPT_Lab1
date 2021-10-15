using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad3
{
    class MainClass
    {
        private const string outputPath = "output.txt";
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            if (args.Length < 2 || args.Length > 3)
                Console.WriteLine("Invalid argument count.");
            else if (!File.Exists(args[0]) || !File.Exists(args[1]))
                Console.WriteLine("Input file path is invalid.");
            else if (args.Length == 3 && !Int32.TryParse(args[2], out _))
                Console.WriteLine("Invalid index.");
            else
            {
                if (File.Exists(outputPath))
                    File.Delete(outputPath);
                string[] patterns = GetPatternsFromFile(args[1]);
                FileStream input = File.OpenRead(args[0]);
                StreamReader sr = new StreamReader(input);
                if (args.Length == 3)
                {
                    Int32.TryParse(args[2], out int index);
                    Match(sr, patterns[index]);
                }
                else
                {
                    foreach(string pattern in patterns)
                        Match(sr, pattern);
                }
            }
            stopwatch.Stop();
            Console.WriteLine("Done in " + stopwatch.Elapsed.Milliseconds + "ms (" + ((double)stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000000000.0) + "ns)");
            Console.ReadLine();
        }

        private static string[] GetPatternsFromFile(string path)
        {
            string pattern;
            string[] patterns;
            List<string> patternList = new List<string>();
            FileStream input = File.OpenRead(path);
            StreamReader sr = new StreamReader(input);
            while ((pattern = sr.ReadLine()) != null)
                patternList.Add(pattern);

            patterns = patternList.ToArray();
            input.Close();
            sr.Close();
            return patterns;
        }

        private static void Match(StreamReader input, string pattern, string outPath = outputPath)
        {
            FileStream output = File.OpenWrite(outPath);
            StreamWriter sw = new StreamWriter(output);
            sw.BaseStream.Seek(0, SeekOrigin.End);
            long n = input.BaseStream.Length / sizeof(char);
            long m = pattern.Length;
            char[] buffer = new char[m];
            for (int s=0; s <= n-m; s++)
            {
                input.BaseStream.Seek(s, SeekOrigin.Begin);
                input.ReadBlock(buffer, 0, (int)m);
                if (pattern.Equals(new String(buffer)))
                    sw.WriteLine("Pattern " + pattern + " occurs with shift " + s);
                input.BaseStream.Position = 0;
                input.DiscardBufferedData();
            }
            input.BaseStream.Position = 0;
            input.DiscardBufferedData();
            sw.Close();
            output.Close();
        }
    }
}
