using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XmlConvert1
{
    class Program
    {
        static void Main(string[] args)
        {
            string orgStr = "øæåØÆÅ<>\"//\n§!#¤%&()=?`";
            TestConv(orgStr, XmlConvert.EncodeName);
            TestConv(orgStr, XmlConvert.EncodeLocalName);
            TestConv(orgStr, XmlConvert.EncodeNmToken);
        }

        private static void TestConv(string orgStr, Func<string,string> conv)
        {
            string convStr = conv(orgStr);
            Console.WriteLine($"{orgStr} -> {convStr}");
            Console.WriteLine($"And back to original: {XmlConvert.DecodeName(convStr)}");
        }
    }
}
