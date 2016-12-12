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

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// A simple waypoint class that can be used to calculate a route.
    /// </summary>
    public class SimpleWaypoint
    {
        #region Constructor

        /// <summary>
        /// A waypoint to calculate a route through.
        /// </summary>
        public SimpleWaypoint()
        {
        }

        /// <summary>
        /// A waypoint to calculate a route through.
        /// </summary>
        /// <param name="coordinate">Coordinate of the waypoint.</param>
        public SimpleWaypoint(Coordinate coordinate)
        {
            this.Coordinate = coordinate;
        }

        /// <summary>
        /// A waypoint to calculate a route through.
        /// </summary>
        /// <param name="address">Address or location description of waypoint.</param>
        public SimpleWaypoint(string address)
        {
            this.Address = address;
        }

        /// <summary>
        /// A waypoint to calculate a route through.
        /// </summary>
        /// <param name="coordinate">Coordinate of the waypoint.</param>
        /// <param name="address">Address or location description of waypoint.</param>
        public SimpleWaypoint(Coordinate coordinate, string address)
        {
            this.Coordinate = coordinate;
            this.Address = address;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The coordinate of the waypoint. When specified this will be used instead of the Address value in requests.
        /// </summary>
        public Coordinate Coordinate { get; set; }

        /// <summary>
        /// The address query for the waypoint. 
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// A bool indicating whether the waypoint is a via point.
        /// </summary>
        public bool IsViaPoint { get; set; }

        #endregion
    }
}
