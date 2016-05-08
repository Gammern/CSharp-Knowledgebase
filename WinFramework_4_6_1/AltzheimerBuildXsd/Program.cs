using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace AltzheimerBuildXsd
{
    /// <summary>
    /// In this project I will build mybase.xsd, myderived.xsd and mystatus.xsd in code just bacause previous project failed so dramatically
    /// Lets se how long this knowledge stays before it is gone to the land of Altzheimer
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            BuildMyBase();
            BuildMyDerived();
            BuildMyStatus();
        }

        //<xsd:schema id = "mystatus"
        //    targetNamespace="http://tempuri.org/mystatus.xsd"
        //    elementFormDefault="qualified"
        //    xmlns="http://tempuri.org/mystatus.xsd"
        //    xmlns:cbc="http://tempuri.org/myderived.xsd"
        //    xmlns:xsd="http://www.w3.org/2001/XMLSchema">
        //  <xsd:import namespace="http://tempuri.org/myderived.xsd" schemaLocation="myderived.xsd"/>
        //  <xsd:element name = "Status" type="StatusType"/>
        //  <xsd:complexType name = "StatusType" >
        //    < xsd:sequence>
        //      <xsd:element ref="cbc:Text" minOccurs="0" maxOccurs="1">
        //      </xsd:element>
        //    </xsd:sequence>
        //  </xsd:complexType>
        //</xsd:schema>

        private static void BuildMyStatus()
        {
            string filename = @"..\..\mystatus.xsd";
            var myderivedXsd = new XmlSchema
            {
                Id = "mystatus",
                TargetNamespace = "http://tempuri.org/mystatus.xsd",
                ElementFormDefault = XmlSchemaForm.Qualified,
                Includes =
                {
                    new XmlSchemaImport { Namespace = "http://tempuri.org/myderived.xsd", SchemaLocation="myderived.xsd" },
                },
                Items =
                {
                    new XmlSchemaElement
                    {
                        Name = "Status",
                        SchemaTypeName = new XmlQualifiedName("StatusType")
                    },
                    new XmlSchemaComplexType
                    {
                        Name = "StatusType",
                        Particle = new XmlSchemaSequence
                        {
                            Items =
                            {
                                new XmlSchemaElement
                                {
                                    RefName = new XmlQualifiedName("Text", "http://tempuri.org/myderived.xsd"),
                                    MinOccurs = 0,
                                    MaxOccurs = 1
                                }
                            }
                        }
                    }
                }
            };
            myderivedXsd.Namespaces.Add("", "http://tempuri.org/mystatus.xsd");
            myderivedXsd.Namespaces.Add("xsd", XmlSchema.Namespace);
            myderivedXsd.Namespaces.Add("cbc", "http://tempuri.org/myderived.xsd");
            using (var stream = File.CreateText(filename))
            {
                myderivedXsd.Write(stream);
            }

        }

        //<xsd:schema id = "myderived"
        //    targetNamespace="http://tempuri.org/myderived.xsd"
        //    elementFormDefault="qualified"
        //    xmlns="http://tempuri.org/myderived.xsd"
        //    xmlns:udt="http://tempuri.org/mybase.xsd"
        //    xmlns:xsd="http://www.w3.org/2001/XMLSchema">
        //  <xsd:import namespace="http://tempuri.org/mybase.xsd" schemaLocation="mybase.xsd"/>
        //  <xsd:element name = "Text" type="TextType"/>
        //  <xsd:complexType name = "TextType" >
        //    < xsd:simpleContent>
        //      <xsd:extension base="udt:TextType"/>
        //    </xsd:simpleContent>
        //  </xsd:complexType>
        //</xsd:schema>
        private static void BuildMyDerived()
        {
            string filename = @"..\..\myderived.xsd";
            var myderivedXsd = new XmlSchema
            {
                Id = "myderived",
                TargetNamespace = "http://tempuri.org/myderived.xsd",
                ElementFormDefault = XmlSchemaForm.Qualified,
                Includes =
                {
                    new XmlSchemaImport { Namespace = "http://tempuri.org/mybase.xsd", SchemaLocation="mybase.xsd" },
                },
                Items =
                {
                    new XmlSchemaElement
                    {
                        Name = "Text",
                        SchemaTypeName = new XmlQualifiedName("TextType") // This is the clue!!! Get it from dictionary!
                    },
                    new XmlSchemaComplexType // can be removed when above is changed
                    {
                        Name = "TextType", // this should go as key in dictionary
                        ContentModel = new XmlSchemaSimpleContent
                        {
                            Content = new XmlSchemaSimpleContentExtension
                            {
                                BaseTypeName = new XmlQualifiedName("TextType", "http://tempuri.org/mybase.xsd") // this should go as value in dictionary
                            }
                        }
                    }
                }
            };
            myderivedXsd.Namespaces.Add("", "http://tempuri.org/myderived.xsd");
            myderivedXsd.Namespaces.Add("xsd", XmlSchema.Namespace);
            myderivedXsd.Namespaces.Add("udt", "http://tempuri.org/mybase.xsd");
            using (var stream = File.CreateText(filename))
            {
                myderivedXsd.Write(stream);
            }

        }

        //<xsd:complexType name = "TextType" >
        //  < xsd:simpleContent>
        //    <xsd:extension base="xsd:string">
        //      <xsd:attribute name = "languageID" type="xsd:language" use="optional">
        //      </xsd:attribute>
        //    </xsd:extension>
        //  </xsd:simpleContent>
        //</xsd:complexType>
        private static void BuildMyBase()
        {
            string filename = @"../../mybase.xsd";
            XmlSchema mybasexsd = new XmlSchema
            {
                TargetNamespace = "http://tempuri.org/mybase.xsd",
                ElementFormDefault = XmlSchemaForm.Qualified,
                AttributeFormDefault = XmlSchemaForm.Unqualified,
                Id = "mybase",
                Items =
                {
                    new XmlSchemaComplexType
                    {
                        Name = "TextType",
                        // post compilation content model. Where is the pre compilation one??????
                        ContentModel = new XmlSchemaSimpleContent
                        {
                            Content = new XmlSchemaSimpleContentExtension
                            {
                                BaseTypeName = new XmlQualifiedName("string", XmlSchema.Namespace),
                                Attributes =
                                {
                                    new XmlSchemaAttribute
                                    {
                                        Name = "languageId",
                                        SchemaTypeName = new XmlQualifiedName("language", XmlSchema.Namespace),
                                        Use = XmlSchemaUse.Optional
                                    }
                                }
                            }
                        }
                    }
                }
            };
            mybasexsd.Namespaces.Add("", mybasexsd.TargetNamespace);
            mybasexsd.Namespaces.Add("xsd", XmlSchema.Namespace);
            using (var stream = File.CreateText(filename))
            {
                mybasexsd.Write(stream);
            }
        }
    }
}
