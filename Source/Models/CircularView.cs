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

using System.Globalization;
using System.Runtime.Serialization;


namespace BingMapsRESTToolkit
{
    /// <summary>
    /// A Circular Screen Search Area.
    /// NOTE: someone created this without looking around for similar classes (CoordwithRadius) does the same thing. 
    /// This class has been modified to wrap CoordwithRadius, to be backwards compatible.
    /// </summary>
    [DataContract]
    public class CircularView : Coordinate
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="latitude">Latitude of point</param>
        /// <param name="longitude">Longitude of point</param>
        /// <param name="radius">Radius, in meters</param>
        public CircularView(double latitude, double longitude, int radius): base()
        {
            if (radius >= 0)
                this.Radius = radius;
            else
                throw new System.Exception("Radius in UserCircularMapView Constructor must be greater than 0");

            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        /// <summary>
        /// Radius (Meters) of Circular Region
        /// </summary>
        [DataMember(Name="radius")]
        public int? Radius { get; set; }

        public override string ToString()
        {
            if (Radius.HasValue)
            {
                return string.Format(CultureInfo.InvariantCulture, "{0:0.######},{1:0.######},{2}", Latitude, Longitude, Radius.Value);
            } else
            {
                return string.Format(CultureInfo.InvariantCulture, "{0:0.######},{1:0.######}", Latitude, Longitude);
            }
        }
    }
}
