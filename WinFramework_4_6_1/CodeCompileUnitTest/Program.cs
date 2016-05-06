using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace CodeCompileUnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            CodeCompileUnit ccu = new CodeCompileUnit();
            CodeNamespace globalNs = new CodeNamespace();
            ccu.Namespaces.Add(globalNs);
            var schemaSet = new XmlSchemaSet();
            var mybaseSchema = new XmlSchema().Create("mybase.xsd");
            var myderivedSchema = new XmlSchema().Create("myderived.xsd");

            var schemas = new XmlSchemas();
            schemas.AddReference(mybaseSchema);
            schemas.Add(myderivedSchema);
            schemas.Compile(ValidationEventHandler, true);

            XmlSchemaImporter importer = new XmlSchemaImporter(schemas);
            XmlCodeExporter exporter = new XmlCodeExporter(globalNs, null, CodeGenerationOptions.GenerateOrder);
            foreach (XmlSchemaElement element in myderivedSchema.Elements.Values)
            {
                XmlTypeMapping mapping = importer.ImportTypeMapping(element.QualifiedName);
                exporter.ExportTypeMapping(mapping);
            }
            using (StreamWriter writer = File.CreateText("statystype.cs"))
            {
                new CSharpCodeProvider().GenerateCodeFromCompileUnit(ccu, writer, new CodeGeneratorOptions { BracingStyle = "C" });
            }
        }

        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            Console.WriteLine($"{e.Severity} {e.Message}");
        }
    }
}
