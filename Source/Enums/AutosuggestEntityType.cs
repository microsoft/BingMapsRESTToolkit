using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// Autosuggest Returns either Address, Place or LocalBusiness Response
    /// </summary>
    public enum AutosuggestEntityType
    {
        /// <summary>
        /// Standard Address Resource
        /// </summary>
        Address,
        /// <summary>
        /// Natural POI Resource
        /// </summary>
        Place,
        /// <summary>
        /// Location Business Resource
        /// </summary>
        LocalBusiness
    }
}
