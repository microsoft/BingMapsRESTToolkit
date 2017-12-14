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
using BingMapsRESTToolkit.Extensions;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// A request that calculates routes between waypoints.
    /// </summary>
    public class RouteRequest : BaseRestRequest
    {
        #region Private Properties

        private int batchSize = 25;

        /// <summary>
        /// Hash values used to quickly check if any thing needed for waypoint optimization has changed since the last travelling salesmen calculation.
        /// </summary>
        private int waypointsHash;
        private string optimizationOptionHash;

        #endregion

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

        /// <summary>
        /// The maximium number of waypoints that can be in a single request. If the batchSize is smaller than the number of waypoints, 
        /// when the request is executed, it will break the request up into multiple requests. Must by between 2 and 25. Default: 25.
        /// </summary>
        public int BatchSize
        {
            get
            {
                return batchSize;
            }
            set
            {
                if (value >= 2 && value <= 25)
                {
                    batchSize = value;
                }
            }
        }

        /// <summary>
        /// Specifies if the waypoint order should be optimized using a travelling salesmen algorithm which metric to optimize on. 
        /// If less than 10 waypoints, brute force is used, for more than 10 waypoints, a genetic algorithm is used. 
        /// Ignores IsViaPoint on waypoints and makes them waypoints.
        /// Default: false
        /// Warning: If travel time or travel distance is used, a standard Bing Maps key will need to be required, not a session key, as the distance matrix API will be used to process the waypoints.
        /// This can generate a lot of billable transactions.
        /// </summary>
        public TspOptimizationType? WaypointOptimization { get; set; }

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
            Response response = null;
            
            if (WaypointOptimization != null && WaypointOptimization.HasValue && Waypoints.Count >= 2)
            {
                var wpHash = ServiceHelper.GetSequenceHashCode<SimpleWaypoint>(Waypoints);

                var optionHash = Enum.GetName(typeof(TspOptimizationType), WaypointOptimization);
                var mode = TravelModeType.Driving;
                DateTime? depart = null;

                if (RouteOptions != null)
                {
                    optionHash += "|" + Enum.GetName(typeof(RouteTimeType), RouteOptions.TimeType);

                    if (RouteOptions.TimeType == RouteTimeType.Departure && RouteOptions.DateTime != null && RouteOptions.DateTime.HasValue)
                    {
                        depart = RouteOptions.DateTime;
                        optionHash += "|" + RouteOptions.DateTime.ToString();
                    }

                    mode = RouteOptions.TravelMode;

                    optionHash += "|" + Enum.GetName(typeof(TravelModeType), mode);
                }
                else
                {
                    optionHash += "|" + Enum.GetName(typeof(RouteTimeType), RouteTimeType.Departure); 
                }
                
                //Check to see if the waypoints have changed since they were last optimized. 
                if (waypointsHash != wpHash || string.Compare(optimizationOptionHash, optionHash) != 0)
                {
                    var tspResult = await TravellingSalesmen.Solve(Waypoints, mode, WaypointOptimization, depart, BingMapsKey);
                    Waypoints = tspResult.OptimizedWaypoints;

                    //Update the stored hashes to prevent unneeded optimizations in the future if not needed.
                    waypointsHash = ServiceHelper.GetSequenceHashCode<SimpleWaypoint>(Waypoints);
                    optimizationOptionHash = optionHash;
                }
            }

            var requestUrl = GetRequestUrl();

            if (RouteOptions != null && RouteOptions.TravelMode == TravelModeType.Truck)
            {
                var requestBody = GetTruckPostRequestBody();

                response = await ServiceHelper.MakeAsyncPostRequest<Route>(requestUrl, requestBody, remainingTimeCallback);
            }
            else
            {
                if (Waypoints.Count <= batchSize)
                {
                    using (var responseStream = await ServiceHelper.GetStreamAsync(new Uri(requestUrl)))
                    {
                        return ServiceHelper.DeserializeStream<Response>(responseStream);
                    }
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
                if (errorResponse != null)
                {
                    return errorResponse;
                }

                response.ResourceSets[0].Resources[0] = await MergeRoutes(routes);
            }

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

        #region Private Methods

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

            if (TravelMode != TravelModeType.Truck)
            {
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

        /// <summary>
        /// Gets a POST body for a truck route request.
        /// </summary>
        /// <returns>A POST body for a truck route request.</returns>
        private string GetTruckPostRequestBody()
        {
            var sb = new StringBuilder();

            sb.Append("{");

            if (Waypoints.Count > 25)
            {
                throw new Exception("More than 25 waypoints or viaWaypoints specified.");
            }

            sb.Append("\"waypoints\":[");

            foreach (var wp in Waypoints)
            {
                if (wp.Coordinate != null)
                {
                    sb.AppendFormat("{{\"latitude\":{0:0.#####},\"longitude\":{1:0.#####}", wp.Latitude, wp.Longitude);
                }
                else if (!string.IsNullOrWhiteSpace(wp.Address))
                {
                    sb.AppendFormat("{{\"address\":\"{0}\"", wp.Address);
                }
                else
                {
                    throw new Exception("Invalid waypoint, an Address or Coordinate must be specified.");
                }

                if (wp.IsViaPoint)
                {
                    sb.Append(",\"isViaPoint\":true},");
                }
                else
                {
                    sb.Append("},");
                }
            }

            //Remove trailing comma.
            sb.Length--;
            sb.Append("]");

            if (RouteOptions != null)
            {
                if (RouteOptions.Avoid != null && RouteOptions.Avoid.Count > 0)
                {
                    sb.Append(",\"avoid\":\"");

                    for (var i = 0; i < RouteOptions.Avoid.Count; i++)
                    {
                        sb.Append(Enum.GetName(typeof(AvoidType), RouteOptions.Avoid[i]));
                        sb.Append(",");
                    }

                    //Remove trailing comma.
                    sb.Length--;
                    sb.Append("\"");
                }

                if (RouteOptions.DistanceBeforeFirstTurn > 0)
                {
                    sb.AppendFormat(",\"distanceBeforeFirstTurn\":{0}", RouteOptions.DistanceBeforeFirstTurn);
                }

                if (RouteOptions.Heading.HasValue)
                {
                    sb.AppendFormat(",\"heading\":{0}", RouteOptions.Heading.Value);
                }

                if (RouteOptions.MaxSolutions > 1 && RouteOptions.MaxSolutions <= 3)
                {
                    sb.AppendFormat(",\"maxSolutions\":{0}", RouteOptions.MaxSolutions);
                }

                if (RouteOptions.Optimize == RouteOptimizationType.Time)
                {
                    sb.Append(",\"optimize\":\"time\"");
                }
                else if (RouteOptions.Optimize == RouteOptimizationType.TimeWithTraffic)
                {
                    sb.Append(",\"optimize\":\"timeWithTraffic\"");
                }
                else
                {
                    throw new Exception("Truck routes must be optimized based on time or timeWithTraffic.");
                }

                if (RouteOptions.RouteAttributes != null && RouteOptions.RouteAttributes.Count > 0)
                {
                    sb.Append(",\"routeAttributes\":\"");

                    //Route summaries only supported, but other route attributes try to do something similar, so have them short circuit to this option.
                    if(RouteOptions.RouteAttributes.Contains(RouteAttributeType.RouteSummariesOnly) || RouteOptions.RouteAttributes.Contains(RouteAttributeType.All) || RouteOptions.RouteAttributes.Contains(RouteAttributeType.ExcludeItinerary))
                    {
                        sb.Append("routeSummariesOnly,");
                    }

                    if (RouteOptions.RouteAttributes.Contains(RouteAttributeType.RoutePath) || RouteOptions.RouteAttributes.Contains(RouteAttributeType.All))
                    {
                        sb.Append("routePath,");
                    }

                    //Remove trailing comma.
                    sb.Length--;
                    sb.Append("\"");
                }

                if (RouteOptions.DistanceUnits == DistanceUnitType.Kilometers)
                {
                    sb.Append(",\"distanceUnit\":\"kilometer\"");
                }
                else
                { 
                    sb.Append(",\"distanceUnit\":\"mile\"");
                }

                if (RouteOptions.DateTime != null)
                {
                    sb.AppendFormat(DateTimeFormatInfo.InvariantInfo, ",\"dateTime\":\"{0:G}\"", RouteOptions.DateTime);
                }

                if (RouteOptions.Tolerances != null && RouteOptions.Tolerances.Count > 0)
                {
                    sb.Append(",\"tolerances\":[");

                    int cnt = Math.Min(RouteOptions.Tolerances.Count, 7);

                    for (int i = 0; i < cnt; i++)
                    {
                        sb.AppendFormat(CultureInfo.InvariantCulture, "{0:0.######}", RouteOptions.Tolerances[i]);
                        sb.Append(",");
                    }

                    //Remove trailing comma.
                    sb.Length--;
                    sb.Append("]");
                }

                if (RouteOptions.VehicleSpec != null)
                {
                    sb.Append(",\"vehicleSpec\":{");
                    sb.AppendFormat("\"dimensionUnit\":\"{0}\"", Enum.GetName(typeof(DimensionUnitType), RouteOptions.VehicleSpec.DimensionUnit).ToLowerInvariant());
                    sb.AppendFormat(",\"weightUnit\":\"{0}\"", Enum.GetName(typeof(WeightUnitType), RouteOptions.VehicleSpec.WeightUnit).ToLowerInvariant());
                    
                    if (RouteOptions.VehicleSpec.VehicleAvoidCrossWind)
                    {
                        sb.Append(",\"vehicleAvoidCrossWind\":true");
                    }

                    if (RouteOptions.VehicleSpec.VehicleAvoidGroundingRisk)
                    {
                        sb.Append(",\"vehicleAvoidGroundingRisk\":true");
                    }

                    if (RouteOptions.VehicleSpec.VehicleSemi)
                    {
                        sb.Append(",\"VehicleSemi\":true");
                    }

                    if (RouteOptions.VehicleSpec.VehicleAxles > 1)
                    {
                        sb.AppendFormat(",\"vehicleAxles\":{0}", RouteOptions.VehicleSpec.VehicleAxles);
                    }

                    if (RouteOptions.VehicleSpec.VehicleHeight > 0)
                    {
                        sb.AppendFormat(",\"vehicleHeight\":{0}", RouteOptions.VehicleSpec.VehicleHeight);
                    }

                    if (RouteOptions.VehicleSpec.VehicleWidth > 0)
                    {
                        sb.AppendFormat(",\"vehicleWidth\":{0}", RouteOptions.VehicleSpec.VehicleWidth);
                    }

                    if (RouteOptions.VehicleSpec.VehicleLength > 0)
                    {
                        sb.AppendFormat(",\"vehicleLength\":{0}", RouteOptions.VehicleSpec.VehicleLength);
                    }

                    if (RouteOptions.VehicleSpec.VehicleWeight > 0)
                    {
                        sb.AppendFormat(",\"vehicleWeight\":{0}", RouteOptions.VehicleSpec.VehicleWeight);
                    }

                    if (RouteOptions.VehicleSpec.VehicleMaxGradient > 0)
                    {
                        sb.AppendFormat(",\"vehicleMaxGradient\":{0}", RouteOptions.VehicleSpec.VehicleMaxGradient);
                    }

                    if (RouteOptions.VehicleSpec.VehicleMinTurnRadius > 0)
                    {
                        sb.AppendFormat(",\"vehicleMinTurnRadius\":{0}", RouteOptions.VehicleSpec.VehicleMinTurnRadius);
                    }

                    if (RouteOptions.VehicleSpec.VehicleTrailers > 0)
                    {
                        sb.AppendFormat(",\"vehicleTrailers\":{0}", RouteOptions.VehicleSpec.VehicleTrailers);
                    }

                    if (RouteOptions.VehicleSpec.VehicleHazardousMaterials != null && RouteOptions.VehicleSpec.VehicleHazardousMaterials.Count > 0 &&
                        !(RouteOptions.VehicleSpec.VehicleHazardousMaterials.Count == 1 && RouteOptions.VehicleSpec.VehicleHazardousMaterials[0] == HazardousMaterialType.None))
                    {
                        sb.Append(",\"vehicleHazardousMaterials\":\"");

                        foreach (var vhm in RouteOptions.VehicleSpec.VehicleHazardousMaterials)
                        {
                            sb.Append(EnumHelper.HazardousMaterialTypeToString(vhm));
                            sb.Append(",");
                        }

                        //Remove trailing comma.
                        sb.Length--;
                        sb.Append("\"");
                    }

                    if (RouteOptions.VehicleSpec.VehicleHazardousPermits != null && RouteOptions.VehicleSpec.VehicleHazardousPermits.Count > 0 &&
                        !(RouteOptions.VehicleSpec.VehicleHazardousPermits.Count == 1 && RouteOptions.VehicleSpec.VehicleHazardousPermits[0] == HazardousMaterialPermitType.None))
                    {
                        sb.Append(",\"vehicleHazardousPermits\":\"");

                        foreach (var vhp in RouteOptions.VehicleSpec.VehicleHazardousPermits)
                        {
                            sb.Append(EnumHelper.HazardousMaterialPermitTypeToString(vhp));
                            sb.Append(",");
                        }

                        //Remove trailing comma.
                        sb.Length--;
                        sb.Append("\"");
                    }

                    sb.Append("}");
                }
            }

            sb.Append("}");

            return sb.ToString();
        }

        #endregion
    }
}