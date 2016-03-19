using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

// Will the name DataContract clash with the attribute by the same name? NO
namespace DataContract
{
    class Program
    {
        static void Main(string[] args)
        {
            DataContractTest();
        }

        private static void DataContractTest()
        {
            Person p = new Person { Name = "Stacey", Age = 30 };
            var ds = new DataContractSerializer(typeof(Person));
            using (var s = File.Create("person.xml"))
            {
                ds.WriteObject(s, p);
            }

            Person p2;
            using (var s = File.OpenRead("person.xml"))
            {
                p2 = (Person)ds.ReadObject(s);
            }

            Console.WriteLine(p2);
        }
    }
}
