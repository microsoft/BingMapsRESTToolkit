using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BingMapsRESTToolkit
{
    public class ListTimeZonesRequest : BaseRestRequest
    {
        private TimeZoneStandardType tz_standard { get; set; }

        private bool list_operation { get; set; } 

        /// <summary>
        ///  If set to true then DST rule information will be returned in the response.
        /// </summary>
        public bool IncludeDstRules { get; set; }

        /// <summary>
        /// The ID of the destination time zone.
        /// </summary>
        public string DestinationTZID { get; set; }

        public string TimeZoneStandard
        {
            get
            {
                switch (tz_standard)
                {
                    case TimeZoneStandardType.IANA:
                        return "iana";
                    case TimeZoneStandardType.WINDOWS:
                    default:
                        return "windows";
                }
            }

            set
            {
                switch(value.Trim().ToLower())
                {
                    case "iana":
                        tz_standard = TimeZoneStandardType.IANA;
                        break;
                    case "windows":
                        tz_standard = TimeZoneStandardType.WINDOWS;
                        break;
                }
            }
        }
        /// <summary>
        /// Constructor for using the List Operaiton
        /// </summary>
        /// <param name="input">The Time Zone Standard Name if `use_list_operation` is true, otherwise `input` is assumed to be a Time Zone ID.</param>
        /// <param name="use_list_operation">Whether to use the List operation</param>
        public ListTimeZonesRequest(bool use_list_operation)
        {
            tz_standard = TimeZoneStandardType.WINDOWS;
            IncludeDstRules = false;
            list_operation = use_list_operation;
        }

        public override string GetRequestUrl()
        {
            List<string> param_list = new List<string>()
            {
                string.Format("key={0}", BingMapsKey.ToString()),
                string.Format("includeDstRules={0}", IncludeDstRules.ToString().ToLower())
            };

            string headStr;
            if (list_operation)
            {
                headStr = "TimeZone/List/?";
                if (TimeZoneStandard != null)
                    param_list.Add(string.Format("tzstd={0}", TimeZoneStandard));
                else
                    throw new Exception("Standard TZ Name ('Windows' or 'IANA') required.");
            }
            else
            {
                headStr = "TimeZone/?";
                if (DestinationTZID != null)
                    param_list.Add(string.Format("desttz={0}", DestinationTZID));
                else
                    throw new Exception("Destination TZ ID required.");


            }

            return this.Domain + headStr + string.Join("&", param_list);
        }
    }
}
