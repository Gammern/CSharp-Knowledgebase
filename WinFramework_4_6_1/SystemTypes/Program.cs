using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N = SystemTypes.myns;
using MT = SystemTypes.myns.MyValueType;
using static System.Console;
using System.Reflection;
using System.Collections;

namespace SystemTypes
{
    internal class Countdown : IEnumerator
    {
        int count = 11;
        public object Current => count;
        public bool MoveNext() => count-- > 0;
        public void Reset() { throw new NotSupportedException(); }
    }

    namespace myns
    {
        public interface IPoint { int x { get; } int y{ get; } }
        public class Point { public int X, Y; }
        struct MyValueType
        {
            internal int x, y;
        }
    }
    class Program
    {
        const string Msg1 = "This valu will never change"; // evaluated at compile time (const = #define)
        static readonly string Msg2 = "This value wil also never change";

        // const int sum1 = Enumerable.Range(1, 1000).Sum(); // eval at compile time, error, linq not a constant
        const int sum1 = 1 + 2 + 3 + 4 + 5 + 6 + 7;
        static readonly int sum2 = Enumerable.Range(1, 1000).Sum();  // eval at runtime
        //const int sum3 = sum2;  // error CS0133: The expression being assigned to 'Program.sum3' must be constant
        const int sum4 = sum1 + 8;

        [Flags]
        public enum BorderSide { None, Left, Right, Top, Bottom }

        static void Main(string[] args)
        {
            //Msg1 = "";  // error CS0131: The left-hand side of an assignment must be a variable, property or indexer
            //Msg2 = "";  // error CS0198: A static readonly field cannot be assigned to (except in a static constructor or a variable initializer)
            Type mt = Msg1.GetType();
            WriteLine($"{mt.FullName}");
            mt = Msg2.GetType();
            WriteLine($"{mt.FullName}");

            N.Point p = new N.Point();
            WriteLine(p.GetType().FullName);
            WriteLine(typeof(N.Point).FullName);
            WriteLine(Msg1);

            //Action<Type> showInheritance = null; showInheritance = t => { if (t != null) { Write(t.FullName);  Write(t.BaseType != null ? "->": "\n"); showInheritance(t.BaseType); } }; // recursive is overkill
            Func<Type, string[]> getInheritance = t => { List<string> sl = new List<string>(); while (t != null) { sl.Add(t.FullName); t = t.BaseType; } return sl.ToArray(); };
            // Func<Type,IEnumerable<Type>> inher = t => { while (t != null) { yield return t; t = t.BaseType; } }; // yield in lambd expression illegal
            Console.WriteLine(string.Join("->", inheritanceHierarchy(typeof(double))));
            Console.WriteLine(string.Join("->", getInheritance(typeof(float))));

            // Boxing between value type and object
            int i = 7;
            object o = i;   // boxing
            i = (int)o + 100; // unboxing
            long j = (int)o;
            //j = (long)o;    // System.InvalidCastException: Specified cast is not valid.
            j = (long)(int)o;
            WriteLine(j);

            MT[] vals = new MT[] { new MT{ x = 1, y = 2 }, new MT { x = 2, y = 4 } };
            Console.WriteLine(string.Join("->", getInheritance(vals.GetType())));

            Console.WriteLine("Array length = " + vals.Length);
            string key = string.Join("", Assembly.GetExecutingAssembly().GetName().GetPublicKey().Select(b => b.ToString("x2")));
            Console.WriteLine(key);
            Console.WriteLine("Key length = " + key.Length);

            IEnumerator cd = new Countdown();
            while (cd.MoveNext())
                Write(cd.Current + ",");
            Console.WriteLine();
            foreach (var item in ((BorderSide[])Enum.GetValues(typeof(BorderSide))).Distinct())
            {
                Console.WriteLine($"{(int)item} {item} {item.GetType()} ");
            }

        }
        public static IEnumerable<string> inheritanceHierarchy(Type t)
        {
            while (t != null)
            {
                yield return t.FullName;
                t = t.BaseType;
            }
        }
    }
}
