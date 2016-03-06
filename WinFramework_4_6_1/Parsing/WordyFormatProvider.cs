using System;
using System.Globalization;
using System.Text;

namespace Parsing
{
    public class WordyFormatProvider : IFormatProvider, ICustomFormatter
    {
        static readonly string[] numberWords =
            "null en to tre fire fem seks syv åtte ni minus komma".Split();

        IFormatProvider parent;

        public WordyFormatProvider() : this(CultureInfo.CurrentCulture) {}
        public WordyFormatProvider(IFormatProvider parent) { this.parent = parent; }

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter)) return this;
            return null;
        }
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg == null || format != "W")
                return string.Format(parent, "{0:" + format + "}", arg);
            StringBuilder result = new StringBuilder();
            string digitList = string.Format(CultureInfo.InvariantCulture, "{0}", arg);
            foreach (char digit in digitList)
            {
                int i = "0123456789-.".IndexOf(digit);
                if (i == -1) continue;
                if (result.Length > 0) result.Append(' ');
                result.Append(numberWords[i]);
            }
            return result.ToString();
        }

    }
}
