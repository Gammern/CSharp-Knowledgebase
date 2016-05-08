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

            ccu.ReferencedAssemblies.AddRange(new[] { "System", "System.Xml.Serialization" });
            var schemaSet = new XmlSchemaSet(); // reprocess and compile as much as we want
            schemaSet.Add(null, @"xsd/mystatus.xsd"); // will add the others
            schemaSet.Compile();

            var mybaseSchema = schemaSet.Schemas("http://tempuri.org/mybase.xsd").OfType<XmlSchema>().Single();
            var myderivedSchema = schemaSet.Schemas("http://tempuri.org/myderived.xsd").OfType<XmlSchema>().Single();
            var mystatusSchema = schemaSet.Schemas("http://tempuri.org/mystatus.xsd").OfType<XmlSchema>().Single();
            //ReplaceBaseWithBasesBase(myderivedSchema);
            schemaSet.Reprocess(myderivedSchema);
            schemaSet.Compile();

            var xmlSysImport = new CodeNamespaceImport("System");
            var xmlNsImport = new CodeNamespaceImport("System.Xml.Serialization");
            var mybaseNs = new CodeNamespace("MyBase");
            var myDerivedNs = new CodeNamespace("MyDerived") { Imports = { xmlSysImport, xmlNsImport, new CodeNamespaceImport(mybaseNs.Name) } };
            var myStatusNs = new CodeNamespace("MyStatus") { Imports = { xmlSysImport, xmlNsImport, new CodeNamespaceImport(myDerivedNs.Name) } };
            ccu.Namespaces.AddRange(new[] { mybaseNs, myDerivedNs, myStatusNs });

            var schemas = new XmlSchemas();
            foreach (XmlSchema schema in schemaSet.Schemas())
            {
                schemas.Add(schema);
            }
            //schemas.Compile(ValidationEventHandler, true); // can't change model after compile! XmlSchamaSet.Reprocess handle this!
            schemas.Compile(ValidationEventHandler, true);

            CodeIdentifiers typeIdentifiers = new CodeIdentifiers(true);
            ImportContext context = new ImportContext(typeIdentifiers, shareTypes: false);
            CodeGenerationOptions options = CodeGenerationOptions.GenerateOrder;
            var providerOptions = new Dictionary<string, string>();
            var provider = new CSharpCodeProvider(providerOptions);
            XmlSchemaImporter importer = new XmlSchemaImporter(schemas, options, provider, context); // for xml schemas
            importer.Extensions.Clear();
            //importer.Extensions.Add(new MyImporterExtension());
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

            using (StreamWriter writer = File.CreateText(@"..\..\..\TestGeneratedCode\status.cs"))
            {
                //globalNs.Comments.Clear();
                //globalNs.Name = "MyGlobal";
                provider.GenerateCodeFromCompileUnit(ccu, writer, new CodeGeneratorOptions { BracingStyle = "C", BlankLinesBetweenMembers = false });
            }
            Console.WriteLine("Press enter");
            Console.ReadLine();
        }

        private static void ReplaceBaseWithBasesBase(XmlSchema schema)
        {
            var dic = schema.Items.OfType<XmlSchemaComplexType>().ToDictionary(k => k.QualifiedName, v => (v.ContentModel.Content as XmlSchemaSimpleContentExtension).BaseTypeName);

            //Do not work. gets reverted if schmeas has been compiled!
            //List<XmlSchemaElement> elements = schema.Elements.Values.OfType<XmlSchemaElement>()
            //    .Select(s => new XmlSchemaElement
            //    {
            //        Name = s.Name,
            //        SchemaTypeName = dic[s.SchemaTypeName]
            //    })
            //    .ToList();

            //schema.Items.Clear();
            //elements.ForEach(e => schema.Items.Add(e));

            foreach (var element in schema.Elements.Values.OfType<XmlSchemaElement>())
            {
                element.SchemaTypeName = dic[element.SchemaTypeName];
            }
            foreach (var cp in schema.Items.OfType<XmlSchemaComplexType>().ToList())
            {
                schema.Items.Remove(cp);
            }
            // All broke here due to obvious brain problems after TIA. It is now a fact!
            // this do not work. revert from this investigation project to a new investigate project for the inverstigation that is an investigation of another project because of altzheimer
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
