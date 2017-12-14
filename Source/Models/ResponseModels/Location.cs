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
    /// A Location response object which is returned when geocoding or reverse geocoding.
    /// </summary>
    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class Location : Resource
    {
        /// <summary>
        /// The name of the resource.
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// The latitude and longitude coordinates of the location. 
        /// </summary>
        [DataMember(Name = "point", EmitDefaultValue = false)]
        public Point Point { get; set; }

        /// <summary>
        /// The classification of the geographic entity returned, such as Address.
        /// </summary>
        [DataMember(Name = "entityType", EmitDefaultValue = false)]
        public string EntityType { get; set; }

        /// <summary>
        /// The postal address for the location.
        /// </summary>
        [DataMember(Name = "address", EmitDefaultValue = false)]
        public Address Address { get; set; }

        /// <summary>
        /// The level of confidence that the geocoded location result is a match. Can be High, Medium, Low.
        /// </summary>
        [DataMember(Name = "confidence", EmitDefaultValue = false)]
        public string Confidence
        {
            get
            {
                return Enum.GetName(typeof(ConfidenceLevelType), ConfidenceLevelType);
            }
            set
            {
                ConfidenceLevelType = EnumHelper.ConfidenceLevelTypeStringToEnum(value);
            }
        }

        /// <summary>
        /// The level of confidence that the geocoded location result is a match as an Enum.
        /// </summary>
        public ConfidenceLevelType ConfidenceLevelType { get; set; }

        /// <summary>
        /// One or more match code values that represent the geocoding level for each location in the response. Can be Good, Ambiguous, UpHierarchy.
        /// </summary>
        [DataMember(Name = "matchCodes", EmitDefaultValue = false)]
        public string[] MatchCodes { get; set; }

        /// <summary>
        /// A collection of geocoded points that differ in how they were calculated and their suggested use. 
        /// </summary>
        [DataMember(Name = "geocodePoints", EmitDefaultValue = false)]
        public Point[] GeocodePoints { get; set; }

        /// <summary>
        /// A collection of parsed values that shows how a location query string was parsed into one or more of the following address values. 
        /// AddressLine, Locality, AdminDistrict, AdminDistrict2, PostalCode, CountryRegion, Landmark
        /// </summary>
        [DataMember(Name = "queryParseValues", EmitDefaultValue = false)]
        public QueryParseValue[] QueryParseValues { get; set; }
    }
}
