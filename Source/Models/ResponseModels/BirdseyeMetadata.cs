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
    /// Imagery metadata bout a Birdseye image.
    /// </summary>
    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class BirdseyeMetadata : ImageryMetadata
    {
        /// <summary>
        /// The orientation of the viewport for the imagery metadata in degrees where 0 = North, 90 = East, 180 = South, 270 = West.
        /// </summary>
        [DataMember(Name = "orientation", EmitDefaultValue = false)]
        public double Orientation { get; set; }

        /// <summary>
        /// The horizontal dimension of the imagery in number of tiles.
        /// </summary>
        [DataMember(Name = "tilesX", EmitDefaultValue = false)]
        public int TilesX { get; set; }

        /// <summary>
        /// The vertical dimension of the imagery in number of tiles.
        /// </summary>
        [DataMember(Name = "tilesY", EmitDefaultValue = false)]
        public int TilesY { get; set; }
    }
}
