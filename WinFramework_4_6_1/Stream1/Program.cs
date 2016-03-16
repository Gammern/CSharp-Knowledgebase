using System;
using System.Text;
using System.IO;

namespace Stream1
{
    class Program
    {
        static void Main(string[] args)
        {
            ByteDemo();
            Person p = new Person { Name = "Ola Nordmann", Age = 44, Height = 180.0 };
            MemoryStream ms = new MemoryStream();
            p.SaveData(ms);
            ms.Position = 0;
            Person p2 = new Person();
            p2.LoadData(ms);
            Console.WriteLine($"{p2.Name} {p2.Age} {p2.Height}");
        }

        static void StremDemo()
        {
            var encoding = new UTF8Encoding(
                encoderShouldEmitUTF8Identifier: false,
                throwOnInvalidBytes: true);

            Action<string, Encoding> BOM = (s, e) => Console.WriteLine(s + ": " + string.Join(",", e.GetPreamble()));
            BOM("ASCII", Encoding.ASCII);
            BOM("UTF7", Encoding.UTF7);
            BOM("UTF8", Encoding.UTF8);
            BOM("Unicode", Encoding.Unicode);
            BOM("UTF32",Encoding.UTF32);
            BOM("BigEndianUnicode",Encoding.BigEndianUnicode);
            
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

        static void ByteDemo()
        {
            using (TextWriter w = File.CreateText("but.txt"))
                w.WriteLine("but—");

            using (Stream s = File.OpenRead("but.txt"))
            {
                for (int b; (b = s.ReadByte()) > 1;)
                    Console.WriteLine(b + "\t" + (char)b);
            }
        }


    }

    public class Person
    {
        public string Name;
        public int Age;
        public double Height;
        public void SaveData(Stream s)
        {
            var w = new BinaryWriter(s);
            w.Write(Name);
            w.Write(Age);
            w.Write(Height);
            w.Flush();
        }
        public void LoadData(Stream s)
        {
            var r = new BinaryReader(s);
            Name = r.ReadString();
            Age = r.ReadInt32();
            Height = r.ReadDouble();
        }
    }
}
