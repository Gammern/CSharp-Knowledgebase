using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace XDom1
{
    class Program
    {
        const string filename = "UBL-Invoice-2.1-Example.xml";

        static void Main(string[] args)
        {
            //LoadTest();
            //FixNS();
            //DiffTest();
            NameSpaces();
        }

        private static void NameSpaces()
        {
            XNamespace ns = "Oreilly.Nutshell.CSharp";
            XNamespace nsa = "Oreilly.Nutshell.CSharp.Address";
            XNamespace xsi = XmlSchema.InstanceNamespace;
            var nil = new XAttribute(xsi + "nil", true);

            XDocument doc = new XDocument(new XElement(ns+"Invoice", new XElement(nsa+"Customer", new XElement(ns+"Address", "444 My Road"), new XElement(ns+"Credit", nil))));
            doc.Root.SetAttributeValue(XNamespace.Xmlns + "ns", ns);
            doc.Root.SetAttributeValue(XNamespace.Xmlns + "nsa", nsa);
            doc.Root.SetAttributeValue(XNamespace.Xmlns + "xsi", xsi);
            doc.Declaration = new XDeclaration("1.0", "utf-8", "true");
            doc.Root.Descendants(ns + "Credit").First().SetAttributeValue(ns+"myattrib", 123);//  AddAnnotation("My annotation");

            var output = new StringBuilder(); // utf-16 type
            var settings = new XmlWriterSettings { Indent = true };
            using (XmlWriter xw = XmlWriter.Create(output, settings))
                doc.Save(xw);
            Console.WriteLine(output.ToString());
            Console.WriteLine();
            Console.WriteLine(doc.Root.Descendants(ns+"Address").First().ToString());
        }

        private static void FixNS()
        {
            var doc1 = XDocument.Load(filename);
            XNamespace ns = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";
            // remove namespace attributes in child elements since we already have a ns declaration i root
            var attribs = doc1.Root.Descendants().SelectMany(n => n.Attributes()).Where(n => n.IsNamespaceDeclaration && n.Value.Equals(ns.NamespaceName) ).ToList();
            attribs.ForEach(a => a.Parent.SetAttributeValue(a.Name, null));
            doc1.Save("test1.xml");
        }

        private static void DiffTest()
        {
            var doc1 = XDocument.Load(filename);
            var doc2 = XDocument.Load("test1.xml"); // generated in FixNS()

            var comparer = new XNodeEqualityComparer();
            
            bool same = comparer.Equals(doc1, doc2);
            Console.WriteLine("Docs equal: " + same);
            if (!same)
            {
                var diff2 = doc1.Document.DescendantNodes().Except(doc2.Document.DescendantNodes()).ToArray();
                int count = diff2.Count();
            }

            var diff = doc2.Descendants().Cast<XNode>().Except(doc1.Descendants().Cast<XNode>(), new XNodeEqualityComparer());
            foreach (var item in diff.Cast<XElement>())
            {
                Console.WriteLine( PrintLocalName(item));
            }
        }

        private static string PrintLocalName(XElement item)
        {
            if (item == null)
            {
                return string.Empty;
            }
            string res = PrintLocalName(item.Parent) + "->" + item.Name.LocalName;
            if (!item.HasElements) res = res + " (" + item.Value + ")";
            return res;
        }

        private static void LoadTest()
        {
            XDocument doc = XDocument.Load(filename);
            Console.WriteLine(doc.Declaration);
            var d = doc.Document.Root.Elements();

            foreach (var att in doc.Root.Attributes())
            {
                Console.WriteLine($"{att.IsNamespaceDeclaration,-6} {att.Name.LocalName,-6} {att.Value}");
            }

            foreach (var elem in doc.Document.Root.Elements())  //same as Nodes().OfType<XElement>()) 
            {
                
                XName name = elem.Name;
                Console.WriteLine($"{elem.NodeType,-12} {elem.Name.LocalName,-30} {elem.Value.Substring(0, Math.Min(elem.Value.Length,65))}");
            }
        }

        static void ParseTest()
        {
            string xml = 
                @"<customer id='123' status='archived'>
                <!--Dummy Text-->
                <firstname>Joe</firstname>
                <lastname>Bloggs<!--nice name--></lastname>
                </customer>";

            XElement customer = XElement.Parse(xml);
            XDocument doc = XDocument.Parse(xml);

            foreach (XAttribute a1 in customer.Attributes())
            {
                Console.WriteLine(a1);
            }

            foreach (var n1 in customer.Nodes().OfType<XElement>())
            {
                Console.WriteLine(n1.GetType().ToString() + " \t" + n1.Name );
                foreach (var n2 in n1.Nodes())
                {
                    Console.WriteLine("\t" + n2.GetType().ToString() + " \t" + n2.ToString());
                }
            }
        }
    }
}
