using DAL.L2S.Nutshell;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;

namespace L2Test
{
    // Nutshell lack info on code first, only have L2S and old EF
    class Program
    {
        static void Main(string[] args)
        {
            Expression<Func<Customer, bool>> first = (c) =>  c.ID == 1 ; // ->LambdaExpression -> Expression
            Console.WriteLine(first.Body.NodeType);
            Console.WriteLine(((BinaryExpression)first.Body).Left);

            using (var db = new NutshellContextDataContext())
            {
                var query = from c in db.Customers
                            where c.Name.Contains("a")
                            orderby c.Name.Length
                            select c.Name.ToUpper();

                System.Console.WriteLine(query);
                System.Console.WriteLine("Result: " + TS(query));

                DataLoadOptions opt = new DataLoadOptions();
                //opt.AssociateWith<Customer>(c => c.Purchases.Where(p => p.Price > 900));
                opt.LoadWith<Customer>(c => c.Purchases); //force eager loading
                db.LoadOptions = opt;
                var q2 = from c in db.Customers
                         select
                            from p in c.Purchases
                            select new { c.Name, p.Price };

                //System.Console.WriteLine(q2);

                foreach (var c1 in q2)
                {
                    foreach (var namePrice in c1)
                        Console.WriteLine(namePrice);
                    //Console.WriteLine($"{namePrice.Name} +spent {namePrice.Price}");
                }
            }
        }

        static string TS<T>(T seq) where T : IEnumerable<string> => string.Join(" ", seq); 
    }
}
