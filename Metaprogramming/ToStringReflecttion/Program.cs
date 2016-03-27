using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToStringReflecttion
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomerReflection c = new CustomerReflection { Age = 33, FirstName = "Ola", LastName = "Nordmann" };

            Console.WriteLine(c);
        }
    }
}
