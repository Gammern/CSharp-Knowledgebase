using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

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
        }
    }
}
