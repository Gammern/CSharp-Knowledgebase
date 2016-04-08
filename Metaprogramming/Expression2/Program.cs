using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Expression2
{
    class Program
    {
        static void Main(string[] args)
        {
            var containers = new Container[]
            {
                new Container { Value="bag" },
                new Container { Value="bed" },
                new Container { Value="car" }
            };

            var filteredResult = from container in containers
                                 where container.Value.Contains("a")
                                 select container;

            // using a LINQ Expression to create the filter
            ParameterExpression argument = Expression.Parameter(typeof(Container));
            MemberExpression valueProperty = Expression.Property(argument, "Value");
            MethodCallExpression containsCall = Expression.Call(valueProperty, typeof(string).GetMethod("Cointains", new Type[] { typeof(string) }), Expression.Constant("a", typeof(string)));
            Expression<Func<Container,bool>> wherePredicate = Expression.Lambda<Func<Container, bool>>(containsCall, argument);
            var whereCall = Expression.Call(typeof(Queryable), "Where", new Type[] { typeof(Container) }, containers.AsQueryable().Expression, wherePredicate);
            var expressionResult = containers.AsQueryable().Provider.CreateQuery<Container>(whereCall);

        }
    }
}
