using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace BuildXsd
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlSchema customerSchema = CreateCustomerSchema();
            XmlSchemaSet schemaSet = new XmlSchemaSet();
            schemaSet.ValidationEventHandler += SchemaSet_ValidationEventHandler;
            schemaSet.Add(customerSchema);
            schemaSet.Compile();

            foreach (XmlSchema schema in schemaSet.Schemas())
            {
                customerSchema = schema;
            }

            customerSchema.Write(Console.Out);
            using (var writer = XmlWriter.Create("customer.xsd", new XmlWriterSettings { Indent = true }))
            {
                customerSchema.Write(writer);
            }
        }

        private static XmlSchema CreateCustomerSchema()
        {
            string targetNamespace = "http://www.tempuri.org";
            
            XmlSchema custSchema = new XmlSchema
            {
                TargetNamespace = targetNamespace,
                Items =
                {
                    new XmlSchemaElement
                    {
                        Name = "Customer",
                        SchemaType = new XmlSchemaComplexType
                        {
                            Attributes =
                            {
                                new XmlSchemaAttribute
                                {
                                    Name = "CustomerId",
                                    Use = XmlSchemaUse.Required,
                                    SchemaTypeName = XsdConstants.QnPositiveInt,
                                },
                            },
                            Particle = new XmlSchemaSequence()
                            {
                                Items =
                                {
                                    new XmlSchemaElement
                                    {
                                        Name = "FirstName",
                                        SchemaTypeName = XsdConstants.QnString
                                    },
                                    new XmlSchemaElement
                                    {
                                        Name = "LastName",
                                        SchemaTypeName = new XmlQualifiedName("LastNameType", targetNamespace),
                                    }
                                }
                            }
                        }
                    },
                    new XmlSchemaSimpleType
                    {
                        Name = "LastNameType",
                        Content = new XmlSchemaSimpleTypeRestriction
                        {
                            BaseTypeName = XsdConstants.QnString,
                            Facets =
                            {
                                new XmlSchemaMaxLengthFacet
                                {
                                    Value = "20"
                                }
                            }
                        }
                    },
                    new XmlSchemaSimpleType
                    {
                        Name = "CarType",
                        Content = new XmlSchemaSimpleTypeRestriction
                        {
                            BaseTypeName = XsdConstants.QnString,
                            Facets =
                            {
                                new XmlSchemaEnumerationFacet { Value = "Audi" },
                                new XmlSchemaEnumerationFacet { Value = "Golf" },
                                new XmlSchemaEnumerationFacet { Value = "BMW" },
                            }
                        }
                    }
                }
            };
            return custSchema;
        }

        private static XmlSchema CreateCustomerSchema2()
        {
            var firstNameElement = new XmlSchemaElement();
            firstNameElement.Name = "FirstName";
            var lastNameElement = new XmlSchemaElement();
            lastNameElement.Name = "LastName";
            var idAttribute = new XmlSchemaAttribute();
            idAttribute.Name = "CustomerId";
            idAttribute.Use = XmlSchemaUse.Required;

            // Create the simple type for LastName element
            XmlSchemaSimpleType lastNameType = new XmlSchemaSimpleType();
            lastNameType.Name = "LastNameType";
            var lastNameRestriction = new XmlSchemaSimpleTypeRestriction();
            lastNameRestriction.BaseTypeName = new XmlQualifiedName("string", XmlSchema.Namespace);
            var maxLength = new XmlSchemaMaxLengthFacet();
            maxLength.Value = "20";
            lastNameRestriction.Facets.Add(maxLength);
            lastNameType.Content = lastNameRestriction;

            firstNameElement.SchemaTypeName = new XmlQualifiedName("string", XmlSchema.Namespace);
            // user defined
            lastNameElement.SchemaTypeName = new XmlQualifiedName("LastNameType", "http://www.tempuri.org");
            idAttribute.SchemaTypeName = new XmlQualifiedName("positiveInteger", XmlSchema.Namespace);

            // customer top level element
            XmlSchemaElement customerElement = new XmlSchemaElement { Name = "Customer" };
            var customerType = new XmlSchemaComplexType();
            var sequence = new XmlSchemaSequence();
            sequence.Items.Add(firstNameElement);
            sequence.Items.Add(lastNameElement);
            customerType.Particle = sequence;
            customerType.Attributes.Add(idAttribute);

            customerElement.SchemaType = customerType;

            XmlSchema customerSchema = new XmlSchema();
            customerSchema.TargetNamespace = "http://www.tempuri.org";
            customerSchema.Items.Add(customerElement);
            customerSchema.Items.Add(lastNameType);

            return customerSchema;
        }

        private static void SchemaSet_ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            Console.WriteLine($"{e.Severity}: {e.Message}");
        }
    }
}
