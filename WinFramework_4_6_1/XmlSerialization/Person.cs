using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlSerialization
{
    // props/fields must be public in order to be serialized with XmlSerializer
    // XmlSerializer writes elements in the order that they’re defined in the class.

    [XmlRoot("Person", Namespace = "http://mynamespace/test/2016/3")]
    [XmlInclude(typeof(Student))]
    [XmlInclude(typeof(Teacher))]
    public class PersonType
    {
        [XmlAttribute]
        public int ID;
        [XmlElement("FirstName")]
        public string Name;
        public Address HomeAddress;
        public int Age;
        [XmlIgnore]
        public DateTime DOB;
        public byte[] KeyData;
        public override string ToString() => $"Name: {Name}, Age: {Age}";
    }

    public class Student : PersonType { };
    public class Teacher : PersonType { };

    public class Address
    {
        public string Street, PostCode;
    }
}
