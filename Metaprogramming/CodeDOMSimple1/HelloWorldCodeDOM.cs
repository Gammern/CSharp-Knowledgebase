using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDOMSimple1
{
    partial class HelloWorldCodeDOM
    {
        public static CodeNamespace BuildProgram()
        {
            return new CodeNamespace("MetaWorld")
            {
                Imports =
                {
                    new CodeNamespaceImport("System")
                },
                Comments =
                {
                    new CodeCommentStatement("This is the namespace comment")
                },
                Types =
                {
                    new CodeTypeDeclaration("Program")
                    {
                        Members =
                        {
                            new CodeMemberMethod
                            {
                                Attributes = MemberAttributes.Static,
                                Name = "Main",
                                Statements = { new CodeMethodInvokeExpression(new CodeSnippetExpression("Console"), "WriteLine", new CodePrimitiveExpression("Hello, world!")) },
                                Comments =
                                {
                                    new CodeCommentStatement("This is the main comment")
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
