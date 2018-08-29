using System;
using System.Collections.Generic;
using System.Linq;

using System;

namespace BingMapsRESTToolkit
{


    public class FindTimeZoneRequest : BaseRestRequest
    {
        public string Query { get; set; }
        public Coordinate Point { get; set; }

        public DateTime? LocationDateTime { get; set; }

        public bool IncludeDstRules { get; set; }


        public FindTimeZoneRequest(string query, DateTime datetime)
        {
            Point = null;
            Query = query;
            IncludeDstRules = false;
            LocationDateTime = datetime;
        }

        /// <summary>
        /// Find a TimeZone at a specified Location
        /// </summary>
        /// <param name="point"></param>
        public FindTimeZoneRequest(Coordinate point)
        {
            Point = point;
            Query = "";
            IncludeDstRules = false;
        }

        public FindTimeZoneRequest()
        {
            Point = null;
            Query = "";
            IncludeDstRules = false;
        }

        public override string GetRequestUrl()
        {
            string headStr;

            if ((Query != "" && Query != null) && Point == null)
            {
                var query = Uri.EscapeDataString(Query);
                headStr = string.Format("TimeZone/?query={0}&", query);
            }
            else if (Point != null && (Query == "" || Query == null))
            {
                headStr = string.Format("TimeZone/{0}?", Point.ToString());
            }
            else
            {
                throw new Exception("To use Find a Timezone specify either `Point` or `Query` but not both.");
            }

            List<string> param_list = new List<string>()
            {
                string.Format("key={0}", BingMapsKey.ToString()),
                string.Format("includeDstRules={0}", IncludeDstRules.ToString().ToLower())
            };

            if (LocationDateTime.HasValue)
                param_list.Add(string.Format("dt={0}", DateTimeHelper.GetUTCString(LocationDateTime.Value)));

            return this.Domain + headStr + string.Join("&", param_list);
        }
    }
}
