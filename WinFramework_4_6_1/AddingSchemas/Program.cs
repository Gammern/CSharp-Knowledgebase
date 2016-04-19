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
            Console.WriteLine("Files not parsed:");
            schemaSet.GetFilesNotLoaded(ublBasePath).ForEach(Console.WriteLine);

            Console.WriteLine("Compiling ...");
            schemaSet.Compile();

            // show namespaces and prefix
            // schemaSet.GetNamespacePrefixes().ToList().ForEach(ns => Console.WriteLine($"{ns.Name}\t {ns.Namespace}"));

            var comparer = new XmlSchemaElementComparer();
            var props = schemaSet.MaindocSchemas()
                    .Select(s => s.Items)
                    .SelectMany(o => o.OfType<XmlSchemaComplexType>())
                    .Select(c => (c.ContentTypeParticle as XmlSchemaSequence).Items.Cast<XmlSchemaElement>())
                    .ToList();
            var commonProps = props.Skip(1).Aggregate(
                new HashSet<XmlSchemaElement>(props.First(), comparer), (h, e) => { h.IntersectWith(e); return h; }).ToList();

            foreach (var item in commonProps)
            {
                Console.WriteLine(item.QualifiedName.Name);
            }

            using (var writer = File.CreateText("result.txt"))
            {
                int len = schemaSet.Schemas().Cast<XmlSchema>().First().SourceUri.IndexOf("/xsd/") + 5;
                foreach (XmlSchema schema in schemaSet.Schemas())
                {
                    string s = $"{schema.SourceUri.Remove(0, len)}\t {schema.TargetNamespace}";
                    writer.WriteLine(s);
                }
                Console.WriteLine("\nCommon properties:");
                foreach (var p in commonProps)
                {
                    writer.WriteLine(p.QualifiedName.Name);
                }

            }

        }

        private static void MaindocSchemaSet_ValidationEventHandler(object sender, ValidationEventArgs arg)
        {
            if(!HandleXmlSchemaException(arg.Exception))
                Console.WriteLine($"{arg.Severity}: {arg.Message}");
        }

        private static bool HandleXmlSchemaException(Exception exception, int tab = 0)
        {
            string s = new string(' ', tab);
            XmlSchemaException ex = exception as XmlSchemaException;
            if (null != ex)
            {
                Console.WriteLine($"{s}{Path.GetFileName(ex.SourceUri)}: {ex.Message}" );
                HandleXmlSchemaException(ex.InnerException, tab+2);
                return true;
            }
            XmlException ex2 = exception as XmlException;
            if (null != ex2)
            {
                Console.WriteLine($"{s}{Path.GetFileName(ex2.SourceUri)}: {ex2.Message}");
                HandleXmlSchemaException(ex2.InnerException, tab + 2);
                return true;
            }
            return false;
        }

        private static bool HandleErr<T>(T ex, string source, int tab = 0) 
            where T: SystemException
        {
            if (ex is XmlSchemaException || ex is XmlException)
            {
                string s = new string(' ', tab);
                Console.WriteLine($"{s}{Path.GetFileName(source)}: {ex.Message}");
                HandleXmlSchemaException(ex.InnerException, tab + 2);
                return true;
            }
            return false;
        }
    }
}
