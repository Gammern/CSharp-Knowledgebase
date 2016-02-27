using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
/*
Nummeric suffixes
F float float f = 1.0F;
D double double d = 1D;
M decimal decimal d = 1.0M;
U uint uint i = 1U;
L long long i = 1L;
UL ulong ulong i = 1UL;
*/

namespace NumericTypes
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Environment.Is64BitOperatingSystem {Environment.Is64BitOperatingSystem}");
            Console.WriteLine($"Environment.Is64BitProcess {Environment.Is64BitProcess}");

            Integral();

            UnsignedIntegral();

            Real();

            Console.WriteLine();
            checked // will only work for integral over/under flow
            {
                int a = int.MaxValue;
                try
                {
                    a++;
                }
                catch (OverflowException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.WriteLine("a is " + a + " whereas MaxInt is " + int.MaxValue);
            }

            bool b = true;
            Console.WriteLine($"\nbool is type {b.GetType()} of size {Marshal.SizeOf(b)} or {sizeof(bool)}");
            string s = "\u00A9 2016 - Johan øæåØÆÅ";
            Console.WriteLine(s);
        }
        private static void Real()
        {
            // for scientific and graphics (performance)
            float f = 2.0F;     // won't compile without F
            double d = 3.0D;    // default 64bit
            var infearedReal = 3.0;
            // for financial base-10-accurate calculations
            decimal c = 4.0M;   // won't compile without M

            Console.WriteLine("\nReal underlying types:");
            Console.WriteLine($"float \t{f.GetType()} \t{Marshal.SizeOf(f)}");
            Console.WriteLine($"double \t{d.GetType()} \t{Marshal.SizeOf(d)}");
            Console.WriteLine($"Infered\t{infearedReal.GetType()} \t{Marshal.SizeOf(infearedReal)}");
            Console.WriteLine($"decimal\t{c.GetType()} \t{Marshal.SizeOf(c)}");
        }

        private static void UnsignedIntegral()
        {
            byte bi = 4;
            ushort usi = 5;
            uint ui = 6U;
            ulong uli = 7UL;

            Console.WriteLine("\nUnsigned integral undelying type:");
            Console.WriteLine($"byte \t{bi.GetType()} \t{Marshal.SizeOf(bi)}");
            Console.WriteLine($"ushort \t{usi.GetType()} \t{Marshal.SizeOf(usi)}");
            Console.WriteLine($"uint \t{ui.GetType()} \t{Marshal.SizeOf(ui)}");
            Console.WriteLine($"ulong \t{uli.GetType()} \t{Marshal.SizeOf(uli)}");
        }

        private static void Integral()
        {
            // for interoperability
            sbyte sbi = 4;
            short si = 5;
            // First-class citizen
            int i = 6;          // default 
            long li = 7L;
            var defi = 9;       // int/Int32

            System.Numerics.BigInteger bi = 11; // 16byte/128bit  System.Int128?

            Console.WriteLine("\nSigned integral underlying type:");
            Console.WriteLine($"sbyte \t{sbi.GetType()} \t{Marshal.SizeOf(sbi)}");
            Console.WriteLine($"short \t{si.GetType()} \t{Marshal.SizeOf(si)}");
            Console.WriteLine($"int \t{i.GetType()} \t{Marshal.SizeOf(i)}");
            Console.WriteLine($"long \t{li.GetType()} \t{Marshal.SizeOf(li)}");
            Console.WriteLine($"default\t{defi.GetType()} \t{Marshal.SizeOf(defi)}");
            Console.WriteLine($"BigInteger\t{bi.GetType()} \t{Marshal.SizeOf(bi)}");
        }
    }
}
