﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BasicLINQ
{
    /*
        LINQ - Language INtegrated Query
        Works over collection/sequence implementing IEnumerable<out T>
        Query operatos in System.Core.dll System.Linq namespace in
        static classes: Enumerable, ParallelEnumerable (local decorator based)
        Static class Queryable works on IQueryable<T> (remote) sequences 
        built up of expression trees intepreted at runtime.
    */
    class Program
    {
        static void Main(string[] args)
        {
            var emptySeq = Enumerable.Empty<string>();

            IEnumerable<string> localSequence = new[] { "Zero", "One", "Two", "Three", "Four", "Five" };
            var even = localSequence.Where((s, i) => i % 2 == 0);
            var odd = localSequence.Except(even);
            odd = System.Linq.Enumerable.Except(localSequence, even);

            Func<IEnumerable<string>, string> SJoin = seq => string.Join(",", seq);
            Console.WriteLine("Even ones: " + string.Join(",", even));
            Console.WriteLine("Odd ones: " + SJoin(odd));

            //Predicate<string>
            Func<string, bool> containA = (s) => s.Contains('a');
            string[] names = { "Tom", "Dick", "Harry", "Mary", "Jay" };
            var query = names
                //.Where(n => n.Contains('a'))
                .Where(containA)
                .OrderBy(n => n.Length)
                .Select(n => n.ToUpper());
            Console.WriteLine("Names : " + SJoin(query));
            // query syntax
            var query2 = from n in names
                         where n.Contains('a')
                         orderby n.Length
                         select n.ToUpper();
            Console.WriteLine("Names2: " + SJoin(query2));

            query = names
                .Select(n => n.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", ""))
                .Where(n => n.Length > 2)
                .OrderBy(n => n);

            Console.WriteLine("RESULT: " + SJoin(query));

            query =
                from n in names
                where n.Length > 2
                orderby n
                select n.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", "");

            Console.WriteLine("RESULT: " + SJoin(query)); // wrong
            // solve by "into" (kind of restart query)
            query =
                from n in names
                select n.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", "")
                into noVowel
                where noVowel.Length > 2
                orderby noVowel
                select noVowel;

            Console.WriteLine("RESULT: " + SJoin(query));
        }
    }
}
