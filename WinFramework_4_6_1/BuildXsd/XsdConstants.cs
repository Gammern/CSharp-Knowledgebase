using System.Xml;
using System.Xml.Schema;

namespace BuildXsd
{
    internal static class XsdConstants
    {
        public static XmlQualifiedName QnString = new XmlQualifiedName("string", XmlSchema.Namespace);
        public static XmlQualifiedName QnShort = new XmlQualifiedName("short", XmlSchema.Namespace);
        public static XmlQualifiedName QnInt = new XmlQualifiedName("int", XmlSchema.Namespace);
        public static XmlQualifiedName QnLong = new XmlQualifiedName("long", XmlSchema.Namespace);
        public static XmlQualifiedName QnBoolean = new XmlQualifiedName("boolean", XmlSchema.Namespace);
        public static XmlQualifiedName QnUnsignedByte = new XmlQualifiedName("unsignedByte", XmlSchema.Namespace);
        public static XmlQualifiedName QnChar = new XmlQualifiedName("char", XmlSchema.Namespace);
        public static XmlQualifiedName QnDateTime = new XmlQualifiedName("dateTime", XmlSchema.Namespace);
        public static XmlQualifiedName QnDecimal = new XmlQualifiedName("decimal", XmlSchema.Namespace);
        public static XmlQualifiedName QnDouble = new XmlQualifiedName("double", XmlSchema.Namespace);
        public static XmlQualifiedName QnSbyte = new XmlQualifiedName("byte", XmlSchema.Namespace);
        public static XmlQualifiedName QnFloat = new XmlQualifiedName("float", XmlSchema.Namespace);
        public static XmlQualifiedName QnDuration = new XmlQualifiedName("duration", XmlSchema.Namespace);
        public static XmlQualifiedName QnUsignedShort = new XmlQualifiedName("usignedShort", XmlSchema.Namespace);
        public static XmlQualifiedName QnUnsignedInt = new XmlQualifiedName("unsignedInt", XmlSchema.Namespace);
        public static XmlQualifiedName QnUnsignedLong = new XmlQualifiedName("unsignedLong", XmlSchema.Namespace);
        public static XmlQualifiedName QnPositiveInt = new XmlQualifiedName("positiveInteger", XmlSchema.Namespace);
        public static XmlQualifiedName QnUri = new XmlQualifiedName("anyURI", XmlSchema.Namespace);
        public static XmlQualifiedName QnBase64Binary = new XmlQualifiedName("base64Binary", XmlSchema.Namespace);
        public static XmlQualifiedName QnXmlQualifiedName = new XmlQualifiedName("QName", XmlSchema.Namespace);
    }
}
