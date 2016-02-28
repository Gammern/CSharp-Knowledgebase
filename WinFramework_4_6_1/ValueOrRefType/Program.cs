using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ValueOrRefType
{
    // Struct is a value type, mostly on the stack
    struct SPoint
    {
        public SPoint(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public int X;
        public int Y;
    }

    // Class is a reference type, new'ed on the heap
    class CPoint
    {
        public CPoint(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public int X;
        public int Y;
    }

    class Program
    {
        static void Main(string[] args)
        {
            SPoint sp1 = new SPoint(1, 2);
            SPoint sp2 = sp1; // copy entire structure
            sp1.X += 100;
            Assert.AreNotEqual(sp1.X, sp2.X);

            CPoint cp1 = new CPoint(1, 2);
            CPoint cp2 = cp1; // copy reference only
            cp1.X += 999;
            Assert.AreEqual(cp1.X, cp2.X);
        }
    }
}
