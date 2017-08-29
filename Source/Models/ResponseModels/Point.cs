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
    ///  A point on the Earth specified by a latitude and longitude.
    /// </summary>
    [DataContract]
    public class Point : Shape
    {
        /// <summary>
        /// The type information of the point.
        /// </summary>
        [DataMember(Name = "type", EmitDefaultValue = false)]
        public string Type { get; set; }

        /// <summary>
        /// The coordinate information of the point [Latitude,Longitude].
        /// </summary>
        [DataMember(Name = "coordinates", EmitDefaultValue = false)]
        public double[] Coordinates { get; set; }

        /// <summary>
        /// The method that was used to compute the geocode point. Can be one of the following values:
        /// Interpolation: The geocode point was matched to a point on a road using interpolation.
        /// InterpolationOffset: The geocode point was matched to a point on a road using interpolation with an additional offset to shift the point to the side of the street.
        /// Parcel: The geocode point was matched to the center of a parcel.
        /// Rooftop: The geocode point was matched to the rooftop of a building.
        /// </summary>
        [DataMember(Name = "calculationMethod", EmitDefaultValue = false)]
        public string CalculationMethod { get; set; }

        /// <summary>
        /// The best use for the geocode point. Can be Display or Route. 
        /// Each geocode point is defined as a Route point, a Display point or both.
        /// Use Route points if you are creating a route to the location.Use Display points if you are showing the location on a map. 
        /// For example, if the location is a park, a Route point may specify an entrance to the park where you can enter with a car, 
        /// and a Display point may be a point that specifies the center of the park.
        /// </summary>
        [DataMember(Name = "usageTypes", EmitDefaultValue = false)]
        public string[] UsageTypes { get; set; }

        /// <summary>
        /// Converts the coordinate information in the point into a Coordinate object.
        /// </summary>
        /// <returns>Coordinate object representing the coordinate information in the point.</returns>
        public Coordinate GetCoordinate()
        {
            if (Coordinates.Length >= 2)
            {
                return new Coordinate(Coordinates[0], Coordinates[1]);
            }

            return null;
        }
    }
}
