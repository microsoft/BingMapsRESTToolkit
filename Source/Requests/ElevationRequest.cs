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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BingMapsRESTToolkit
{
    //http://msdn.microsoft.com/en-us/library/jj158961.aspx

    /// <summary>
    /// A request for elevation data.
    /// </summary>
    public class ElevationRequest : BaseRestRequest
    {
        #region Private Properties

        private int row = 2, col = 2, samples = 0;

        #endregion

        #region Public Properties

        /// <summary>
        /// A set of coordinates on the Earth to use in elevation calculations. 
        /// The exact use of these points depends on the type of elevation request. 
        /// Overrides the Bounds value if both are specified.
        /// The maximum number of points is 1024.
        /// </summary>
        public List<Coordinate> Points { get; set; }

        /// <summary>
        /// Specifies the rectangular area over which to provide elevation values. 
        /// </summary>
        public BoundingBox Bounds { get; set; }

        /// <summary>
        /// Specifies the number of rows to use to divide the bounding box area into a grid. The rows and columns that define the bounding box each count as two (2) of the rows and columns. Elevation values are returned for all vertices of the grid. 
        /// </summary>
        public int Row
        {
            get { return row; }
            set
            {
                if (value < 2)
                {
                    row = 2;
                }
                else if (value > 512)
                {
                    row = 512;
                }
                else
                {
                    row = value;
                }
            }
        }

        /// <summary>
        /// Specifies the number of columns to use to divide the bounding box area into a grid. The rows and columns that define the bounding box each count as two (2) of the rows and columns. Elevation values are returned for all vertices of the grid. 
        /// </summary>
        public int Col
        {
            get { return col; }
            set
            {
                if (value < 2)
                {
                    col = 2;
                }else if (value > 512){
                    col = 512;
                }
                else
                {
                    col = value;
                }
            }
        }

        /// <summary>
        /// Specifies the number of equally-spaced elevation values to provide along a polyline path. Used when Points value is set. Make = 1024
        /// </summary>
        public int Samples
        {
            get { return samples; }
            set
            {
                if (value < 0)
                {
                    samples = 0;
                }
                else if (value > 1024)
                {
                    samples = 1024;
                }
                else
                {
                    samples = value;
                }
            }
        }

        /// <summary>
        /// Specifies which sea level model to use to calculate elevation.
        /// </summary>
        public ElevationType Height { get; set; }

        /// <summary>
        /// A boolean indicating if the offset from the geoid should be returned. Requires a list of points to be specified.
        /// </summary>
        public bool GetGeoidOffset { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <returns>A response containing the requested data.</returns>
        public override async Task<Response> Execute()
        {
            return await this.Execute(null);
        }

        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <param name="remainingTimeCallback">A callback function in which the estimated remaining time in seconds is sent.</param>
        /// <returns>A response containing the requested data.</returns>
        public override async Task<Response> Execute(Action<int> remainingTimeCallback)
        {
            Stream responseStream = null;

            if (Points != null && Points.Count > 50)
            {
                //Make a post request when there are more than 50 points as there is a risk of URL becoming too large for a GET request.
                responseStream = await ServiceHelper.PostStringAsync(new Uri(GetPostRequestUrl()), GetPointsAsString(), null);
            }
            else
            {
                responseStream = await ServiceHelper.GetStreamAsync(new Uri(GetRequestUrl()));
            }

            if (responseStream != null)
            {
                var r = ServiceHelper.DeserializeStream<Response>(responseStream);
                responseStream.Dispose();
                return r;
            }

            return null;
        }

        /// <summary>
        /// Gets a URL for requesting elevation data for a GET request.
        /// </summary>
        /// <returns>Elevation request URL for GET request.</returns>
        public override string GetRequestUrl()
        {
            return GetPostRequestUrl() + "&" + GetPointsAsString();
        }

        /// <summary>
        /// Gets a URL for requesting elevation data for a POST request.
        /// </summary>
        /// <returns>Elevation request URL for POST request.</returns>
        public string GetPostRequestUrl()
        {
            //https://dev.virtualearth.net/REST/v1/Elevation/Bounds?bounds=boundingBox&rows=rows&cols=cols&heights=heights&key=BingMapsKey
            //https://dev.virtualearth.net/REST/v1/Elevation/SeaLevel?points=lat1,long1,lat2,long2,latn,longn&key=BingMapsKey
            //https://dev.virtualearth.net/REST/v1/Elevation/List?points=lat1,long1,lat2,long2,latn,longn&heights=heights&key=BingMapsKey
            //https://dev.virtualearth.net/REST/v1/Elevation/Polyline?points=lat1,long1,lat2,long2,latn,longn&heights=heights&samples=samples&key=BingMapsKey
            
            var sb = new StringBuilder(this.Domain);
            sb.Append("Elevation/");

            string seperator = "?";

            if (Points != null && Points.Count > 0)
            {
                if (GetGeoidOffset)
                {
                    //Get elevation geoid offsets
                    sb.Append("SeaLevel");
                }
                else if (samples == 0)
                {
                    //Get elevations for a list of points
                    sb.Append("List");
                }
                else
                {
                    //Get elevations along a Polyline with samples
                    sb.AppendFormat("Polyline?samples={0}", samples);
                    seperator = "&";
                }
            }
            else if (Bounds != null)
            {
                if (Row * Col > 1024)
                {
                    throw new Exception("Row * Col value is greater than 1024.");
                }
                
                sb.AppendFormat("Bounds?bounds={0}&rows={1}&cols={2}", Bounds.ToString(), row, col);
                seperator = "&";
            }
            else
            {
                throw new Exception("No Points or Bounds value specified.");
            }

            if (!GetGeoidOffset && Height == ElevationType.Ellipsoid)
            {
                sb.AppendFormat("{0}height=ellipsoid", seperator);
            }

            sb.AppendFormat("{0}key={1}", seperator, BingMapsKey);

            return sb.ToString();
        }

        /// <summary>
        /// Returns the Point information as a formatted string. Only the first 1024 points will be used.
        /// </summary>
        /// <returns>A formatted string of point data.</returns>
        public string GetPointsAsString()
        {
            //points=38.8895,77.0501,38.8877,-77.0472,38.8904,-77.0474,38.8896,77.0351

            var sb = new StringBuilder();
            sb.Append("points=");

            if (Points != null && Points.Count > 0)
            {
                int max = Math.Min(Points.Count, 1024);

                for (var i = 0; i < max; i++)
                {
                    //Only need 5 decimal places. Any more are insignificant.
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0:0.#####},{1:0.#####}", Points[i].Latitude, Points[i].Longitude);

                    if (i < max - 1)
                    {
                        sb.Append(",");
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets a list of coordinates that are related to the returned index of the elevation data.
        /// </summary>
        /// <returns>A list of coordinates that are related to the index of the returned elevation data.</returns>
        public List<Coordinate> GetElevationCoordinates()
        {            
            if (Points != null && Points.Count > 0)
            {
                if (GetGeoidOffset || samples == 0)
                {
                    return Points;                    
                }
                else
                {
                    //Calculate distance of polyline
                    double totalDistance = 0;
                    for (int i = 0; i < Points.Count - 1; i++)
                    {
                        totalDistance += SpatialTools.HaversineDistance(Points[i], Points[i + 1], DistanceUnitType.Kilometers);
                    }

                    double segementLength = totalDistance / samples;

                    var coords = new List<Coordinate>(samples);
                    coords.Add(Points[0]);

                    int idx = 0;

                    //Calculate equally spaced coordinates along polyline
                    for(var s = 0; s < samples; s++)
                    {
                        double dist = 0;
                        double travel = segementLength * s;
                        double dx = travel;

                        for (var i = 0; i < Points.Count - 1; i++)
                        {
                            dist += SpatialTools.HaversineDistance(Points[i], Points[i + 1], DistanceUnitType.Kilometers);
                            
                            if (dist >= travel)
                            {
                                idx = i;
                                break;
                            }

                            dx = travel - dist;
                        }

                        if (dx != 0 && idx < Points.Count - 1)
                        {
                            var bearing = SpatialTools.CalculateBearing(Points[idx], Points[idx + 1]);
                            coords.Add(SpatialTools.CalculateCoord(Points[idx], bearing, dx, DistanceUnitType.Kilometers));
                        }
                    }

                    return coords;
                }
            }
            else if (Bounds != null)
            {
                double dLat = Math.Abs(Bounds.NorthLatitude - Bounds.SouthLatitude) / row;
                double dLon = Math.Abs(Bounds.WestLongitude - Bounds.EastLongitude) / col;

                double x, y;

                var coords = new Coordinate[row * col];
                //The elevation values are ordered starting with the southwest corner, and then proceed west to east and south to north.
                for (int r = 0; r < row; r++)
                {
                    y = Bounds.SouthLatitude + (dLat * r);

                    for (int c = 0; c < col; c++)
                    {
                        x = Bounds.WestLongitude + (dLon * c);

                        int idx = r * row + c;

                        coords[idx] = new Coordinate()
                        {
                            Latitude = y,
                            Longitude = x
                        };
                    }
                }

                return new List<Coordinate>(coords);
            }

            return null;
        }

        #endregion
    }
}
