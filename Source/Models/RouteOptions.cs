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

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// A class that defines the options that can to use when calculating a route.
    /// </summary>
    public class RouteOptions
    {
        #region Private Properties

        private int distanceBeforeFirstTurn = 0, maxSolutions = 1;
        private int? heading = null;

        #endregion

        #region Constructor

        /// <summary>
        /// A class that defines the options that can to use when calculating a route.
        /// </summary>
        public RouteOptions()
        {
            Optimize = RouteOptimizationType.Time;
            TravelMode = TravelModeType.Driving;
            TimeType = RouteTimeType.Departure;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Specifies the road types to minimize or avoid when a route is created for the driving travel mode. 
        /// </summary>
        public List<AvoidType> Avoid { get; set; }

        /// <summary>
        /// Specifies the distance before the first turn is allowed in the route. 
        /// This option only applies to the driving travel mode.
        /// An integer distance specified in meters. 
        /// Use this parameter to make sure that the moving vehicle has enough distance to make the first turn. 
        /// </summary>
        public int DistanceBeforeFirstTurn
        {
            get { return distanceBeforeFirstTurn; }
            set
            {
                if (value >= 0)
                {
                    distanceBeforeFirstTurn = value;
                }
            }
        }

        /// <summary>
        /// Specifies the initial heading for the route. An integer value between 0 and 359 that represents degrees from 
        /// north where north is 0 degrees and the heading is specified clockwise from north.
        /// </summary>
        public int? Heading
        {
            get { return heading; }
            set
            {
                if (value < 0)
                {
                    heading = 0;
                }
                else if (value > 359)
                {
                    heading = 359;
                }
                else
                {
                    heading = value;
                }
            }
        }

        /// <summary>
        /// Specifies what parameters to use to optimize the route. Default: Time
        /// </summary>
        public RouteOptimizationType Optimize { get; set; }

        /// <summary>
        /// The mode of travel for the route. Default: Driving
        /// </summary>
        public TravelModeType TravelMode { get; set; }

        /// <summary>
        /// The dateTime parameter identifies the desired time to be used when calculating a route. This is supported by driving and transit routes. 
        /// When calculating driving routes the route optimization type should be TimeWithTraffic. The route time will be used as the departure time.
        /// When calculating transit routes timeType can be specified. 
        /// </summary>
        public DateTime? DateTime { get; set; }

        /// <summary>
        /// Specifies how to interpret the date and transit time value that is specified by the dateTime parameter.
        /// </summary>
        public RouteTimeType TimeType { get; set; }

        /// <summary>
        /// Specifies to include or exclude parts of the routes response.
        /// </summary>
        public List<RouteAttributeType> RouteAttributes { get; set; }

        /// <summary>
        /// The units to use for distance.
        /// </summary>
        public DistanceUnitType DistanceUnits { get; set; }

        /// <summary>
        ///  Specifies a series of tolerance values. Each value produces a subset of points that approximates the route 
        ///  that is described by the full set of points. This parameter is only valid when the routePathOutput parameter 
        ///  is set to Points. You can specify a maximum of seven (7) tolerance values.
        /// </summary>
        public List<double> Tolerances { get; set; }

        /// <summary>
        /// Specifies the maximum number of transit or driving routes to return. An interger between 1 and 3.
        /// This parameter is available for the Driving and Transit travel modes for routes between two waypoints. 
        /// This parameter does not support routes with more than two waypoints. For driving routes, you must not set 
        /// the avoid and distanceBeforeFirstTurn parameters.  The maxSolutions parameter is supported for routes in 
        /// the United States, Canada, Mexico, United Kingdom, Australia, and India.
        /// </summary>
        public int MaxSolutions
        {
            get { return maxSolutions; }
            set
            {
                if (value < 0)
                {
                    maxSolutions = 1;
                }
                else if (value > 3)
                {
                    maxSolutions = 3;
                }
                else
                {
                    maxSolutions = value;
                }
            }
        }

        /// <summary>
        /// Truck routing specific vehicle attribute. 
        /// </summary>
        public VehicleSpec VehicleSpec { get; set; }

        #endregion

        #region Internal Methods

        internal string GetUrlParam(int startIdx)
        {
            var sb = new StringBuilder();

            if (Optimize != RouteOptimizationType.Time)
            {
                sb.AppendFormat("&optmz={0}", Enum.GetName(typeof(RouteOptimizationType), Optimize));
            }

            if (TravelMode == TravelModeType.Driving && Avoid != null && Avoid.Count > 0)
            {
                sb.Append("&avoid=");
                
                for (var i = 0; i < Avoid.Count; i++)
                {
                    sb.Append(Enum.GetName(typeof(AvoidType), Avoid[i]));

                    if (i < Avoid.Count - 1)
                    {
                        sb.Append(",");
                    }
                }
            }

            if (startIdx == 0)
            {
                if (TravelMode == TravelModeType.Driving && distanceBeforeFirstTurn > 0)
                {
                    sb.AppendFormat("&dbft={0}", distanceBeforeFirstTurn);
                }

                if (heading.HasValue)
                {
                    sb.AppendFormat("&hd={0}", heading.Value);
                }
            }

            if (DistanceUnits == DistanceUnitType.Kilometers)
            {
                sb.Append("&du=kilometer");
            }
            else
            {
                sb.Append("&du=mile");
            }

            if (Tolerances != null && Tolerances.Count > 0)
            {
                sb.Append("&tl=");

                int cnt = Math.Min(Tolerances.Count, 7);

                for (int i = 0; i < cnt; i++)
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0:0.######}", Tolerances[i]);

                    if (i < Tolerances.Count - 1)
                    {
                        sb.Append(",");
                    }
                }
            }

            if (RouteAttributes != null && RouteAttributes.Count > 0)
            {
                sb.Append("&ra=");

                for (var i = 0; i < RouteAttributes.Count; i++)
                {
                    sb.Append(Enum.GetName(typeof(RouteAttributeType), RouteAttributes[i]));

                    if (i < RouteAttributes.Count - 1)
                    {
                        sb.Append(",");
                    }
                }
            }

            if (DateTime != null && DateTime.HasValue)
            {
                if (TravelMode == TravelModeType.Transit)
                {
                    sb.AppendFormat(DateTimeFormatInfo.InvariantInfo, "&dt={0:G}", DateTime.Value);
                    sb.AppendFormat("&tt={0}", Enum.GetName(typeof(RouteTimeType), TimeType));
                }
                else if (TravelMode == TravelModeType.Driving)
                {
                    sb.AppendFormat(DateTimeFormatInfo.InvariantInfo, "&dt={0:G}", DateTime.Value);
                }
            }

            if (TravelMode != TravelModeType.Walking && maxSolutions > 1 && maxSolutions <= 3)
            {
                sb.AppendFormat("&maxSolns={0}", maxSolutions);
            }

            return sb.ToString();
        }

        #endregion
    }
}
