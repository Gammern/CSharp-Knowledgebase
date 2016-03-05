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
            TimeSpan timeNow = DateTime.Now.TimeOfDay;
            Console.WriteLine(timeNow);
            // DateTime and DateTime offset cover year 0001 to 9999

            Console.WriteLine(DateTime.Now);
            Console.WriteLine(DateTimeOffset.Now); // add offset (+/-)nn:nn
            DateTime summer = new DateTime(2016, 7, 15, 12,0,0, DateTimeKind.Local);
            Console.WriteLine(summer.ToString("o")); // 2016-07-15T12:00:00.0000000+02:00

            DateTime appointMent = new DateTime(2016, 10, 10, 12, 0, 0);  // Unspecified
            DateTime ap2 = new DateTime(2016, 10, 10, 12, 0, 0, DateTimeKind.Utc);
            DateTime ap3 = new DateTime(2016, 10, 10, 12, 0, 0, DateTimeKind.Local);
            Console.WriteLine($"{ap2}{ap2.Kind} == {ap3}{ap3.Kind} ?: {ap2 == ap3}");

            DateTimeOffset dto2 = new DateTimeOffset(ap2);
            DateTimeOffset dto3 = new DateTimeOffset(ap3);
            Console.WriteLine($"{dto2} == {dto3} ?: {dto2 == dto3}");
            Console.WriteLine($"{dto2.ToString("o")} == {dto3.ToString("o")} ?: {dto2 == dto3}");

            DateTime oneHourFromNow = DateTime.Now + TimeSpan.FromHours(1); // Local
            Console.WriteLine(DateTime.Now.Kind + " " + appointMent.Kind + " " + oneHourFromNow.Kind); // Local Unspecified Local

            TimeZone zone = TimeZone.CurrentTimeZone;
            Console.WriteLine(zone.StandardName);
            Console.WriteLine(zone.DaylightName);
            Console.WriteLine(zone.GetUtcOffset(DateTime.Now));
        }
    }
}
