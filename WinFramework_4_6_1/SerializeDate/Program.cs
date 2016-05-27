using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SerializeDate
{
    public class Timetype
    {

        [XmlElement(DataType = "time")]
        public DateTime Value { get; set; }

        public string Kind { get { return Value.Kind.ToString(); } set { } }

        [XmlNamespaceDeclarations]
        XmlSerializerNamespaces Xmlns = new XmlSerializerNamespaces( new[] { new XmlQualifiedName("my", "http://my") } );
    }


    class Program
    {
        static XmlSerializer ser = new XmlSerializer(typeof(Timetype));

        static void Ser(DateTimeKind? kind)
        {
            Timetype tt = new Timetype { Value = DateTime.Parse("12:01.02") };
            if (kind.HasValue)
                tt.Value = DateTime.SpecifyKind(tt.Value, kind.Value);
            ser.Serialize(Console.Out, tt );
            Console.WriteLine();
        }

        static void ParseTime(string timeString)
        {
            string fmt = "HH':'mm':'ss.fffffffK";
            DateTime t;
            if(DateTime.TryParse(timeString, out t))
            {
                if (timeString.EndsWith("Z", StringComparison.InvariantCultureIgnoreCase))
                {
                    //t = DateTime.SpecifyKind(t, DateTimeKind.Utc);
                    t = t.ToUniversalTime();
                }
                Console.WriteLine($"{timeString+t.Kind,-25} -> {t.ToString(fmt)}");
            }
            else
            {
                Console.WriteLine("Error: " + timeString);
            }
        }
        static void Main(string[] args)
        {
            //Ser(null);
            //Ser(DateTimeKind.Unspecified);
            //Ser(DateTimeKind.Utc);
            //Ser(DateTimeKind.Local);
            ParseTime("12:31.00Z");
            ParseTime("12:31:00.0+02:00");
            ParseTime("12:31.00");
            ParseTime("12:31:00Z");
            ParseTime("14:31:00.0000000+02:00");

        }
    }
}
