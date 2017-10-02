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
    /// A waypoint in which the user stops at.
    /// </summary>
    [DataContract]
    public class Waypoint : Point
    {
        /// <summary>
        /// A short description identifying the waypoint.
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        /// A value of true indicates that this is a via-waypoint.
        /// </summary>
        [DataMember(Name = "isVia", EmitDefaultValue = false)]
        public bool IsVia { get; set; }

        /// <summary>
        /// A unique identifier for the location. 
        /// </summary>
        [DataMember(Name = "locationIdentifier", EmitDefaultValue = false)]
        public string LocationIdentifier { get; set; }

        /// <summary>
        /// Specifies the route path point associated with the waypoint. 
        /// </summary>
        [DataMember(Name = "routePathIndex", EmitDefaultValue = false)]
        public int RoutePathIndex { get; set; }
    }
}
