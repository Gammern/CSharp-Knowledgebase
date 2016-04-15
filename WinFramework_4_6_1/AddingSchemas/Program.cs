using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace AddingSchemas
{
    class Program
    {
        static void Main(string[] args)
        {
            // download and unzip http://docs.oasis-open.org/ubl/os-UBL-2.1/UBL-2.1.zip
            string ublBasePath = "C:/Users/johan/Documents/ubl/UBL-2.1/xsd";


            XmlNameTable nt = new NameTable();
            XmlSchemaSet schemaSet = new XmlSchemaSet(nt);
            schemaSet.ValidationEventHandler += MaindocSchemaSet_ValidationEventHandler;
            schemaSet.AddFilesFromDirectory(ublBasePath, MaindocSchemaSet_ValidationEventHandler);

            Console.WriteLine("Compiling ...");
            schemaSet.Compile();

            using (var writer = File.CreateText("result.txt"))
            {
                int len = schemaSet.Schemas().Cast<XmlSchema>().First().SourceUri.IndexOf("/xsd/") + 5;
                foreach (XmlSchema schema in schemaSet.Schemas())
                {
                    string s = $"{schema.SourceUri.Remove(0, len)}\t {schema.TargetNamespace}";
                    writer.WriteLine(s);
                }
            }

        }

        private static void MaindocSchemaSet_ValidationEventHandler(object sender, ValidationEventArgs arg)
        {
            Console.WriteLine($"{arg.Severity}: {arg.Message}");
        }
    }
}
