using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsing
{
    class Program
    {
        static void Main(string[] args)
        {
            const char beta = '\u03b2';
            const char bitcoin = '\u20bf';

            Console.WriteLine(CultureInfo.CurrentCulture.DisplayName);
            Console.WriteLine("CurrentCulture: " + CultureInfo.CurrentCulture.Name);
            Console.WriteLine("CurrentUICulture: " + CultureInfo.CurrentUICulture.Name);

            Console.WriteLine(true.ToString() + " " + false.ToString()); // output capitalize first letter: True False
            bool b;
            Console.WriteLine($"tryparse(1) is {bool.TryParse("1", out b)}");
            if (bool.TryParse("true", out b)) Console.WriteLine("tryparse(true) succeded");
            if (bool.TryParse("True", out b)) Console.WriteLine("tryparse(True) succeded");

            // real-type-suffix: one of F f D d M m
            Console.WriteLine(1.234D);
            // assume norwegian
            Console.WriteLine(double.Parse("1,234"));

            //CultureInfo no = new CultureInfo("nb-NO");
            CultureInfo no = CultureInfo.GetCultureInfo("nb-NO");
            if (CultureInfo.CurrentCulture.LCID == no.LCID)
            {
                Console.WriteLine("Norwegian is current culture");
            }
            NumberFormatInfo f = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
            Console.WriteLine(33333333.44.ToString("C",f));
            f.CurrencySymbol = new string(bitcoin, 5);
            f.CurrencyGroupSeparator = string.Empty;
            Console.WriteLine(33333333.44.ToString("C",f));
        }
    }
}
