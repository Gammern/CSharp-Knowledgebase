using System;
using System.Reflection;

namespace DuckTyping1
{
    public static class ObjectExtensions
    {
        public static object Call(this object @this, string methodName, params object[] parameters)
        {
            var method = @this.GetType().GetMethod(methodName,
                BindingFlags.Instance | BindingFlags.Public, null, Array.ConvertAll<object, Type>(parameters, x => x.GetType()), null);
            return method.Invoke(@this, parameters);
        }
    }
}
