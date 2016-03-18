using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Files
{
    class Program
    {
        static void Main(string[] args)
        {
            FileInfoTest();
        }

        private static void FileInfoTest()
        {
            FileInfo fi = new FileInfo("c:/bin/FileInfo.txt");
            Console.WriteLine(fi.Exists);
            using (TextWriter w = fi.CreateText())
                w.Write("Some text");
            Console.WriteLine(fi.Exists);
            fi.Refresh();
            Console.WriteLine(fi.Exists);

            Console.WriteLine(fi.Name);
            Console.WriteLine(fi.FullName);
            Console.WriteLine(fi.DirectoryName);
            Console.WriteLine(fi.Directory.Name);
            Console.WriteLine(fi.Extension);
            Console.WriteLine(fi.Length);

            fi.Encrypt();
            //fi.Attributes ^= FileAttributes.Hidden;
            //fi.IsReadOnly = true;
            Console.WriteLine(fi.Attributes);
            Console.WriteLine(fi.CreationTime);

            if (File.Exists("c:/bin/FileInfoX.txt"))
            {
                File.Delete("c:/bin/FileInfoX.txt");
            }
            fi.MoveTo("c:/bin/FileInfoX.txt");

            DirectoryInfo di = fi.Directory;
            Console.WriteLine(di.Name);
            Console.WriteLine(di.FullName);
            Console.WriteLine(di.Parent.FullName);
            di.CreateSubdirectory("SubFolder");
            Console.WriteLine(fi.FullName);
            //fi.Delete();
            Console.WriteLine(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory());
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        }
    }
}
