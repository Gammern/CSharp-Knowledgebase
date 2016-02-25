using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace NewInCS6
{
    class MyClass
    {
        public int TimewTwo(int x) => x * 2;

        public string MyProperty { get; private set; } = "MyProperty is not set yet";

        public string SomeProperty => "Property value";
    }

    class Program
    {
        static void Main(string[] args)
        {
            ElvisNullConditional();
            TestExpressionBodiedFunctions();
            IndexInitializers();
            ExceptionFilter();
        }

        private static void ExceptionFilter()
        {
            var client = new WebClient();
            try
            {
                client.DownloadString("http://bogus");
            }
            catch (WebException ex) when (ex.Status == WebExceptionStatus.NameResolutionFailure)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void IndexInitializers()
        {
            var dic = new Dictionary<int, string>
            {
                [3] = "Three",
                [11] = "eleven"
            };

            foreach (var item in dic)
            {
                Console.WriteLine($"{item.Key} = {item.Value}");
            }
        }

        private static void TestExpressionBodiedFunctions()
        {
            const int operand = 4;
            var mc = new MyClass();
            int res = mc.TimewTwo(operand);
            string s = mc.MyProperty;
            // String interpolation
            Console.WriteLine($"{operand} times two is {res} and \"{s}\"");
            s = mc.SomeProperty;
            //mc.SomeProperty = "New value"; // Error, read only
        }

        private static void ElvisNullConditional()
        {
            StringBuilder sb = null;
            var res = sb?.ToString(); // Wont throw NullReferenceException, return null
            Console.WriteLine(res ?? "res is null");
        }
    }
}
