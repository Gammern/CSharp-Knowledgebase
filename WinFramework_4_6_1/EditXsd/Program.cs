using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

// https://msdn.microsoft.com/en-us/library/1yce79bk.aspx

namespace EditXsd
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlSchema customerSchema = null;
            using (var reader = XmlReader.Create("customer.xsd")) 
            {
                customerSchema = XmlSchema.Read(reader, SchemaSet_ValidationEventHandler);
            }
            string customerTargetNamespace = customerSchema.TargetNamespace;

            XmlSchema addressSchema = null;
            using (var reader = XmlReader.Create("address.xsd"))
            {
                addressSchema = XmlSchema.Read(reader, SchemaSet_ValidationEventHandler);
            }
            string addressTargetNamespace = addressSchema.TargetNamespace;


            XmlSchemaSet schemaSet = new XmlSchemaSet();
            schemaSet.ValidationEventHandler += SchemaSet_ValidationEventHandler;
            schemaSet.Add(customerSchema);
            schemaSet.Add(addressSchema);
            schemaSet.Compile();

            foreach (XmlSchema schema in schemaSet.Schemas())
            {
                if (schema.TargetNamespace == customerTargetNamespace)
                {
                    customerSchema = schema; // now it points to a schema that has post compilation info
                }
                if (schema.TargetNamespace == addressTargetNamespace)
                {
                    addressSchema = schema;
                }
            }

            //// create a new phonenumber element
            XmlSchemaElement phoneElement = CreatePhoneElement();

            XmlQualifiedName customerElementName = new XmlQualifiedName("Customer", customerTargetNamespace);

            foreach (XmlSchemaElement element in customerSchema.Elements.Values)
            {
                if (element.QualifiedName == customerElementName)
                {
                    XmlSchemaComplexType customerType = element.ElementSchemaType as XmlSchemaComplexType;
                    XmlSchemaSequence sequence = customerType.Particle as XmlSchemaSequence; // XmlSchemaElement[]
                    sequence.Items.Add(phoneElement);

                    //var firstName = sequence.Items
                }
            }

            XmlQualifiedName firstnameElementName = new XmlQualifiedName("FirstName", customerTargetNamespace);
            var firstNameComplextType = CreateFirstNameComplexType();
            customerSchema.Items.Add(firstNameComplextType);

            var p = customerSchema.Items.OfType<XmlSchemaElement>().Where(e => e.QualifiedName == customerElementName)
                .SelectMany(e => ((e.ElementSchemaType as XmlSchemaComplexType).Particle as XmlSchemaSequence).Items.OfType<XmlSchemaElement>().Where(f => f.Name == "FirstName")).Single();
            p.SchemaTypeName = new XmlQualifiedName(firstNameComplextType.Name, customerTargetNamespace);

            /*foreach (XmlSchemaElement element in customerSchema.Items.OfType<XmlSchemaElement>().Where(e => e.QualifiedName == customerElementName))
            {
                if (element.QualifiedName == customerElementName)
                {
                    XmlSchemaComplexType customerType = element.ElementSchemaType as XmlSchemaComplexType;
                    XmlSchemaSequence sequence = customerType.Particle as XmlSchemaSequence;
                    
                    foreach (XmlSchemaElement particle in sequence.Items.OfType<XmlSchemaElement>())
                    {
                        if (particle.QualifiedName.Name == firstnameElementName.Name ) // bug
                        {
                            particle.SchemaTypeName = new XmlQualifiedName(firstNameComplextType.Name, targetNamespace);
                        }
                    }
                }
            }*/

            XmlSchemaImport import = new XmlSchemaImport
            {
                Namespace = addressTargetNamespace,
                Schema = addressSchema,
                SchemaLocation = "address.xsd",
                
            };
            customerSchema.Includes.Add(import);


            schemaSet.Reprocess(customerSchema);
            schemaSet.Compile();
            using (var writer = XmlWriter.Create("newcustomer.xsd", new XmlWriterSettings { Indent = true }))
            {
                customerSchema.Write(writer);
            }
        }

        /* Create the following:
         *
        <xs:element name="PhoneNumber">
          <xs:simpleType>
            <xs:restriction base="xs:string">
              <xs:pattern value="\d{3}-\d{3}-\d{4}" />
            </xs:restriction>
          </xs:simpleType>
        </xs:element>
        */
        private static XmlSchemaElement CreatePhoneElement()
        {
            return new XmlSchemaElement
            {
                Name = "PhoneNumber",
                SchemaType = new XmlSchemaSimpleType
                {
                    Content = new XmlSchemaSimpleTypeRestriction
                    {
                        BaseTypeName = new XmlQualifiedName("string", XmlSchema.Namespace),
                        Facets =
                        {
                            new XmlSchemaPatternFacet
                            {
                                Value = "\\d{3}-\\d{3}-\\d{4}"
                            }
                        }
                    }
                }
            };
        }

        /* Create the following:
         *   <xs:complexType name="FirstNameComplexType">
                <xs:simpleContent>
                  <xs:extension base="xs:string">
                    <xs:attribute name="Title" type="xs:string" />
                  </xs:extension>
                </xs:simpleContent>
              </xs:complexType>
  */
        private static XmlSchemaComplexType CreateFirstNameComplexType()
        {
            XmlSchemaComplexType fistnameComplexType = new XmlSchemaComplexType
            {
                Name = "FirstNameComplextType",
                ContentModel = new XmlSchemaSimpleContent
                {
                    Content = new XmlSchemaSimpleContentExtension
                    {
                        BaseTypeName = new XmlQualifiedName("string", XmlSchema.Namespace),
                        Attributes =
                        {
                            new XmlSchemaAttribute
                            {
                                Name = "Title",
                                SchemaTypeName = new XmlQualifiedName("string", XmlSchema.Namespace)
                            }
                        }
                    }
                }
            };
            return fistnameComplexType;
        }

        private static void SchemaSet_ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            Console.WriteLine($"{e.Severity.ToString()}: {e.Message}");
        }
    }
}
