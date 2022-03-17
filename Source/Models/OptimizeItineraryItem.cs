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
using System.Runtime.Serialization;
using System.Text;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// A stop or delivery location.
    /// </summary>
    [DataContract]
    public class OptimizeItineraryItem
    {
        /// <summary>
        /// A stop or delivery location.
        /// </summary>
        public OptimizeItineraryItem()
        {
            //Default the name as a Guid so that we start off with a unique name, even if not specified by the user.
            Name = Guid.NewGuid().ToString();
            Priority = 1;
            DwellTimeSpan = new TimeSpan(0, 15, 0);
        }

        /// <summary>
        /// A unique identifier for the itinerary item (stop). Default is a Guid.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Opening time of location. Must arrive after this time.
        /// </summary>
        [DataMember(Name = "openingTime")]
        internal string OpeningTime { get; set; }

        /// <summary>
        /// Opening time of location. Must arrive after this time.
        /// </summary>
        public DateTime? OpeningTimeUtc
        {
            get
            {
                if (string.IsNullOrEmpty(OpeningTime))
                {
                    return null;
                }
                else
                {
                    return DateTimeHelper.GetDateTimeFromUTCString(OpeningTime);
                }
            }
            set
            {
                if (value == null)
                {
                    OpeningTime = string.Empty;
                }
                else
                {
                    var v = DateTimeHelper.GetUTCString(value.Value);

                    if (v != null)
                    {
                        OpeningTime = v;
                    }
                    else
                    {
                        OpeningTime = null;
                    }
                }
            }
        }

        /// <summary>
        /// Closing time of location. Must arrive before this time.
        /// </summary>
        [DataMember(Name = "closingTime")]
        internal string ClosingTime { get; set; }

        /// <summary>
        ///  Closing time of location. Must arrive before this time.
        /// </summary>
        public DateTime? ClosingTimeUtc
        {
            get
            {
                if (string.IsNullOrEmpty(ClosingTime))
                {
                    return null;
                }
                else
                {
                    return DateTimeHelper.GetDateTimeFromUTCString(ClosingTime);
                }
            }
            set
            {
                if (value == null)
                {
                    ClosingTime = string.Empty;
                }
                else
                {
                    var v = DateTimeHelper.GetUTCString(value.Value);

                    if (v != null)
                    {
                        ClosingTime = v;
                    }
                    else
                    {
                        ClosingTime = null;
                    }
                }
            }
        }

        /// <summary>
        /// How long the agent is expected to stay at this location. Default is 15 minutes.
        /// </summary>
        [DataMember(Name = "dwellTime")]
        internal string DwellTime { get; set; }

        /// <summary>
        /// How long the agent is expected to stay at this location. Default is 15 minutes.
        /// </summary>
        public TimeSpan? DwellTimeSpan
        {
            get
            {
                if (TimeSpan.TryParse(DwellTime, out TimeSpan ts))
                {
                    return ts;
                }

                return null;
            }
            set
            {
                if (value == null)
                {
                    DwellTime = string.Empty;
                }
                else
                {
                    DwellTime = string.Format("{0:g}", value);
                }
            }
        }

        /// <summary>
        /// The priority takes an integer value from 1 to 100. Use larger values for priority to indicate higher priority itinerary items. Default: 1
        /// </summary>
        [DataMember(Name = "priority")]
        public int Priority { get; set; }

        /// <summary>
        /// The location of the itinerary item (stop).
        /// </summary>
        [DataMember(Name = "location")]
        internal Coordinate Location
        {
            get
            {
                if (Waypoint != null)
                {
                    return Waypoint.Coordinate;
                }

                return null;
            }
            set
            {
                if (Waypoint == null)
                {
                    Waypoint = new SimpleWaypoint();
                }

                Waypoint.Coordinate = value;
            }
        }

        /// <summary>
        /// The location of the itinerary item (stop). Can be used instead of location.
        /// </summary>
        [DataMember(Name = "address")]
        internal string Address
        {
            get
            {
                if(Waypoint != null)
                {
                    return Waypoint.Address;
                }

                return string.Empty;
            }
            set 
            { 
                if(Waypoint == null)
                {
                    Waypoint = new SimpleWaypoint();
                }

                Waypoint.Address = value;
            }
        }

        /// <summary>
        /// The location waypoint.
        /// </summary>
        public SimpleWaypoint Waypoint { get; set; }

        /// <summary>
        /// Optional. The quantity parameter defines the numeric values that represent the load quantity in terms of volume, weight, 
        /// pallets or case count, or passenger count, etc., that the vehicle delivers to or picks up from each location. 
        /// A positive value denotes pick up, while a negative value denotes drop off.
        /// </summary>
        [DataMember(Name = "quantity")]
        public int[] Quantity { get; set; }

        /// <summary>
        /// Optional. The depot parameter is either true or false, to signify if a location (e.g., a warehouse) can be visited more than 
        /// once for picking up loads. Default: false
        /// </summary>
        [DataMember(Name = "depot")]
        public bool Depot { get; set; }

        /// <summary>
        /// Optional. The dropOffFrom parameter defines the location(s) that the agent needs to visit before visting the current location.
        /// This should be a list of itinerary item names.
        /// </summary>
        [DataMember(Name = "dropOffFrom")]
        public string[] DropOffFrom { get; set; }


        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("{");
            sb.AppendFormat("\"name\":\"{0}\",", Name);

            if (OpeningTimeUtc.HasValue)
            {
                sb.AppendFormat("\"openingTime\":\"{0}\",", DateTimeHelper.GetUTCString(OpeningTimeUtc.Value));
            }
            else
            {
                throw new Exception("No opening time specified.");
            }

            if (ClosingTimeUtc.HasValue)
            {
                sb.AppendFormat("\"closingTime\":\"{0}\",", DateTimeHelper.GetUTCString(ClosingTimeUtc.Value));
            }
            else
            {
                throw new Exception("No closing time specified.");
            }

            if (DwellTime != null) {
                sb.AppendFormat("\"dwellTime\":\"{0:g}\",", DwellTime);
            }

            if(Priority <= 0 || Priority > 100)
            {
                throw new Exception("Priority must be between 1 and 100.");
            }

            sb.AppendFormat("\"priority\":{0},", Priority);

            if (Location != null)
            {
                sb.Append("\"location\":{");
                sb.AppendFormat(CultureInfo.InvariantCulture, "\"latitude\":{0:0.#####},\"longitude\":{1:0.#####}", Location.Latitude, Location.Longitude);
                sb.Append("},");
            }
            else if (!string.IsNullOrWhiteSpace(Address))
            {
                sb.AppendFormat("\"address\":\"{0}\",", Address);
            }
            else
            {
                throw new Exception("Itinerary item location must be specified in shift.");
            }

            if(Quantity != null && Quantity.Length > 0)
            {
                sb.Append("\"quantity\":[");

                //Loop through an append Quantity values with invariant culture.
                foreach (var item in Quantity)
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0}", item);
                    sb.Append(',');
                }
                sb.Length -= 1;
                sb.Append("],");
            }

            sb.AppendFormat("\"depot\":{0},", Depot.ToString().ToLowerInvariant());

            if (DropOffFrom != null && DropOffFrom.Length > 0)
            {
                sb.AppendFormat("\"dropOffFrom\":[\"{0}\"],", string.Join("\",\"", DropOffFrom));
            }
   
            //Remove trailing comma.
            sb.Length--;

            sb.Append("}");

            return sb.ToString();
        }
    }
}