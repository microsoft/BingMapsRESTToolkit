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
    /// Information about the transit stop associated with the itinerary item.
    /// </summary>
    [DataContract]
    public class TransitStop
    {
        /// <summary>
        /// The ID associated with the transit agency.
        /// </summary>
        [DataMember(Name = "stopId", EmitDefaultValue = false)]
        public int StopId { get; set; }

        /// <summary>
        /// The name of a transit stop.        
        /// </summary>
        [DataMember(Name = "stopName", EmitDefaultValue = false)]
        public string StopName { get; set; }

        /// <summary>
        /// The coordinates of a point on the Earth. A point contains Latitude and Longitude elements.
        /// </summary>
        [DataMember(Name = "position", EmitDefaultValue = false)]
        public Point Position { get; set; }
    }
}
