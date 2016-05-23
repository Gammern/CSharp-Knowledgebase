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
            foreach (CompilerInfo ci in CodeDomProvider.GetAllCompilerInfo())
            {
                foreach (string lang in ci.GetLanguages())
                {
                    Console.Write($"{lang} \t");
                }
                Console.WriteLine();
            }

            CodeNamespace progNamespace = HelloWorldCodeDOM.BuildProgram();
            var compilerOptions = new CodeGeneratorOptions
            {
                IndentString = "  ",
                BracingStyle = "C",
                BlankLinesBetweenMembers = false
            };

            AddMyClass(progNamespace);

            var codeText = new StringBuilder();
            using (var codeWriter = new StringWriter(codeText))
            {
                CodeDomProvider.CreateProvider("c#").GenerateCodeFromNamespace(progNamespace, codeWriter, compilerOptions);
            }
            var script = codeText.ToString();
            Console.WriteLine(script);
        }

        private static void AddMyClass(CodeNamespace progNamespace)
        {
            var myClass = new CodeTypeDeclaration("MyClass");
            //myClass.Members.Add()
            var implicitAssignMethod = new CodeMemberMethod
            {
                Attributes = MemberAttributes.Static,
                Name = "MyClassAssign",
                Parameters = 
                {
                    new CodeParameterDeclarationExpression(typeof(string), "value")
                },
                Statements =
                {
                    new CodeMethodInvokeExpression(new CodeSnippetExpression("Console"), "WriteLine", new CodePrimitiveExpression("Hello, world!"))
                },
            };
            myClass.Members.Add(implicitAssignMethod);
            progNamespace.Types.Add(myClass);
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
