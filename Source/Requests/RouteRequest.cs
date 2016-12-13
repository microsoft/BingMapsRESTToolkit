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
using System.Text;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// A request that calculates routes between waypoints.
    /// </summary>
    public class RouteRequest : BaseRestRequest
    {
        #region Public Properties

        /// <summary>
        /// Specifies two or more locations that define the route and that are in sequential order. 
        /// A route is defined by a set of waypoints and viaWaypoints (intermediate locations that the route must pass through). 
        /// You can have a maximum of 25 waypoints, and a maximum of 10 viaWaypoints between each set of waypoints. 
        /// The start and end points of the route cannot be viaWaypoints.
        /// </summary>
        public List<SimpleWaypoint> Waypoints { get; set; }

        /// <summary>
        /// Options to use when calculate route.
        /// </summary>
        public RouteOptions RouteOptions { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the request URL to perform a query for route directions.
        /// </summary>
        /// <returns>A request URL to perform a query for route directions.</returns>
        public override string GetRequestUrl()
        {
            //https://dev.virtualearth.net/REST/v1/Routes?wayPoint.1=wayPoint1&viaWaypoint.2=viaWaypoint2&waypoint.3=wayPoint3&wayPoint.n=wayPointn&heading=heading&optimize=optimize&avoid=avoidOptions&distanceBeforeFirstTurn=distanceBeforeFirstTurn&routeAttributes=routeAttributes&maxSolutions=maxSolutions&tolerances=tolerance1,tolerance2,tolerancen&distanceUnit=distanceUnit&mfa=mfa&key=BingMapsKey

            if (Waypoints == null)
            {
                throw new Exception("Waypoints not specified.");
            }
            else if (Waypoints.Count < 2)
            {
                throw new Exception("Not enough Waypoints specified.");
            }
            else if (Waypoints[0].IsViaPoint || Waypoints[Waypoints.Count - 1].IsViaPoint)
            {
                throw new Exception("Start and end waypoints must not be ViaWaypoints.");
            }

            var sb = new StringBuilder();

            var TravelMode = (RouteOptions != null) ? RouteOptions.TravelMode : TravelModeType.Driving;

            sb.AppendFormat("https://dev.virtualearth.net/REST/v1/Routes/{0}?", Enum.GetName(typeof(TravelModeType), TravelMode));

            int wayCnt = 0, viaCnt = 0;

            for (int i = 0; i < Waypoints.Count; i++)
            {
                if (Waypoints[i].IsViaPoint)
                {
                    sb.AppendFormat("&vwp.{0}=", i);
                    viaCnt++;

                    if (TravelMode == TravelModeType.Transit)
                    {
                        throw new Exception("ViaWaypoints not supported for Transit directions.");
                    }
                }
                else
                {
                    sb.AppendFormat("&wp.{0}=", i);                    

                    if (viaCnt > 10)
                    {
                        throw new Exception("More than 10 viaWaypoints between waypoints.");
                    }

                    wayCnt++;
                    viaCnt = 0;
                }

                if (Waypoints[i].Coordinate != null)
                {
                    sb.AppendFormat("{0:0.#####},{1:0.#####}", Waypoints[i].Coordinate.Latitude, Waypoints[i].Coordinate.Longitude);
                }
                else
                {
                    sb.AppendFormat("{0}", Uri.EscapeDataString(Waypoints[i].Address));
                }
            }

            if (wayCnt > 25)
            {
                throw new Exception("More than 25 waypoints in route request.");
            }

            if (RouteOptions != null)
            {
                sb.Append(RouteOptions.GetUrlParam());
            }    

            sb.Append(GetBaseRequestUrl());

            return sb.ToString();
        }

        #endregion
    }
}