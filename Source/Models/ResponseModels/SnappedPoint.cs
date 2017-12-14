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
    /// Represents a snapped or interpolated point.
    /// </summary>
    [DataContract]
    public class SnappedPoint
    {
        /// <summary>
        /// The coordinate in which a point was snapped to or an interpolated point.
        /// </summary>
        [DataMember(Name = "coordinate", EmitDefaultValue = false)]
        public Coordinate Coordinate { get; set; }

        /// <summary>
        /// The index in the original list of points passed into the query that this snapped point corresponds to. 
        /// Can be 0 to the number of elements in the input array - 1. It can also be the value -1, which means this is an interpolated point.
        /// </summary>
        [DataMember(Name = "index", EmitDefaultValue = false)]
        public int Index { get; set; }

        /// <summary>
        /// The name of the street this snapped point lies on, if available.  
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// The posted speed limit. If unavailable, will be set to 0. If not requested, will be set to null. 
        /// </summary>
        [DataMember(Name = "speedLimit", EmitDefaultValue = false)]
        public double? SpeedLimit { get; set; }

        /// <summary>
        /// The posted truck speed limit. If unavailable, will be set to 0. If not requested, will be set to null.
        /// </summary>
        [DataMember(Name = "truckSpeedLimit", EmitDefaultValue = false)]
        public double? TruckSpeedLimit { get; set; }
    }
}
