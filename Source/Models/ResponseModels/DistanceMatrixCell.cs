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
    /// A Distance Matrix response object which is returned when geocoding or reverse geocoding.
    /// </summary>
    [DataContract]
    public class DistanceMatrixCell
    {
        #region Public Properties

        /// <summary>
        /// The index of the origin in the provided origins array used to calculate this cell.
        /// </summary>
        [DataMember(Name = "originIndex", EmitDefaultValue = false)]
        public int OriginIndex { get; set; }

        /// <summary>
        /// The index of the destination in the provided destinations array used to calculate this cell.
        /// </summary>
        [DataMember(Name = "destinationIndex", EmitDefaultValue = false)]
        public int DestinationIndex { get; set; }

        /// <summary>
        /// The physical distance covered to complete a route between the origin and destination. 
        /// When travel mode is set to transit or a path between the origin and destination can't be calculated, this value is set to -1.
        /// </summary>
        [DataMember(Name = "travelDistance", EmitDefaultValue = false)]
        public double TravelDistance { get; set; }

        /// <summary>
        /// The time that it takes, in minutes, to travel a corresponding TravelDistance.
        /// If a path a path between the origin and destination can't be calculated, this value will be a negative number.
        /// </summary>
        [DataMember(Name = "travelDuration", EmitDefaultValue = false)]
        public double TravelDuration { get; set; }

        /// <summary>
        /// The portion of the total route duration which requires walking. This may occur when the travel mode is set to transit. 
        /// </summary>
        [DataMember(Name = "totalWalkDuration", EmitDefaultValue = false)]
        public double TotalWalkDuration { get; set; }

        /// <summary>
        /// The departure time in which this cell was calculated for. Only returned when a startTime is specified. 
        /// When an endTime is specified in the request several cells will be returned for the same origin and destination pairs, 
        /// each having a different departure time for each time interval in the generated histogram request. This string is in ISO 8601 date format.
        /// </summary>
        [DataMember(Name = "departureTime", EmitDefaultValue = false)]
        public string DepartureTime { get; set; }

        /// <summary>
        /// The departure time in which this cell was calculated for. Only returned when a startTime is specified. 
        /// When an endTime is specified in the request several cells will be returned for the same origin and destination pairs, 
        /// each having a different departure time for each time interval in the generated histogram request.
        /// </summary>
        public DateTime? DepartureTimeUtc
        {
            get
            {
                if (!string.IsNullOrEmpty(DepartureTime))
                {
                    DateTime dt;

                    if (DateTime.TryParse(DepartureTime, out dt))
                    {
                        return dt;
                    }
                }

                return null;
            }
            set
            {
                if (value == null)
                {
                    DepartureTime = string.Empty;
                }
                else
                {
                    if (value.HasValue)
                    {
                        DepartureTime = value.Value.ToString("O");
                    }
                    else {
                        DepartureTime = null;
                    }
                }
            }
        }

        /// <summary>
        /// A boolean indicating if an error occurred when calculating this cell.
        /// </summary>
        [DataMember(Name = "hasError", EmitDefaultValue = false)]
        public bool HasError { get; set; }

        #endregion
    }
}
