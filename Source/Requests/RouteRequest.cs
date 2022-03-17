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

        /// <summary>
        /// The maximium number of waypoints that can be in a single request. If the batchSize is smaller than the number of waypoints, 
        /// when the request is executed, it will break the request up into multiple requests. Must by between 2 and 25. Default: 25.
        /// </summary>
        private int batchSize = 25;

        /// <summary>
        /// Hash values used to quickly check if any thing needed for waypoint optimization has changed since the last travelling salesmen calculation.
        /// </summary>
        private int waypointsHash;

        /// <summary>
        /// A hash code value that represents the last waypointed optimized results. This is used to determine if waypoint optimization has already 
        /// been done in a previous requests or if it needs to be calculated again.
        /// </summary>
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
        /// Warning: If travel time or travel distance is used, a standard Bing Maps key will be required, not a session key, as the distance matrix API will be used to process the waypoints.
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
            return await this.Execute(null).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes the request. If there are more waypoints than the batchSize value (default 25), only a MaxSolutions is set to 1, and Tolerances is set to null.
        /// </summary>
        /// <param name="remainingTimeCallback">A callback function in which the estimated remaining time in seconds is sent.</param>
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
                    var tspResult = await TravellingSalesmen.Solve(Waypoints, mode, WaypointOptimization, depart, BingMapsKey).ConfigureAwait(false);
                    Waypoints = tspResult.OptimizedWaypoints;

                    //Update the stored hashes to prevent unneeded optimizations in the future if not needed.
                    waypointsHash = ServiceHelper.GetSequenceHashCode<SimpleWaypoint>(Waypoints);
                    optimizationOptionHash = optionHash;
                }
            }

            var requestUrl = GetRequestUrl();

            int startIdx = 0;
            int endIdx = 0;

            if (Waypoints.Count <= batchSize)
            {
                if (RouteOptions != null && RouteOptions.TravelMode == TravelModeType.Truck)
                {
                    var requestBody = GetTruckPostRequestBody(startIdx, out endIdx);

                    response = await ServiceHelper.MakeAsyncPostRequest(requestUrl, requestBody, remainingTimeCallback).ConfigureAwait(false);
                }
                else
                {
                    remainingTimeCallback?.Invoke(1);

                    using (var responseStream = await ServiceHelper.GetStreamAsync(new Uri(requestUrl)).ConfigureAwait(false))
                    {
                        response = ServiceHelper.DeserializeStream<Response>(responseStream);
                    }
                }
            }
            else
            {
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

                var requestUrls = new List<string>();
                var requestBodies = new List<string>();

                while (endIdx < Waypoints.Count - 1)
                {
                    if (RouteOptions != null && RouteOptions.TravelMode == TravelModeType.Truck)
                    {
                        requestUrls.Add(requestUrl);
                        requestBodies.Add(GetTruckPostRequestBody(startIdx, out endIdx));
                    }
                    else
                    {
                        requestUrls.Add(GetRequestUrl(startIdx, out endIdx));
                        requestBodies.Add(null);
                    }

                    startIdx = endIdx - 1;
                }

                if (remainingTimeCallback != null)
                {
                    int batchProcessingTime = (int)Math.Ceiling((double)requestUrls.Count / (double)ServiceManager.QpsLimit);

                    if (RouteOptions != null && RouteOptions.TravelMode == TravelModeType.Truck)
                    {
                        //Use an average of 4 seconds per batch for processing truck routes as multiplier for the processing time.
                        //Other routes typically take less than a second and as such 1 second is used for those but isn't needed as a multiplier.
                        batchProcessingTime *= 4;
                    }

                    remainingTimeCallback(batchProcessingTime);
                }

                var routes = new Route[requestUrls.Count];
                var routeTasks = new List<Task>();

                for (var i = 0; i < routes.Length; i++)
                {
                    routeTasks.Add(CalculateRoute(requestUrls[i], requestBodies[i], i, routes));
                }

                if (routeTasks.Count > 0)
                {
                    await ServiceHelper.WhenAllTaskLimiter(routeTasks).ConfigureAwait(false);
                }

                try
                {
                    response = new Response()
                    {
                        StatusCode = 200,
                        StatusDescription = "OK",
                        ResourceSets = new ResourceSet[]
                        {
                                new ResourceSet()
                                {
                                    Resources = new Resource[]
                                    {
                                        await MergeRoutes(routes).ConfigureAwait(false)
                                    }
                                }
                        }
                    };
                }
                catch (Exception ex)
                {
                    return new Response()
                    {
                        StatusCode = 500,
                        StatusDescription = "Error",
                        ErrorDetails = new string[]
                        {
                                ex.Message
                        }
                    };
                }
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

            return GetRequestUrl(0, out int endIdx);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generates a route REST request 
        /// </summary>
        /// <param name="startIdx">The starting index for the request when breaking waypoints up into a batch.</param>
        /// <param name="endIdx">The last waypoint index that was in the request.</param>
        /// <returns></returns>
        private string GetRequestUrl(int startIdx, out int endIdx)
        {
            endIdx = Waypoints.Count;

            var sb = new StringBuilder(this.Domain);

            var TravelMode = (RouteOptions != null) ? RouteOptions.TravelMode : TravelModeType.Driving;

            if (TravelMode == TravelModeType.Truck)
            {
                sb.Append("Routes/TruckAsync?");
            }
            else
            {
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
                                if (routes[i].RouteLegs[j].ItineraryItems != null)
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
                        else
                        {
                            throw new Exception("An error occured when calculating one of the route segments.");
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
        /// <param name="startIdx">The starting index for the request when breaking waypoints up into a batch.</param>
        /// <param name="endIdx">The last waypoint index that was in the request.</param>
        /// <returns>A POST body for a truck route request.</returns>
        private string GetTruckPostRequestBody(int startIdx, out int endIdx)
        {
            endIdx = Waypoints.Count;

            var sb = new StringBuilder();

            sb.Append("{");

            sb.Append("\"waypoints\":[");

            int wayCnt = 0;

            for (int i = startIdx; i < Waypoints.Count; i++)
            {
                if (Waypoints[i].Coordinate != null)
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{{\"latitude\":{0:0.#####},\"longitude\":{1:0.#####}", Waypoints[i].Latitude, Waypoints[i].Longitude);
                }
                else if (!string.IsNullOrWhiteSpace(Waypoints[i].Address))
                {
                    sb.AppendFormat("{{\"address\":\"{0}\"", Waypoints[i].Address);
                }
                else
                {
                    throw new Exception("Invalid waypoint, an Address or Coordinate must be specified.");
                }

                if (Waypoints[i].IsViaPoint)
                {
                    sb.Append(",\"isViaPoint\":true},");
                }
                else
                {
                    wayCnt++;
                    sb.Append("},");
                }

                //Only allow up to the batchSize waypoints in a request.
                if (wayCnt == batchSize)
                {
                    endIdx = i;
                    break;
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

                //Truck routing doesn't support max solutions and throws an error if included in the request.
                if (RouteOptions.TravelMode != TravelModeType.Truck && RouteOptions.MaxSolutions > 1 && RouteOptions.MaxSolutions <= 3)
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

                //sb.Append(",\"timeType\":\"minute\"");

                if (RouteOptions.RouteAttributes != null && RouteOptions.RouteAttributes.Count > 0)
                {
                    string rt = "";

                    if (RouteOptions.TravelMode == TravelModeType.Truck) {
                        //Truck supports routePath and regionTravelSummary, and only allows one to be specified.
                        if(RouteOptions.RouteAttributes.Contains(RouteAttributeType.RoutePath) || RouteOptions.RouteAttributes.Contains(RouteAttributeType.All))
                        {
                            rt += "routePath,";
                        }
                        else if (RouteOptions.RouteAttributes.Contains(RouteAttributeType.RegionTravelSummary))
                        {
                            rt += "regionTravelSummary,";
                        }
                        else
                        {
                            rt += ",";
                        }
                    } else {
                        if (RouteOptions.RouteAttributes.Contains(RouteAttributeType.RoutePath) || RouteOptions.RouteAttributes.Contains(RouteAttributeType.All))
                        {
                            rt += "routePath,";
                        }

                        //Route summaries only supported, but other route attributes try to do something similar, so have them short circuit to this option.
                        if (RouteOptions.RouteAttributes.Contains(RouteAttributeType.RouteSummariesOnly) || RouteOptions.RouteAttributes.Contains(RouteAttributeType.All) || RouteOptions.RouteAttributes.Contains(RouteAttributeType.ExcludeItinerary))
                        {
                            rt += "routeSummariesOnly,";
                        }

                        if (RouteOptions.RouteAttributes.Contains(RouteAttributeType.RegionTravelSummary))
                        {
                            rt += "regionTravelSummary,";
                        }
                    }

                    if (!string.IsNullOrEmpty(rt))
                    {
                        sb.Append(",\"routeAttributes\":\"");
                        sb.Append(rt);

                        //Remove trailing comma.
                        sb.Length--;
                        sb.Append("\"");
                    }
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
                        sb.AppendFormat(CultureInfo.InvariantCulture, ",\"vehicleHeight\":{0}", RouteOptions.VehicleSpec.VehicleHeight);
                    }

                    if (RouteOptions.VehicleSpec.VehicleWidth > 0)
                    {
                        sb.AppendFormat(CultureInfo.InvariantCulture, ",\"vehicleWidth\":{0}", RouteOptions.VehicleSpec.VehicleWidth);
                    }

                    if (RouteOptions.VehicleSpec.VehicleLength > 0)
                    {
                        sb.AppendFormat(CultureInfo.InvariantCulture, ",\"vehicleLength\":{0}", RouteOptions.VehicleSpec.VehicleLength);
                    }

                    if (RouteOptions.VehicleSpec.VehicleWeight > 0)
                    {
                        sb.AppendFormat(CultureInfo.InvariantCulture, ",\"vehicleWeight\":{0}", RouteOptions.VehicleSpec.VehicleWeight);
                    }

                    if (RouteOptions.VehicleSpec.VehicleMaxGradient > 0)
                    {
                        sb.AppendFormat(CultureInfo.InvariantCulture, ",\"vehicleMaxGradient\":{0}", RouteOptions.VehicleSpec.VehicleMaxGradient);
                    }

                    if (RouteOptions.VehicleSpec.VehicleMinTurnRadius > 0)
                    {
                        sb.AppendFormat(CultureInfo.InvariantCulture, ",\"vehicleMinTurnRadius\":{0}", RouteOptions.VehicleSpec.VehicleMinTurnRadius);
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

                        //Remove trailing comma and quote.
                        sb.Length--;
                        sb.Append("\"");
                    }

                    sb.Append("}");
                }
            }

            sb.Append("}");

            return sb.ToString();
        }

        /// <summary>
        /// Calculates a route use a request URL and adds it to an array of route results. This is used when batching multiple route calls together when supporting long routes that have more than 25 waypoints.
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        /// <param name="body">The JSON POST body.</param>
        /// <param name="idx">The route index in the batch.</param>
        /// <param name="routes">The arrary in which the batch route results are stored.</param>
        /// <returns>A task for calculating a route as part of a batch.</returns>
        private Task CalculateRoute(string requestUrl, string body, int idx, Route[] routes)
        {
            return Task.Run(() =>
            {
                try
                {
                    Response r = null;

                    //Make the call synchronously as we are in a parrallel for loop and need this to block, otherwise the for loop will exist before the async code has completed.

                    if (!string.IsNullOrEmpty(body))
                    {
                        r = ServiceHelper.MakeAsyncPostRequest(requestUrl, body, null).GetAwaiter().GetResult();
                    }
                    else
                    {
                        using (var responseStream = ServiceHelper.GetStreamAsync(new Uri(requestUrl)).GetAwaiter().GetResult())
                        {
                            r = ServiceHelper.DeserializeStream<Response>(responseStream);
                        }
                    }

                    if (Response.HasResource(r))
                    {
                        routes[idx] = Response.GetFirstResource(r) as Route;
                        return;
                    }

                }
                catch { }

                routes[idx] = null;
            });
        }


        #endregion
    }
}