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
using System.Threading.Tasks;
using System.Linq;

namespace BingMapsRESTToolkit.Extensions
{
    /// <summary>
    /// An algorithm for solving the travelling salesmen problem. 
    /// </summary>
    internal class BaseTspAlgorithm
    {
        #region Protected Properties

        /// <summary>
        /// Random number generator.
        /// </summary>
        protected Random random;

        #endregion

        #region Constructor

        /// <summary>
        ///  An algorithm for solving the travelling salesmen problem. 
        /// </summary>
        public BaseTspAlgorithm()
        {
            random = new Random();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Calculates an efficient path between all waypoints based on time or distance.
        /// </summary>
        /// <param name="waypoints">The waypoints to calculate a path through.</param>
        /// <param name="travelMode">The mode of transportation.</param>
        /// <param name="tspOptimization">The metric in which to base the TSP algorithm.</param>
        /// <param name="departureTime">The departure time in which to consider predictive traffic for. Only used when travel mode is driving and tsp optimiation is based on travel time or travel distance. Ignored if there is more than 10 waypoints as that would exceed limits of a distance matrix call.</param>
        /// <param name="bingMapsKey">A bing maps key.</param>
        /// <returns>An efficient path between all waypoints based on time or distance.</returns>
        public async Task<TspResult> Solve(List<SimpleWaypoint> waypoints, TravelModeType? travelMode, TspOptimizationType? tspOptimization, DateTime? departureTime, string bingMapsKey)
        {
            if(waypoints == null || waypoints.Count == 0)
            {
                throw new Exception("No waypoints specified.");
            }

            //Ensure that unique waypoints are in the list. This will reduce the number of cells generated in the distance matrix, thus lower cost.
            waypoints = waypoints.Distinct().ToList();

            if (tspOptimization == null || !tspOptimization.HasValue)
            {
                tspOptimization = TspOptimizationType.TravelTime;
            }

            DistanceMatrix dm = null;

            if (tspOptimization.Value == TspOptimizationType.StraightLineDistance)
            {
                //Calculate a distance matrix based on straight line distances (haversine). 
                dm = await DistanceMatrix.CreateStraightLineNxNMatrix(waypoints, DistanceUnitType.Kilometers, bingMapsKey).ConfigureAwait(false);
            }
            else 
            {
                if (travelMode == null || !travelMode.HasValue)
                {
                    //Default to driving if not specified.
                    travelMode = TravelModeType.Driving;
                }

                var distanceMatrixRequest = new DistanceMatrixRequest()
                {
                    TravelMode = travelMode.Value,
                    BingMapsKey = bingMapsKey
                };                

                if (departureTime.HasValue &&
                    (distanceMatrixRequest.TravelMode == TravelModeType.Driving && waypoints.Count <= 10 || 
                    distanceMatrixRequest.TravelMode == TravelModeType.Truck))
                {
                    distanceMatrixRequest.StartTime = departureTime.Value;
                }

                distanceMatrixRequest.Origins = waypoints;

                var r = await distanceMatrixRequest.Execute().ConfigureAwait(false);

                if (r != null)
                {
                    if (r.ErrorDetails != null && r.ErrorDetails.Length > 0)
                    {
                        throw new Exception(String.Join("", r.ErrorDetails));
                    }

                    if (Response.HasResource(r))
                    {
                        var res = Response.GetFirstResource(r);
                        if (res is DistanceMatrix)
                        {
                            dm = res as DistanceMatrix;
                        }
                    }
                }
            }

            if(dm != null)
            {
                var solution = await Solve(dm, tspOptimization.Value).ConfigureAwait(false);
                solution.TspOptimization = tspOptimization.Value;
                solution.TravelMode = travelMode.Value;
                return solution;
            }

            throw new Exception("Unable to calculate distance matrix.");
        }

        /// <summary>
        /// Calculates an efficient path between all waypoints based on time or distance.
        /// </summary>
        /// <param name="matrix">A precalculated distance matrix (n x n).</param>
        /// <param name="tspOptimization">The metric in which to base the TSP algorithm.</param>
        /// <returns>An efficient path between all waypoints based on time or distance.</returns>
        #pragma warning disable 1998
        public virtual async Task<TspResult> Solve(DistanceMatrix matrix, TspOptimizationType tspOptimization)
        {
            throw new NotImplementedException();
        }
        #pragma warning restore 1998

        /// <summary>
        /// Generates the optimized list of waypoints based on an optimized array of waypoint indicies.
        /// </summary>
        /// <param name="waypoints">Waypoints to optimize.</param>
        /// <param name="minTour">An optimized array of waypoint indicies</param>
        /// <returns>An optimized lis tof waypoints.</returns>
        protected List<SimpleWaypoint> GetOptimizedWaypoints(List<SimpleWaypoint> waypoints, int[] minTour)
        {
            var optimizedOrder = new List<SimpleWaypoint>();
            
            for (var i = 0; i < minTour.Length; i++)
            {
                optimizedOrder.Add(waypoints[minTour[i]]);
            }

            //Close the loop
            if (minTour[minTour.Length - 1] != 0)
            {
                optimizedOrder.Add(waypoints[0]);
            }

            return optimizedOrder;
        }

        #endregion
    }
}
