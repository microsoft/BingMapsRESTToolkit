/*
 * Copyright(c) 2018 Microsoft Corporation. All rights reserved. 
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
    /// Resource returned by Find TimeZone by Query
    /// </summary>
    [DataContract(Name = "timeZoneAtLocation")]
    public class TimeZoneAtLocationResource
    {
        /// <summary>
        /// Name of Location
        /// </summary>
        [DataMember(Name = "placeName", EmitDefaultValue = false)]
        public string PlaceName { get; set; }

        /// <summary>
        /// Time Zone Resource List
        /// </summary>
        [DataMember(Name = "timeZone", EmitDefaultValue = false)]
        public TimeZoneResponse[] TimeZone { get; set; }
    }

    /// <summary>
    /// Response for Find Time Zone by Query
    /// </summary>
    [DataContract(Name= "RESTTimeZone", Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class RESTTimeZone : Resource
    {
        /// <summary>
        /// Time Zone At Location Resource List
        /// </summary>
        [DataMember(Name = "timeZoneAtLocation")]
        public TimeZoneAtLocationResource[] TimeZoneAtLocation { get; set; }

        /// <summary>
        /// Time Zone Resource List
        /// </summary>
        [DataMember(Name = "timeZone", EmitDefaultValue = false)]
        public TimeZoneResponse TimeZone { get; set; }
    }
}
