using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemaDependencies
{
    /*
     * Import add schema with different targetnamespace
     * include insert schema with same targetnamespace
     * 
     * maindoc/UBL-Invoice-2.1.xsd
     *   <xsd:import namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2" schemaLocation="../common/UBL-CommonAggregateComponents-2.1.xsd"/>
     *     <xsd:import namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2"   schemaLocation="UBL-CommonBasicComponents-2.1.xsd"/> (already imported)
     *   <xsd:import namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2"     schemaLocation="../common/UBL-CommonBasicComponents-2.1.xsd"/>
     *     <xsd:import namespace="urn:oasis:names:specification:ubl:schema:xsd:QualifiedDataTypes-2" schemaLocation="UBL-QualifiedDataTypes-2.1.xsd"/>
     *       <xsd:import namespace="urn:oasis:names:specification:ubl:schema:xsd:UnqualifiedDataTypes-2" schemaLocation="UBL-UnqualifiedDataTypes-2.1.xsd"/>
     *     <xsd:import namespace="urn:oasis:names:specification:ubl:schema:xsd:UnqualifiedDataTypes-2" schemaLocation="UBL-UnqualifiedDataTypes-2.1.xsd"/>
     *       <xsd:import schemaLocation="CCTS_CCT_SchemaModule-2.1.xsd" namespace="urn:un:unece:uncefact:data:specification:CoreComponentTypeSchemaModule:2"/>
     *   <xsd:import namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2" schemaLocation="../common/UBL-CommonExtensionComponents-2.1.xsd"/>
     *     <xsd:import namespace="urn:oasis:names:specification:ubl:schema:xsd:UnqualifiedDataTypes-2" schemaLocation="UBL-UnqualifiedDataTypes-2.1.xsd"/>
     *     <xsd:import namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2" schemaLocation="UBL-CommonBasicComponents-2.1.xsd"/>
  ****     <xsd:include schemaLocation="UBL-ExtensionContentDataType-2.1.xsd"/>
     *       <xsd:import namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonSignatureComponents-2" schemaLocation="UBL-CommonSignatureComponents-2.1.xsd"/>
     *         <xsd:import namespace="urn:oasis:names:specification:ubl:schema:xsd:SignatureAggregateComponents-2" schemaLocation="UBL-SignatureAggregateComponents-2.1.xsd"/>
     *            <xsd:import namespace="urn:oasis:names:specification:ubl:schema:xsd:SignatureBasicComponents-2" schemaLocation="UBL-SignatureBasicComponents-2.1.xsd"/>
     *               <xsd:import namespace="urn:oasis:names:specification:ubl:schema:xsd:QualifiedDataTypes-2" schemaLocation="UBL-QualifiedDataTypes-2.1.xsd"/>
     *               <xsd:import namespace="urn:oasis:names:specification:ubl:schema:xsd:UnqualifiedDataTypes-2" schemaLocation="UBL-UnqualifiedDataTypes-2.1.xsd"/>
     *            <xsd:import namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2" schemaLocation="UBL-CommonBasicComponents-2.1.xsd"/>
     *            <xsd:import namespace="http://www.w3.org/2000/09/xmldsig#" schemaLocation="UBL-xmldsig-core-schema-2.1.xsd"/> // MUST BE PRELOADED!!
     *            <xsd:import namespace="http://uri.etsi.org/01903/v1.3.2#" schemaLocation="UBL-XAdESv132-2.1.xsd"/>
     *              <xsd:import namespace="http://www.w3.org/2000/09/xmldsig#" schemaLocation="UBL-xmldsig-core-schema-2.1.xsd"/>
     *            <xsd:import namespace="http://uri.etsi.org/01903/v1.4.1#" schemaLocation="UBL-XAdESv141-2.1.xsd"/>
     *              <xsd:import namespace="http://uri.etsi.org/01903/v1.3.2#" schemaLocation="UBL-XAdESv132-2.1.xsd"/>
     */
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
