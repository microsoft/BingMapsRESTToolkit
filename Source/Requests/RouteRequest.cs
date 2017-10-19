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
using System.Text;
using System.Threading.Tasks;
using System.Linq;

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
        /// You can have a maximum of 10 viaWaypoints between each set of waypoints. 
        /// The start and end points of the route cannot be viaWaypoints.
        /// </summary>
        public List<SimpleWaypoint> Waypoints { get; set; }

        /// <summary>
        /// Options to use when calculate route.
        /// </summary>
        public RouteOptions RouteOptions { get; set; }

        private int batchSize = 25;

        /// <summary>
        /// The maximium number of waypoints that can be in a single request. If the batchSize is smaller than the number of waypoints, when the request is executed, it will break the request up into multiple requests. Must by between 2 and 25. Default: 25.
        /// </summary>
        public int BatchSize
        {
            get
            {
                return batchSize;
            }
            set
            {
                if(value >=2 && value <= 25)
                {
                    batchSize = value;
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Executes the request. If there are more waypoints than the batchSize value (default 25), only a MaxSolutions is set to 1, and Tolerances is set to null.
        /// </summary>
        /// <returns>A response containing the requested data.</returns>
        public override async Task<Response> Execute()
        {
            return await this.Execute(null);
        }

        /// <summary>
        /// Executes the request. If there are more waypoints than the batchSize value (default 25), only a MaxSolutions is set to 1, and Tolerances is set to null.
        /// </summary>
        /// <param name="remainingTimeCallback">A callback function in which the estimated remaining time is sent.</param>
        /// <returns>A response containing the requested data.</returns>
        public override async Task<Response> Execute(Action<int> remainingTimeCallback)
        {
            if(Waypoints.Count <= batchSize)
            {
                return await base.Execute();
            }

            //There is more waypoints than the batchSize value (default 25), break it up into multiple requests. Only allow a single route in the response and no tolerances.
            if (RouteOptions != null)
            {
                if (RouteOptions.MaxSolutions > 1)
                {
                    RouteOptions.MaxSolutions = 1;
                }

                RouteOptions.Tolerances = null;
            }

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
            
            int startIdx = 0;
            int endIdx = 0;

            var requestUrls = new List<string>();

            while (endIdx < Waypoints.Count - 1)
            {
                requestUrls.Add(GetRequestUrl(startIdx, out endIdx));
                startIdx = endIdx - 1;
            }

            var routes = new Route[requestUrls.Count];
            Response response = null;
            Response errorResponse = null;

            Parallel.For(0, requestUrls.Count, (i) =>
            {
                try
                {
                    //Make the call synchronously as we are in a parrallel for loop and need this to block, otherwise the for loop will exist before the async code has completed.
                    using (var responseStream = ServiceHelper.GetStreamAsync(new Uri(requestUrls[i])).GetAwaiter().GetResult())
                    {
                        var r = ServiceHelper.DeserializeStream<Response>(responseStream);

                        if (r != null)
                        {
                            if (r.ErrorDetails != null && r.ErrorDetails.Length > 0)
                            {
                                errorResponse = r;
                            }
                            else if (r.ResourceSets != null && r.ResourceSets.Length > 0 &&
                                r.ResourceSets[0].Resources != null && r.ResourceSets[0].Resources.Length > 0)
                            {
                                routes[i] = r.ResourceSets[0].Resources[0] as Route;
                            }
                        }

                        if (i == 0)
                        {
                            response = r;
                        }
                    }
                }
                catch (Exception ex)
                {
                    errorResponse = new Response()
                    {
                        ErrorDetails = new string[]
                        {
                            ex.Message
                        }
                    };
                }
            });

            //If any of the responses failed to process, do not merge results, return the error info. 
            if(errorResponse != null)
            {
                return errorResponse;
            }

            response.ResourceSets[0].Resources[0] = await MergeRoutes(routes);

            return response;
        }

        /// <summary>
        /// Gets the request URL to perform a query for route directions. This method will only generate a URL that includes the first batchSize waypoints in the request. 
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

            int endIdx;

            return GetRequestUrl(0, out endIdx);
        }

        #endregion

        /// <summary>
        /// Generates a route REST request 
        /// </summary>
        /// <param name="startIdx"></param>
        /// <param name="endIdx"></param>
        /// <returns></returns>
        private string GetRequestUrl(int startIdx, out int endIdx)
        {
            endIdx = Waypoints.Count;

            var sb = new StringBuilder(this.Domain);

            var TravelMode = (RouteOptions != null) ? RouteOptions.TravelMode : TravelModeType.Driving;

            sb.AppendFormat("Routes/{0}?", Enum.GetName(typeof(TravelModeType), TravelMode));

            int wayCnt = 0, viaCnt = 0;

            for (int i = startIdx; i < Waypoints.Count; i++)
            {
                if (Waypoints[i].IsViaPoint)
                {
                    sb.AppendFormat("&vwp.{0}=", i - startIdx);
                    viaCnt++;

                    if (TravelMode == TravelModeType.Transit)
                    {
                        throw new Exception("ViaWaypoints not supported for Transit directions.");
                    }
                }
                else
                {
                    sb.AppendFormat("&wp.{0}=", i - startIdx);

                    if (viaCnt > 10)
                    {
                        throw new Exception("More than 10 viaWaypoints between waypoints.");
                    }

                    wayCnt++;
                    viaCnt = 0;
                }

                if (Waypoints[i].Coordinate != null)
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0:0.#####},{1:0.#####}", Waypoints[i].Coordinate.Latitude, Waypoints[i].Coordinate.Longitude);
                }
                else
                {
                    sb.AppendFormat("{0}", Uri.EscapeDataString(Waypoints[i].Address));
                }

                //Only allow up to the batchSize waypoints in a request.
                if (wayCnt == batchSize)
                {
                    endIdx = i;
                    break;
                }
            }

            if (RouteOptions != null)
            {
                sb.Append(RouteOptions.GetUrlParam(startIdx));
            }

            sb.Append(GetBaseRequestUrl());

            return sb.ToString();
        }

        /// <summary>
        /// Merges an array of Route objects together into a single route object. 
        /// </summary>
        /// <param name="routes">Array of Route objects to merge.</param>
        /// <returns>A single route that consists of all the merged routes.</returns>
        private Task<Route> MergeRoutes(Route[] routes)
        {
            return Task<Route>.Run(() =>
            {
                if (routes.Length > 0)
                {
                    var mergedRoute = routes[0];

                    for (var i = 1; i < routes.Length; i++)
                    {
                        if (routes[i] != null)
                        {
                            mergedRoute.Id = null;

                            if (mergedRoute.BoundingBox != null)
                            {
                                if (routes[i].BoundingBox != null)
                                {
                                    mergedRoute.BoundingBox[0] = Math.Min(mergedRoute.BoundingBox[0], routes[i].BoundingBox[0]);
                                    mergedRoute.BoundingBox[1] = Math.Min(mergedRoute.BoundingBox[1], routes[i].BoundingBox[1]);
                                    mergedRoute.BoundingBox[2] = Math.Max(mergedRoute.BoundingBox[2], routes[i].BoundingBox[2]);
                                    mergedRoute.BoundingBox[3] = Math.Max(mergedRoute.BoundingBox[3], routes[i].BoundingBox[3]);
                                }
                            }
                            else
                            {
                                mergedRoute.BoundingBox = routes[i].BoundingBox;
                            }

                            int routePathOffset = (mergedRoute.RoutePath != null) ? mergedRoute.RoutePath.Line.Coordinates.Length : 0;

                            //Merge the route legs.
                            var legs = mergedRoute.RouteLegs;

                            //Loop through each leg and offset path indicies to align with merged path.
                            for (var j = 0; j < routes[i].RouteLegs.Length; j++)
                            {
                                for (var k = 0; k < routes[i].RouteLegs[j].ItineraryItems.Length; k++)
                                {
                                    for (var l = 0; l < routes[i].RouteLegs[j].ItineraryItems[k].Details.Length; l++)
                                    {
                                        for (var m = 0; m < routes[i].RouteLegs[j].ItineraryItems[k].Details[l].EndPathIndices.Length; m++)
                                        {
                                            routes[i].RouteLegs[j].ItineraryItems[k].Details[l].EndPathIndices[m] += routePathOffset;
                                        }

                                        for (var m = 0; m < routes[i].RouteLegs[j].ItineraryItems[k].Details[l].StartPathIndices.Length; m++)
                                        {
                                            routes[i].RouteLegs[j].ItineraryItems[k].Details[l].StartPathIndices[m] += routePathOffset;
                                        }
                                    }
                                }

                                for (var k = 0; k < routes[i].RouteLegs[j].RouteSubLegs.Length; k++)
                                {
                                    routes[i].RouteLegs[j].RouteSubLegs[k].EndWaypoint.RoutePathIndex += routePathOffset;
                                    routes[i].RouteLegs[j].RouteSubLegs[k].StartWaypoint.RoutePathIndex += routePathOffset;
                                }
                            }

                            mergedRoute.RouteLegs = mergedRoute.RouteLegs.Concat(routes[i].RouteLegs).ToArray();

                            if (mergedRoute.RoutePath != null)
                            {
                                if (routes[i].RoutePath != null)
                                {
                                    mergedRoute.RoutePath.Line.Coordinates[0] = mergedRoute.RoutePath.Line.Coordinates[0].Concat(routes[i].RoutePath.Line.Coordinates[0]).ToArray();
                                }
                            }
                            else
                            {
                                mergedRoute.RoutePath = routes[i].RoutePath;
                            }

                            mergedRoute.TravelDistance += routes[i].TravelDistance;
                            mergedRoute.TravelDuration += routes[i].TravelDuration;
                            mergedRoute.TravelDurationTraffic += routes[i].TravelDurationTraffic;
                        }
                    }

                    return mergedRoute;
                }

                return null;
            });
        }
    }
}