using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatorOverloading
{
    class Program
    {
        static void Main(string[] args)
        {
            Note B = new Note(2);
            Note CSharp = B + 2;
            CSharp += 2;
            CSharp += -2;

            Note n = (Note)554.37;  // explicit conversion
            double x = n;           // implicit conversion
        }
    }
}
