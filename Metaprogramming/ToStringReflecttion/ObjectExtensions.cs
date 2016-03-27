using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ToStringReflecttion
{
    public static class ObjectExtensions
    {
        const string separator = " || ";
        public static string ToStringReflection<T>(this T @this)
        {
            return string.Join(separator,
                new List<string>(
                    from prop in @this.GetType().GetProperties(
                        BindingFlags.Instance | BindingFlags.Public)
                    where prop.CanRead
                    select $"{prop.Name}: {prop.GetValue(@this, null)}"
                    ).ToArray());
        }
    }
}
