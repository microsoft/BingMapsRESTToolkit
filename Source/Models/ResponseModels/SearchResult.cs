using System;
using System.Runtime.Serialization;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// A SearchResult response object for deserializing responses to the LocalSearch REST endpoint.
    /// </summary>
    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class SearchResult : Resource
    {
        /// <summary>
        /// The name of the resource.
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// The latitude and longitude coordinates of the local search result.
        /// </summary>
        [DataMember(Name = "point", EmitDefaultValue = false)]
        public Point Point { get; set; }

        /// <summary>
        /// The classification of the geographic entity returned, such as Address.
        /// </summary>
        [DataMember(Name = "entityType", EmitDefaultValue = false)]
        public string EntityType { get; set; }

        /// <summary>
        /// The postal address for this local search result.
        /// </summary>
        [DataMember(Name = "Address", EmitDefaultValue = false)]
        public Address Address { get; set; }

        /// <summary>
        /// The phone number for this local search result.
        /// </summary>
        [DataMember(Name = "PhoneNumber", EmitDefaultValue = false)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The website for this local search result.
        /// </summary>
        [DataMember(Name = "Website", EmitDefaultValue = false)]
        public string Website { get; set; }

        /// <summary>
        /// A collection of geocoded points that differ in how they were calculated and their suggested use.
        /// </summary>
        [DataMember(Name = "geocodePoints", EmitDefaultValue = false)]
        public Point[] GeocodePoints { get; set; }
    }
}