using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Zad1
{
    class MainClass
    {
        const string dnaPath = @"E:\Pobrane\dna.100MB\dna.100MB";
        const string englishPath = @"E:\Pobrane\english.100MB\english.100MB";

        static void Main(string[] args)
        {
            Console.WriteLine("Processing...");
            PreprocessEnglish();
            PreprocessDNA();
            Console.WriteLine("Finished");
        }

        /*
         * replace - czy zamieniać oryginalny plik z przerobionym
         */
        static void PreprocessEnglish(bool replace = false)
        {
            try
            {
                const string outputName = "out_english.100MB";
                if (File.Exists(englishPath))
                {
                    FileStream input = File.OpenRead(englishPath);
                    FileStream output = File.OpenWrite(outputName);
                    StreamReader sr = new StreamReader(input, System.Text.Encoding.GetEncoding(1257));
                    StreamWriter sw = new StreamWriter(output, System.Text.Encoding.GetEncoding(1257));
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        //zamiana dużych na małe
                        line = line.ToLower();
                        //usunięcie wszystkich znaków interpunkcyjnych poza kropką
                        line = Regex.Replace(line, @"[\p{P}-[\.]]", string.Empty);
                        //dodanie spacji przed i po kropce
                        line = Regex.Replace(line, @"\.", " . ");
                        //usunięcie białych znaków, pozostawienie pojedynczych spacji
                        line = Regex.Replace(line, @"\s+", " ");
                        sw.WriteLine(line);
                    }
                    sr.Close();
                    sw.Close();
                    input.Close();
                    output.Close();
                    if (replace)
                    {
                        File.Delete(englishPath);
                        File.Move(outputName, englishPath);
                    }
                    else
                    {
                        string temp = Path.GetDirectoryName(englishPath) + @"\" + outputName;
                        File.Delete(temp);
                        File.Move(outputName, temp);
                    }
                }
                else
                {
                    Console.WriteLine("Given english.100MB directory does not exist.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error during processing the english.100MB file.\nStacktrace: " + e.Message);
            }
        }

        static private void PreprocessDNA(bool replace = false)
        {
            try
            {
                const string outputName = "out_dna.100MB";
                if (File.Exists(dnaPath))
                {
                    FileStream input = File.OpenRead(dnaPath);
                    FileStream output = File.OpenWrite(outputName);
                    StreamReader sr = new StreamReader(input, System.Text.Encoding.GetEncoding(1257));
                    StreamWriter sw = new StreamWriter(output, System.Text.Encoding.GetEncoding(1257));
                    char[] buffer = new char[1024];
                    int read;
                    while ((read = sr.ReadBlock(buffer, 0, buffer.Length)) > 0)
                    {
                        string line = new string(buffer);
                        //usunięcie białych znaków oraz symboli innych niż A, T, G oraz C
                        line = Regex.Replace(line, @"[^ATGC\s+]", string.Empty);
                        sw.Write(line);
                    }
                    sr.Close();
                    sw.Close();
                    input.Close();
                    output.Close();
                    if (replace)
                    {
                        File.Delete(dnaPath);
                        File.Move(outputName, dnaPath);
                    }
                    else
                    {
                        string temp = Path.GetDirectoryName(dnaPath) + @"\" + outputName;
                        File.Delete(temp);
                        File.Move(outputName, temp);
                    }
                }
                else
                {
                    Console.WriteLine("Given dna.100MB directory does not exist.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error during processing the dna.100MB file.\nStacktrace: " + e.Message);
            }
        }
    }
}

