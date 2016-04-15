using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                ValidationFlags = XmlSchemaValidationFlags.ProcessInlineSchema,
                DtdProcessing = DtdProcessing.Parse,
                NameTable = @this.NameTable,
                XmlResolver = new XmlPreloadedResolver(new XmlUrlResolver())
            };


            // For some reason unknow, need to add/load this file first
            // Can I somhow pre-parse the files and find out the correct load order? 
            @this.XsdAdd(Path.Combine(commonPath, "UBL-xmldsig-core-schema-2.1.xsd"), settings, validationEventHandler);
            @this.XsdAdd(Path.Combine(commonPath, "UBL-CommonAggregateComponents-2.1.xsd"), settings, validationEventHandler);
            @this.XsdAdd(Path.Combine(commonPath, "UBL-CommonExtensionComponents-2.1.xsd"), settings, validationEventHandler);
            //@this.XsdAdd(Path.Combine(commonPath, "UBL-ExtensionContentDataType-2.1.xsd"), settings, validationEventHandler);
            //@this.XsdAdd(Path.Combine(commonPath, "UBL-CoreComponentParameters-2.1.xsd"), settings, validationEventHandler);


            DirectoryInfo dirInfo = new DirectoryInfo(maindocPath);
            foreach (var xsdFile in dirInfo.GetFiles("*.xsd"))
            {
                @this.XsdAdd( xsdFile.FullName, settings, validationEventHandler);
            }

            var commonFiles = Directory.GetFiles(commonPath, "*.xsd").Select(f => Path.GetFileName(f)).ToList();
            var addedFiles = @this.Schemas().Cast<XmlSchema>().Select(s => Path.GetFileName(s.SourceUri)).ToList();
            var missedFiles = commonFiles.Except(addedFiles).ToList();
            missedFiles.ForEach(f => Console.WriteLine($"Not included: {f}"));
        }

        private static void XsdAdd(this XmlSchemaSet @this, string filename, XmlReaderSettings settings, ValidationEventHandler validationEventHandler)
        {
            using (var reader = XmlReader.Create(filename, settings))
            {
                XmlSchema schema = XmlSchema.Read(reader, validationEventHandler);
                @this.Add(schema);
            }
        }
    }
}
