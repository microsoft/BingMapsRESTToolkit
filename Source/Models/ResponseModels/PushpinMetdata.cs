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
    /// Metadata about pushpins in a static image.
    /// </summary>
    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class PushpinMetdata
    {
        /// <summary>
        /// The placement of the pushpin on the static map that is defined by an offset in pixels from the upper left hand corner of the map.
        /// </summary>
        [DataMember(Name = "anchor", EmitDefaultValue = false)]
        public Pixel Anchor { get; set; }

        /// <summary>
        /// The offset of the bottom right corner of the pushpin icon with respect to the anchor point. 
        /// </summary>
        [DataMember(Name = "bottomRightOffset", EmitDefaultValue = false)]
        public Pixel BottomRightOffset { get; set; }

        /// <summary>
        /// The offset of the top left corner of the pushpin icon with respect to the anchor point. 
        /// </summary>
        [DataMember(Name = "topLeftOffset", EmitDefaultValue = false)]
        public Pixel TopLeftOffset { get; set; }

        /// <summary>
        /// The latitude and longitude coordinates of the pushpin.
        /// </summary>
        [DataMember(Name = "point", EmitDefaultValue = false)]
        public Point Point { get; set; }
    }
}
