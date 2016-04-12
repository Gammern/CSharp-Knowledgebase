using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace EditXsd
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlSchemaSet schemaSet = new XmlSchemaSet();
            schemaSet.ValidationEventHandler += SchemaSet_ValidationEventHandler;
            schemaSet.Add("http://www.tempuri.org", "customer.xsd");
            schemaSet.Compile();

            XmlSchema customerSchema = null;
            foreach (XmlSchema schema in schemaSet.Schemas())
            {
                customerSchema = schema;
            }

            // create a new phonenumber element
            XmlSchemaElement phoneElement = new XmlSchemaElement();
            phoneElement.Name = "PhoneNumber";

            // xs:string simpletype
            XmlSchemaSimpleType phoneType = new XmlSchemaSimpleType();
            XmlSchemaSimpleTypeRestriction restriction = new XmlSchemaSimpleTypeRestriction();
            restriction.BaseTypeName = new XmlQualifiedName("string", XmlSchema.Namespace);

            // add a pattern facet to the restriction
            XmlSchemaPatternFacet phonePattern = new XmlSchemaPatternFacet();
            phonePattern.Value = "\\d{3}-\\d{3}-\\d{4}";
            restriction.Facets.Add(phonePattern);

            phoneType.Content = restriction;
            phoneElement.SchemaType = phoneType;

            foreach (XmlSchemaElement element in customerSchema.Elements.Values)
            {
                if (element.QualifiedName.Name.Equals("Customer"))
                {
                    XmlSchemaComplexType customerType = element.ElementSchemaType as XmlSchemaComplexType;
                    XmlSchemaSequence sequence = customerType.Particle as XmlSchemaSequence;
                    sequence.Items.Add(phoneElement);
                }
            }

            schemaSet.Reprocess(customerSchema);
            schemaSet.Compile();
            using (var writer = XmlWriter.Create("newcustomer.xsd", new XmlWriterSettings { Indent = true }))
            {
                customerSchema.Write(writer);
            }
        }

        private static void SchemaSet_ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            Console.WriteLine($"{e.Severity.ToString()}: {e.Message}");
        }
    }
}
