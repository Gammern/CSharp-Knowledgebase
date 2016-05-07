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
using System.Collections;

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
            var mybaseSchema = XmlSchemaExtensions.Create("mybase.xsd");
            var myderivedSchema = XmlSchemaExtensions.Create("myderived.xsd");
            var mystatusSchema = XmlSchemaExtensions.Create("mystatus.xsd");

            var mybaseNs = new CodeNamespace("MyBase");
            var myDerivedNs = new CodeNamespace("MyDerived") { Imports = { new CodeNamespaceImport(mybaseNs.Name) } };
            var myStatusNs = new CodeNamespace("MyStatus") { Imports = { new CodeNamespaceImport(myDerivedNs.Name) } };
            ccu.Namespaces.AddRange(new [] { mybaseNs, myDerivedNs, myStatusNs});            

            var schemas = new XmlSchemas();
            schemas.Add(mybaseSchema);
            schemas.Add(myderivedSchema);
            schemas.Add(mystatusSchema);
            schemas.Compile(ValidationEventHandler, true);

            CodeIdentifiers typeIdentifiers = new CodeIdentifiers(true);
            ImportContext context = new ImportContext(typeIdentifiers, shareTypes: true);
            CodeGenerationOptions options = CodeGenerationOptions.GenerateOrder;
            var providerOptions = new Dictionary<string, string>();
            var provider = new CSharpCodeProvider(providerOptions);
            XmlSchemaImporter importer = new XmlSchemaImporter(schemas, options, provider, context); // for xml schemas
            Hashtable mappings = new Hashtable();

            MapTypes(mybaseSchema, importer, mybaseNs, ccu, provider, options, mappings);
            MapTypes(myderivedSchema, importer, myDerivedNs, ccu, provider, options, mappings);
            MapTypes(mystatusSchema, importer, myStatusNs, ccu, provider, options, mappings);

            //XmlCodeExporter exporter = new XmlCodeExporter(myStatusNs, ccu, CodeGenerationOptions.GenerateOrder);
            //foreach (XmlSchemaElement element in mystatusSchema.Elements.Values)
            //{
            //    XmlTypeMapping mapping = importer.ImportTypeMapping(element.QualifiedName);
            //    exporter.ExportTypeMapping(mapping);
            //}

            using (StreamWriter writer = File.CreateText("status.cs"))
            {
                globalNs.Comments.Clear();
                provider.GenerateCodeFromCompileUnit(ccu, writer, new CodeGeneratorOptions { BracingStyle = "C", BlankLinesBetweenMembers = false });
            }
            Console.WriteLine("Press enter");
            Console.ReadLine();
        }

        static void MapTypes(XmlSchema schema, XmlSchemaImporter importer, CodeNamespace cns, CodeCompileUnit ccu, CodeDomProvider codeProvider, CodeGenerationOptions options, Hashtable mappings)
        {
            XmlCodeExporter exporter = new XmlCodeExporter(cns, ccu, codeProvider, options, mappings);
            foreach (XmlSchemaElement element in schema.Elements.Values)
            {
                XmlTypeMapping mapping = importer.ImportTypeMapping(element.QualifiedName);
                exporter.ExportTypeMapping(mapping);
            }

        }

        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            Console.WriteLine($"{e.Severity} {e.Message}");
        }
    }
}
