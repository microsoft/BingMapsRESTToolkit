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
using System.Linq;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// Requests traffic information.
    /// </summary>
    public class TrafficRequest : BaseRestRequest
    {
        #region Public Properties

        /// <summary>
        /// Specifies the area to search for traffic incident information. 
        /// A rectangular area specified as a bounding box. 
        /// The size of the area can be a maximum of 500 km x 500 km. 
        /// </summary>
        public BoundingBox MapArea { get; set; }

        /// <summary>
        /// Specifies whether to include traffic location codes in the response. 
        /// Traffic location codes provide traffic incident information for pre-defined road segments. 
        /// A subscription is typically required to be able to interpret these codes for a geographical area or country.
        /// Default is false.
        /// </summary>
        public bool IncludeLocationCodes { get; set; }

        /// <summary>
        /// Specifies severity level of traffic incidents to return. 
        /// The default is to return traffic incidents for all severity levels.
        /// </summary>
        public List<SeverityType> Severity { get; set; }

        /// <summary>
        /// Specifies the type of traffic incidents to return.
        /// </summary>
        public List<TrafficType> TrafficType { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a URL for requesting traffic data for a GET request.
        /// </summary>
        /// <returns>Traffic request URL for GET request.</returns>
        public override string GetRequestUrl()
        {
            //URL Schema:
            //https://dev.virtualearth.net/REST/v1/Traffic/Incidents/mapArea/includeLocationCodes?severity=severity1,severity2,severityn&type=type1,type2,typen&key=BingMapsKey

            //Examples:
            //https://dev.virtualearth.net/REST/v1/Traffic/Incidents/37,-105,45,-94?key=YourBingMapsKey
            //https://dev.virtualearth.net/REST/V1/Traffic/Incidents/37,-105,45,-94/true?t=9,2&s=2,3&o=xml&key=BingMapsKey

            if (MapArea == null)
            {
                throw new Exception("MapArea not specified.");
            }

            string url = string.Format(CultureInfo.InvariantCulture, "{5}Traffic/Incidents/{0:0.#####},{1:0.#####},{2:0.#####},{3:0.#####}{4}",
                    MapArea.SouthLatitude,
                    MapArea.WestLongitude,
                    MapArea.NorthLatitude,
                    MapArea.EastLongitude,
                    (IncludeLocationCodes)? "/true?" : "?", this.Domain);

            if (Severity != null && Severity.Count > 0)
            {
                url += "&severity=" + string.Join(",", Severity.Cast<int>().ToArray());
            }

            if (TrafficType != null && TrafficType.Count > 0)
            {
                url += "&type=" + string.Join(",", TrafficType.Cast<int>().ToArray());
            }

            return url + GetBaseRequestUrl();
        }

        #endregion
    }
}
