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
    /// A genetic algorithim for solving the Travelling Salesmen problem using the Bing Maps Distance Matrix service, based on travel time. 
    /// </summary>
    internal class GeneticTspAlgorithm: BaseTspAlgorithm
    {
        #region Public Properties

        /// <summary>
        /// The initial number of random tours that are created when the algorithm starts.
        /// A large number of generations takes longer to find a result. 
        /// A smaller number of generations increases the chance that every tour will eventually look the same. This increases the chance that the best solution will not be found.
        /// </summary>
        public int Generations = 10000;

        /// <summary>
        /// The percentage that each crossover will undergo a mutution. When a mutation occurs, one of the waypoints is randomly within the order.
        /// </summary>
        public double MutationRate = 0.2;

        #endregion

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
                int population = matrix.Origins.Count;

                double[] weight = new double[population];

                var minTour = new int[population];

                int[,] chromosome = new int[population, population];

                double minWeight = double.MaxValue;

                for (int p = 0; p < population; p++)
                {
                    bool[] used = new bool[population];
                    int[] currentOrder = new int[population];

                    for (int n = 0; n < population; n++)
                    {
                        used[n] = false;
                    }

                    for (int n = 0; n < population; n++)
                    {
                        int i;

                        do
                        {
                            i = random.Next(population);
                        }
                        while (used[i]);

                        used[i] = true;
                        currentOrder[n] = i;
                    }

                    for (int n = 0; n < population; n++)
                    {
                        chromosome[p, n] = currentOrder[n];
                    }

                    if (tspOptimization == TspOptimizationType.TravelTime)
                    {
                        weight[p] = matrix.GetEdgeTime(currentOrder, true);
                    }
                    else
                    {
                        weight[p] = matrix.GetEdgeDistance(currentOrder, true);
                    }

                    if (weight[p] < minWeight)
                    {
                        minWeight = weight[p];

                        for (int n = 0; n < population; n++)
                        {
                            minTour[n] = chromosome[p, n];
                        }
                    }
                }

                for (int g = 0; g < Generations; g++)
                {
                    if (random.NextDouble() < MutationRate)
                    {
                        int i, j, parent1, parent2;
                        int[] p1 = new int[population];
                        int[] p2 = new int[population];
                        int[] o1 = new int[population];
                        int[] o2 = new int[population];

                        i = random.Next(population);
                        j = random.Next(population);

                        if (weight[i] < weight[j])
                        {
                            parent1 = i;
                        }
                        else
                        {
                            parent1 = j;
                        }

                        i = random.Next(population);
                        j = random.Next(population);

                        if (weight[i] < weight[j])
                        {
                            parent2 = i;
                        }
                        else
                        {
                            parent2 = j;
                        }

                        for (i = 0; i < population; i++)
                        {
                            p1[i] = chromosome[parent1, i];
                            p2[i] = chromosome[parent2, i];
                        }

                        int cp1 = -1, cp2 = -1;

                        do
                        {
                            cp1 = random.Next(population);
                            cp2 = random.Next(population);
                        } while (cp1 == cp2 || cp1 > cp2);

                        Crossover(cp1, cp2, p1, p2, o1, o2, population, random);

                        double o1Fitness; 

                        if (tspOptimization == TspOptimizationType.TravelTime)
                        {
                            o1Fitness = matrix.GetEdgeTime(o1, true);
                        }
                        else
                        {
                            o1Fitness = matrix.GetEdgeDistance(o1, true);
                        }

                        if (o1Fitness < weight[parent1])
                        {
                            for (i = 0; i < population; i++)
                            {
                                chromosome[parent1, i] = o1[i];
                            }
                        }

                        double o2Fitness;

                        if (tspOptimization == TspOptimizationType.TravelTime)
                        {
                            o2Fitness = matrix.GetEdgeTime(o2, true);
                        }
                        else
                        {
                            o2Fitness = matrix.GetEdgeDistance(o2, true);
                        }

                        if (o2Fitness < weight[parent2])
                        {
                            for (i = 0; i < population; i++)
                            {
                                chromosome[parent2, i] = o2[i];
                            }
                        }

                        for (int p = 0; p < population; p++)
                        {
                            if (weight[p] < minWeight)
                            {
                                minWeight = weight[p];

                                for (int n = 0; n < population; n++)
                                {
                                    minTour[n] = chromosome[p, n];
                                }
                            }
                        }
                    }
                    else
                    {
                        int i, j, p;
                        int[] child = new int[population];

                        i = random.Next(population);
                        j = random.Next(population);

                        if (weight[i] < weight[j])
                        {
                            p = i;
                        }
                        else
                        {
                            p = j;
                        }

                        double childWeight;

                        for (int n = 0; n < population; n++)
                        {
                            child[n] = chromosome[p, n];
                        }

                        do
                        {
                            i = random.Next(population);
                            j = random.Next(population);
                        }
                        while (i == j);

                        int t = child[i];

                        child[i] = child[j];
                        child[j] = t;
                        
                        if (tspOptimization == TspOptimizationType.TravelTime)
                        {
                            childWeight = matrix.GetEdgeTime(child, true);
                        }
                        else
                        {
                            childWeight = matrix.GetEdgeDistance(child, true);
                        }
                        
                        int maxIndex = int.MaxValue;
                        double maxD = double.MinValue;

                        for (int q = 0; q < population; q++)
                        {
                            if (weight[q] >= maxD)
                            {
                                maxIndex = q;
                                maxD = weight[q];
                            }
                        }

                        int[] index = new int[population];
                        int count = 0;

                        for (int q = 0; q < population; q++)
                        {
                            if (weight[q] == maxD)
                            {
                                index[count++] = q;
                            }
                        }

                        maxIndex = index[random.Next(count)];

                        if (childWeight < weight[maxIndex])
                        {
                            weight[maxIndex] = childWeight;

                            for (int n = 0; n < population; n++)
                            {
                                chromosome[maxIndex, n] = child[n];
                            }

                            if (childWeight < minWeight)
                            {
                                minWeight = childWeight;

                                for (int n = 0; n < population; n++)
                                {
                                    minTour[n] = child[n];
                                }
                            }
                        }
                    }
                }

                //Ensure first point is the starting point. Indicies form a cycle, so just need to shift.
                if (minTour[0] != 0)
                {
                    var minTourList = minTour.ToList();
                    var startIdx = minTourList.IndexOf(0);

                    var order = new List<int>();
                    order.AddRange(minTourList.GetRange(startIdx, minTourList.Count - startIdx));
                    order.AddRange(minTourList.GetRange(0, startIdx));

                    minTour = order.ToArray();
                }

                return new TspResult()
                {
                    DistanceMatrix = matrix,
                    OptimizedWeight = minWeight,
                    OptimizedWaypoints = GetOptimizedWaypoints(matrix.Origins, minTour)
                };
            }).ConfigureAwait(false);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Partially Mapped Crossover.
        /// </summary>
        private static void Crossover(int cp1, int cp2, int[] p1, int[] p2, int[] o1, int[] o2, int number, Random random)
        {
            for (int i = 0; i < number; i++)
            {
                o1[i] = o2[i] = -1;
            }

            for (int i = cp1; i <= cp2; i++)
            {
                o1[i] = p2[i];
                o2[i] = p1[i];
            }

            for (int i = 0; i < cp1; i++)
            {
                bool found = false;
                int t = p1[i];

                for (int j = i + 1; !found && j < number; j++)
                {
                    found = t == o1[j];
                }

                if (!found)
                {
                    o1[i] = t;
                }
            }

            for (int i = cp2 + 1; i < number; i++)
            {
                bool found = false;
                int t = p1[i];

                for (int j = 0; !found && j < number; j++)
                {
                    found = t == o1[j];
                }

                if (!found)
                {
                    o1[i] = t;
                }
            }

            List<int> used = new List<int>();

            for (int i = 0; i < number; i++)
            {
                if (o1[i] != -1)
                {
                    used.Add(o1[i]);
                }
            }

            for (int i = 0; i < number; i++)
            {
                if (o1[i] == -1)
                {
                    int x;

                    do
                    {
                        x = random.Next(number);
                    } while (used.Contains(x));

                    o1[i] = x;
                    used.Add(x);
                }
            }

            for (int i = 0; i < cp1; i++)
            {
                bool found = false;
                int t = p2[i];

                for (int j = i + 1; !found && j < number; j++)
                {
                    found = t == o2[j];
                }

                if (!found)
                {
                    o2[i] = t;
                }
            }

            for (int i = cp2 + 1; i < number; i++)
            {
                bool found = false;
                int t = p2[i];

                for (int j = 0; !found && j < number; j++)
                {
                    found = t == o2[j];
                }

                if (!found)
                {
                    o2[i] = t;
                }
            }

            used = new List<int>();

            for (int i = 0; i < number; i++)
            {
                if (o2[i] != -1)
                {
                    used.Add(o2[i]);
                }
            }

            for (int i = 0; i < number; i++)
            {
                if (o2[i] == -1)
                {
                    int x;

                    do
                    {
                        x = random.Next(number);
                    } while (used.Contains(x));

                    o2[i] = x;
                    used.Add(x);
                }
            }
        }

        #endregion
    }
}
