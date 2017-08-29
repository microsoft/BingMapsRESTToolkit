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
    /// Information about a segments of the route leg defined by the route leg waypoints and any intermediate via-waypoints. 
    /// For example, if the route leg has two via-waypoints in addition to start and end waypoints, there would be three (3) route sub-legs.
    /// </summary>
    [DataContract]
    public class RouteSubLeg
    {
        /// <summary>
        /// Information about the end waypoint of the route sub-leg. 
        /// </summary>
        [DataMember(Name = "endWaypoint", EmitDefaultValue = false)]
        public Waypoint EndWaypoint { get; set; }

        /// <summary>
        /// Information about the start waypoint of the route sub-leg. 
        /// </summary>
        [DataMember(Name = "startWaypoint", EmitDefaultValue = false)]
        public Waypoint StartWaypoint { get; set; }

        /// <summary>
        /// The physical distance covered by the sub-leg. The units are defined by the DistanceUnit field.
        /// </summary>
        [DataMember(Name = "travelDistance", EmitDefaultValue = false)]
        public double TravelDistance { get; set; }

        /// <summary>
        /// The time, in seconds, that it takes to travel the corresponding TravelDistance.
        /// </summary>
        [DataMember(Name = "travelDuration", EmitDefaultValue = false)]
        public double TravelDuration { get; set; }
    }
}
