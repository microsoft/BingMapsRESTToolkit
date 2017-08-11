using System.Globalization;
using System.Runtime.Serialization;

namespace BingMapsRESTToolkit
{
    [DataContract]
    public class BoundingBox
    {
        #region Constructor

        public BoundingBox()
        {

        }

        /// <summary>
        /// Bounding box generated from edge coordinates.  
        /// </summary>
        /// <param name="edges">The edges of the bounding box. Structure [South Latitude, West Longitude, North Latitude, East Longitude]</param>
        public BoundingBox(double[] edges)
        {
            if (edges != null && edges.Length >= 4)
            {
                SouthLatitude = edges[0];
                WestLongitude = edges[1];
                NorthLatitude = edges[2];
                EastLongitude = edges[3];
            }
        }

        #endregion

        #region Public Properties

        [DataMember(Name = "southLatitude", EmitDefaultValue = false)]
        public double SouthLatitude { get; set; }

        [DataMember(Name = "westLongitude", EmitDefaultValue = false)]
        public double WestLongitude { get; set; }

        [DataMember(Name = "northLatitude", EmitDefaultValue = false)]
        public double NorthLatitude { get; set; }

        [DataMember(Name = "eastLongitude", EmitDefaultValue = false)]
        public double EastLongitude { get; set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:0.#####},{1:0.#####},{2:0.#####},{3:0.#####}",
                    SouthLatitude,
                    WestLongitude,
                    NorthLatitude,
                    EastLongitude);
        }

        #endregion
    }
}
