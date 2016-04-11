using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeActions;

namespace Roslyn1
{
    public static class ToStringViaRoslynExtensions
    {
        public sealed class Host<T>
        {
            public Host(T target)
            {
                this.Target = target;
            }
            public T Target { get; private set; }
        }
        public static string Generate<T>(this T @this)
        {
            var code =
                "new StringBuilder()" +
                string.Join(".Append(\" || \")",
                    from property in @this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    where property.CanRead
                    select string.Format(".Append(\"{0}: \").Append(Target.{0})", property.Name)) + ".ToString()";
            // var hostReference = new AssemblyFileReference(typeof(ToStringViaRoslynExtensions).Assembly.Location); null on google. ppl don't use it
            return string.Empty;
        }
    }
}
