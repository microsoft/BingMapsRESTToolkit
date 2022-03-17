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

using System.Runtime.Serialization;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// A resource that is returned depending on the request.
    /// </summary>
    [DataContract]
    [KnownType(typeof(SearchResult))]
    [KnownType(typeof(Location))]
    [KnownType(typeof(Route))]
    [KnownType(typeof(TrafficIncident))]
    [KnownType(typeof(ImageryMetadata))]
    [KnownType(typeof(ElevationData))]
    [KnownType(typeof(SeaLevelData))]
    [KnownType(typeof(CompressedPointList))]
    [KnownType(typeof(GeospatialEndpointResponse))]
    [KnownType(typeof(DistanceMatrix))]
    [KnownType(typeof(DistanceMatrixAsyncStatus))]
    [KnownType(typeof(IsochroneResponse))]
    [KnownType(typeof(SnapToRoadResponse))]
    [KnownType(typeof(LocationRecog))]
    [KnownType(typeof(LocalInsightsResponse))]    
    [KnownType(typeof(TimeZoneResponse))]
    [KnownType(typeof(RESTTimeZone))]
    [KnownType(typeof(Autosuggest))]
    [KnownType(typeof(OptimizeItinerary))]
    public class Resource
    {
        /// <summary>
        /// Bounding box of the response. Structure [South Latitude, West Longitude, North Latitude, East Longitude]
        /// </summary>
        [DataMember(Name = "bbox", EmitDefaultValue = false)]
        public double[] BoundingBox { get; set; }

        /// <summary>
        /// The type of resource returned.
        /// </summary>
        [DataMember(Name = "__type", EmitDefaultValue = false)]
        public string Type { get; set; }
    }
}
