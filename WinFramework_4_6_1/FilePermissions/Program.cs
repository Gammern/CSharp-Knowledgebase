using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.AccessControl;
using System.IO;
using System.Security.Principal;

namespace FilePermissions
{
    class Program
    {
        static void Main(string[] args)
        {
            ListPermission();
        }

        private static void ListPermission()
        {
            string filename = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string configfile = filename + ".config";
            if (!File.Exists(configfile))
                throw new FileNotFoundException(configfile);
            FileSecurity sec = File.GetAccessControl(configfile);
            AuthorizationRuleCollection rules = sec.GetAccessRules(true, true, typeof(NTAccount));
            foreach (FileSystemAccessRule rule in rules)
            {
                Console.WriteLine(rule.AccessControlType);
                Console.WriteLine(rule.FileSystemRights);
                Console.WriteLine(rule.IdentityReference.Value);
                Console.WriteLine();
            }
            var sid = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);
            string useraccount = sid.Translate(typeof(NTAccount)).ToString();
            FileSystemAccessRule newRule = new FileSystemAccessRule(useraccount, FileSystemRights.ExecuteFile, AccessControlType.Allow);
            sec.AddAccessRule(newRule);
            File.SetAccessControl(configfile, sec);
        }
    }
}
