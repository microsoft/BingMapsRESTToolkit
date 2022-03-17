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

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// The type of route attributes to include in a route response.
    /// </summary>
    public enum RouteAttributeType
    {
        /// <summary>
        /// Used to specify the following attributes as a group: excluteItinerary, routePath, and transitStops.
        /// </summary>
        All,

        /// <summary>
        /// Do not include detailed directions in the response. Detailed directions are provided as itinerary items and contain details such as written instructions and traffic location codes.
        /// </summary>
        ExcludeItinerary,
        
        /// <summary>
        /// Include a set of point (latitude and longitude) values that describe the route’s path in the response.
        /// </summary>
        RoutePath,

        /// <summary>
        /// Include only travel time and distance for the route, and does not provide other information. 
        /// </summary>
        RouteSummariesOnly,

        /// <summary>
        /// Include information about transit stops for transit routes.
        /// </summary>
        TransitStops,

        /// <summary>
        /// Include travel summary of distance, time, and toll road distance by two entity types: country (e.g. US, Canada) and administrative division or subregion (e.g. “state” in US and “province” in Canada).
        /// </summary>
        RegionTravelSummary
    }
}
