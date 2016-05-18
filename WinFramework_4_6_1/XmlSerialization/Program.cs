using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace XmlSerialization
{
    class Program
    {
        static Type[] subtypes = new Type[] {typeof(StudentType), typeof(TeacherType) };
        static void Main(string[] args)
        {
            PersonType t = new TeacherType { Name = "Teacher", Age = 30, ID = 2, KeyData = new byte[] { 1, 2, 3, 4, 5 } };
            PersonType s = new StudentType { Name = "Student", Age = 30, ID = 2, KeyData = new byte[] { 1, 2, 3, 4, 5 }, HomeAddress = new Address { Street="Mystreet 5", PostCode="1234" } };
            SerializePerson(t, "teacher.xml");
            SerializePerson(s, "student.xml");


            PersonType p2 = DeserializePerson("teacher.xml");
            Console.WriteLine(p2);
            p2 = DeserializePerson("student.xml");
            Console.WriteLine(p2);
        }

        private static PersonType DeserializePerson(string path)
        {
            EnvelopeType e;
            PersonType p2 = null;
            using (var s = File.OpenRead(path))
            {
                var xs = new XmlSerializer(typeof(EnvelopeType));//,subtypes);
                e = (EnvelopeType)xs.Deserialize(s);
                p2 = e.Person;
            }
            return p2;
        }
        static public XmlSerializerNamespaces XmlNs = new XmlSerializerNamespaces(new[]
        {
            new XmlQualifiedName("e", "http://mynamespace/test/2016/envelope"),
            new XmlQualifiedName("p", "http://mynamespace/test/2016/person"),
            new XmlQualifiedName("s", "http://mynamespace/test/2016/student"),
            new XmlQualifiedName("t", "http://mynamespace/test/2016/teacher")
        });

        private static void SerializePerson(PersonType p, string path)
        {
            EnvelopeType e = new EnvelopeType
            {
                Person = p
            };
            var xs = new XmlSerializer(typeof(EnvelopeType));
            using (var s = File.Create(path))
            {
                xs.Serialize(s, e);//, XmlNs);
            }
        }
    }

}
