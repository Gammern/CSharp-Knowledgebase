using System;

namespace Delegates
{
    public delegate TRes Transformer<in T, out TRes>(T x);
    // Multicast delegates "must" return void
    public delegate void ProgressReporter(int percentComplete);

    class Util
    {
        public static void Transform<T>(T[] values, 
            Func<T,T> t)
            //Transformer<T,T> t)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = t (values[i]);
            }
        }
        public static void HardWork(ProgressReporter p)
        {
            for (int i = 0; i < 10; i++)
            {
                p(i * 10);
                System.Threading.Thread.Sleep(100);
            }
        }
    }

    class Program
    {
        static int Square(int x) => x * x;

        //multicast sample
        static void WriteProgressToConsole(int percentComplete)
            => Console.WriteLine(percentComplete);
        static void WriteProgressToFile(int percentComplete)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(percentComplete);
            Console.ResetColor();
        }

        static void Main(string[] args)
        {
            Func<int, int> Square2 = x => x * x;    // incompatible with Transformer, but not Func<int,int>
            Transformer<int,int> Sqr = x => x * x;      // same signature, different type
            //Square2 = Sqr; error
            //Sqr = Square2; error

            int[] values = { 1, 2, 3 };
            Util.Transform(values, Square2);
            foreach (var i in values)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();

            // Multicast
            ProgressReporter p = WriteProgressToConsole;
            p += WriteProgressToFile;
            Console.WriteLine("Target: " + p.Target);   // null if static method
            Console.WriteLine("Method: " + p.Method);
            Util.HardWork(p);

        }
    }
}
