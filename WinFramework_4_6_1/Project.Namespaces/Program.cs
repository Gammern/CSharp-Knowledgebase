
using Project.Namespaces.Middle.Inner;
using System;
using static System.Console;
using MyClass2 = Project.Namespaces.Middle.Inner.MyClass;
using I = Project.Namespaces.Middle.Inner;


namespace Project.Namespaces
{
    /* From .csproj file:
    <ProjectReference Include="..\ClassLibrary1\ClassLibrary1.csproj">
      <Name>ClassLibrary1</Name>
      <Aliases>W1</Aliases>      This is the important bit!!!! changed in Reference properties from "global" to "W1"
    </ProjectReference>
    <ProjectReference Include="..\ClassLibrary2\ClassLibrary2.csproj">
      <Name>ClassLibrary2</Name>
      <Aliases>W2</Aliases>      This is the important bit!!!!
    </ProjectReference>
*/
    extern alias W1;
    extern alias W2;

    namespace Other
    {
        // Class access modifiers: public , internal(default)
        internal class MyClassBase { }  
    }
    namespace Middle
    {
        public class MyClassBase {}
        namespace Inner
        {
            sealed class MyClass : Other.MyClassBase
            {
                internal int Id { get; set; } // class member access modifiers: public, protected internal, protected, internal or private(default)
            }
        }
    }


    class Program
    {

        static void Main(string[] args)
        {
            // Fully qualified
            Middle.Inner.MyClass class1 = new Middle.Inner.MyClass();
            class1.Id = 1;

            // types from namespace imported by "using"
            MyClass class2 = new MyClass();

            // Partially qualified
            var class3 = new Middle.Inner.MyClass() { Id = 2 };

            var class4 = new MyClass2() { Id = 4 };
            Type class4type = class4.GetType();
            // no Console. prefix :-)
            WriteLine($"class4 alias'ed as MyClass2 is of type {class4type.Name} in namespace {class4type.Namespace} IsPublic={class4type.IsPublic}");

            I.MyClass class5 = new I.MyClass();

            // use external lib with same ns and class
            // ClassLibrary.Widget w = new ClassLibrary.Widget(); // won't compile, exists in two different assemblies
            W1.ClassLibrary.Widget w1 = new W1.ClassLibrary.Widget();
            W2.ClassLibrary.Widget w2 = new W2.ClassLibrary.Widget();
        }
    }
}
