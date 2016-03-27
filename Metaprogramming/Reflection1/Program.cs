using System;

namespace Reflection1
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateObject();
            CallObjectMethod();
        }

        static void CreateObject()
        {
            var lazyIntType = typeof(Lazy<int>);
            var lazyContructor = lazyIntType.GetConstructor(new Type[] { typeof(Func<int>) });
            var lazyInt = lazyContructor.Invoke(new object[] 
            {
                new Func<int>(() => { return new Random().Next(); })
            }) as Lazy<int>;
            Console.WriteLine(lazyInt.Value);
        }

        static void CallObjectMethod()
        {
            var randomType = typeof(Random);
            var nextMethod = randomType.GetMethod("Next", new Type[] { typeof(int), typeof(int) });
            var random = Activator.CreateInstance(randomType);
            Console.WriteLine(nextMethod.Invoke(random, new object[] {0,10}));
        }
    }
}
