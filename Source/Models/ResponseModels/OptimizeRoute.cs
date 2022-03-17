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
using System.Runtime.Serialization;

namespace BingMapsRESTToolkit
{
    [DataContract]
    public class OptimizeRoute
    {
        /// <summary>
        /// The estimated start time of the route.
        /// </summary>
        [DataMember(Name = "startTime")]
        internal string StartTime { get; set; }

        /// <summary>
        /// The estimated start time of the route.
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
                    return DateTimeHelper.GetDateTimeFromUTCString(StartTime);
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
                    var v = DateTimeHelper.GetUTCString(value);

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
        /// The coordinates for the start location of the agent's first shift.
        /// </summary>
        [DataMember(Name = "startLocation")]
        public Coordinate StartLocation { get; set; }

        /// <summary>
        /// The estimated end time of the route.
        /// </summary>
        [DataMember(Name = "endTime")]
        internal string EndTime { get; set; }

        /// <summary>
        /// The estimated start time of the instruction.
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
                    return DateTimeHelper.GetDateTimeFromUTCString(EndTime);
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
                    var v = DateTimeHelper.GetUTCString(value);

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
        /// The coordinates for the ending location of the agent's last shift.
        /// </summary>
        [DataMember(Name = "endLocation")]
        public Coordinate EndLocation { get; set; }

        /// <summary>
        /// Sorted list of coordinate resources representing the waypoints of the constructed route between the agent's starting and ending locations.
        /// </summary>
        [DataMember(Name = "wayPoints")]
        public Coordinate[] Waypoints { get; set; }

        /// <summary>
        /// The estimated total travel distance for the agent itinerary in meters.
        /// </summary>
        [DataMember(Name = "totalTravelDistance")]
        public double TotalTravelDistance { get; set; }

        /// <summary>
        /// The estimated total travel time for the agent itinerary.
        /// </summary>
        [DataMember(Name = "totalTravelTime")]
        internal string TotalTravelTime { get; set; }

        /// <summary>
        /// The estimated total travel time for the agent itinerary.
        /// </summary>
        public TimeSpan? TotalTravelTimeSpan
        {
            get
            {
                if (TimeSpan.TryParse(TotalTravelTime, out TimeSpan ts))
                {
                    return ts;
                }

                return null;
            }
            set
            {
                if (value == null)
                {
                    TotalTravelTime = string.Empty;
                }
                else
                {
                    TotalTravelTime = string.Format("{0:g}", value);
                }
            }
        }

        /// <summary>
        /// Gets the waypoints from start, stops, to end. This can easily be passed into a route request.
        /// </summary>
        /// <returns>The waypoints from start, stops, to end.</returns>
        public List<SimpleWaypoint> GetAllWaypoints()
        {
            var points = new List<SimpleWaypoint>();

            points.Add(new SimpleWaypoint(StartLocation));

            foreach(var wp in Waypoints)
            {
                points.Add(new SimpleWaypoint(wp));
            }

            points.Add(new SimpleWaypoint(EndLocation));

            return points;
        }
    }
}
