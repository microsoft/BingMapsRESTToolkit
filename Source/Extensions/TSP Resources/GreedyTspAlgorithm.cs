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

using System.Threading.Tasks;

namespace BingMapsRESTToolkit.Extensions
{
    /// <summary>
    /// A genetic algorithim for solving the Travelling Salesmen problem using the Bing Maps Distance Matrix service, based on travel time. 
    /// This uses brute force and recommended for 10 or less waypoints.
    /// </summary>
    internal class GreedyAlgorithm : BaseTspAlgorithm
    {
        #region Public Methods

        /// <summary>
        /// Calculates an efficient path between all waypoints based on time or distance.
        /// </summary>
        /// <param name="matrix">A precalculated distance matrix (n x n).</param>
        /// <param name="tspOptimization">The metric in which to base the TSP algorithm.</param>
        /// <returns>An efficient path between all waypoints based on time or distance.</returns>
        public override async Task<TspResult> Solve(DistanceMatrix matrix, TspOptimizationType tspOptimization)
        {
            return await Task<TspResult>.Run<TspResult>(() =>
            {
                var tour = new int[matrix.Origins.Count];

                for (var i = 0; i < matrix.Origins.Count; i++)
                {
                    tour[i] = i;
                }

                double minWeight = double.MaxValue;
                double weight;
                int[] minTour = (int[])tour.Clone();

                while (NextPermutation(tour))
                {
                    //Break out if the first location isn't in the first position.
                    if (tour[0] != 0)
                    {
                        break;
                    }

                    if (tspOptimization == TspOptimizationType.TravelTime)
                    {
                        weight = matrix.GetEdgeTime(tour, true);
                    }
                    else
                    {
                        weight = matrix.GetEdgeDistance(tour, true);
                    }

                    if (weight < minWeight)
                    {
                        minWeight = weight;
                        minTour = (int[])tour.Clone();
                    }
                }

                return new TspResult()
                {
                    DistanceMatrix = matrix,
                    OptimizedWeight = minWeight,
                    OptimizedWaypoints = GetOptimizedWaypoints(matrix.Origins, minTour)
                };
            });
        }

        #endregion

        #region Private Properties

        /// <summary>
        ///  Computes the next lexicographical permutation of the given array of integers in place, returning whether a next permutation existed. 
        ///  Returns false when the argument is already the last possible permutation.
        /// </summary>
        /// <param name="array">The array of integers to permutate through.</param>
        /// <returns>A boolean indicating if more permutations exist.</returns>
        public static bool NextPermutation(int[] array)
        {
            // Find non-increasing suffix
            int i = array.Length - 1;
            while (i > 0 && array[i - 1] >= array[i])
            {
                i--;
            }

            if (i <= 0)
            {
                return false;
            }

            // Find successor to pivot
            int j = array.Length - 1;
            while (array[j] <= array[i - 1])
            {
                j--;
            }

            int temp = array[i - 1];
            array[i - 1] = array[j];
            array[j] = temp;

            // Reverse suffix
            j = array.Length - 1;

            while (i < j)
            {
                temp = array[i];
                array[i] = array[j];
                array[j] = temp;
                i++;
                j--;
            }

            return true;
        }

        #endregion
    }
}
