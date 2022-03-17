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

using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// A simple waypoint class that can be used to calculate a route.
    /// </summary>
    [DataContract]
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

        /// <summary>
        /// A waypoint to calculate a route through.
        /// </summary>
        /// <param name="latitude">The latitude coordinate.</param>
        /// <param name="longitude">The longitude coordinate.</param>
        public SimpleWaypoint(double latitude, double longitude)
        {
            this.Coordinate = new Coordinate(latitude, longitude);
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// The coordinate of the waypoint. When specified this will be used instead of the Address value in requests.
        /// </summary>
        public Coordinate Coordinate { get; set; }

        /// <summary>
        /// A serializable Latitude property.
        /// </summary>
        [DataMember(Name = "latitude")]
        internal double? Latitude
        {
            get
            {
                if (Coordinate != null)
                {
                    return Coordinate.Latitude;
                }

                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    if (Coordinate == null)
                    {
                        Coordinate = new Coordinate(value.Value, 0);
                    }
                    else
                    {
                        Coordinate.Latitude = value.Value;
                    }
                }
                else
                {
                    Coordinate = null;
                }
            }
        }

        /// <summary>
        /// A serializable longitude property.
        /// </summary>
        [DataMember(Name = "longitude")]
        internal double? Longitude
        {
            get
            {
                if (Coordinate != null)
                {
                    return Coordinate.Longitude;
                }

                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    if (Coordinate == null)
                    {
                        Coordinate = new Coordinate(0, value.Value);
                    }
                    else
                    {
                        Coordinate.Longitude = value.Value;
                    }
                }
                else
                {
                    Coordinate = null;
                }
            }
        }

        /// <summary>
        /// The address query for the waypoint. 
        /// </summary>
        [DataMember(Name = "address")]
        public string Address { get; set; }

        /// <summary>
        /// A bool indicating whether the waypoint is a via point.
        /// </summary>
        [DataMember(Name = "isViaPoint")]
        public bool IsViaPoint { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Compares two simple waypoints. Intially only concern with coordinates, but if no coordinates specified, falls back on comparing address. Ignored IsViaPoint.
        /// </summary>
        /// <param name="obj">Simple waypoint to compare to.</param>
        /// <returns>A boolean indicating if the two simple waypoints are equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj != null && obj is SimpleWaypoint)
            {
                var wp = obj as SimpleWaypoint;

                if ((Coordinate == null && wp.Coordinate != null) || (Coordinate != null && wp.Coordinate == null))
                {
                    return false;
                }
                else if (Coordinate != null && wp.Coordinate != null)
                {
                    //Limit comparison to coordinates.
                    return Coordinate.Equals(wp.Coordinate);
                }
                else if (string.Compare(Address, wp.Address, System.StringComparison.OrdinalIgnoreCase) == 0)
                {
                    //Compare address strings.
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get hash for simple waypoint.
        /// </summary>
        /// <returns>Hash for simple waypoint.</returns>
        public override int GetHashCode()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}|{1:0.######}|{2:0.######}", Address, Latitude, Longitude).GetHashCode();
        }

        /// <summary>
        /// Tries to geocode a simple waypoint. 
        /// </summary>
        /// <param name="waypoint">The simple waypoint to geocode.</param>
        /// <param name="bingMapsKey">The Bing Maps key to use when geocoding.</param>
        /// <returns>A Task in which the simple waypoint will be geocoded.</returns>
        public static async Task TryGeocode(SimpleWaypoint waypoint, string bingMapsKey)
        {
            var request = new GeocodeRequest()
            {
                BingMapsKey = bingMapsKey
            };

            await TryGeocode(waypoint, request).ConfigureAwait(false);
        }

        /// <summary>
        /// Tries to geocode a simple waypoint. 
        /// </summary>
        /// <param name="waypoint">The simple waypoint to geocode.</param>
        /// <param name="baseRequest">A base request that has the information need to perform a geocode, primarily a Bing Maps key.</param>
        /// <returns>A Task in which the simple waypoint will be geocoded.</returns>
        public static async Task TryGeocode(SimpleWaypoint waypoint, BaseRestRequest baseRequest)
        {
            if (waypoint != null && waypoint.Coordinate == null && !string.IsNullOrEmpty(waypoint.Address))
            {
                var request = new GeocodeRequest()
                {
                    Query = waypoint.Address,
                    MaxResults = 1,
                    BingMapsKey = baseRequest.BingMapsKey,
                    Culture = baseRequest.Culture,
                    Domain = baseRequest.Domain,
                    UserIp = baseRequest.UserIp,
                    UserLocation = baseRequest.UserLocation,
                    UserMapView = baseRequest.UserMapView,
                    UserRegion = baseRequest.UserRegion
                };

                try
                {
                    var r = await ServiceManager.GetResponseAsync(request).ConfigureAwait(false);

                    if (Response.HasResource(r))
                    {
                        var l = Response.GetFirstResource(r) as Location;

                        waypoint.Coordinate = new Coordinate(l.Point.Coordinates[0], l.Point.Coordinates[1]);
                    }
                }
                catch
                {
                    //Do nothing.
                }
            }
        }

        /// <summary>
        /// Attempts to geocode a list of simple waypoints.
        /// </summary>
        /// <param name="waypoints">A list of simple waypoints to geocode.</param>
        /// <param name="baseRequest">A base request that has the information need to perform a geocode, primarily a Bing Maps key.</param>
        /// <returns>A Task in which a list of simple waypoints will be geocoded.</returns>
        public static async Task TryGeocodeWaypoints(List<SimpleWaypoint> waypoints, BaseRestRequest baseRequest)
        {
            var geocodeTasks = new List<Task>();

            foreach (var wp in waypoints)
            {
                if (wp != null && wp.Coordinate == null && !string.IsNullOrEmpty(wp.Address))
                {
                    geocodeTasks.Add(TryGeocode(wp, baseRequest));
                }
            }

            if (geocodeTasks.Count > 0)
            {
                await ServiceHelper.WhenAllTaskLimiter(geocodeTasks).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Attempts to geocode a list of simple waypoints.
        /// </summary>
        /// <param name="waypoints">A list of simple waypoints to geocode.</param>
        /// <param name="bingMapsKey">The Bing Maps key to use when geocoding.</param>
        /// <returns>A Task in which a list of simple waypoints will be geocoded.</returns>
        public static async Task TryGeocodeWaypoints(List<SimpleWaypoint> waypoints, string bingMapsKey)
        {
            var request = new GeocodeRequest()
            {
                BingMapsKey = bingMapsKey
            };

            await TryGeocodeWaypoints(waypoints, request).ConfigureAwait(false);
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Parses a simple waypoint value from a string. If it has the format "latitude,longitude", it will be used as a coordinate, otherwise as an address. 
        /// </summary>
        /// <param name="waypointString">String containing a coordinate or address</param>
        /// <returns>A simple waypoint.</returns>
        public static SimpleWaypoint Parse(string waypointString)
        {
            var c = Coordinate.Parse(waypointString);

            if (c == null)
            {
                return new SimpleWaypoint(waypointString);
            }

            return new SimpleWaypoint(c);
        }

        #endregion

        public override string ToString()
        {
            if(!string.IsNullOrWhiteSpace(Address))
            {
                return Address;
            }

            return string.Format(CultureInfo.InvariantCulture, "{0:0.######}, {1:0.######}", Latitude, Longitude);
        }
    }
}
