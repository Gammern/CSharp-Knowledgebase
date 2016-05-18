using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XsdTimeIssue
{
    class Program
    {
        static void Main(string[] args)
        {
            UtcDate ud = new UtcDate { EventTime = new DateTime(2009, 12, 24, 8, 30, 0, DateTimeKind.Utc) };
            string timeFmt = "HH':'mm':'ss.fffffffK";

            XmlSerializer xs = new XmlSerializer(typeof(UtcDate));
            xs.Serialize(Console.Out, ud);

            Console.WriteLine();
            DateTime d = DateTime.Parse("12:30:00Z");
            Console.WriteLine($"{d:u} {d.Kind} {d.ToString(timeFmt)}");
            d = DateTime.SpecifyKind(d, DateTimeKind.Utc);
            Console.WriteLine($"{d:u} {d.Kind} {d.ToString(timeFmt)}");
            d = DateTime.SpecifyKind(d, DateTimeKind.Unspecified);
            Console.WriteLine($"{d:u} {d.Kind} {d.ToString(timeFmt)}");

            Console.WriteLine(DateTimeFormatInfo.InvariantInfo.LongTimePattern + " " + d.ToString(DateTimeFormatInfo.InvariantInfo.LongTimePattern, CultureInfo.InvariantCulture));
            DateTimeFormatInfo invDTF = new DateTimeFormatInfo();
            String[] formats = invDTF.GetAllDateTimePatterns();

            Console.WriteLine("{0,-40} {1}\n", "Pattern", "Result String");
            foreach (var fmt in formats)
                Console.WriteLine("{0,-40} {1}", fmt, d.ToString(fmt));
        }
    }

    [XmlRootAttribute(Namespace = "http://mytempuri")]
    public class UtcDate
    {
        private DateTime eventTime;
        [XmlElement("EventTime", Namespace = "http://mytempuri")]
        public DateTime EventTime //: ISerializable
        {
            get { return eventTime; }
            set { eventTime = value; }
        }

        [XmlElement("EventTimedateTime", Namespace = "http://mytempuri", DataType = "dateTime")]
        public DateTime EventTime2
        {
            get { return eventTime; }
            set { }
        }
        [XmlElement("EventTimedate", Namespace = "http://mytempuri", DataType = "date")]
        public DateTime EventTime3
        {
            get { return eventTime; }
            set { }
        }
        [XmlElement("EventTimettime", Namespace = "http://mytempuri", DataType = "time")]
        public DateTime EventTime4
        {
            get { return eventTime; }
            set { }
        }

    }
}
