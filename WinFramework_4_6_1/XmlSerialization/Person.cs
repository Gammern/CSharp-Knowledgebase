using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace XmlSerialization
{
    [Serializable]
    [XmlType("Envelope", Namespace = "http://mynamespace/test/2016/envelope")]
    public class EnvelopeType
    {
        public PersonType Person { get; set; }
    }
    // props/fields must be public in order to be serialized with XmlSerializer
    // XmlSerializer writes elements in the order that they’re defined in the class.
    [Serializable]
    [XmlIncludeAttribute(typeof(StudentType))]
    [XmlIncludeAttribute(typeof(TeacherType))]
    [XmlType("Person",Namespace = "http://mynamespace/test/2016/person")]
    [XmlRoot("Person", Namespace = "http://mynamespace/test/2016/person", IsNullable = false)]
    public abstract class PersonType
    {
        [XmlAttribute(Namespace = "http://mynamespace/test/2016/person")]
        public int ID;
        [XmlElement("FirstName")]
        public string Name;
        public Address HomeAddress;
        public int Age;
        [XmlIgnore]
        public DateTime DOB;
        public byte[] KeyData;
        public override string ToString() => $"Name: {Name}, Age: {Age}";
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces XmlNs = new XmlSerializerNamespaces(new[]
        {
            new XmlQualifiedName("p", "http://mynamespace/test/2016/person"),
            new XmlQualifiedName("s", "http://mynamespace/test/2016/student"),
            new XmlQualifiedName("t", "http://mynamespace/test/2016/teacher")
        });
    }

    [Serializable]
    //[XmlRoot("Student", Namespace = "http://mynamespace/test/2016/sudent", IsNullable = false)]
    [XmlType("Student", Namespace = "http://mynamespace/test/2016/student")]
    public class StudentType : PersonType { };
    [Serializable]
    [XmlType("Teacher", Namespace = "http://mynamespace/test/2016/teacher")]
    //[XmlRoot("Teacher", Namespace = "http://mynamespace/test/2016/teacher", IsNullable = false)]
    public class TeacherType : PersonType { };

    public class Address
    {
        public string Street, PostCode;
    }
}
