using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;

namespace Reflection1
{
    class Walnut
    {
        private bool cracked;
        public void Crack() { cracked = true; }
        public override string ToString() => (cracked ? "Cracked" : "Uncracked") + " walnut";
    }
    class Program
    {
        static void Main(string[] args)
        {
            Type t = Assembly.GetExecutingAssembly().GetType("Reflection1.Program");
            Console.WriteLine(t.AssemblyQualifiedName);
            Type t2 = Type.GetType(t.AssemblyQualifiedName);
            Type it = typeof(Int32);
            Console.WriteLine(it.AssemblyQualifiedName + " " + it.Assembly.CodeBase);

            foreach (Type tt in typeof(System.Environment).GetNestedTypes())
            {
                Console.WriteLine(tt.FullName);
            }

            int i = (int)Activator.CreateInstance(typeof(int));


            MemberInfo[] members = typeof(Walnut).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (MemberInfo m in members)
            {
                Console.WriteLine(m);
            }
            Walnut w = new Walnut();
            w.Crack();
            Console.WriteLine(w);
            FieldInfo f = typeof(Walnut).GetField("cracked", BindingFlags.NonPublic | BindingFlags.Instance);
            f.SetValue(w, false);
            Console.WriteLine(w);

            ToStringInfo();

            PropertyInfo unbound = typeof(IEnumerator<>).GetProperty("Current");
            PropertyInfo closed = typeof(IEnumerator<int>).GetProperty("Current");

            Console.WriteLine(unbound + " \t" + unbound.PropertyType.IsGenericParameter);
            Console.WriteLine(closed + " \t" + closed.PropertyType.IsGenericParameter);

            string s = "Hello";
            //int length = s.Length;
            PropertyInfo prop = s.GetType().GetProperty("Length");
            int length = (int)prop.GetValue(s);
            Console.WriteLine("Length = " + length);

            Console.WriteLine("stamp".Substring(2));
            MethodInfo method = typeof(string).GetMethod("Substring",new Type[]{ typeof(int) } );
            string substr = (string)method.Invoke("stamp", new object[] { 2 });
            Console.WriteLine(substr);
        }

        private static void ToStringInfo()
        {
            MethodInfo test = typeof(Program).GetMethod("ToString");
            MethodInfo obj = typeof(object).GetMethod("ToString");

            Console.WriteLine("DeclaringType: " + test.DeclaringType);
            Console.WriteLine("DeclaringType: " + obj.DeclaringType);

            Console.WriteLine("ReflectedType: " + test.ReflectedType);
            Console.WriteLine("ReflectedType: " + obj.ReflectedType);

            Console.WriteLine(test == obj);
            Console.WriteLine("test.MethodHandle == obj.MethodHandle " + (test.MethodHandle == obj.MethodHandle));
            var cm = MethodBase.GetCurrentMethod();
            Console.WriteLine(cm);
            var members = typeof(Walnut).GetRuntimeFields();
            foreach (var member in members)
            {
                Console.WriteLine("Member: " + member);
            }
        }
    }
}
