using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using System.Reflection;

namespace ReflEmit1
{
    public class Program
    {
        delegate int BinaryFunction(int n1, int m2);

        static void Main(string[] args)
        {
           var dynMeth = new DynamicMethod("Foo", null, null, typeof(Program));
            ILGenerator gen = dynMeth.GetILGenerator();
            gen.EmitWriteLine("Hello World");
            gen.Emit(OpCodes.Ret);
            dynMeth.Invoke(null, null);

            dynMeth = new DynamicMethod("Foo", null, null, typeof(Program));
            gen = dynMeth.GetILGenerator();
            MethodInfo privateMethod = typeof(Program).GetMethod("HelloWorld", BindingFlags.NonPublic | BindingFlags.Static);
            gen.Emit(OpCodes.Call, privateMethod);
            gen.Emit(OpCodes.Ret);
            dynMeth.Invoke(null, null);

            dynMeth = new DynamicMethod("Foo", null, null, typeof(Program));
            gen = dynMeth.GetILGenerator();
            MethodInfo WriteLineInt = typeof(Console).GetMethod("WriteLine", new Type[] { typeof(int) });
            gen.Emit(OpCodes.Ldc_I4, 123);
            gen.Emit(OpCodes.Ldc_I4, 123);
            gen.Emit(OpCodes.Add);
            gen.Emit(OpCodes.Call, WriteLineInt);
            gen.Emit(OpCodes.Ret);
            dynMeth.Invoke(null, null);


            dynMeth = new DynamicMethod("Foo",
                typeof(int), // Return type = int
                new[] { typeof(int), typeof(int) }, // Parameter types = int, int
                typeof(void));
            gen = dynMeth.GetILGenerator();
            gen.Emit(OpCodes.Ldarg_0); // Push first arg onto eval stack
            gen.Emit(OpCodes.Ldarg_1); // Push second arg onto eval stack
            gen.Emit(OpCodes.Add); // Add them together (result on stack)
            gen.Emit(OpCodes.Ret); // Return with stack having 1 value
            BinaryFunction f1 = (BinaryFunction)dynMeth.CreateDelegate(typeof(BinaryFunction));
            Func<int, int, int> f2 = (Func<int, int, int>)dynMeth.CreateDelegate(typeof(Func<int, int, int>));
            int result = (int)dynMeth.Invoke(null, new object[] { 123, 123 });
            Console.WriteLine(result);
            Console.WriteLine(f1(123,124));
            Console.WriteLine(f2(123,125));

        }

        static private void HelloWorld()
        {
            Console.WriteLine("Hello World #2");
        }
    }
}
