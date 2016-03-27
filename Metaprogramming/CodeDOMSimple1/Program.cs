using System;
using System.IO;
using System.Text;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace CodeDOMSimple1
{
    class Program
    {

        static void Main(string[] args)
        {
            CodeNamespace progNamespace = HelloWorldCodeDOM.BuildProgram();
            var compilerOptions = new CodeGeneratorOptions
            {
                IndentString = "  ",
                BracingStyle = "C",
                BlankLinesBetweenMembers = false
            };
            var codeText = new StringBuilder();
            using (var codeWriter = new StringWriter(codeText))
            {
                CodeDomProvider.CreateProvider("c++").GenerateCodeFromNamespace(progNamespace, codeWriter, compilerOptions);
            }
            var script = codeText.ToString();
            Console.WriteLine(script);
        }

        static void ShowInstalledLanguages()
        {
            foreach (CompilerInfo ci in CodeDomProvider.GetAllCompilerInfo())
            {
                foreach (string language in ci.GetLanguages())
                    Console.Write(language + " \t");
                Console.WriteLine();
            }
        }
    }
}
