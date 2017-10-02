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
    /// The styles options that can be applied to map elements. 
    /// </summary>
    [DataContract]
    public class MapElementStyle
    {
        /// <summary>
        /// Hex color used for filling polygons, the background of point icons, and for the center of lines if they have split.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string fillColor { get; set; }

        /// <summary>
        /// The hex color of a map label.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string labelColor { get; set; }

        /// <summary>
        /// The outline hex color of a map label.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string labelOutlineColor { get; set; }

        /// <summary>
        /// Species if a map label type is visible or not.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool? labelVisible { get; set; }

        /// <summary>
        /// Hex color used for the outline around polygons, the outline around point icons, and the color of lines.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string strokeColor { get; set; }

        /// <summary>
        /// Specifies if the map element is visible or not.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool? visible { get; set; }
    }
}
