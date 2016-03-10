using DAL.L2S.Nutshell;
using System.Collections.Generic;
using System.Linq;


namespace L2Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new NutshellContextDataContext())
            {
                var query = from c in db.Customers
                            where c.Name.Contains("a")
                            orderby c.Name.Length
                            select c.Name.ToUpper();

                System.Console.WriteLine(query);
                System.Console.WriteLine("Result: " + WR(query));
            }
        }

        static string WR(IEnumerable<string> seq)
        {
            return string.Join(" ", seq);
        }
    }
}
