using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;

namespace CodeDomSimple2
{
    class Program
    {
        static void Main(string[] args)
        {
            CodeNamespace mimsyNamespace = CreateNamespace();
            Console.WriteLine(GenerateCSharpCodeFromNamespace(mimsyNamespace));
            Console.WriteLine( CompileAndExcerciseJubjub(mimsyNamespace, 8, 6, 7, 5, 3, -1, 9));

        }
        static CodeNamespace CreateNamespace()
        {
            CodeNamespace mimsyNamespace = new CodeNamespace
            {
                Name = "Mimsy",
                Imports =
                {
                    new CodeNamespaceImport("System"),
                    new CodeNamespaceImport("System.Text"),
                    new CodeNamespaceImport("System.Collections"),
                }
            };

            CodeTypeDeclaration jubJubClass = new CodeTypeDeclaration("Jubjub")
            {
                TypeAttributes = TypeAttributes.Public,
                Members =
                {
                    new CodeMemberField(typeof(int), "_wabeCount") { Attributes = MemberAttributes.Private },
                    new CodeMemberField(new CodeTypeReference("ArrayList"), "_updates")
                }
            };
            CodeFieldReferenceExpression this_wabeCount = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_wabeCount");
            var suppliedPropertyValue = new CodePropertySetValueReferenceExpression();
            var zero = new CodePrimitiveExpression(0);
            var suppliedPropValIsLessThanZero = new CodeBinaryOperatorExpression(suppliedPropertyValue, CodeBinaryOperatorType.LessThan, zero);
            var testSuppliedPropValAndAssign = new CodeConditionStatement(suppliedPropValIsLessThanZero,
                new CodeStatement[]
                {
                    new CodeAssignStatement(this_wabeCount, zero)
                },
                new CodeStatement[]
                {
                    new CodeAssignStatement(this_wabeCount,suppliedPropertyValue)
                }
                );
            CodeFieldReferenceExpression refUpdatesFld = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_updates");
            CodeMemberProperty wabeCountProp = new CodeMemberProperty()
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Type = new CodeTypeReference(typeof(int)),
                Name = "WabeCount",
                GetStatements =
                {
                    new CodeMethodReturnStatement(this_wabeCount)
                },
                SetStatements =
                {
                    testSuppliedPropValAndAssign,
                    new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(
                            refUpdatesFld, "Add"),
                            this_wabeCount
                        )
                }
            };
            jubJubClass.Members.Add(wabeCountProp);

            mimsyNamespace.Types.Add(jubJubClass);
            CodeConstructor jubjubCtor = new CodeConstructor
            {
                Attributes = MemberAttributes.Public,
                Parameters =
                {
                    new CodeParameterDeclarationExpression(typeof(int), "wabeCount")
                },
                Statements =
                {
                    new CodeAssignStatement(
                        refUpdatesFld,
                        new CodeObjectCreateExpression(new CodeTypeReference("ArrayList"))
                        ),
                    new CodeAssignStatement(
                        new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "WabeCount"),
                        new CodeArgumentReferenceExpression("wabeCount")
                    )
                }
            };
            jubJubClass.Members.Add(jubjubCtor);
            CodeMemberMethod methGetWabeCountHistory =
                new CodeMemberMethod
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Final,
                    Name = "GetWabeCountHistory",
                    ReturnType = new CodeTypeReference(typeof(String))
                };
            jubJubClass.Members.Add(methGetWabeCountHistory);
            methGetWabeCountHistory.Statements.Add(new CodeVariableDeclarationStatement("StringBuilder", "result"));
            var refResultVar = new CodeVariableReferenceExpression("result");
            methGetWabeCountHistory.Statements.Add(
                new CodeAssignStatement(
                    refResultVar,
                    new CodeObjectCreateExpression("StringBuilder")));
            methGetWabeCountHistory.Statements.Add(
                new CodeVariableDeclarationStatement(typeof(int), "ndx"));
            var refNdxVar = new CodeVariableReferenceExpression("ndx");

            methGetWabeCountHistory.Statements.Add(
    new CodeIterationStatement(
        new CodeAssignStatement(refNdxVar,
        new CodePrimitiveExpression(0)),
        new CodeBinaryOperatorExpression(
        refNdxVar,
    CodeBinaryOperatorType.LessThan,
    new CodePropertyReferenceExpression(
    refUpdatesFld,
    "Count")),
    new CodeAssignStatement(
    refNdxVar,
    new CodeBinaryOperatorExpression(
    refNdxVar,
    CodeBinaryOperatorType.Add,
    new CodePrimitiveExpression(1))),
    new CodeConditionStatement(
    new CodeBinaryOperatorExpression(
    refNdxVar,
    CodeBinaryOperatorType.ValueEquality,
    new CodePrimitiveExpression(0)),
    new CodeStatement[] {
    new CodeExpressionStatement(
    new CodeMethodInvokeExpression(
    new CodeMethodReferenceExpression(
    refResultVar,
    "AppendFormat"),
    new CodePrimitiveExpression("{0}"),
    new CodeArrayIndexerExpression(
    refUpdatesFld,
    refNdxVar)))},
    new CodeStatement[] {
    new CodeExpressionStatement(
        new CodeMethodInvokeExpression(
    new CodeMethodReferenceExpression(
    refResultVar,
    "AppendFormat"),
    new CodePrimitiveExpression(", {0}"),
    new CodeArrayIndexerExpression(
    refUpdatesFld,
    refNdxVar)))})));
            methGetWabeCountHistory.Statements.Add(
                new CodeMethodReturnStatement(
                new CodeMethodInvokeExpression(
                new CodeMethodReferenceExpression(
                refResultVar, "ToString"))));

            return mimsyNamespace;
        }

        static string GenerateCSharpCodeFromNamespace(CodeNamespace ns)
        {
            var opts = new CodeGeneratorOptions
            {
                BracingStyle = "C",
                IndentString = "    ",
                BlankLinesBetweenMembers = true
            };
            var codeProv = CodeDomProvider.CreateProvider("c#");

            StringBuilder sb = new StringBuilder();
            using (StreamWriter sw2 = File.CreateText("code.cs"))
            {
                codeProv.GenerateCodeFromNamespace(ns, sw2, opts);

            }
            using (var sw = new StringWriter(sb))
            {
                codeProv.GenerateCodeFromNamespace(ns, sw, opts);
            }
            return sb.ToString();
        }

        static Assembly CompileNamespaceToAssembly(CodeNamespace ns)
        {
            var ccu = new CodeCompileUnit();
            ccu.Namespaces.Add(ns);
            CompilerParameters cp = new CompilerParameters()
            {
                OutputAssembly = "dummy",
                GenerateInMemory = true
            };
            CompilerResults cr = CodeDomProvider.CreateProvider("c#").CompileAssemblyFromDom(cp, ccu);
            return cr.CompiledAssembly;
        }

        static dynamic InstantiateDynamicType(Assembly asm, string typeName, params object[] ctorParams)
        {
            Type targetType = asm.GetType(typeName);
            return Activator.CreateInstance(targetType, ctorParams);
        }

        static string CompileAndExcerciseJubjub(CodeNamespace thNamespace, params int[] wabes)
        {
            if (!(wabes?.Length > 0))
                return string.Empty;


            Assembly compiledAssembly = CompileNamespaceToAssembly(thNamespace);
            // Mimsy.Jubjub bird = new Mimsy.Jubjub(wabes[0]); OK!!!
            dynamic bird = InstantiateDynamicType(compiledAssembly, "Mimsy.Jubjub", new object[] { wabes[0] });
            var props = compiledAssembly.GetType("Mimsy.Jubjub").GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                Console.WriteLine(prop.Name);
            }

            for (int ndx = 1; ndx < wabes.Length; ndx++)
            {
                bird.WabeCount = wabes[ndx];
            }

            return bird.GetWabeCountHistory();
        }
    }
}
