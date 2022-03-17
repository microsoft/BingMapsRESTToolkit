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

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// Snaps a set of coordinates to roads.
    /// </summary>
    public class SnapToRoadRequest : BaseRestRequest
    {
        #region Private Properties

        //TODO: Update max points in the future as these may increase. Monitor documentation.

        /// <summary>
        /// Maximum number of points supported in an asynchronous Snap to road request.
        /// </summary>
        private int maxAsyncPoints = 1000; 

        /// <summary>
        /// Maximum number of points supported in a synchronous Snap to road request.
        /// </summary>
        private int maxSyncPoints = 100;

        /// <summary>
        /// The maximium distance in KM allowed between points.
        /// </summary>
        private double maxDistanceKmBetweenPoints = 2.5;

        #endregion

        #region Constructor 

        /// <summary>
        /// A request that calculates an isochrone.
        /// </summary>
        public SnapToRoadRequest(): base()
        {
            SpeedUnit = SpeedUnitType.KPH;
            TravelMode = TravelModeType.Driving;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// A set of coordinates to snap to roads. 
        /// </summary>
        public List<Coordinate> Points { get; set; }

        /// <summary>
        /// Indicates if the space between the snapped points should be filled with additional points along the road, thus returning the full route path. 
        /// </summary>
        public bool Interpolate { get; set; }

        /// <summary>
        /// Indicates if speed limitation data should be returned for the snapped points. 
        /// </summary>
        public bool IncludeSpeedLimit { get; set; }

        /// <summary>
        /// Indicates if speed limitation data should be returned for the snapped points. 
        /// </summary>
        public bool IncludeTruckSpeedLimit { get; set; }

        /// <summary>
        /// Indicates the units in which the returned speed limit data is in. 
        /// </summary>
        public SpeedUnitType SpeedUnit { get; set; }

        /// <summary>
        /// Indicates which routing profile to snap the points to. Default: Driving
        /// </summary>
        public TravelModeType TravelMode { get; set; }

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
            var requestBody = GetPostRequestBody();

            return await ServiceHelper.MakeAsyncPostRequest<SnapToRoadResponse>(requestUrl, requestBody, remainingTimeCallback).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Gets the request URL to perform a query to snap points to roads. This method will only generate an post URL. 
        /// </summary>
        /// <returns>A request URL to perform a query to snap points to roads.</returns>
        public override string GetRequestUrl()
        {
            //https://dev.virtualearth.net/REST/v1/Routes/SnapToRoad?key=BingMapsKey

            if (Points == null || Points.Count < 1)
            {
                throw new Exception("Points not specified.");
            }
            else if (Points.Count > maxAsyncPoints)
            {
                throw new Exception(string.Format("More than {0} Points specified.", maxAsyncPoints));
            }

            if(TravelMode == TravelModeType.Transit)
            {
                throw new Exception("Transit is not supported by SnapToRoad API.");
            }

            for(int i = 1; i < Points.Count; i++)
            {
                var d = SpatialTools.HaversineDistance(Points[i - 1], Points[i], DistanceUnitType.Kilometers);
                if (d > maxDistanceKmBetweenPoints)
                {
                    throw new Exception(string.Format("The distance between point {0} and point {1} is greater than {2} kilometers.", i-1, i, maxDistanceKmBetweenPoints));
                }
            }

            if (Points.Count > maxSyncPoints)
            {
                //Make an async request.
                return this.Domain + "Routes/SnapToRoadAsync?key=" + this.BingMapsKey;
            }

            return this.Domain + "Routes/SnapToRoad?key=" + this.BingMapsKey;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets a POST body for a snap to road request.
        /// </summary>
        /// <returns>A POST body for a snap to road request.</returns>
        private string GetPostRequestBody()
        {
            var sb = new StringBuilder();

            sb.Append("{");

            sb.Append("\"points\":[");

            foreach (var p in Points)
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, "{{\"latitude\":{0:0.#####},\"longitude\":{1:0.#####}}},", p.Latitude, p.Longitude);
            }

            //Remove trailing comma.
            sb.Length--;
            sb.Append("]");

            if (Interpolate)
            {
                sb.Append(",\"interpolate\":true");
            }

            if (IncludeSpeedLimit)
            {
                sb.Append(",\"includeSpeedLimit\":true");
            }

            if (IncludeTruckSpeedLimit)
            {
                sb.Append(",\"includeTruckSpeedLimit\":true");
            }

            sb.AppendFormat(",\"speedUnit\":\"{0}\"", Enum.GetName(typeof(SpeedUnitType), SpeedUnit));

            //TODO: Truck mode is not supported, so fall back to driving. Remove this if truck routing support added. 
            //Note, difference between snapping car vs truck should be minor since the truck GPS points should only be on roads it is meant to be on.
            if (TravelMode == TravelModeType.Truck)
            {
                TravelMode = TravelModeType.Driving;
            }

            sb.AppendFormat(",\"travelMode\":\"{0}\"", Enum.GetName(typeof(TravelModeType), TravelMode));

            sb.Append("}");

            return sb.ToString();
        }

        #endregion
    }
}
