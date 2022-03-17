/*
 * Copyright(c) 2018 Microsoft Corporation. All rights reserved. 
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

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// Returns a list of local entities within the specified maximum driving time or distance traveled from a specified point on Earth.
    /// </summary>
    public class LocalInsightsRequest : BaseRestRequest
    {
        #region Constructor

        /// <summary>
        /// Constructor with default values
        /// </summary>
        public LocalInsightsRequest() : base()
        {
            Optimize = RouteOptimizationType.Time;
            TimeUnit = TimeUnitType.Second;
            DistanceUnit = DistanceUnitType.Kilometers;
            TravelMode = TravelModeType.Driving;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The query or coordinates for a location around which the isochrone is created. 
        /// </summary>
        public SimpleWaypoint Waypoint { get; set; }

        /// <summary>
        /// Required, if optimize is Time or TimeWithTraffic. The longest possible travel time used to generate the isochrone.
        /// Any positive integer less than or equal to the maximum time, which is 60 minutes.
        /// Note: Cannot be used when maxDistance is specified.
        /// </summary>
        public double? MaxTime { get; set; }

        /// <summary>
        /// The unit of time for the parameter maxTime.
        /// Default: Second
        /// </summary>
        public TimeUnitType TimeUnit { get; set; }

        /// <summary>
        /// If travelModel is Driving. The datetime parameter identifies the desired departure time used to return the list of local entities 
        /// within the specified maximum driving time as specified using the maxTime parameter.
        /// </summary>
        public DateTime? DateTime { get; set; }

        /// <summary>
        /// Required, if travelMode is Driving or Walking. The longest possible distance used define the geographic region in which to search for local entities.
        /// Any positive integer less than or equal to 50 miles.
        /// Note: Distance-based Local Insight API calls are unavailable for transit.
        /// Note: Cannot be used when maxTime is specified.
        /// </summary>
        public double? MaxDistance { get; set; }

        /// <summary>
        /// The unit of distance for the maxDistance parameter.
        /// Default: Kilometers.
        /// </summary>
        public DistanceUnitType DistanceUnit { get; set; }

        /// <summary>
        /// Specifies what parameters to use to optimize the isochrone route.
        /// </summary>
        public RouteOptimizationType Optimize { get; set; }

        /// <summary>
        /// Indicates the which routing profile to snap the points to.
        /// </summary>
        public TravelModeType TravelMode { get; set; }

        /// <summary>
        /// Required. The specified types used to filter the local entities returned by the Local Search API.
        /// A comma-separated list of string type identifiers. 
        /// See the list of available Type IDs https://docs.microsoft.com/en-us/bingmaps/rest-services/common-parameters-and-types/type-identifiers/
        /// </summary>
        public List<string> Types { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Executes the request. 
        /// </summary>
        /// <returns>A response containing the requested data.</returns>
        public override async Task<Response> Execute()
        {
            return await this.Execute(null).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes the request. 
        /// </summary>
        /// <param name="remainingTimeCallback">A callback function in which the estimated remaining time in seconds is sent.</param>
        /// <returns>A response containing the requested data.</returns>
        public override async Task<Response> Execute(Action<int> remainingTimeCallback)
        {
            var requestUrl = GetRequestUrl();

            return await ServiceHelper.MakeAsyncGetRequest(requestUrl, remainingTimeCallback).ConfigureAwait(false);
        }

        public override string GetRequestUrl()
        {
            if (Waypoint == null || (Waypoint.Coordinate == null && string.IsNullOrWhiteSpace(Waypoint.Address)))
            {
                throw new Exception("A waypoiny must be specified.");
            }

            if (Types == null || Types.Count == 0)
            {
                throw new Exception("One or more types must be specified.");
            }

            //Truck mode is not supported, so fall back to driving. 
            if (TravelMode == TravelModeType.Truck)
            {
                TravelMode = TravelModeType.Driving;
            }

            var sb = new StringBuilder(this.Domain);
            sb.AppendFormat("Routes/LocalInsightsAsync?travelMode={0}", Enum.GetName(typeof(TravelModeType), TravelMode));

            if (Waypoint.Coordinate != null)
            {
                sb.AppendFormat("&waypoint={0}", Waypoint.Coordinate.ToString());
            }
            else if (!string.IsNullOrWhiteSpace(Waypoint.Address))
            {
                sb.AppendFormat("&waypoint={0}", string.Join(",", Waypoint.Address));
            }

            if (MaxTime > 0)
            {
                if (TimeUnit == TimeUnitType.Second && MaxTime > 3600)
                {
                    throw new Exception("MaxTime value must be <= 3600 seconds.");
                }
                else if (TimeUnit == TimeUnitType.Minute && MaxTime > 60)
                {
                    throw new Exception("MaxTime value must be <= 60 minutes.");
                }

                sb.AppendFormat("&maxTime={0}&timeUnit={1}", MaxTime, Enum.GetName(typeof(TimeUnitType), TimeUnit));

                if (TravelMode != TravelModeType.Walking && DateTime != null && DateTime.HasValue)
                {
                    sb.AppendFormat(DateTimeFormatInfo.InvariantInfo, "&dt={0:G}", DateTime.Value);
                }

                //Can only optimize based on time or time with traffic when generating time based isochrones.
                if (Optimize != RouteOptimizationType.Time && Optimize != RouteOptimizationType.TimeWithTraffic)
                {
                    Optimize = RouteOptimizationType.Time;
                }
            }
            else if (MaxDistance > 0)
            {
                if (TravelMode == TravelModeType.Transit)
                {
                    throw new Exception("Distance based isochrones are not supported for transit travel mode. Use maxTime.");
                }

                sb.AppendFormat(CultureInfo.InvariantCulture, "&maxDistance={0}&distanceUnit={1}", MaxDistance, EnumHelper.DistanceUnitTypeToString(DistanceUnit));

                //Can only optimize based on distance when generating distance based isochrones.
                if (Optimize != RouteOptimizationType.Distance)
                {
                    Optimize = RouteOptimizationType.Distance;
                }
            }
            else
            {
                throw new Exception("A max time or distance must be specified.");
            }

            sb.AppendFormat("&optimize={0}", Enum.GetName(typeof(RouteOptimizationType), Optimize));
            sb.AppendFormat("&type={0}", string.Join(",", Types));

            sb.Append(GetBaseRequestUrl());

            return sb.ToString();
        }

        #endregion
    }
}