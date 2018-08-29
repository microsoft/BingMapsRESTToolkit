using System;
using System.Runtime.Serialization;

namespace BingMapsRESTToolkit
{
    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class NaturalPOIAtLocationEntity
    {
        [DataMember(Name = "entityName", EmitDefaultValue = false)]
        public string EntityName { get; set; }

        [DataMember(Name = "latitude", EmitDefaultValue = false)]
        public double Latitude { get; set; }

        [DataMember(Name = "longitude", EmitDefaultValue = false)]
        public double Longitude { get; set; }

        [DataMember(Name = "type", EmitDefaultValue = false)]
        public string Type { get; set; }
    }
}
