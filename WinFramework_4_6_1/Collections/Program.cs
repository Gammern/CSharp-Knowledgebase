using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    class Program
    {
        static void Main(string[] args)
        {
            MyGenCollection c1 = new MyGenCollection();

            foreach (var item in c1) Console.WriteLine(item);

            using (IEnumerator<int> rtor = c1.GetEnumerator())
            {
                while (rtor.MoveNext()) Console.WriteLine(rtor.Current);
                // rtor.Dispose();
            }

            MyIntList c2 = new MyIntList();
            foreach (int i in c2) Console.WriteLine(i);

            List<int> il = new List<int>(c1);
            foreach (var item in il.AsReadOnly())
            {
                Console.WriteLine(item);
            }

            LinkedList<double> dll = new LinkedList<double>(new double[] { 1.0, 2.71, 3.14 });
            foreach (var item in dll)
            {
                Console.WriteLine(item);
            }
        }

    }
}
