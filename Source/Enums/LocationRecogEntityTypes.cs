using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// The different kinds of Location Recog Entities
    /// </summary>
    public enum LocationRecogEntityTypes
    {
        /// <summary>
        ///  Bussiness AND Point of Intrest Entities
        /// </summary>
        BusinessAndPOI,
        /// <summary>
        ///  Only natural Point of Interest Entities
        /// </summary>
        NaturalPOI,
        /// <summary>
        /// Location Recog Address
        /// </summary>
        Address
    }
}
