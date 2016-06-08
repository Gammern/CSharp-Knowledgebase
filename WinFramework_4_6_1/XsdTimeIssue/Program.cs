using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace XsdTimeIssue
{
    public class Appointment
    {
        //[XmlElement("Time", DataType = "time")]
        [XmlIgnore]
        public DateTime Time { get; set; }

        [XmlElement("Time")]
        public string TimeAsXmlString
        {
            get { return XmlConvert.ToString(Time, XmlDateTimeSerializationMode.RoundtripKind).Split('T').Last(); }
            set { Time = XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.RoundtripKind); } // Prefixed with current date DateTime.Now
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            TestAppointment();
        }

        static XmlSerializer ser = new XmlSerializer(typeof(Appointment));

        private static void TestAppointment()
        {
            Console.WriteLine(DateTimeFormatInfo.InvariantInfo.SortableDateTimePattern); // lack timezone info
            Console.WriteLine(DateTimeFormatInfo.InvariantInfo.UniversalSortableDateTimePattern); // lack T for split
            TestTime(DateTimeKind.Local);
            TestTime(DateTimeKind.Unspecified);
            TestTime(DateTimeKind.Utc);
        }

        static XmlWriterSettings settings = new XmlWriterSettings
        {
            Indent = true,
            OmitXmlDeclaration = true,
        };

        static void TestTime(DateTimeKind kind)
        {
            DateTime d = DateTime.SpecifyKind(DateTime.Parse("09:15:33.002"), kind);
            Appointment a1 = new Appointment { Time = d };
            using (var writer =  XmlWriter.Create(Console.Out, settings))
            {
                ser.Serialize(writer, a1);
            }
            Console.WriteLine();
            try
            {
                using (var stream = new MemoryStream())
                {
                    ser.Serialize(stream, a1);
                    stream.Position = 0;
                    Appointment a2 = (Appointment)ser.Deserialize(stream);
                    if (a1.Time.TimeOfDay != a2.Time.TimeOfDay || a1.Time.Kind != a2.Time.Kind)
                    {
                        throw new ApplicationException("Time differs after serialization");
                    }
                }
            }
            catch ( Exception ex)
            {
                Console.WriteLine("------------------------" + ex.Message + "----------------------------");
            }
        }

        static void AnotherTest(string[] args)
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
