using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace GenerateCSharpCodeFromXsd
{
    public sealed class Processor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xsdFilename"></param>
        /// <param name="targetNamespace"></param>
        /// <returns></returns>
        public static CodeNamespace Process(string xsdFilename, string targetNamespace)
        {
            XmlReaderSettings settings = new XmlReaderSettings
            {
                
            };
            XmlSchema xsd;
            using (var reader = XmlReader.Create(xsdFilename))
            {
                xsd = XmlSchema.Read(reader, MyValidationEventHandler);
            }

            XmlSchemas schemas = new XmlSchemas(); // Internal class

            int elementCount = xsd.Elements.Count; // 0 Elements is a post-schema-compilation property
            int indexOf = schemas.Add(xsd); // Method will only add (to end) if it does not exists, and return the one stored internally
            elementCount = xsd.Elements.Count; // 1 OOOPS! Looks like Add do some magic to added XmlSchema


            schemas.Compile(MyValidationEventHandler, true); // What is fullCompile?
            //var appinfos = xsd.Items.OfType<XmlSchemaAnnotation>().SelectMany(a => a.Items.OfType<XmlSchemaAppInfo>().SelectMany(m => m.Markup)).ToList();

            //foreach (var attr in xsd.UnhandledAttributes)
            //{
            //    Console.WriteLine("UnhandledAttribute: " + attr.LocalName);
            //}

            // Create the importer for these schemas.
            CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp"); // shared import & export
            CodeGenerationOptions options = CodeGenerationOptions.GenerateProperties; // shared import & export
            CodeIdentifiers typeIdentifiers = new CodeIdentifiers();
            ImportContext context = new ImportContext(typeIdentifiers, true); // true=share custom types amongst schemas

            XmlSchemaImporter importer = new XmlSchemaImporter(schemas, options, context);


            // System.CodeDom namespace for the XmlCodeExporter to put classes in.
            CodeNamespace ns = new CodeNamespace(targetNamespace);
            CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
            Hashtable mappings = new Hashtable();

            XmlCodeExporter exporter = new XmlCodeExporter(ns, codeCompileUnit, options, mappings);

            // Test identifier uniqueness
            string s = "FirstName";
            var ustr = typeIdentifiers.MakeUnique(s); // FirstName
            ustr = typeIdentifiers.MakeUnique(s); // FirstName
            typeIdentifiers.Add(s, s);
            ustr = typeIdentifiers.MakeUnique(s); // FirstName1
            typeIdentifiers.Remove(s);
            ustr = typeIdentifiers.MakeUnique(s); // FirstName
            typeIdentifiers.Add(s, s);


            // Iterate schema top-level elements and export code for each.
            foreach (XmlSchemaElement element in xsd.Elements.Values)
            {
                //var appinfos = element.Annotation.Items.OfType<XmlSchemaAppInfo>().ToArray();

                // Import the mapping first.
                var ss = typeIdentifiers.ToArray(typeof(string)); // 1
                XmlTypeMapping mapping = importer.ImportTypeMapping(element.QualifiedName);
                ss = typeIdentifiers.ToArray(typeof(string)); // 2


                // Export the code finally.
                int count = mappings.Count; // 0
                exporter.ExportTypeMapping(mapping);
                count = mappings.Count; // 5
            }

            foreach (var schemaType in xsd.SchemaTypes.Values.Cast<XmlSchemaType>())
            {
                var map2 = importer.ImportSchemaType(schemaType.QualifiedName);
                string s2 = map2.TypeFullName;
            }
            return ns;
        }

        private static void MyValidationEventHandler(object sender, ValidationEventArgs e)
        {
            Console.WriteLine($"{e.Severity.ToString()}: {e.Message}");
            if (e.Severity == XmlSeverityType.Error)
            {
                throw e.Exception;
            }
        }
    }
}