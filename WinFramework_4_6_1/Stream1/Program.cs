using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Stream1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var s = new FileStream("test.txt", FileMode.Create))
            {
                Console.WriteLine(s.CanRead);
                Console.WriteLine(s.CanWrite);
                Console.WriteLine(s.CanSeek);

                s.WriteByte(101);
                s.WriteByte(102);
                byte[] block = { 1, 2, 3, 4, 5 };
                s.Write(block, 0, block.Length);
                Console.WriteLine(s.Length);
                Console.WriteLine(s.Position);
                s.Position = 0;
                Console.WriteLine(s.ReadByte());
                Console.WriteLine(s.ReadByte());
                Console.WriteLine(s.Read(block,0,block.Length));
                Console.WriteLine(s.Read(block,0,block.Length));
            }
            AsyncDemo();
            Console.WriteLine("Press a key to quit");
            Console.ReadKey();
        }

        async static void AsyncDemo()
        {
            using (var s = new FileStream("test.txt", FileMode.Create))
            {
                byte[] block = { 1, 2, 3, 4, 5 };
                await s.WriteAsync(block, 0, block.Length);
                s.Position = 0;
                Console.WriteLine(await s.ReadAsync(block,0,block.Length));
            }
        }
    }
}
