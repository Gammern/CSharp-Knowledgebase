using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace ReadAndTraverseXsd
{
    class Program
    {
        static void Main(string[] args)
        {
            var schemaSet = new XmlSchemaSet();
            schemaSet.ValidationEventHandler += SchemaSet_ValidationEventHandler;
            schemaSet.Add("http://www.tempuri.org", "customer.xsd");
            schemaSet.Compile();

            XmlSchema customerSchema = null;
            foreach (XmlSchema schema in schemaSet.Schemas())
            {
                customerSchema = schema;
            }

            foreach (XmlSchemaElement element in customerSchema.Elements.Values)
            {
                Console.WriteLine($"Element: {element.Name} {element.SchemaTypeName}");
                XmlSchemaComplexType complexType = element.ElementSchemaType as XmlSchemaComplexType;
                foreach (XmlSchemaAttribute attribute in complexType.AttributeUses.Values.Cast<XmlSchemaAttribute>())
                {
                    Console.WriteLine($"Attrinute: {attribute.Name}  {attribute.SchemaTypeName}");
                }
                var sequence = complexType.ContentTypeParticle as XmlSchemaSequence;
                foreach (XmlSchemaElement childElement in sequence.Items)
                {
                    Console.WriteLine($"Element: {childElement.Name} {childElement.SchemaTypeName}");
                }
            }

            foreach (var item in customerSchema.Items.OfType<XmlSchemaType>())
            {
                Console.WriteLine($"Item: {item.QualifiedName} {item.BaseXmlSchemaType}");
            }
        }

        private static void SchemaSet_ValidationEventHandler(object sender, ValidationEventArgs args)
        {
            Console.WriteLine($"{args.Severity.ToString()}: {args.Message}");
        }
    }
}
