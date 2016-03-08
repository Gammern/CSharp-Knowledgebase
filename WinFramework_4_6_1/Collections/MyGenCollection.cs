using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    class MyGenCollection : IEnumerable<int>
    {
        private int[] data = { 1, 2, 3, 4 };

        public IEnumerator<int> GetEnumerator()
        {
            foreach (int i in data)
            {
                yield return i;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
