using DAL.L2S.Nutshell;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operators
{
    class Program
    {
        static readonly string[] names = { "Tom", "Dick", "Harry", "Mary", "Jay" };

        static string TS<T>(IEnumerable<T> seq) => string.Join(" ", seq);

        static void Main(string[] args)
        {
            using (var db = new NutshellContextDataContext())
            {
                var t = Enumerable.Empty<int>();
            }
            WhereSample();
            WhereSample2();
            TakeWhileSample();
            Projecting();
        }

        private static void Projecting()
        {
            var query = from f in System.Windows.Media.Fonts.SystemFontFamilies
                        select f.ToString();
            Console.WriteLine("Fonts: " + TS(query));
        }

        private static void TakeWhileSample()
        {
            int[] numbers = {3,5,2,400,4,1 };
            var query = numbers.TakeWhile(n => n < 100);
            Console.WriteLine("Numbers: " + TS(query));
        }

        private static void WhereSample2()
        {
            var query = names.Where(s => new[] { "Tom", "Jay" }.Contains(s));
            Console.WriteLine("Where2: " + TS(query));
        }

        private static void WhereSample()
        {
            var query = names.Where(s => s.EndsWith("y"));
            query = from n in names
                    where n.EndsWith("y")
                    select n;
            Console.WriteLine("Where: " + TS(query));
            query = names.Select((s,i) => $"{i}:{s}").Where((s, i) => i % 2 == 0);
            Console.WriteLine("Where i even: " + TS(query));
        }
    }
}
