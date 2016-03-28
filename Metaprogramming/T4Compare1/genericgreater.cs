using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4Compare1.generic
{
    public static class greater
    {
        public static T of<T>(T left, T right) where T : IComparable<T>
        {
            return Comparer<T>.Default.Compare(left, right) > 0 ? left : right;
        }
    }
}
