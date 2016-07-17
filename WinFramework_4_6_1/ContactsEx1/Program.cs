using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ContactsEx1
{
    class Program
    {
        static void Main(string[] args)
        {
            var xmlSchemas = new XmlSchemas();
            xmlSchemas.Add(XmlSchema.Read(File.OpenRead("contacts.xsd"), validationHandler));
            xmlSchemas.Compile(validationHandler, true);
            XmlSchemaImporter schemaImporter = new XmlSchemaImporter(xmlSchemas);

            CodeNamespace codeNamespace = new CodeNamespace("CSharpGenerated");
            XmlCodeExporter codeExporter = new XmlCodeExporter(codeNamespace, null, CodeGenerationOptions.None);

            foreach (XmlSchema xsd in xmlSchemas)
            {
                foreach (XmlSchemaElement schemaElement in xsd.Elements.Values)
                {
                    Console.WriteLine($"{schemaElement.Name} is {schemaElement.GetType().Name}");
                    var t = schemaImporter.ImportTypeMapping(schemaElement.QualifiedName);
                    codeExporter.ExportTypeMapping(t);
                }
                foreach (XmlSchemaType schemaType in xsd.SchemaTypes.Values) // XmlSchemaComplexType or XmlSchemaSimpleType
                {
                    var t = schemaImporter.ImportSchemaType(schemaType.QualifiedName);
                    Console.WriteLine($"{schemaType.Name} is a {schemaType.GetType().Name}");
                    // Generates far to many xmlroot attributes
                    //codeExporter.ExportTypeMapping(schemaImporter.ImportSchemaType(schemaType.QualifiedName));
                }
            }

            RemoveAttributes(codeNamespace);
            CodeGenerator.ValidateIdentifiers(codeNamespace);
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            CodeGeneratorOptions opts = new CodeGeneratorOptions
            {
                BlankLinesBetweenMembers = false,
                VerbatimOrder = true
            };
            codeProvider.GenerateCodeFromNamespace(codeNamespace, Console.Out, opts);

            foreach (CodeTypeDeclaration codeType in codeNamespace.Types)
            {
                Console.WriteLine($"{codeType.Name} is a {codeType.GetType().Name}");
                var t = codeType.TypeParameters;

            }

            Console.ReadLine();


        }

        private static void validationHandler(object sender, ValidationEventArgs e)
        {
            Console.WriteLine($"{e.Severity}: {e.Message}");
        }

        // Remove all the attributes from each type in the CodeNamespace, except
        // System.Xml.Serialization.XmlTypeAttribute
        private static void RemoveAttributes(CodeNamespace codeNamespace)
        {
            foreach (CodeTypeDeclaration codeType in codeNamespace.Types)
            {
                codeType.Comments.Clear();
                foreach (CodeTypeMember member in codeType.Members)
                {
                    member.Comments.Clear();
                }
                foreach (CodeAttributeDeclaration codeAttribute in codeType.CustomAttributes.OfType<CodeAttributeDeclaration>().ToList())
                {
                    if (!codeAttribute.Name.StartsWith("System.Xml.Serialization."))
                    {
                        codeType.CustomAttributes.Remove(codeAttribute);
                    }
                }
            }
        }
    }
}
