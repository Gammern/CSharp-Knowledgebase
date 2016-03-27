using System;

namespace DuckTyping1
{
    class Program
    {
        static void Main(string[] args)
        {
            // simple nobrainer way
            dynamic caller = new Golfer();
            Console.WriteLine(caller.Drive("Dynamic"));

            // use extension method
            Console.WriteLine(new Golfer().Call("Drive", "Reflection"));
            Console.WriteLine(new RaceCarDriver().Call("Drive", "Reflection"));

        }
    }
}
