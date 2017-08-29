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

using System.Globalization;
using System.Runtime.Serialization;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// A bounding box represent the four sides of a rectangular area on the Earth. 
    /// </summary>
    [DataContract]
    public class BoundingBox
    {
        #region Constructor

        /// <summary>
        /// A bounding box represent the four sides of a rectangular area on the Earth. 
        /// </summary>
        public BoundingBox()
        {

        }

        /// <summary>
        /// A bounding box represent the four sides of a rectangular area on the Earth.  
        /// </summary>
        /// <param name="edges">The edges of the bounding box. Structure [South Latitude, West Longitude, North Latitude, East Longitude]</param>
        public BoundingBox(double[] edges)
        {
            if (edges != null && edges.Length >= 4)
            {
                SouthLatitude = edges[0];
                WestLongitude = edges[1];
                NorthLatitude = edges[2];
                EastLongitude = edges[3];
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The southern most latitude value of the bounding box.
        /// </summary>
        [DataMember(Name = "southLatitude", EmitDefaultValue = false)]
        public double SouthLatitude { get; set; }

        /// <summary>
        /// The western most longitude value of the bounding box.
        /// </summary>
        [DataMember(Name = "westLongitude", EmitDefaultValue = false)]
        public double WestLongitude { get; set; }

        /// <summary>
        /// The northern most latitude value of the bounding box.
        /// </summary>
        [DataMember(Name = "northLatitude", EmitDefaultValue = false)]
        public double NorthLatitude { get; set; }

        /// <summary>
        /// The eastern most longitude value of the bounding box.
        /// </summary>
        [DataMember(Name = "eastLongitude", EmitDefaultValue = false)]
        public double EastLongitude { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a formated string with the bounding box data in the format "South Latitude,West Longitude,North Latitude,East Longitude" rounded off to 5 decimal places.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:0.#####},{1:0.#####},{2:0.#####},{3:0.#####}",
                    SouthLatitude,
                    WestLongitude,
                    NorthLatitude,
                    EastLongitude);
        }

        #endregion
    }
}
