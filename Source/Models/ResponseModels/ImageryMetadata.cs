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
    /// A Imagery metadata response object.
    /// </summary>
    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    [KnownType(typeof(StaticMapMetadata))]
    [KnownType(typeof(BirdseyeMetadata))]
    public class ImageryMetadata : Resource
    {
        /// <summary>
        /// The height of the image tile.
        /// </summary>
        [DataMember(Name = "imageHeight", EmitDefaultValue = false)]
        public int ImageHeight { get; set; }

        /// <summary>
        /// The width of the image tile.
        /// </summary>
        [DataMember(Name = "imageWidth", EmitDefaultValue = false)]
        public int ImageWidth { get; set; }

        /// <summary>
        /// Information about the data providers of the imagery.
        /// </summary>
        [DataMember(Name = "imageryProviders", EmitDefaultValue = false)]
        public ImageryProvider[] ImageryProviders { get; set; }

        /// <summary>
        /// A URL template for an image tile if a specific point is specified or a general URL template for the specified imagery set depending on the request.
        /// </summary>
        [DataMember(Name = "imageUrl", EmitDefaultValue = false)]
        public string ImageUrl { get; set; }

        /// <summary>
        /// One or more URL subdomains that may be used when constructing an image tile URL.
        /// </summary>
        [DataMember(Name = "imageUrlSubdomains", EmitDefaultValue = false)]
        public string[] ImageUrlSubdomains { get; set; }

        /// <summary>
        /// The latest date found in an imagery set or for a specific imagery tile.
        /// </summary>
        [DataMember(Name = "vintageEnd", EmitDefaultValue = false)]
        public string VintageEnd { get; set; }

        /// <summary>
        /// The earliest date found in an imagery set or for a specific imagery tile.
        /// </summary>
        [DataMember(Name = "vintageStart", EmitDefaultValue = false)]
        public string VintageStart { get; set; }

        /// <summary>
        /// The maximum zoom level available for this imagery set.
        /// </summary>
        [DataMember(Name = "zoomMax", EmitDefaultValue = false)]
        public int ZoomMax { get; set; }

        /// <summary>
        /// The minimum zoom level available for this imagery set. 
        /// </summary>
        [DataMember(Name = "zoomMin", EmitDefaultValue = false)]
        public int ZoomMin { get; set; }
    }

}
