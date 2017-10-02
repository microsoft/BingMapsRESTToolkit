using BingMapsRESTToolkit;
using System;

namespace RESTToolkitTestConsoleApp.NetStandard
{
    class Program
    {
        static void Main(string[] args)
        {
            var r = ServiceManager.GetResponseAsync(new GeocodeRequest()
            {
                BingMapsKey = System.Configuration.ConfigurationManager.AppSettings.Get("BingMapsKey"),
                Query = "Seattle"
            }).GetAwaiter().GetResult();

            if (r != null && r.ResourceSets != null &&
                r.ResourceSets.Length > 0 &&
                r.ResourceSets[0].Resources != null &&
                r.ResourceSets[0].Resources.Length > 0)
            {
                for (var i = 0; i < r.ResourceSets[0].Resources.Length; i++)
                {
                    Console.WriteLine((r.ResourceSets[0].Resources[i] as Location).Name);
                }
            }
            else
            {
                Console.WriteLine("No results found.");
            }

            Console.ReadLine();
        }
    }
}
