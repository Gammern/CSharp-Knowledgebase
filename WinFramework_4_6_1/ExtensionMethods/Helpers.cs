namespace Utils
{
    static class StringHelper
    {
        public static bool IsCapitalized(this string s) => char.IsUpper(s[0]);

        // Allows chaining
        public static string Capitalize(this string s) => s.Substring(0,1).ToUpper() + s.Substring(1);
    }

    static class ObjectHelper
    {
        // Will only be used for string if StringHelper.IsCapitalized is removed 
        public static bool IsCapitalized(this object o) => char.IsUpper(((string)o)[0]);
    }
}
