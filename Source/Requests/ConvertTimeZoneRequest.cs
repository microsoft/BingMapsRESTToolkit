using System;
using System.Collections.Generic;


namespace BingMapsRESTToolkit
{
    /// <summary>
    /// ConvertTime API Operation Reqeust
    /// </summary>
    public class ConvertTimeZoneRequest : BaseRestRequest
    {
        #region Public Props
        /// <summary>
        ///  If set to true then DST rule information will be returned in the response.
        /// </summary>
        public bool IncludeDstRules { get; set; }

        /// <summary>
        /// The UTC date time string for the specified location. The date must be specified to apply the correct DST.
        /// </summary>
        public DateTime LocationDateTime { get; set; }

        /// <summary>
        /// The ID of the destination time zone.
        /// </summary>
        public string DestinationTZID { get; set; }
        #endregion

        #region Constructor
        public ConvertTimeZoneRequest(DateTime datetime, string DestID)
        {
            LocationDateTime = datetime;
            DestinationTZID = Uri.EscapeDataString(DestID);
        }
        #endregion

        #region Public Methods
        public override string GetRequestUrl()
        {
            string headStr = "TimeZone/Convert/?";

            List<string> param_list = new List<string>()
            {
                string.Format("key={0}", BingMapsKey.ToString()),
                string.Format("includeDstRules={0}", IncludeDstRules.ToString().ToLower()),
                string.Format("dt={0}", DateTimeHelper.GetUTCString(LocationDateTime)),
                string.Format("desttz={0}", DestinationTZID)
            };

            return this.Domain + headStr + string.Join("&", param_list);
        }
        #endregion
    }
}
