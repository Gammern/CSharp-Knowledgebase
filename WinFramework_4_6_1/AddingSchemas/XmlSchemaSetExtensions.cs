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
    public static class XmlSchemaSetExtensions
    {
        public static void AddFilesFromDirectory(this XmlSchemaSet @this, string path, ValidationEventHandler validationEventHandler)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            XmlReaderSettings settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema,
                ValidationFlags = XmlSchemaValidationFlags.ProcessInlineSchema,
                DtdProcessing = DtdProcessing.Parse,
                NameTable = @this.NameTable
            };

            // For some reason unknow, need to add this file first
            using (var reader = XmlReader.Create(@"C:\Users\johan\Documents\ubl\UBL-2.1\xsd\common\UBL-xmldsig-core-schema-2.1.xsd", settings))
            {
                Console.WriteLine($"Processing: {Path.GetFileName(reader.BaseURI)} (PRE-LOAD)");
                XmlSchema schema = XmlSchema.Read(reader, validationEventHandler);
                @this.Add(schema);
                //@this.Compile();
            }

            foreach (var xsdFile in dirInfo.GetFiles("*.xsd"))
            {
                using (var reader = XmlReader.Create(xsdFile.FullName, settings))
                {
                    Console.WriteLine($"Processing: {xsdFile.Name}");
                    // Message = "For security reasons DTD is prohibited in this XML document. 
                    // To enable DTD processing set the DtdProcessing property on XmlReaderSettings to Parse and pass the settings into XmlReader.Create method."
                    XmlSchema schema = XmlSchema.Read(reader, validationEventHandler);
                    @this.Add(schema);
                }
                //Console.WriteLine(xsdFile.Name);
            }
        }
    }
}
