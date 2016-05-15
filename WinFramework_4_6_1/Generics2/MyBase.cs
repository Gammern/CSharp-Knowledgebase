using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Generics2
{
    //public class MyBase<T> where T:MyBase<T> // or class from
    [System.Xml.Serialization.XmlTypeAttribute("MyBase", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:MyBase")]
    public abstract class MyBase<T> where T:MyBase<T>
    {
        // T is not used, need it to make static field unique amongst inherited classes
        static string ns;

        static MyBase()
        {
            Console.WriteLine($"static ctor ");
        }

        public MyBase()
        {
         //   this.Namespace = (this.GetType().GetCustomAttributes(typeof(XmlTypeAttribute), false).FirstOrDefault() as XmlTypeAttribute).Namespace;
        }

        public string Namespace
        {
            get
            {
                if(string.IsNullOrEmpty(ns))
                {
                    ns = (this.GetType().GetCustomAttributes(typeof(XmlTypeAttribute), false).FirstOrDefault() as XmlTypeAttribute).Namespace;
                    Console.WriteLine($"ns set to {ns}");
                }
                return ns;
            }
        }
    }

    [System.Xml.Serialization.XmlTypeAttribute("Class1", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:Class1")]
    class MyClass1 : MyBase<MyClass1>
    {

    }

    [System.Xml.Serialization.XmlTypeAttribute("Class1", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:Class2")]
    class MyClass2 : MyBase<MyClass2>
    {
        
    }

}
