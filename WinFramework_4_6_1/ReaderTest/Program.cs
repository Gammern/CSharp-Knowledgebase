using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReaderTest
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlReaderSettings settings = new XmlReaderSettings
            { IgnoreWhitespace = true, DtdProcessing = DtdProcessing.Parse, IgnoreComments = true, IgnoreProcessingInstructions = true };
            using (XmlReader r = XmlReader.Create("customer.xml", settings))
            {
                r.MoveToContent();
                r.ReadStartElement("customer");
                string firstname = r.ReadElementContentAsString("firstname", "");
                string lastname = r.ReadElementContentAsString("lastname", "");
                string quote = r.ReadElementContentAsString("quote", "");
                Console.WriteLine(r.Name + " " + r.NodeType);
                string notes = r.ReadElementContentAsString("notes", "");
                Console.WriteLine(r.Name + " " + r.NodeType);
                r.ReadEndElement();
            }

        }

        static void ReadNodesInFile()
        {
            XmlReaderSettings settings = new XmlReaderSettings
                { IgnoreWhitespace = true, DtdProcessing = DtdProcessing.Parse };
            using (XmlReader r = XmlReader.Create("customer.xml", settings))
            {
                while (r.Read())
                {
                    Console.Write(r.NodeType.ToString().PadRight(17, '-'));
                    Console.Write("> ".PadRight(r.Depth * 3));

                    switch (r.NodeType)
                    {
                        case XmlNodeType.Element:
                        case XmlNodeType.EndElement:
                            Console.WriteLine(r.Name);
                            break;

                        case XmlNodeType.Text:
                        case XmlNodeType.CDATA:
                        case XmlNodeType.Comment:
                        case XmlNodeType.XmlDeclaration:
                            Console.WriteLine(r.Value);
                            break;

                        case XmlNodeType.DocumentType:
                            Console.WriteLine(r.Name + " - " + r.Value.Trim());
                            break;

                        case XmlNodeType.Attribute:
                        case XmlNodeType.EntityReference:
                        case XmlNodeType.Entity:
                        case XmlNodeType.ProcessingInstruction:
                        case XmlNodeType.DocumentFragment:
                        case XmlNodeType.Notation:
                        case XmlNodeType.Whitespace:
                        case XmlNodeType.SignificantWhitespace:
                        case XmlNodeType.EndEntity:
                        case XmlNodeType.Document:
                            break;

                        default:
                            break;
                    }
                }
            }
        }
    }
}
