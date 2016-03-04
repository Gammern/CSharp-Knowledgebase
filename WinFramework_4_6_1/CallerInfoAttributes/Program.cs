using System.Diagnostics;

namespace CallerInfoAttributes
{
    class Program
    {
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            SourceCodeTrace.TraceMessage("From main()");

            SourceCodeTrace.TestMessage = "Hello world";

            AcmeViewModel vm = new AcmeViewModel();
            vm.PropertyChanged += (s, e) => Trace.WriteLine($"{e.PropertyName} has changed!");
            vm.CustomerName = "IT Larsen";
        }
    }
}
