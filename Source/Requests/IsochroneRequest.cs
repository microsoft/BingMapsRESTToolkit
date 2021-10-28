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
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// A request that calculates an isochrone.
    /// </summary>
    public class IsochroneRequest : BaseRestRequest
    {
        #region Constructor 

        /// <summary>
        /// A request that calculates an isochrone.
        /// </summary>
        public IsochroneRequest(): base()
        {
            Optimize = RouteOptimizationType.Time;
            TimeUnit = TimeUnitType.Second;
            DistanceUnit = DistanceUnitType.Kilometers;
            TravelMode = TravelModeType.Driving;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The point around which the isochrone will be calculated. 
        /// </summary>
        public SimpleWaypoint Waypoint { get; set; }

        /// <summary>
        /// The maximum travel time in the specified time units in which the isochrone polygon is generated. Cannot be set when maxDistance is set. Maximum value is 120 minutes.
        /// </summary>
        public double MaxTime { get; set; }

        /// <summary>
        /// The maximum travel distance in the specified distance units in which the isochrone polygon is generated. Cannot be set when maxTime is set.
        /// </summary>
        public double MaxDistance { get; set; }

        /// <summary>
        /// Specifies what parameters to use to optimize the isochrone route. One of the following values:
        /// • distance: The route is calculated to minimize the distance. Traffic information is not used. Use with maxDistance.
        /// • time [default]: The route is calculated to minimize the time. Traffic information is not used. Use with maxTime.
        /// • timeWithTraffic: The route is calculated to minimize the time and uses current or predictive traffic information depending on if a dateTime value is specified. Use with maxTime.
        /// </summary>
        public RouteOptimizationType Optimize { get; set; }

        /// <summary>
        /// The units in which the maxTime value is specified. Default: Seconds
        /// </summary>
        public TimeUnitType TimeUnit { get; set; }

        /// <summary>
        /// The units in which the maxDistance value is specified. Default: Kilometers
        /// </summary>
        public DistanceUnitType DistanceUnit { get; set; }

        //TODO: uncomment when/if avoid is supported.
        /// <summary>
        /// Specifies the road types to minimize or avoid when a route is created for the driving travel mode. 
        /// </summary>
        //public List<AvoidType> Avoid { get; set; }

        /// <summary>
        /// The mode of travel for the isochrone. Default: Driving
        /// </summary>
        public TravelModeType TravelMode { get; set; }

        /// <summary>
        /// When travel mode is set to driving, and optimize is set to timeWithTraffic, predictive traffic data is used to calculate the best isochrone route for the specified date time.
        /// </summary>
        public DateTime? DateTime { get; set; }

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

        /// <summary>
        /// Gets the request URL to perform a query for an isochrone. 
        /// </summary>
        /// <returns>A request URL to perform a query for an isochron.</returns>
        public override string GetRequestUrl()
        {
            var sb = new StringBuilder(this.Domain);

            sb.Append("Routes/IsochronesAsync");

            //Truck mode is not supported, so fall back to driving. 
            if (TravelMode == TravelModeType.Truck)
            {
                TravelMode = TravelModeType.Driving;
            }

            sb.AppendFormat("?travelMode={0}", Enum.GetName(typeof(TravelModeType), TravelMode));

            if(Waypoint == null)
            {
                throw new Exception("A waypoint must be specified.");
            }

            if (Waypoint.Coordinate != null)
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, "&waypoint={0:0.#####},{1:0.#####}", Waypoint.Coordinate.Latitude, Waypoint.Coordinate.Longitude);
            }
            else if (!String.IsNullOrWhiteSpace(Waypoint.Address))
            {
                sb.AppendFormat("&waypoint={0}", Waypoint.Address);
            }
            else
            {
                throw new Exception("Invalid waypoint: A coordinate or address must be specified.");
            }

            if(MaxTime > 0)
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
                if(Optimize != RouteOptimizationType.Time && Optimize != RouteOptimizationType.TimeWithTraffic)
                {
                    Optimize = RouteOptimizationType.Time;
                }
            }
            else if (MaxDistance > 0)
            {
                if(TravelMode == TravelModeType.Transit)
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

            //TODO: uncomment when/if avoid is supported.
            //if (TravelMode == TravelModeType.Driving)
            //{
            //    if (Avoid != null && Avoid.Count > 0)
            //    {
            //        sb.Append("&avoid=");

            //        for (var i = 0; i < Avoid.Count; i++)
            //        {
            //            sb.Append(Enum.GetName(typeof(AvoidType), Avoid[i]));

            //            if (i < Avoid.Count - 1)
            //            {
            //                sb.Append(",");
            //            }
            //        }
            //    }
            //}

            sb.Append(GetBaseRequestUrl());

            return sb.ToString();
        }

        #endregion
    }
}
