using System;
using System.Runtime.Serialization;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// Bussiness Info Entity Resource, used by Location Recognition
    /// </summary>
    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class BusinessInfoEntity
    {
        /// <summary>
        /// Unique ID for Business
        /// </summary>
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id { get; set; }

        /// <summary>
        /// Name of the business entity
        /// </summary>
        [DataMember(Name = "entityName", EmitDefaultValue = false)]
        public string EntityName { get; set; }

        /// <summary>
        /// Website URL of the business entity
        /// </summary>
        [DataMember(Name = "url", EmitDefaultValue = false)]
        public Uri URL { get; set; }

        /// <summary>
        /// Phone number of the business entity
        /// </summary>
        [DataMember(Name = "phone", EmitDefaultValue = false)]
        public string Phone { get; set; }

        /// <summary>
        /// Category Business Type ID
        /// </summary>
        [DataMember(Name = "typeId", EmitDefaultValue = false)]
        public int TypeId { get; set; }

        /// <summary>
        /// Category Business Type ID List
        /// </summary>
        [DataMember(Name = "otherTypeIds", EmitDefaultValue = false)]
        public int[] OtherTypeIds { get; set; }
    }
}