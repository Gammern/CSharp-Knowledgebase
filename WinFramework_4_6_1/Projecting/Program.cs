using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.L2S.Nutshell;
using System.Xml.Linq;

namespace Projecting
{
    class Program
    {
        static void Main(string[] args)
        {
            ProjectionFromPRJFile();
        }

        private static void ProjectionFromPRJFile()
        {
    //          < ItemGroup >
    //< Reference Include = "System" />

             string filename = "../../Projecting.csproj";
            var doc = XDocument.Load(filename);
            XNamespace ns = doc.Root.Name.Namespace;
            var query = new XDocument(new XElement("ProjectReport",
                from n in doc.Root.Elements(ns + "ItemGroup").Elements(ns + "Reference")
                let include = n.Attribute("Include")
                where include != null
                select new XElement("File", include.Value)));
            Console.WriteLine("Assemblies included in the project:");
            Console.WriteLine(query);
        }

        static void ProjectionFromDB()
        {
            XDocument doc;
            using (var db = new NutshellContextDataContext())
            {
                var custList = (from c in db.Customers
                                select new { Name = c.Name, ID = c.ID, Count = c.Purchases.Count }).ToList();

                doc = new XDocument( new XStreamingElement("Customers",
                    from c in custList
                    select new XElement("Customer", new XAttribute("id", c.ID),
                        new XElement("Name", c.Name),
                        new XElement("Buys", c.Count))
                    ));

                Console.WriteLine(doc.ToString());
            }
        }
    }
}
