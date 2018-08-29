using System.Runtime.Serialization;
using System;
namespace BingMapsRESTToolkit
{
    [DataContract]
    public class DstRuleResource
    {
        private DateTime StartTime {get; set;}
        private DateTime EndTime { get; set; }

        /// <summary>
        /// The month (three-letter abbreviation) when DST starts, e.g. Mar
        /// </summary>
        [DataMember(Name = "dstStartMonth", EmitDefaultValue = false)]
        public string DstStartMonth { get; set; }

        /// <summary>
        /// DST starting date rule: See https://data.iana.org/time-zones/tz-how-to.html
        /// </summary>
        [DataMember(Name = "dstStartDateRule", EmitDefaultValue = false)]
        public string DstStartDateRule { get; set; }

        /// <summary>
        /// The local time when DST starts, hh:mm format
        /// </summary>
        [DataMember(Name = "dstStartTime", EmitDefaultValue = false)]
        public string DstStartTime
        {
            get
            {
                return DateTimeHelper.GetUTCString(StartTime);
            }
            set
            {
                StartTime = DateTimeHelper.GetDateTimeFromUTCString(value);
            }
        }

        /// <summary>
        /// The offset to apply during DST, (+/-)h:mm format
        /// </summary>
        [DataMember(Name = "dstAdjust1", EmitDefaultValue = false)]
        public string DstAdjust1 { get; set; }

        /// <summary>
        /// The month (three-letter abbreviation) when DST Ends, e.g. Sep
        /// </summary>
        [DataMember(Name = "dstEndMonth", EmitDefaultValue = false)]
        public string DstEndMonth { get; set; }

        /// <summary>
        /// DST ending date rule: See https://data.iana.org/time-zones/tz-how-to.html
        /// </summary>
        [DataMember(Name = "dstEndDateRule", EmitDefaultValue = false)]
        public string DstEndDateRule { get; set; }

        /// <summary>
        /// The local time when DST starts, hh:mm format
        /// </summary>
        [DataMember(Name = "dstEndTime", EmitDefaultValue = false)]
        public string DstEndTime
        {
            get
            {
                return DateTimeHelper.GetUTCString(EndTime);
            }
            set
            {
                EndTime = DateTimeHelper.GetDateTimeFromUTCString(value);
            }
        }

        /// <summary>
        /// The offset to apply outside DST, (+/-)h:mm format
        /// </summary>
        [DataMember(Name = "dstAdjust2", EmitDefaultValue = false)]
        public string DstAdjust2 { get; set; }
    }
}
