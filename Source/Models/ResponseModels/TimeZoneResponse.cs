using System.Runtime.Serialization;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// TimeZone Resource
    /// </summary>
    [DataContract(Name = "timeZone")]
    public class TimeZoneResponse : Resource
    {

        /// <summary>
        /// Standard name of the time zone, e.g. Pacific standard time
        /// </summary>
        [DataMember(Name = "genericName", EmitDefaultValue = false)]
        public string GenericName { get; set; }

        /// <summary>
        /// Abbreviation for the time zone
        /// </summary>
        [DataMember(Name = "abbreviation", EmitDefaultValue = false)]
        public string Abbreviation { get; set; }

        /// <summary>
        /// Time zone name per the IANA standard
        /// </summary>
        [DataMember(Name = "ianaTimeZoneId", EmitDefaultValue = false)]
        public string IANATimeZoneId { get; set; }

        /// <summary>
        /// Time zone name as per the Microsoft Windows standard
        /// </summary>
        [DataMember(Name = "windowsTimeZoneId", EmitDefaultValue = false)]
        public string WindowsTimeZoneId { get; set; }

        /// <summary>
        /// Offset of time zone from UTC, in (+/-)hh:mm format
        /// </summary>
        [DataMember(Name = "utcOffset", EmitDefaultValue = false)]
        public string UtcOffset { get; set; }

        /// <summary>
        /// ConvertedTime Resource List
        /// </summary>
        [DataMember(Name = "convertedTime", EmitDefaultValue = false)]
        public ConvertedTimeResource ConvertedTime { get; set; }

        /// <summary>
        /// Dst Rule Resource List
        /// </summary>
        [DataMember(Name = "dstRule", EmitDefaultValue = false)]
        public DstRuleResource DstRule { get; set; }

    }
}
