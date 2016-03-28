using System;
using System.Collections.Generic;
using System.Linq;
//using T4Compare1.generic;
using T4Compare1.T4;

namespace T4Compare1
{
    class Program
    {
        static void Main(string[] args)
        {
            object o1 = 3, o2 = 4, ores;
            string s1 = "Hello"; long l1 = 3;
            try
            {
                ores = greater.of(o1, o2);
                var res = greater.of(s1, l1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            int ires = greater.of(1, 2);
            string sres = greater.of("A", "B");
            Console.WriteLine(ires);

            Tuple<int, int, string> t3 = new Tuple<int, int, string>(2,2,"444");
            Console.WriteLine(ExpandedTypeName(t3.GetType()));
        }

        private static string ExpandedTypeName(Type t)
        {
            if (!t.IsGenericType)
                return t.Name;
            else
                return string.Format("{0}<{1}>", 
                    t.Name.Substring(0, t.Name.IndexOf('`')), 
                    string.Join(", ",t.GetGenericArguments().Select(a => a.Name)));
        }
    }
}
