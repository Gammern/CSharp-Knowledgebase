using System.Collections.Generic;
using System.Xml.Schema;

namespace AddingSchemas
{
    public class XmlSchemaElementComparer : IEqualityComparer<XmlSchemaElement>
    {
        public bool Equals(XmlSchemaElement x, XmlSchemaElement y)
        {
            if(x.QualifiedName == y.QualifiedName)
            {
                return true;
            }
            return false;
        }

        public int GetHashCode(XmlSchemaElement obj)
        {
            return obj.QualifiedName.GetHashCode();
        }
    }
}
