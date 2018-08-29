using System;
using System.Runtime.Serialization;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// A LocationRecog response object returned by the LocationRecog operation.
    /// </summary>
    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class LocationRecog : Resource
    {
        [DataMember(Name = "isPrivateResidence", EmitDefaultValue = false)]
        public string _IsPrivateResidence { get; set; }

        public bool IsPrivateResidence
        {
            get
            {
                return bool.Parse(_IsPrivateResidence);
            }
            set
            {
                _IsPrivateResidence = value.ToString();
            }
        }

        [DataMember(Name = "businessesAtLocation", EmitDefaultValue = false)]
        public LocalBusiness[] BusinessAtLocation { get; set; }

        [DataMember(Name = "addressOfLocation", EmitDefaultValue = false)]
        public Address[] AddressOfLocation { get; set; }

        [DataMember(Name = "naturalPOIAtLocation", EmitDefaultValue = false)]
        public NaturalPOIAtLocationEntity[] NaturalPOIAtLocation { get; set;}

    }
}
