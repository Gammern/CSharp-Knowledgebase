using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Resolvers;
using System.Xml.Schema;

namespace AddingSchemas
{
    public static class XmlSchemaSetExtensions
    {
        public static void AddFilesFromDirectory(this XmlSchemaSet @this, string ublBasePath, ValidationEventHandler validationEventHandler)
        {
            string maindocPath = Path.Combine(ublBasePath, "maindoc");
            string commonPath = Path.Combine(ublBasePath, "common");
            XmlReaderSettings settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema,
                DtdProcessing = DtdProcessing.Parse, // will crash woithout this
                NameTable = @this.NameTable,
            };


            // For some reason unknow, need to add/load this file first
            @this.XsdAdd(Path.Combine(commonPath, "UBL-xmldsig-core-schema-2.1.xsd"), settings, validationEventHandler); // Must be preloaded!

            DirectoryInfo dirInfo = new DirectoryInfo(maindocPath);
            foreach (var xsdFile in dirInfo.GetFiles("*.xsd"))
            {
                @this.XsdAdd( xsdFile.FullName, settings, validationEventHandler);
            }

        }

        private static void XsdAdd(this XmlSchemaSet @this, string filename, XmlReaderSettings settings, ValidationEventHandler validationEventHandler)
        {
            using (var reader = XmlReader.Create(filename, settings))
            {
                XmlSchema schema = XmlSchema.Read(reader, validationEventHandler);
                var ret = @this.Add(schema);
                if (ret == null)
                {
                    Console.WriteLine($"Error adding schema {filename}");
                }
            }
        }

        public static List<string> GetFilesNotLoaded(this XmlSchemaSet @this, string ublBasePath)
        {
            string maindocPath = Path.Combine(ublBasePath, "maindoc");
            string commonPath = Path.Combine(ublBasePath, "common");
            var xsdFiles = Directory.GetFiles(commonPath, "*.xsd").Union(Directory.GetFiles(maindocPath, "*.xsd")).Select(f => Path.GetFileName(f)).ToList();
            var addedFiles = @this.Schemas().Cast<XmlSchema>().Select(s => Path.GetFileName(s.SourceUri)).ToList();
            return xsdFiles.Except(addedFiles).ToList();
        }

        // use to build XmlSerializerNamespaces for the serializer
        public static XmlQualifiedName[] GetNamespacePrefixes(this XmlSchemaSet @this)
        {
            List<XmlQualifiedName> nsList = new List<XmlQualifiedName>();
            foreach (XmlSchema schema in @this.Schemas())
            {
                nsList.AddRange(schema.Namespaces.ToArray().Where(ns => !string.IsNullOrEmpty(ns.Name)));
            }
            return nsList.Distinct().ToArray();
        }

        public static ICollection<XmlSchema> MaindocSchemas(this XmlSchemaSet @this)
        {
            return @this.Schemas().Cast<XmlSchema>().Where(s => s.SourceUri.Contains("maindoc") && s.TargetNamespace.StartsWith("urn:oasis:names:specification:ubl:schema:xsd:")).ToList();
        }
    }
}
