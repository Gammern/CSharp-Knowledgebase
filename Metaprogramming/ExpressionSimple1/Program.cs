using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Reflection;

namespace ExpressionSimple1
{
    class Program
    {
        static Func<int,int,bool> CompileLambda()
        {
            ParameterExpression Left = Expression.Parameter(typeof(int), "Left");
            ParameterExpression Right = Expression.Parameter(typeof(int), "Left");

            Expression<Func<int, int, bool>> GreaterThanExpr =
                Expression.Lambda<Func<int, int, bool>>(Expression.GreaterThan(Left,Right), Left, Right);

            return GreaterThanExpr.Compile();
        }

        static Expression<Func<T,T,bool>> Generate<T>(string op)
        {
            ParameterExpression x = Expression.Parameter(typeof(T), "x");
            ParameterExpression y = Expression.Parameter(typeof(T), "y");
            return Expression.Lambda<Func<T,T,bool>>
                (
                    (op.Equals(">")) ? Expression.GreaterThan(x, y) :
                    (op.Equals("<")) ? Expression.LessThan(x, y) :
                    (op.Equals(">=")) ? Expression.GreaterThanOrEqual(x, y) :
                    (op.Equals("<=")) ? Expression.LessThanOrEqual(x, y) :
                    (op.Equals("!=")) ? Expression.NotEqual(x, y) :
                    Expression.Equal(x, y),
                    x, y
                );
        }

        static void Main(string[] args)
        {
            int L = 7, R = 11;
            Console.WriteLine($"{L} > {R} is {CompileLambda()(L, R)}");

            {
                Expression<Func<int, int, int>> add = (x, y) => x + y;
                Func<int, int, int> cadd = add.Compile();
                int result = cadd(2, 3);
                Console.WriteLine($"Result {result}");
            }
            {
                ParameterExpression x = Expression.Parameter(typeof(int), "x");
                ParameterExpression y = Expression.Parameter(typeof(int), "y");
                Func<int, int, int> cadd = Expression.Lambda(Expression.Add(x, y), x, y).Compile() as Func<int, int, int>;
                int result = cadd(2, 3);
                Console.WriteLine($"Result {result}");
            }
            // hardcore IL
            {
                var method = new DynamicMethod("m", typeof(int), new[] { typeof(int), typeof(int) });
                var x = method.DefineParameter(1, ParameterAttributes.In, "x");
                var y = method.DefineParameter(2, ParameterAttributes.In, "y");
                var generator = method.GetILGenerator();
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Add);
                generator.Emit(OpCodes.Ret);
                Func<int,int,int> cadd = method.CreateDelegate(typeof(Func<int, int, int>)) as Func<int,int,int>;
                int result = cadd(2, 3);
                Console.WriteLine($"Result {result}");
            }

        }
    }
}
