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
            string path = args[0];
            if (!Directory.Exists(path))
            {
                path = @"C:\Users\johan\Documents\ubl\UBL-2.1\xsd\maindoc";
            }

            XmlSchemaSet maindocSchemaSet = new XmlSchemaSet(new NameTable());
            maindocSchemaSet.ValidationEventHandler += MaindocSchemaSet_ValidationEventHandler;

            maindocSchemaSet.AddFilesFromDirectory(path, MaindocSchemaSet_ValidationEventHandler);

            Console.WriteLine("Compiling ...");
            maindocSchemaSet.Compile();

            using (var writer = File.CreateText("result.txt"))
            {
                foreach (XmlSchema schema in maindocSchemaSet.Schemas())
                {
                    //Console.WriteLine(schema.TargetNamespace);
                    writer.WriteLine(schema.TargetNamespace);
                }
            }

        }

        private static void MaindocSchemaSet_ValidationEventHandler(object sender, ValidationEventArgs arg)
        {
            Console.WriteLine($"{arg.Severity}: {arg.Message}");
        }
    }
}
