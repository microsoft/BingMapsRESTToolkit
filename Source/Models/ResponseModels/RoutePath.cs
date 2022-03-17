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
    /// A representation of the path of a route. A RoutePath is defined by a Line element that contains of a collection of latitude and longitude points. 
    /// The path of the route is the line that connects these points. 
    /// </summary>
    [DataContract]
    public class RoutePath
    {
        /// <summary>
        /// The route path coordinate information.
        /// </summary>
        [DataMember(Name = "line", EmitDefaultValue = false)]
        public Line Line { get; set; }

        /// <summary>
        /// Information on generalized coordinates to use in the route path for different resolutions with different zoom levels in a map.
        /// </summary>
        [DataMember(Name = "generalizations", EmitDefaultValue = false)]
        public Generalization[] Generalizations { get; set; }

        /// <summary>
        /// Gets an array of coordinate objects for the route path.
        /// </summary>
        /// <returns>An array of coordinate objects for the route path.</returns>
        public Coordinate[] GetCoordinates()
        {
            if(Line != null && Line.Coordinates != null && Line.Coordinates.Length > 0)
            {
                var coords = new Coordinate[Line.Coordinates.Length];

                for(int i = 0; i < Line.Coordinates.Length; i++)
                {
                    coords[i] = new Coordinate(Line.Coordinates[i][0], Line.Coordinates[i][1]);
                }

                return coords;
            }

            return null;
        }
    }
}
