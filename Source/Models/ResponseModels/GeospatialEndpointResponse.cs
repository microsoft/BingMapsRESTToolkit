/*
 * Copyright(c) 2017 Microsoft Corporation. All rights reserved. 
 * 
 * This code is licensed under the MIT License (MIT). 
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal 
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
 * of the Software, and to permit persons to whom the Software is furnished to do 
 * so, subject to the following conditions: 
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software. 
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE. 
*/

using System;
using System.Runtime.Serialization;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// This response specifies:
    ///  - Whether this is a politically disputed area, such as an area claimed by more than one country.
    ///  - Whether services are available in the user’s region.
    ///  - A list of available geospatial services including endpoints and language support for each service.
    /// </summary>
    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class GeospatialEndpointResponse : Resource
    {
        /// <summary>
        /// Specifies if this area in the request is claimed by more than one country. 
        /// </summary>
        [DataMember(Name = "isDisputedArea", EmitDefaultValue = false)]
        public bool IsDisputedArea { get; set; }

        /// <summary>
        /// Specifies if Geospatial Platform services are available in the country or region. Microsoft does not support services in embargoed areas.
        /// </summary>
        [DataMember(Name = "isSupported", EmitDefaultValue = false)]
        public bool IsSupported { get; set; }

        /// <summary>
        /// The country or region that was used to determine service support. If you specified a User Location in 
        /// the request that is in a non-disputed country or region, this country or region is returned in the response.
        /// </summary>
        [DataMember(Name = "ur", EmitDefaultValue = false)]
        public string UserRegion { get; set; }

        /// <summary>
        /// Information for each geospatial service that is available in the country or region and language specified in the request.
        /// </summary>
        [DataMember(Name = "services", EmitDefaultValue = false)]
        public GeospatialService[] Services { get; set; }
    }
}
