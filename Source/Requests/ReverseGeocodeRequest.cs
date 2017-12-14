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
using System.Collections.Generic;
using System.Globalization;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// Requests a that converts a coordinate into a location such as an address.
    /// </summary>
    public class ReverseGeocodeRequest : BaseRestRequest
    {
        #region Public Properties

        /// <summary>
        /// A central coordinate to perform the nearby search.
        /// </summary>
        public Coordinate Point { get; set; }

        /// <summary>
        /// Specifies the entity types that you want to return in the response. Only the types you specify will be returned. If the point cannot be mapped to the entity types you specify, no location information is returned in the response.
        /// </summary>
        public List<EntityType> IncludeEntityTypes { get; set; }

        /// <summary>
        /// When you specified the two-letter ISO country code is included for addresses in the response. 
        /// </summary>
        public bool IncludeIso2 { get; set; }

        /// <summary>
        /// Specifies to include the neighborhood in the response when it is available. 
        /// </summary>
        public bool IncludeNeighborhood { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the request URL to perform a reverse geocode query.
        /// </summary>
        /// <returns>A request URL to perform a reverse geocode query.</returns>
        public override string GetRequestUrl()
        {
            string url = string.Format(CultureInfo.InvariantCulture, "{0}Locations/{1:0.#####},{2:0.#####}?",
                this.Domain, 
                Point.Latitude, 
                Point.Longitude);

            if (IncludeEntityTypes != null && IncludeEntityTypes.Count > 0)
            {
                url += "&includeEntityTypes=";
                for (var i = 0; i < IncludeEntityTypes.Count; i++)
                {
                    url += Enum.GetName(typeof(EntityType), IncludeEntityTypes[i]);

                    if (i < IncludeEntityTypes.Count - 1)
                    {
                        url += ",";
                    }
                }
            }

            if (IncludeIso2)
            {
                url += "&incl=ciso2";
            }

            if (IncludeNeighborhood)
            {
                url += "&inclnb=1";
            }

            return url + GetBaseRequestUrl();       
        }

        #endregion
    }
}
