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
using System.Linq;
using System.Threading.Tasks;

namespace BingMapsRESTToolkit.Extensions
{
    /// <summary>
    /// This is a static class that solves the [travelling salesmen problem](https://en.wikipedia.org/wiki/Travelling_salesman_problem). 
    /// Uses a greedy algrithm when 10 or less waypoints are specified, and a genetic algorithm for largre waypoint sets. 
    /// </summary>
    public static class TravellingSalesmen
    {
        /// <summary>
        ///  Solves the travelling salesmen problem.
        /// </summary>
        /// <param name="waypoints">The waypoints to calculate a path through.</param>
        /// <param name="travelMode">The mode of transportation.</param>
        /// <param name="tspOptimization">The metric in which to base the TSP algorithm.</param>
        /// <param name="departureTime">The departure time in which to consider predictive traffic for. Only used when travel mode is driving and tsp optimiation is based on travel time or travel distance. Ignored if there is more than 10 waypoints as that would exceed limits of a distance matrix call.</param>
        /// <param name="bingMapsKey">A bing maps key.</param>
        /// <returns>An efficient path between all waypoints based on time or distance.</returns>
        public static async Task<TspResult> Solve(List<SimpleWaypoint> waypoints, TravelModeType? travelMode, TspOptimizationType? tspOptimization, DateTime? departureTime, string bingMapsKey)
        {
            return await GetTspAlgorithm(waypoints).Solve(waypoints, travelMode, tspOptimization, departureTime, bingMapsKey).ConfigureAwait(false);
        }

        ///<summary>
        ///  Solves the travelling salesmen problem.
        /// </summary>
        /// <param name="matrix">A precalculated distance matrix (n x n).</param>
        /// <param name="tspOptimization">The metric in which to base the TSP algorithm.</param>
        /// <returns>An efficient path between all waypoints based on time or distance.</returns>
        public static async Task<TspResult> Solve(DistanceMatrix matrix, TspOptimizationType tspOptimization)
        {
            return await GetTspAlgorithm(matrix.Origins).Solve(matrix, tspOptimization).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the appropriate TSP algorithm based on the waypoints provided.
        /// </summary>
        /// <param name="waypoints">Waypoints to be optimized.</param>
        /// <returns>The appropriate TSP algorithm based on the waypoints provided.</returns>
        private static BaseTspAlgorithm GetTspAlgorithm(List<SimpleWaypoint> waypoints)
        {
            if (waypoints != null && waypoints.Count > 0)
            {
                //Ensure that unique waypoints are in the list. This will reduce the number of cells generated in the distance matrix, thus lowering cost.
                var wps = waypoints.Distinct().ToList();

                if (wps.Count >= 2)
                {
                    if (wps.Count > 10)
                    {
                       return new GeneticTspAlgorithm();
                    }
                    else
                    {
                        return new GreedyAlgorithm();
                    }
                }
            }

            throw new Exception("Insufficient number of waypoints specified.");
        }
    }
}
