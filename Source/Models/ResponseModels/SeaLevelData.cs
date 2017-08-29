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
    /// An Elevation service response object for Sea Level offset data.
    /// </summary>
    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class SeaLevelData : Resource
    {
        /// <summary>
        /// The difference between sea level models for a set of locations.
        /// </summary>
        [DataMember(Name = "offsets", EmitDefaultValue = false)]
        public int[] Offsets { get; set; }

        /// <summary>
        /// The zoom level used to compute the elevation values. Zoom level values are from 1 to 21. A lower value typically means less 
        /// accurate results because the resolution of the elevation data is less. At lower resolutions, interpolated elevation values 
        /// use data points that are further apart. 
        /// 
        /// The zoom level used depends on the amount of elevation data available in a region and can be lowered when the specified path 
        /// covers a long distance.
        /// </summary>
        [DataMember(Name = "zoomLevel", EmitDefaultValue = false)]
        public int ZoomLevel { get; set; }
    }
}
