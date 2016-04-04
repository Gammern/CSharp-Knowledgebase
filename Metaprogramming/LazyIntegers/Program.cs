using System;
namespace LazyIntegers
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var lazyInteger = new Lazy<int>(() =>
            {
                return new Random().Next();
            });
            Console.Out.WriteLine(lazyInteger.Value);
        }
    }
}