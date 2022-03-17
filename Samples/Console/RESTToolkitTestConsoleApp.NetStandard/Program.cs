using BingMapsRESTToolkit;
using System;

namespace RESTToolkitTestConsoleApp
{
    class Tests
    {
        private string _ApiKey = System.Configuration.ConfigurationManager.AppSettings.Get("BingMapsKey");

        private void PrintTZResource(TimeZoneResponse tz)
        {

            Console.WriteLine($"Name: {tz.GenericName}");
            Console.WriteLine($"Windows ID: {tz.WindowsTimeZoneId}");
            Console.WriteLine($"IANA ID: {tz.IANATimeZoneId}");
            Console.WriteLine($"UTC offset: {tz.UtcOffset}");
            Console.WriteLine($"Abbrev: {tz.Abbreviation}");

            if (tz.ConvertedTime != null)
            {
                var ctz = tz.ConvertedTime;
                Console.WriteLine($"Local Time: {ctz.LocalTime}");
                Console.WriteLine($"TZ Abbr: {ctz.TimeZoneDisplayAbbr} ");
                Console.WriteLine($"TZ Name: {ctz.TimeZoneDisplayName}");
                Console.WriteLine($"UTC offset: {ctz.UtcOffsetWithDst }");
            }

            if (tz.DstRule != null)
            {
                var dst = tz.DstRule;
                Console.WriteLine("Start: {0} - {1} - {2} - {3}", dst.DstStartTime, dst.DstStartMonth, dst.DstStartDateRule, dst.DstAdjust1);
                Console.WriteLine("End: {0} - {1} - {2} - {3}", dst.DstEndTime, dst.DstEndMonth, dst.DstEndDateRule, dst.DstAdjust2);
            }
        }


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
            Console.WriteLine("Running Autosuggest Test");
            var ul = new CircularView(47.668697, -122.376373,  5);

            var request = new AutosuggestRequest()
            {
                BingMapsKey = _ApiKey,
                Query = "El Bur",
                UserLocation = ul,
            };
            Console.WriteLine(request.GetRequestUrl());
            var resources = GetResourcesFromRequest(request);
            var entities = (resources[0] as Autosuggest);

            foreach (var entity in entities.Value)
                Console.Write($"Entity of type {entity.Type} returned.");

            Console.ReadLine();
        }

        public void ConvertTimeZoneTest()
        {
            Console.WriteLine("Running Convert TZ Test");
            var dt = DateTimeHelper.GetDateTimeFromUTCString("2018-05-15T13:14:15Z");
            var request = new ConvertTimeZoneRequest(dt, "Cape Verde Standard Time") { BingMapsKey = _ApiKey };
            var resources = GetResourcesFromRequest(request);
            var tz = (resources[0] as RESTTimeZone);
            PrintTZResource(tz.TimeZone);
            Console.ReadLine();

        }

        public void ListTimeZoneTest()
        {
            Console.WriteLine("Running List TZ Test");
            var list_request = new ListTimeZonesRequest(true)
            {
                BingMapsKey = _ApiKey,
                TimeZoneStandard = "Windows"
            };
            Console.WriteLine(list_request.GetRequestUrl());

            var resources = GetResourcesFromRequest(list_request);
            Console.WriteLine("Printing first three TZ resources:\n");
            for (int i = 0; i < 3; i++)
                PrintTZResource((resources[i] as RESTTimeZone).TimeZone);

            Console.WriteLine("Running Get TZ By ID Test");
            var get_tz_request = new ListTimeZonesRequest(false)
            {
                IncludeDstRules = true,
                BingMapsKey = _ApiKey,
                DestinationTZID = "Cape Verde Standard Time"
            };
            Console.WriteLine(get_tz_request.GetRequestUrl());

            var tz_resources = GetResourcesFromRequest(get_tz_request);
            var tz = (tz_resources[0] as RESTTimeZone);
            PrintTZResource(tz.TimeZone);

            Console.ReadLine();

        }

        public void FindTimeZoneTest()
        {
            Console.WriteLine("Running Find Time Zone Test: By Query");
            var dt = DateTime.Now;
            var query_tz_request = new FindTimeZoneRequest("Seattle, USA", dt) { BingMapsKey = _ApiKey };
            var query_resources = GetResourcesFromRequest(query_tz_request);
            Console.WriteLine(query_tz_request.GetRequestUrl());

            var r_query = (query_resources[0] as RESTTimeZone);

            if (r_query.TimeZoneAtLocation.Length > 0)
            {
                var qtz = (r_query.TimeZoneAtLocation[0] as TimeZoneAtLocationResource);
                Console.WriteLine($"Place Name: {qtz.PlaceName}");
                PrintTZResource(qtz.TimeZone[0] as TimeZoneResponse);
            }
            else
            {
                Console.WriteLine("No Time Zone Query response.");
            }


            Console.WriteLine("\nRunning Find Time Zone Test: By Point");
            Coordinate cpoint = new Coordinate(47.668915, -122.375789);
            var point_tz_request = new FindTimeZoneRequest(cpoint) { BingMapsKey = _ApiKey, IncludeDstRules = true };

            var point_resources = GetResourcesFromRequest(point_tz_request);
            var r_point = (point_resources[0] as RESTTimeZone);
            var tz = (r_point.TimeZone as TimeZoneResponse);

            Console.WriteLine($"Time Zone: {r_point.TimeZone}");
            PrintTZResource(tz);
            Console.ReadLine();
        }

        public void LocationRecogTest()
        {
            Console.WriteLine("Running Location Recognition Test");

            Coordinate cpoint = new Coordinate(47.668915, -122.375789);

            Console.WriteLine("coord: {0}", cpoint.ToString());

            var request = new LocationRecogRequest() { BingMapsKey = _ApiKey, CenterPoint = cpoint };

            var resources = GetResourcesFromRequest(request);

            var r = (resources[0] as LocationRecog);

            if (r.AddressOfLocation.Length > 0)
                Console.WriteLine($"Address:\n{r.AddressOfLocation.ToString()}");

            if (r.BusinessAtLocation != null)
            {
                foreach (BusinessAtLocation business in r.BusinessAtLocation)
                {
                    Console.WriteLine($"Business:\n{business.BusinessInfo.EntityName}");
                }
            }

            if (r.NaturalPOIAtLocation != null)
            {
                foreach (NaturalPOIAtLocationEntity poi in r.NaturalPOIAtLocation)
                {
                    Console.WriteLine($"POI:\n{poi.EntityName}");
                }
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
            tests.FindTimeZoneTest();
            tests.ConvertTimeZoneTest();
            tests.ListTimeZoneTest();
            tests.AutoSuggestTest();
        }
    }
}
