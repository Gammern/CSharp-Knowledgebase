using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.Serialization.Advanced;

namespace CodeCompileUnitTest
{
    public class MyImporterExtension : SchemaImporterExtension
    {
        const string xmlBaseNs = "http://tempuri.org/mybase.xsd";
        const string xmlDerivedNs = "http://tempuri.org/myderived.xsd";
        const string xmlStatusNs = "http://tempuri.org/mystatus.xsd";

        /// <summary>
        ///     Importing: language      http://www.w3.org/2001/XMLSchema
        ///     Importing: TextType http://tempuri.org/myderived.xsd
        ///     Importing: StatusType http://tempuri.org/mystatus.xsd
        /// </summary>
        public override string ImportSchemaType(string name, string ns, XmlSchemaObject context, XmlSchemas schemas, XmlSchemaImporter importer, CodeCompileUnit compileUnit, CodeNamespace mainNamespace, CodeGenerationOptions options, CodeDomProvider codeProvider)
        {
            var ret = base.ImportSchemaType(name, ns, context, schemas, importer, compileUnit, mainNamespace, options, codeProvider);
            Console.WriteLine($"Importing: {name}\t {ns}");
            switch (ns)
            {
                case xmlBaseNs:
                    ret = $"MyBase.{name}";
                    break;
                case xmlDerivedNs:
                    // ret = $"MyDerived.{name}"; will move to the wrong CodeNamespace
                    break;
                case xmlStatusNs:
                    // ret = $"MyStatus.{name}"; break everything
                    break;
                default:
                    break;
            }
            return ret;
        }

        public override string ImportSchemaType(XmlSchemaType type, XmlSchemaObject context, XmlSchemas schemas, XmlSchemaImporter importer, CodeCompileUnit compileUnit, CodeNamespace mainNamespace, CodeGenerationOptions options, CodeDomProvider codeProvider)
        {
            var ret = base.ImportSchemaType(type, context, schemas, importer, compileUnit, mainNamespace, options, codeProvider);
            return ret;
        }

        public override string ImportAnyElement(XmlSchemaAny any, bool mixed, XmlSchemas schemas, XmlSchemaImporter importer, CodeCompileUnit compileUnit, CodeNamespace mainNamespace, CodeGenerationOptions options, CodeDomProvider codeProvider)
        {
            var ret = base.ImportAnyElement(any, mixed, schemas, importer, compileUnit, mainNamespace, options, codeProvider);
            return ret;
        }

        public override CodeExpression ImportDefaultValue(string value, string type)
        {
            var ret = base.ImportDefaultValue(value, type);
            return ret;
        }
    }
}
