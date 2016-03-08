using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    class MyIntList : IEnumerable<int>
    {
        int[] data = { 1, 2, 3, 4 };
        public IEnumerator<int> GetEnumerator() { return new Enumerator(this); }
        IEnumerator IEnumerable.GetEnumerator() { return new Enumerator(this); }

        private class Enumerator : IEnumerator<int>
        {
            MyIntList collection;
            int currentIndex = -1;

            public Enumerator(MyIntList myIntList)
            {
                this.collection = myIntList;
            }

            public int Current => collection.data[currentIndex];
            object IEnumerator.Current => Current;


            public bool MoveNext() => ++currentIndex < collection.data.Length;

            public void Reset() => this.currentIndex = -1;
            void IDisposable.Dispose() { }
        }
    }
}
