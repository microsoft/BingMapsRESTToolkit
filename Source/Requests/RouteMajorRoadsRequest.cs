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
    /// Requests routes from a location to major nearby roads.
    /// </summary>
    public class RouteMajorRoadsRequest : BaseRestRequest
    {
        #region Public Properties

        /// <summary>
        /// Specifies the final location for all the routes. 
        /// A destination can be specified as a Point, a landmark, or an address.
        /// </summary>
        public SimpleWaypoint Destination { get; set; }

        /// <summary>
        /// Specifies to return only starting points for each major route in the response. 
        /// When this option is not specified, detailed directions for each route are returned. 
        /// </summary>
        public bool ExcludeInstructions { get; set; }

        /// <summary>
        /// The units to use for distance.
        /// </summary>
        public DistanceUnitType DistanceUnits { get; set; }

        /// <summary>
        /// Specifies to include or exclude parts of the routes response.
        /// </summary>
        public List<RouteAttributeType> RouteAttributes { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the request URL to perform a query for routes using major roads.
        /// </summary>
        /// <returns>A request URL to perform a query for routes using major roads.</returns>
        public override string GetRequestUrl()
        {
            //https://dev.virtualearth.net/REST/v1/Routes/FromMajorRoads?destination=destination&exclude=routes&rpo=routePathOutput&du=distanceUnit&key=BingMapsKey

            if (Destination == null || (Destination.Coordinate == null && !string.IsNullOrWhiteSpace(Destination.Address)))
            {
                throw new Exception("Destination value is invalid.");
            }

            string url = this.Domain + "Routes/FromMajorRoads?destination=";

            if (Destination.Coordinate != null)
            {
                url += string.Format(CultureInfo.InvariantCulture, "{0:0.#####},{1:0.#####}", Destination.Coordinate.Latitude, Destination.Coordinate.Longitude);
            }
            else
            {
                url += Uri.EscapeDataString(Destination.Address);
            }

            if (ExcludeInstructions)
            {
                url += "&exclude=routes";
            }

            if (RouteAttributes != null && RouteAttributes.Count > 0)
            {
                url += "&ra=";

                for (var i = 0; i < RouteAttributes.Count; i++)
                {
                    url += Enum.GetName(typeof(RouteAttributeType), RouteAttributes[i]);

                    if (i < RouteAttributes.Count - 1)
                    {
                        url += ",";
                    }
                }
            }

            if (DistanceUnits != DistanceUnitType.Kilometers)
            {
                url += "&du=mi";
            }

            return url + GetBaseRequestUrl();
        }

        #endregion
    }
}
