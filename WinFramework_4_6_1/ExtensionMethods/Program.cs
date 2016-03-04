using System;

namespace ExtensionMethods
{
    using Utils;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Johan".Trim().IsCapitalized());
            Console.WriteLine("  j o h a n".Trim().Replace(" ", string.Empty).Capitalize());
        }
    }
}
