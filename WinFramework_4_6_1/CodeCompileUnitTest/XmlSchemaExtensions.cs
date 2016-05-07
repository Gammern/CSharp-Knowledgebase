using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace CodeCompileUnitTest
{
    public static class XmlSchemaExtensions
    {
        public static XmlSchema Create(string filename)
        {
            using (var fs = File.OpenRead(filename))
            {
                return XmlSchema.Read(fs, null);
            }
        }

        
    }
}
