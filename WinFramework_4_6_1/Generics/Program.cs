using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Generics
{
    class Program
    {
        static void Main(string[] args)
        {
            var stack = new Stack<int>();
            stack.Push(5);
            stack.Push(10);
            try
            {
                Console.WriteLine(stack.Pop());
                Console.WriteLine(stack.Pop());
                Console.WriteLine(stack.Pop());
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
            }
            int maxVal = Max(5, 10);
            string largeS = /*Max("AAA", "BBB");*/ "string is a immutable reference type, not a value type.";
            Console.WriteLine($"maxVal = {maxVal}  largeS = \"{largeS}\"");
            BigInteger i1 = 50;
            BigInteger i2 = BigInteger.Pow(long.MaxValue, byte.MaxValue);
            
            //i2 += i2 * i2 * i2;
            var max = Max(i1, i2);
            Console.WriteLine("a big biginteger is:\n" + max.ToString("R") +"\n\n" + max);
        }

        static void Swap<T>(ref T left, ref T right)
        {
            T temp = left;
            left = right;
            right = temp;
        }

        static T Max<T>(T left, T right) where T : struct, IComparable<T>
            => left.CompareTo(right) > 0 ? left : right;
    }
}
