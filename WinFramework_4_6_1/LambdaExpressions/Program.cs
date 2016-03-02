using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaExpressions
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<int, int> sqr = capturedVar => capturedVar * capturedVar;

            // Single-treaded for loop is easy
            for (int i = 1; i <= 5; i++)
            {
                int outerVar = i;   // struct/ValueType make a copy on assignment. delegate/lambda may change it!
                Console.WriteLine(sqr(outerVar));   // outerVar is a captured variable, lambda expression sqr is a closure
            }

            Action[] actions = new Action[3];
            for (int i = 0; i < actions.Length; i++)
            {
                // each closure catches the same 'i' variable
                actions[i] = () => Console.Write(i);
            }
            foreach (var action in actions) action(); // 333
            Console.WriteLine();

            for (int i = 0; i < actions.Length; i++)
            {
                int localScopedVar = i;
                // each closure captures a fresh localScopedVar
                actions[i] = () => Console.Write(localScopedVar);
            }
            foreach (var action in actions) action(); // 012
            Console.WriteLine();

            for (int i = 0; i < 3; i++)
            {
                var f = Natural();
                Console.WriteLine($"{f()} \t{Natural()()}");
            }
        }

        static Func<int> Natural()
        {
            return () => { int seed = 0; return seed++; };
        }
    }
}
