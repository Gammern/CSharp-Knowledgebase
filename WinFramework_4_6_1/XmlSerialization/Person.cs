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

    [Serializable]
    [XmlType(Namespace = "http://mynamespace/test/2016/3")]
    [XmlRoot("Person", Namespace = "http://mynamespace/test/2016/3", IsNullable = false)]
    public abstract class PersonType
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

    [Serializable]
    [XmlRoot("Student", Namespace = "http://mynamespace/test/2016/3", IsNullable = false)]
    [XmlType(Namespace = "http://mynamespace/test/2016/3")]
    public class Student : PersonType { };
    [Serializable]
    [XmlType(Namespace = "http://mynamespace/test/2016/3")]
    [XmlRoot("Teacher", Namespace = "http://mynamespace/test/2016/3", IsNullable = false)]
    public class Teacher : PersonType { };

    public class Address
    {
        public string Street, PostCode;
    }
}
