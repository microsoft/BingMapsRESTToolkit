using System;
using System.Runtime.Serialization;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// Local Business Resoruce, used by Location Recognition
    /// </summary>
    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class LocalBusiness
    {
        [DataMember(Name = "businessAddress", EmitDefaultValue = false)]
        public Address BusinessAddress { get; set; }

        [DataMember(Name = "businessInfo", EmitDefaultValue = false)]
        public BusinessInfoEntity BusinessInfo { get; set; }

        /// <summary>
        /// Type of business entity representing the primary nature of business of the entity. Refer to the Business Entity Types table below for a full list of supported types
        /// </summary>
        [DataMember(Name = "type", EmitDefaultValue = false)]
        public string Type { get; set; }

        /// <summary>
        /// List of types which represent the secondary nature of business of the entity
        /// </summary>
        [DataMember(Name = "otherTypes", EmitDefaultValue = false)]
        public string[] OhterTypes { get; set; }
    }
}
