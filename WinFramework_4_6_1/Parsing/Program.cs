using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

namespace Parsing
{
    class MyFormatter : IFormattable
    {
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return "formatted!";
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            const string beta = "\u03b2";
            const string bitcoin = "\u20bf";

            // Default format provider
            Console.WriteLine("Default format provider: " + CultureInfo.CurrentCulture.DisplayName);
            Console.WriteLine("CurrentCulture: " + CultureInfo.CurrentCulture.Name);
            Console.WriteLine("CurrentUICulture: " + CultureInfo.CurrentUICulture.Name);

            Console.WriteLine(true.ToString() + " " + false.ToString()); // output capitalize first letter: True False
            Console.WriteLine(XmlConvert.ToString(true) + " " + XmlConvert.ToString(false)); // all lowercase
            bool b;
            Console.WriteLine($"tryparse(1) is {bool.TryParse("1", out b)}");
            if (bool.TryParse("true", out b)) Console.WriteLine("tryparse(true) succeded");
            if (bool.TryParse("True", out b)) Console.WriteLine("tryparse(True) succeded");

            // real-type-suffix: one of F f D d M m
            Console.WriteLine(1.234D);
            // assume norwegian
            Console.WriteLine(double.Parse("1,234"));
            // don't assume!!! Safe conversion
            Console.WriteLine(double.Parse("1.234", CultureInfo.InvariantCulture));

            CultureInfo no = CultureInfo.GetCultureInfo("nb-NO");
            if (CultureInfo.CurrentCulture.LCID == no.LCID)
            {
                Console.WriteLine("Norwegian is current culture");
            }

            NumberFormatInfo f = (NumberFormatInfo)no.NumberFormat.Clone();
            Console.WriteLine(33333333.44.ToString("C",f)); // IFormattable.ToString(,)
            f.CurrencySymbol = beta + bitcoin;
            f.CurrencyGroupSeparator = string.Empty;
            Console.WriteLine(33333333.44.ToString("C",f));

            CultureInfo us = CultureInfo.GetCultureInfo("en-US");
            DateTime dt = DateTime.Now;
            DateTimeFormatInfo dtfmt = no.DateTimeFormat;
            Console.WriteLine(dt.ToString(dtfmt) + " or " + dt.ToString(no) + 99.9.ToString("C", no).PadLeft(15)); // PadLeft to make entire string 15 chars long
            dtfmt = us.DateTimeFormat;
            Console.WriteLine(dt.ToString(dtfmt) + " or " + dt.ToString(us) + 99.9.ToString("C", us).PadLeft(15));

            // Composite formatting
            decimal credit = 500M;
            IFormattable s = $"Credit = {credit:c}";
            Console.WriteLine(s);
            Console.WriteLine(s.ToString(null,us));

            decimal price = decimal.Parse("$5.99", NumberStyles.Currency,us);

            price = decimal.Parse("($5.99)", NumberStyles.Currency | NumberStyles.AllowParentheses, us);
            Console.WriteLine(price.ToString("C", us));
            Console.WriteLine(price.ToString("C", no));

            double n = -123.45;
            IFormatProvider fp = new WordyFormatProvider();
            Console.WriteLine(string.Format(fp, "{0:C} in words is {0:W}", n));

            // Numeric format strings GFNDECPXR
            Console.WriteLine("{0:G}", double.MaxValue);
            Console.WriteLine("{0:F4}", 1.2);       // fixed point
            Console.WriteLine("{0:N2}", 234567.9);  // group separator
            Console.WriteLine("{0:D15}", 123); // paD leading zero
            Console.WriteLine($"{123:D15}");
            Console.WriteLine($"{3.14}".PadLeft(15,'0'));
            Console.WriteLine("{0:E}", 9999);
            Console.WriteLine("{0:P}", 0.96);
            Console.WriteLine("{0:x16}", int.MaxValue);
            var num = 1.0d / 3.0d;
            Console.WriteLine("{0}",num);
            Console.WriteLine("{0:R}",num); // Roundtrip

            var nf = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();

            var date = new DateTime(2000, 1, 2, 17, 18, 19, DateTimeKind.Local);
            foreach (var fmtch in "dDtTfFgGmMyYorRsuU")
            {
                string fmt = "{0}: {1:" + fmtch + "}";
                Console.WriteLine(fmt, fmtch, date);
            }
            foreach(var c in Enum.GetValues(typeof(ConsoleColor)))
            {
                Console.ForegroundColor = (ConsoleColor)c;
                Console.Write("{0:G}\t",c);
            }
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine(Convert.ToBase64String(Encoding.Unicode.GetBytes("123456789")));
        }

    }
}
