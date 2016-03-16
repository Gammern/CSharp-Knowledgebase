using System;
using System.IO;
using System.IO.Compression;

namespace StreamCompression
{
    class Program
    {
        static void Main(string[] args)
        {
            CompTest();
            Console.ReadKey();
        }

        private async static void CompTest()
        {
            const string filename = "compressed.bin";
            string[] words = "The quick brown fox jumps over the lazy dog".Split();
            Random rand = new Random();

            using (var s = File.Create(filename))
            {
                using (var ds = new DeflateStream(s, CompressionMode.Compress))
                {
                    using (var w = new StreamWriter(ds))
                    {
                        for (int i = 0; i < 1000; i++)
                        {
                            await w.WriteAsync(words[rand.Next(words.Length)] + " ");
                        }
                    }
                }
            }

            var fi = new FileInfo(filename);
            var length = fi.Length;
            Console.WriteLine("Length of compressed file: " + length);

            using (var s = File.OpenRead(filename))
            {
                using (var ds = new DeflateStream(s, CompressionMode.Decompress))
                {
                    using (var r = new StreamReader(ds))
                    {
                        Console.WriteLine(await r.ReadToEndAsync());
                    }
                }
            }
        }
    }
}
