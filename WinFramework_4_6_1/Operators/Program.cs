using DAL.L2S.Nutshell;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Operators
{
    class Program
    {
        static readonly string[] names = { "Tom", "Dick", "Harry", "Mary", "Jay" };

        static string TS<T>(IEnumerable<T> seq) => string.Join(" ", seq);

        static void Main(string[] args)
        {
            using (var db = new NutshellContextDataContext())
            {
                var t = Enumerable.Empty<int>();
            }
            WhereSample();
            WhereSample2();
            TakeWhileSample();
            Projecting();
            Hierarchy();
            SubQuery();
            SelectManySample();
            SelectManySample2();
            JoinSample();
        }

        private static void JoinSample()
        {
            using (var db = new NutshellContextDataContext())
            {
                var customers = db.Customers.ToArray();
                var purchases = db.Purchases.ToArray();
                var slowQuery =
                    from c in customers
                    from p in purchases
                    where c.ID == p.CustomerID
                    select c.Name + " bought a " + p.Description;

                var fastQuery =
                    from c in customers
                    join p in purchases on c.ID equals p.CustomerID
                    select c.Name + " bought a " + p.Description;


            }
        }

        private static void SelectManySample2()
        {
            string[] players = { "Tom", "Jay", "Mary" };
            var query = from player1 in players
                        from player2 in players
                        where player1.CompareTo(player2) < 0
                        select player1 + " x " + player2;
            Console.WriteLine(TS(query.Select(s => s + "\n")));

        }

        private static void SelectManySample()
        {
            string[] fullNames = { "Anne Williams", "John Fred Smith", "Sue Green" };
            Console.WriteLine(TS(fullNames.Select(n => $"\"{n}\"")));
            var query = fullNames.SelectMany(fn => fn.Split());
            Console.WriteLine(TS(query.Select(n => $"\"{n}\"")));
            // same as query syntax
            query =
                from fullName in fullNames
                from name in fullName.Split()
                orderby fullName, name
                select name + " came from " + fullName;
        }

        private static void SubQuery()
        {
            using (var db = new NutshellContextDataContext())
            {
                // kind of left outer join
                var queryLOJ =
                    from c in db.Customers
                    select new
                    {
                        c.Name,
                        Purchases = from p in c.Purchases
                                    where p.CustomerID == c.ID && p.Price > 600
                                    select new { p.Description, p.Price }
                    };

                // kind of inner join (sql becomes left outer join with exists)
                var queryRIJ =
                    from c in db.Customers
                    let hpur = from p in c.Purchases where p.Price > 600 select new { p.Description, p.Price }
                    where hpur.Any()
                    select new { c.Name, Purchases = hpur };

                //var queryJKL =
                //    from p in db.Purchases
                //    group g by p.CustomerID as ff

                    


                foreach (var cname in queryLOJ)
                {
                    Console.WriteLine($"Customer: {cname.Name}");
                    foreach (var pur in cname.Purchases)
                    {
                        Console.WriteLine($"\t{pur.Price}");
                    }
                }
            }
        }

        private static void Hierarchy()
        {
            var sourceDir = Environment.GetFolderPath(Environment.SpecialFolder.Favorites);
            DirectoryInfo[] dirs = new DirectoryInfo(sourceDir).GetDirectories();

            var query =
                from d in dirs
                where (d.Attributes & FileAttributes.System) == 0
                select new
                {   
                    DirectoryName = d.FullName,
                    Created = d.CreationTime,
                    Files = from f in d.GetFiles()
                            where (f.Attributes & FileAttributes.Hidden) == 0
                            select new { FileName = f.Name, f.Length }
                };

            //foreach (var dir in query)
            //{
            //    Console.WriteLine($"Dir: {dir.DirectoryName}");
            //    foreach (var file in dir.Files)
            //    {
            //        Console.WriteLine($"\t{file.FileName} \t{file.Length}");
            //    }
            //}
        }

        private static void Projecting()
        {
            var query = from f in System.Drawing.FontFamily.Families
                        select new { Name = f.Name, LineSpacing = f.GetLineSpacing(System.Drawing.FontStyle.Regular) };
            //Console.WriteLine("Fonts: " + TS(query));
        }

        private static void TakeWhileSample()
        {
            int[] numbers = { 3, 5, 2, 400, 4, 1 };
            var query = numbers.TakeWhile(n => n < 100);
            Console.WriteLine("Numbers: " + TS(query));
        }

        private static void WhereSample2()
        {
            var query = names.Where(s => new[] { "Tom", "Jay" }.Contains(s));
            Console.WriteLine("Where2: " + TS(query));
        }

        private static void WhereSample()
        {
            var query = names.Where(s => s.EndsWith("y"));
            query = from n in names
                    where n.EndsWith("y")
                    select n;
            Console.WriteLine("Where: " + TS(query));
            query = names.Select((s, i) => $"{i}:{s}").Where((s, i) => i % 2 == 0);
            Console.WriteLine("Where i even: " + TS(query));
        }
    }
}
