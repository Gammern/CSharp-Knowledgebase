using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intersect1
{
    class Program
    {
        static void Main(string[] args)
        {
            var list1 = new List<int>() { 1, 2, 3 };
            var list2 = new List<int>() { 2, 3, 4 };
            var list3 = new List<int>() { 3, 4, 5 };
            var listOfLists = new List<List<int>>() { list1, list2, list3 };
            var intersection = listOfLists
                    .Skip(1)
                    .Aggregate(
                        new HashSet<int>(listOfLists.First()),
                        (h, e) => { h.IntersectWith(e); return h; }
                    );

            foreach (var item in intersection)
            {
                Console.WriteLine(item);
            }
        }
    }
}
