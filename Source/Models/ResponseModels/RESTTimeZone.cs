using System.Runtime.Serialization;

namespace BingMapsRESTToolkit
{
    [DataContract(Name = "timeZoneAtLocation")]
    public class TimeZoneAtLocationResource
    {
        /// <summary>
        /// Name of Location
        /// </summary>
        [DataMember(Name = "placeName", EmitDefaultValue = false)]
        public string PlaceName { get; set; }

        /// <summary>
        /// Time Zone Resource List
        /// </summary>
        [DataMember(Name = "timeZone", EmitDefaultValue = false)]
        public TimeZoneResponse[] TimeZone { get; set; }
    }

    /// <summary>
    /// Response for Find Time Zone by Query
    /// </summary>
    [DataContract(Name= "RESTTimeZone", Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class RESTTimeZone : Resource
    {
        /// <summary>
        /// Time Zone At Location Resource List
        /// </summary>
        [DataMember(Name = "timeZoneAtLocation")]
        public TimeZoneAtLocationResource[] TimeZoneAtLocation { get; set; }

        /// <summary>
        /// Time Zone Resource List
        /// </summary>
        [DataMember(Name = "timeZone", EmitDefaultValue = false)]
        public TimeZoneResponse TimeZone { get; set; }
    }
}
