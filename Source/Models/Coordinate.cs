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
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// A class that defines location coordinate value.
    /// </summary>
    [DataContract]
    public class Coordinate
    {
        #region Private Properties

        private double _latitude, _longitude;

        private static Regex CoordinateRx = new Regex(@"^[\s\r\n\t]*(-?[0-9]{0,2}(\.[0-9]*)?)[\s\t]*,[\s\t]*(-?[0-9]{0,3}(\.[0-9]*)?)[\s\r\n\t]*$");

        #endregion

        #region Constructor

        /// <summary>
        /// A location coordinate.
        /// </summary>
        public Coordinate()
        {
        }

        /// <summary>
        /// A location coordinate.
        /// </summary>
        /// <param name="latitude">Latitude coordinate vlaue.</param>
        /// <param name="longitude">Longitude coordinate value.</param>
        public Coordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        #endregion
        
        #region Public Properties

        /// <summary>
        /// Latitude coordinate.
        /// </summary>
        [DataMember(Name = "latitude")]
        public double Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                if (!double.IsNaN(value) && value <= 90 && value >= -90)
                {
                    //Only need to keep the first 5 decimal places. Any more just adds more data being passed around.
                    _latitude = Math.Round(value, 5, MidpointRounding.AwayFromZero);
                }
            }
        }

        /// <summary>
        /// Longitude coordinate.
        /// </summary>
        [DataMember(Name = "longitude")]
        public double Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                if (!double.IsNaN(value) && value <= 180 && value >= -180)
                {
                    //Only need to keep the first 5 decimal places. Any more just adds more data being passed around.
                    _longitude = Math.Round(value, 5, MidpointRounding.AwayFromZero);
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a formatted string of the coordinate in the format "latitude,longitude", with the values rounded off to 5 decimal places.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:0.#####},{1:0.#####}", Latitude, Longitude);
        }

        /// <summary>
        /// Compares two coordinates. Only compares the first 6 decimal places to avoid floating point errors.
        /// </summary>
        /// <param name="obj">Coordinate to compare to.</param>
        /// <returns>A boolean indicating if the two coordinates are equal.</returns>
        public override bool Equals(object obj)
        {
            if(obj != null && obj is Coordinate)
            {
                var c = obj as Coordinate;

                return Math.Round(Latitude, 6) == Math.Round(c.Latitude, 6) && Math.Round(Longitude, 6) == Math.Round(c.Longitude, 6);
            }

            return false;
        }

        /// <summary>
        /// Compares two coordinates. 
        /// </summary>
        /// <param name="c">Coordinate to compare to.</param>
        /// <param name="decimals">The number of decimal places to compare to.</param>
        /// <returns>A boolean indicating if the two coordinates are equal.</returns>
        public bool Equals(Coordinate c, int decimals)
        {
            return Math.Round(Latitude, decimals) == Math.Round(c.Latitude, decimals) && Math.Round(Longitude, decimals) == Math.Round(c.Longitude, decimals);
        }

        /// <summary>
        /// Get hash for coordinate.
        /// </summary>
        /// <returns>Hash for coordinate.</returns>
        public override int GetHashCode()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:0.######}|{1:0.######}", Latitude, Longitude).GetHashCode();
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Parses a coordinate value from a string with the format "latitude,longitude". 
        /// </summary>
        /// <param name="coordinateString">Coordinate string to parse</param>
        /// <returns>A coordinate or null.</returns>
        public static Coordinate Parse(string coordinateString)
        {
            var m = CoordinateRx.Match(coordinateString);

            if (m.Success)
            {
                if(double.TryParse(m.Groups[1].Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double latitude) &&
                    double.TryParse(m.Groups[3].Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double longitude)) {
                    return new Coordinate(latitude, longitude);
                }
            }

            return null;
        }

        #endregion
    }
}
