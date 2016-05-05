using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;

namespace GenerateCSharpCodeFromXsd
{
    class Program
    {
        static void Main(string[] args)
        {
            CSharpCodeProvider provider = (CSharpCodeProvider) CodeDomProvider.CreateProvider("CSharp");
            
            CodeGeneratorOptions options = new CodeGeneratorOptions { BlankLinesBetweenMembers = false, BracingStyle = "C" };
            var ns = Processor.Process("customer.xsd", "MyTargetNamespace");

            provider.GenerateCodeFromNamespace(ns, Console.Out, options);

            var items = new List<string> { "a", "b", "c" };
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }

            Console.ReadLine();

        }
    }
}
