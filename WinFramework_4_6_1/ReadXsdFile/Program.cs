using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace ReadXsdFile
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlSchema mySchema;
            XmlReaderSettings settings = new XmlReaderSettings
            {
                ConformanceLevel = ConformanceLevel.Document,
                NameTable = new NameTable(),
            };
            try
            {
                using (var reader = new StreamReader("example.xsd"))
                {
                    mySchema = XmlSchema.Read(reader, validationHandler);
                }
                using (var writer = new StreamWriter("copy.xsd"))
                {
                    mySchema.Write(Console.Out);
                    mySchema.Write(writer);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void validationHandler(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                Console.Write("WARNING: ");
            else if (args.Severity == XmlSeverityType.Error)
                Console.Write("ERROR: ");

            Console.WriteLine(args.Message);
        }
    }
}
