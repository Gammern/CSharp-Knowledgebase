using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlSerialization
{
    class Program
    {
        static Type[] subtypes = new Type[] {typeof(Student), typeof(Teacher) };
        static void Main(string[] args)
        {
            PersonType t = new Teacher { Name = "Stacey", Age = 30, ID = 2, KeyData = new byte[] { 1, 2, 3, 4, 5 } };
            PersonType s = new Student { Name = "Stacey", Age = 30, ID = 2, KeyData = new byte[] { 1, 2, 3, 4, 5 }, HomeAddress = new Address { Street="Mystreet 5", PostCode="1234" } };
            SerializePerson(t, "teacher.xml");
            SerializePerson(s, "student.xml");

            PersonType p2 = DeserializePerson("teacher.xml");
            Console.WriteLine(p2);
        }

        private static PersonType DeserializePerson(string path)
        {
            PersonType p2 = null;
            using (var s = File.OpenRead(path))
            {
                var xs = new XmlSerializer(typeof(PersonType),subtypes);
                object o = xs.Deserialize(s);
            }
            return p2;
        }

        private static void SerializePerson(PersonType p, string path)
        {
            var xs = new XmlSerializer(p.GetType());
            using (var s = File.Create(path))
            {
                xs.Serialize(s, p);
            }
        }
    }

}
