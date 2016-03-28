using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Console.WriteLine(ires);
        }
    }
}
