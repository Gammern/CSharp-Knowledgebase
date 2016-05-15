using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics2
{
    class Program
    {
        static void Main(string[] args)
        {
            MyClass1 c1 = new MyClass1();
            Console.WriteLine(c1.Namespace);
            MyClass2 c2 = new MyClass2();
            MyClass2 c21 = new MyClass2();
            MyClass2 c22 = new MyClass2();
            MyClass2 c23 = new MyClass2();
            Console.WriteLine(c2.Namespace);
        }
    }
}
