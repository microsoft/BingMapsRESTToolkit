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
    /// A route response object.
    /// </summary>
    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class Route : Resource
    {
        /// <summary>
        /// A unique ID for the resource.
        /// </summary>
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id { get; set; }

        /// <summary>
        /// The unit used for distance.
        /// </summary>
        [DataMember(Name = "distanceUnit", EmitDefaultValue = false)]
        public string DistanceUnit
        {
            get
            {
                return EnumHelper.DistanceUnitTypeToString(DistanceUnitType);
            }
            set
            {
                DistanceUnitType = EnumHelper.DistanceUnitStringToEnum(value);
            }
        }

        /// <summary>
        /// The unit used for distance as an Enum.
        /// </summary>
        public DistanceUnitType DistanceUnitType { get; set; }

        /// <summary>
        /// The unit used for time of travel.
        /// </summary>
        [DataMember(Name = "durationUnit", EmitDefaultValue = false)]
        public string DurationUnit
        {
            get
            {
                return Enum.GetName(typeof(TimeUnitType), TimeUnitType);
            }
            set
            {
                TimeUnitType = EnumHelper.TimeUnitStringToEnum(value);
            }
        }

        /// <summary>
        /// The unit used for time as an Enum.
        /// </summary>
        public TimeUnitType TimeUnitType { get; set; }
        
        /// <summary>
        /// The physical distance covered by the entire route. This value is not supported for the Transit travel mode.
        /// </summary>
        [DataMember(Name = "travelDistance", EmitDefaultValue = false)]
        public double TravelDistance { get; set; }

        /// <summary>
        /// The time that it takes, in seconds, to travel a corresponding TravelDistance. 
        /// </summary>
        [DataMember(Name = "travelDuration", EmitDefaultValue = false)]
        public double TravelDuration { get; set; }

        /// <summary>
        /// The time that it takes, in seconds, to travel a corresponding TravelDistance with current traffic conditions.
        /// This value is always provided for the complete route and does not depend on the availability of traffic information.
        /// </summary>
        [DataMember(Name = "travelDurationTraffic", EmitDefaultValue = false)]
        public double TravelDurationTraffic { get; set; }

        /// <summary>
        /// A description of the traffic congestion.
        /// </summary>
        [DataMember(Name = "trafficCongestion", EmitDefaultValue = false)]
        public string TrafficCongestion { get; set; }

        /// <summary>
        /// Information about the traffic data used.
        /// </summary>
        [DataMember(Name = "trafficDataUsed", EmitDefaultValue = false)]
        public string TrafficDataUsed { get; set; }

        /// <summary>
        /// Information about a section of a route between two waypoints.
        /// </summary>
        [DataMember(Name = "routeLegs", EmitDefaultValue = false)]
        public RouteLeg[] RouteLegs { get; set; }

        /// <summary>
        /// A representation of the path of a route. A RoutePath is defined by a Line element that contains of a collection of latitude and longitude points. 
        /// The path of the route is the line that connects these points. 
        /// </summary>
        [DataMember(Name = "routePath", EmitDefaultValue = false)]
        public RoutePath RoutePath { get; set; }
    }
}
