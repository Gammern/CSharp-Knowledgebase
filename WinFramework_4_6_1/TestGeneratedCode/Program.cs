using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStatus;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace TestGeneratedCode
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("der", "http://tempuri.org/myderived.xsd");
            // It is a problem that the type has been renamed
            StatusType myStatus = new StatusType { Text = new MyDerived.TextType1 { Value = "MyTestValue" , languageID = "EN" } };
            using (var writer = File.CreateText("status1.xml"))
            {
                var ser = new XmlSerializer(typeof(StatusType));
                ser.Serialize(writer, myStatus, ns);
            }
            Console.WriteLine(myStatus.Text.Value);
        }
    }
}
