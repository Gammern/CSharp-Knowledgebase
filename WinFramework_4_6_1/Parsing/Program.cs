using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsing
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(true.ToString() + " " + false.ToString()); // output capitalize first letter: True False
            bool b;
            Console.WriteLine($"tryparse(1) is {bool.TryParse("1", out b)}");
            if (bool.TryParse("true", out b)) Console.WriteLine("tryparse(true) succeded");
            if (bool.TryParse("True", out b)) Console.WriteLine("tryparse(True) succeded");

            // real-type-suffix: one of F f D d M m
            Console.WriteLine(1.234D);
            // assume norwegian
            Console.WriteLine(double.Parse("1,234"));
        }
    }
}
