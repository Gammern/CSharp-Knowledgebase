using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeCompileUnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            CodeCompileUnit ccu = new CodeCompileUnit();
            CodeNamespace globalNs = new CodeNamespace();
            ccu.Namespaces.Add(globalNs);
            globalNs.Imports.Add(new CodeNamespaceImport( "Bar"));
            
            CodeNamespace barNs = new CodeNamespace("Bar");
            CodeNamespace fooNs = new CodeNamespace("Foo");
            fooNs.Imports.Add(new CodeNamespaceImport("Bar"));
            ccu.Namespaces.Add(barNs);
            ccu.Namespaces.Add(fooNs);
            
            new CSharpCodeProvider().GenerateCodeFromCompileUnit(ccu, Console.Out, null);
            Console.ReadLine();
        }
    }
}
