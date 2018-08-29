using BingMapsRESTToolkit;
using System;

namespace RESTToolkitTestConsoleApp
{
    class Tests
    {
        private string _ApiKey = System.Configuration.ConfigurationManager.AppSettings.Get("BingMapsKey");

        private Resource[] GetResourcesFromRequest(BaseRestRequest rest_request)
        {
            var r = ServiceManager.GetResponseAsync(rest_request).GetAwaiter().GetResult();

            if (!(r != null && r.ResourceSets != null &&
                r.ResourceSets.Length > 0 &&
                r.ResourceSets[0].Resources != null &&
                r.ResourceSets[0].Resources.Length > 0))

                throw new Exception("No results found.");

            return r.ResourceSets[0].Resources;
        }

        public void AutoSuggestTest()
        {

        }

        public void LocationRecogTest()
        {
            Console.WriteLine("Running Location Recognition Test");

            Coordinate cpoint = new Coordinate(47.668915, -122.375789);

            Console.WriteLine("coord: {0}", cpoint.ToString());

            var request = new LocationRecogRequest() { BingMapsKey = _ApiKey, CenterPoint = cpoint };

            Console.WriteLine("constructed!");

            Console.WriteLine(request.GetRequestUrl());

            var resources = GetResourcesFromRequest(request);

            foreach (LocationRecog resource in resources)
            {
                Console.WriteLine(resource);
            }

            Console.ReadLine();

        }

        public void GeoCodeTest()
        {
            Console.WriteLine("Running Geocode Test");
            var request = new GeocodeRequest()
            {
                BingMapsKey = _ApiKey,
                Query = "Seattle"
            };

            var resources = GetResourcesFromRequest(request);

            foreach (var resource in resources)
            {
                Console.WriteLine((resource as Location).Name);
            }

            Console.ReadLine();
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Tests tests = new Tests();
            tests.GeoCodeTest();
            tests.LocationRecogTest();
        }


    }
}
