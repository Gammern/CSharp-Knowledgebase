using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace CallerInfoAttributes
{
    static class SourceCodeTrace
    {
        public static void TraceMessage(string message,
                [CallerMemberName] string memberName = "",
                [CallerFilePath] string sourceFilePath = "",
                [CallerLineNumber] int sourceLineNumber = 0)
        {
            Trace.WriteLine("message: " + message);
            Trace.WriteLine("member name: " + memberName);
            Trace.WriteLine("source file: " + Path.GetFileName(sourceFilePath));
            Trace.WriteLine("source line number: " + sourceLineNumber);
        }

        private static string testMessage;

        public static string TestMessage
        {
            get { return testMessage; }
            set
            {
                testMessage = value;
                TraceMessage($"property {nameof(TestMessage)} setter called.");
            }
        }

    }
}
