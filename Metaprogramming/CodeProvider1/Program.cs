using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;

/* app code can take three forms
    - source code
    - .NET assembly
    - Code graph
    */

namespace CodeProvider1
{
    class Program
    {
        const string source =
@"namespace V3Features
{
    class Program
    {
        static void Main()
        {
            var name = ""Kevin"";
            System.Console.WriteLine(name);
        }
    }
}";

        static void Main(string[] args)
        {
            var providerOptions = new Dictionary<string, string> { ["CompilerVersion"] = "v4.0" };
            //var csProvider = new CSharpCodeProvider(providerOptions);
            CSharpCodeProvider csProvider = (CSharpCodeProvider)CodeDomProvider.CreateProvider("C#", providerOptions);
            //var csParser = csProvider.CreateParser(); // null, Not supported

            var compilerParameters = new CompilerParameters(new string[] { });
            CompilerResults results = csProvider.CompileAssemblyFromSource(compilerParameters, source);
            foreach (string item in results.Output)
            {
                Console.WriteLine(item);
            }
            CodeCompileUnit ccu;

            using (var ss = new StringReader(source))
            {
                ccu = csProvider.Parse(ss);
            }

            var sb = new StringBuilder();
            using (StringWriter tw = new StringWriter(sb))
            {
                CodeGeneratorOptions opt = new CodeGeneratorOptions {  };
                csProvider.GenerateCodeFromCompileUnit(ccu, tw, opt);
            }

            Console.WriteLine(sb.ToString());

        }
    }
}
