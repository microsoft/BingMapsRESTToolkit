using System.Runtime.Serialization;
using System;

namespace BingMapsRESTToolkit
{
    [DataContract]
    public class ConvertedTimeResource
    {
        private DateTime LocalTimeDT { get; set; }

        /// <summary>
        /// Local time for designated time zone, in UTC format
        /// </summary>
        [DataMember(Name = "localTime", EmitDefaultValue = false)]
        public string LocalTime
        {
            get
            {
                return DateTimeHelper.GetUTCString(LocalTimeDT);
            }
            set
            {
                LocalTimeDT = DateTimeHelper.GetDateTimeFromUTCString(value);
            }
        }

        /// <summary>
        /// UTC offset with DST, (+/-)hh:mm format
        /// </summary>
        [DataMember(Name = "utcOffsetWithDst", EmitDefaultValue = false)]
        public string UtcOffsetWithDst { get; set; }

        /// <summary>
        /// Display name of time zone, e.g. Pacific Daylight Time
        /// </summary>
        [DataMember(Name = "timeZoneDisplayName", EmitDefaultValue = false)]
        public string TimeZoneDisplayName { get; set; }

        /// <summary>
        /// Display Time zone abbreviation, e.g. PDT
        /// </summary>
        [DataMember(Name = "timeZoneDisplayAbbr", EmitDefaultValue = false)]
        public string TimeZoneDisplayAbbr { get; set; }
    }
}
