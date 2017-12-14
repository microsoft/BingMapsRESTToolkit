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
    /// Information about one of the maneuvers that is part of the itinerary item. 
    /// </summary>
    [DataContract]
    public class Detail
    {
        /// <summary>
        /// The direction in degrees. This value is not supported for the Transit travel mode.
        /// </summary>
        [DataMember(Name = "compassDegrees", EmitDefaultValue = false)]
        public int CompassDegrees { get; set; }

        /// <summary>
        /// The type of maneuver described by this detail collection. The ManeuverType in A detail collection can provide information for a portion of the maneuver 
        /// described by the maneuverType attribute of the corresponding Instruction.
        /// </summary>
        [DataMember(Name = "maneuverType", EmitDefaultValue = false)]
        public string ManeuverType { get; set; }

        /// <summary>
        /// An enumeration version of the ManeuverType.
        /// </summary>
        public ManeuverType ManeuverTypeEnum
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ManeuverType))
                {
                    try
                    {
                        return (ManeuverType)Enum.Parse(typeof(ManeuverType), ManeuverType);
                    }
                    catch { }
                }

                return BingMapsRESTToolkit.ManeuverType.None;
            }
            set
            {
                ManeuverType = value.ToString();
            }
        }

        /// <summary>
        /// This field specifies the index values for specific route path points that are returned in the response when a route path is returned. This and the EndPathIndices correspond to a maneuver route path. 
        /// </summary>
        [DataMember(Name = "startPathIndices", EmitDefaultValue = false)]
        public int[] StartPathIndices { get; set; }

        /// <summary>
        /// This field specifies the index values for specific route path points that are returned in the response when a route path is returned. This and the StartPathIndices correspond to a maneuver route path. 
        /// </summary>
        [DataMember(Name = "endPathIndices", EmitDefaultValue = false)]
        public int[] EndPathIndices { get; set; }

        /// <summary>
        /// The type of road.
        /// </summary>
        [DataMember(Name = "roadType", EmitDefaultValue = false)]
        public string RoadType { get; set; }

        /// <summary>
        /// A traffic location code. Each location code provides traffic incident information for pre-defined road segments. There may be multiple codes for each Detail collection in the response. 
        /// A subscription is typically required to be able to interpret these codes for a geographical area or country.
        /// </summary>
        [DataMember(Name = "locationCodes", EmitDefaultValue = false)]
        public string[] LocationCodes { get; set; }

        /// <summary>
        /// A street, highway or intersection where the maneuver occurs. If the maneuver is complex, there may be more than one name field in the details collection. 
        /// The name field may also have no value. This can occur if the name is not known or if a street, highway or intersection does not have a name. 
        /// This value is only supported for the transit travel mode.
        /// </summary>
        [DataMember(Name = "names", EmitDefaultValue = false)]
        public string[] Names { get; set; }

        /// <summary>
        /// Describes the mode of transportation used between a pair of indexes. This can differ depending whether the route requires walking, driving, or transit. Not all regions or cultures support all values of this field.
        /// </summary>
        [DataMember(Name = "mode", EmitDefaultValue = false)]
        public string Mode { get; set; }

        /// <summary>
        /// Information on the road shield.
        /// </summary>
        [DataMember(Name = "roadShieldRequestParameters", EmitDefaultValue = false)]
        public RoadShield RoadShieldRequestParameters { get; set; }
    }
}
