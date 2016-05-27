using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateAndTime
{
    class Program
    {
        static void Main(string[] args)
        {
            TimeSpan workDay = new TimeSpan(7, 30, 0);
            TimeSpan workPerWeek = TimeSpan.FromHours(workDay.TotalHours * 5);
            Console.WriteLine($"{workPerWeek.TotalHours} of {24 * 7}");
            //37,5 of 168
            TimeSpan timeNow = DateTime.Now.TimeOfDay;
            Console.WriteLine(timeNow);
            //21:40:09.9752678
            // DateTime and DateTime offset cover year 0001 to 9999

            Console.WriteLine(DateTime.Now);
            //27.05.2016 21.40.09
            Console.WriteLine(DateTimeOffset.Now); // add offset (+/-)nn:nn
            DateTime summer = new DateTime(2016, 7, 15, 12,0,0, DateTimeKind.Local);
            Console.WriteLine(summer.ToString("o"));
            //2016-07-15T12:00:00.0000000+02:00

            DateTime appointMent = new DateTime(2016, 10, 10, 12, 0, 0);  // Unspecified
            DateTime ap2 = new DateTime(2016, 10, 10, 12, 0, 0, DateTimeKind.Utc);
            DateTime ap3 = new DateTime(2016, 10, 10, 12, 0, 0, DateTimeKind.Local);
            DateTime ap4 = new DateTime(2016, 10, 10, 12, 0, 0, DateTimeKind.Unspecified);
            Console.WriteLine($"{ap2}{ap2.Kind} == {ap3}{ap3.Kind} ?: {ap2 == ap3}");
            //10.10.2016 12.00.00Utc == 10.10.2016 12.00.00Local ?: True
            Console.WriteLine($"{ap3}{ap3.Kind} == {ap4}{ap4.Kind} ?: {ap4 == ap3}");
            // 0.10.2016 12.00.00Local == 10.10.2016 12.00.00Unspecified ?: True
            DateTimeOffset dto2 = new DateTimeOffset(ap2);
            DateTimeOffset dto3 = new DateTimeOffset(ap3);
            Console.WriteLine($"{dto2} == {dto3} ?: {dto2 == dto3}");
            Console.WriteLine($"{dto2.ToString("o")} == {dto3.ToString("o")} ?: {dto2 == dto3}");
            //10.10.2016 12.00.00 +00:00 == 10.10.2016 12.00.00 +02:00 ?: False

            DateTime oneHourFromNow = DateTime.Now + TimeSpan.FromHours(1); // Local
            Console.WriteLine(DateTime.Now.Kind + " " + appointMent.Kind + " " + oneHourFromNow.Kind); // Local Unspecified Local
            //Local Unspecified Local

            TimeZone zone = TimeZone.CurrentTimeZone;
            Console.WriteLine(zone.StandardName);
            Console.WriteLine(zone.DaylightName);
            Console.WriteLine(zone.GetUtcOffset(DateTime.Now));
            /*
            //37,5 of 168
            //21:43:54.0579435
            //27.05.2016 21.43.54
            //27.05.2016 21.43.54 +02:00
            //10.10.2016 12.00.00 +00:00 == 10.10.2016 12.00.00 +02:00 ?: False
            //2016-10-10T12:00:00.0000000+00:00 == 2016-10-10T12:00:00.0000000+02:00 ?: False
            //Local Unspecified Local
            //W. Europe Standard Time
            //W. Europe Daylight Time
            //02:00:00
            //Press any key to continue . . .*/
        }
    }
}
