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

using System.Collections.Generic;

namespace BingMapsRESTToolkit.Extensions
{
    /// <summary>
    /// The result from a Travelling Salesmen calculation.
    /// </summary>
    public class TspResult
    {
        /// <summary>
        /// A list of the waypoints in an optimized ordered. 
        /// </summary>
        public List<SimpleWaypoint> OptimizedWaypoints { get; set; }

        /// <summary>
        /// The optimized weight (time or distance) between all waypoints based on the TspOptimizationType.
        /// </summary>
        public double OptimizedWeight { get; set; }

        /// <summary>
        /// Indicates if the path is for a round trip and returns to the origin or not.
        /// </summary>
        public bool IsRoundTrip { get; set; }

        /// <summary>
        /// The metric used to solve the travelling salesmen problem for.
        /// </summary>
        public TspOptimizationType TspOptimization { get; set; }

        /// <summary>
        /// The travel mode used to calculate the distance matrix.
        /// </summary>
        public TravelModeType TravelMode { get; set; }

        /// <summary>
        /// The distance matrix used in the calculation.
        /// </summary>
        public DistanceMatrix DistanceMatrix { get; set; }
    }
}
