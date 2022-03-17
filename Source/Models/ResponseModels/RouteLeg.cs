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
    /// Information about a section of a route between two waypoints.
    /// </summary>
    [DataContract]
    public class RouteLeg
    {
        /// <summary>
        /// The physical distance covered by a route leg.
        /// </summary>
        [DataMember(Name = "travelDistance", EmitDefaultValue = false)]
        public double TravelDistance { get; set; }

        /// <summary>
        /// The time that it takes, in seconds, to travel a corresponding TravelDistance.
        /// </summary>
        [DataMember(Name = "travelDuration", EmitDefaultValue = false)]
        public double TravelDuration { get; set; }

        /// <summary>
        /// The cost of the journey. Provided for transit routes in some countries.
        /// </summary>
        [DataMember(Name = "cost", EmitDefaultValue = false)]
        public double Cost { get; set; }

        /// <summary>
        /// A short description of the route.
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        /// The Point (latitude and longitude) that was used as the actual starting location for the route leg. In most cases, the ActualStart is the 
        /// same as the requested waypoint. However, if a waypoint is not close to a road, the Routes API chooses a location on the nearest road as the 
        /// starting point of the route. This ActualStart element contains the latitude and longitude of this new location.
        /// </summary>
        [DataMember(Name = "actualStart", EmitDefaultValue = false)]
        public Point ActualStart { get; set; }

        /// <summary>
        /// The Point (latitude and longitude) that was used as the actual ending location for the route leg. In most cases, the ActualEnd is the same 
        /// as the requested waypoint. However, if a waypoint is not close to a road, the Routes API chooses a location on the nearest road as the 
        /// ending point of the route. This ActualEnd element contains the latitude and longitude of this new location.
        /// </summary>
        [DataMember(Name = "actualEnd", EmitDefaultValue = false)]
        public Point ActualEnd { get; set; }

        /// <summary>
        /// Information about the location of the starting waypoint for a route. A StartLocation is provided only when the waypoint is specified as a landmark or an address.
        /// </summary>
        [DataMember(Name = "startLocation", EmitDefaultValue = false)]
        public Location StartLocation { get; set; }

        /// <summary>
        /// Information about the location of the endinpoint for a route. An EndLocation is provided only when the waypoint is specified as a landmark or an address.
        /// </summary>
        [DataMember(Name = "endLocation", EmitDefaultValue = false)]
        public Location EndLocation { get; set; }

        /// <summary>
        /// Information that defines a step in the route.
        /// </summary>
        [DataMember(Name = "itineraryItems", EmitDefaultValue = false)]
        public ItineraryItem[] ItineraryItems { get; set; }

        /// <summary>
        /// Information about a segments of the route leg defined by the route leg waypoints and any intermediate via-waypoints. 
        /// For example, if the route leg has two via-waypoints in addition to start and end waypoints, there would be three (3) route sub-legs.
        /// </summary>
        [DataMember(Name = "routeSubLegs", EmitDefaultValue = false)]
        public RouteSubLeg[] RouteSubLegs { get; set; }

        /// <summary>
        /// Used for transit route responses. Shows the starting time for the starting point of the route. This tells you when to be at the starting waypoint depending on what you select as the dateTime and the timeType.
        /// </summary>
        [DataMember(Name = "startTime", EmitDefaultValue = false)]
        public string StartTime { get; set; }

        /// <summary>
        /// Used for transit route responses. Shows the starting time for the starting point of the route. This tells you when to be at the starting waypoint depending on what you select as the dateTime and the timeType.
        /// </summary>
        public DateTime StartTimeUtc
        {
            get
            {
                if (string.IsNullOrEmpty(StartTime))
                {
                    return DateTime.Now;
                }
                else
                {
                    return DateTimeHelper.FromOdataJson(StartTime);
                }
            }
            set
            {
                if (value == null)
                {
                    StartTime = string.Empty;
                }
                else
                {
                    var v = DateTimeHelper.ToOdataJson(value);

                    if (v != null)
                    {
                        StartTime = v;
                    }
                    else
                    {
                        StartTime = string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// Used for transit route responses. Shows the time of arrival when specific route is taken. This tells you when to be at the ending waypoint depending on what you select as the dateTime and the timeType parameters
        /// </summary>
        [DataMember(Name = "endTime", EmitDefaultValue = false)]
        public string EndTime { get; set; }

        /// <summary>
        /// Used for transit route responses. Shows the time of arrival when specific route is taken. This tells you when to be at the ending waypoint depending on what you select as the dateTime and the timeType parameters
        /// </summary>
        public DateTime EndTimeUtc
        {
            get
            {
                if (string.IsNullOrEmpty(EndTime))
                {
                    return DateTime.Now;
                }
                else
                {
                    return DateTimeHelper.FromOdataJson(EndTime);
                }
            }
            set
            {
                if (value == null)
                {
                    EndTime = string.Empty;
                }
                else
                {
                    var v = DateTimeHelper.ToOdataJson(value);

                    if (v != null)
                    {
                        EndTime = v;
                    }
                    else
                    {
                        EndTime = string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// Regional travel information.
        /// </summary>
        [DataMember(Name = "regionTravelSummary", EmitDefaultValue = false)]
        public RegionTravelSummary RegionTravelSummary { get; set; }
    }
}
