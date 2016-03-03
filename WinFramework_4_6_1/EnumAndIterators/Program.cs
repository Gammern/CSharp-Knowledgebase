using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumAndIterators
{
    /// <summary>
    /// Enumerator, readonly forward cursor. Current, MoveNext() and Reset()
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            int[] nums = new int[] { 1, 2, 3, 4, 5 };

            // foreach enumerates over an enumerable object
            // foreach is a consumer of enumerator
            foreach (var item in nums) Console.WriteLine(item);

            IEnumerator ia = nums.GetEnumerator(); // C#1 artifact
            while (ia.MoveNext()) Console.WriteLine(ia.Current); // object, unboxing? No accordig to doc!

            using (IEnumerator<int> gia = nums.ToList().GetEnumerator()) // C#2
                while (gia.MoveNext()) Console.WriteLine(gia.Current); // int

            foreach (var fib in Fibs(12))
            {
                Console.Write(fib + " ");
            }
            Console.WriteLine();

            foreach (var item in Foo())
            {
                Console.Write(item + " ");
            }
            Foo().ToList().ForEach(e => Console.Write(e + " "));
        }

        static IEnumerable<int> Fibs(int fibCount)
        {
            for (int i = 0, prevFib=1, curFib = 1; i < fibCount; i++)
            {
                yield return prevFib;
                int newFib = prevFib + curFib;
                prevFib = curFib;
                curFib = newFib;
            }
        }
        
        // Iterator must return one of the following
        // IEnumerable
        // IEnumerable<>
        // IEnumerator
        // IEnumerator<>

        public static System.Collections.Generic.IEnumerable<string> Foo()
        {
            yield return "One";
            yield return "Two";
            yield return "Three";
            yield break;
        }
    }
}
