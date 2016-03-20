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


            MemberInfo[] members = typeof(Walnut).GetMembers();
            foreach (MemberInfo m in members)
            {
                Console.WriteLine(m);
            }

            ToStringInfo();
        }

        private static void ToStringInfo()
        {
            throw new NotImplementedException();
        }
    }
}
