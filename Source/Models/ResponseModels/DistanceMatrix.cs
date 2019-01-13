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
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Linq;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// A Distance Matrix response object which is returned when geocoding or reverse geocoding.
    /// </summary>
    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class DistanceMatrix : Resource
    {
        #region Private Properties

        /// <summary>
        /// Array of distance matrix cell results containing information for each coordinate pair and time interval.
        /// </summary>
        private DistanceMatrixCell[] results = null;

        /// <summary>
        /// An indexer for the results to allow for fast lookups. Format is [origins][destinations][time intervals].
        /// </summary>
        private int[][][] cellIndex = null;

        /// <summary>
        /// The number of origins that were specified in the request.
        /// </summary>
        private int numOrigins = 0;

        /// <summary>
        /// The number of destinations that were specified in the request. If none were specified, the number of origins is used.
        /// </summary>
        private int numDestinations = 0;

        /// <summary>
        /// The number of time intervals in which distance matrix cells where calculated for. 
        /// </summary>
        private int numTimeIntervals = 0;

        #endregion

        #region Public Properties

        /// <summary>
        /// The array of destinations that were used to calculate the distance matrix.
        /// </summary>
        [DataMember(Name = "destinations", EmitDefaultValue = false)]
        public List<SimpleWaypoint> Destinations { get; set; }

        /// <summary>
        /// The array of origins that were used to calculate the distance matrix.
        /// </summary>
        [DataMember(Name = "origins", EmitDefaultValue = false)]
        public List<SimpleWaypoint> Origins { get; set; }

        /// <summary>
        /// Details of an error that may have occurred when processing the request.
        /// </summary>
        [DataMember(Name = "errorMessage", EmitDefaultValue = false)]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Array of distance matrix cell results containing information for each coordinate pair and time interval.
        /// </summary>
        [DataMember(Name = "results", EmitDefaultValue = false)]
        public DistanceMatrixCell[] Results
        {
            get
            {
                return results;
            }
            set
            {
                results = value;
            }
        }

        #endregion

        #region Additional properties extracted from request and results

        /// <summary>
        /// A list of time intervals in which the distance matrix calculated for.
        /// </summary>
        public List<DateTime> TimeIntervals { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the total travel distance between all waypoints indicies which represent an edge (graph/path).
        /// If a path between to waypoints is not routable, a large distance value will be returned.
        /// </summary>
        /// <param name="waypointIndicies">Waypoint indicies that represent the edge/path.</param>
        /// <returns>The total travel distance between all waypoints indicies.</returns>
        public double GetEdgeDistance(int[] waypointIndicies)
        {
            if(waypointIndicies.Length >= 2)
            {
                double distance = 0;
                
                for(var i = 0; i < waypointIndicies.Length - 1; i++)
                {
                    var d = GetDistance(waypointIndicies[i], waypointIndicies[i + 1]);

                    distance += d;

                    if (d < 0)
                    {
                        return double.MaxValue;
                    }
                }
                return distance;
            }

            return 0;
        }

        /// <summary>
        /// Retrieves the total travel distance between all waypoints indicies which represent an edge (graph/path).
        /// If a path between to waypoints is not routable, a large distance value will be returned.
        /// </summary>
        /// <param name="waypointIndicies">Waypoint indicies that represent the edge/path.</param>
        /// <param name="isRoundTrip">Indicates if the edge should be round trip and return to the first waypoint in the indicies array.</param>
        /// <returns>The total travel distance between all waypoints indicies.</returns>
        public double GetEdgeDistance(int[] waypointIndicies, bool isRoundTrip)
        {
            var distance = GetEdgeDistance(waypointIndicies);

            if (isRoundTrip && waypointIndicies.Length >= 2)
            {
                var d = GetDistance(waypointIndicies[waypointIndicies.Length - 1], waypointIndicies[0]);
                distance += d;

                if (d < 0)
                {
                    return double.MaxValue;
                }
            }

            return distance;
        }

        /// <summary>
        /// Retrieves the total travel time between all waypoints indicies which represent an edge (graph/path). 
        /// If a path between to waypoints is not routable, a large time value will be returned.
        /// </summary>
        /// <param name="waypointIndicies">Waypoint indicies that represent the edge/path.</param>
        /// <returns>The total travel time between all waypoints indicies.</returns>
        public double GetEdgeTime(int[] waypointIndicies)
        {
            if (waypointIndicies.Length >= 2)
            {
                double time = 0;

                for (var i = 0; i < waypointIndicies.Length - 1; i++)
                {
                    var t = GetTime(waypointIndicies[i], waypointIndicies[i + 1]);

                    time += t;

                    if (t < 0)
                    {
                        return double.MaxValue;
                    }
                }
                return time;
            }

            return 0;
        }

        /// <summary>
        /// Retrieves the total travel time between all waypoints indicies which represent an edge (graph/path).
        /// </summary>
        /// <param name="waypointIndicies">Waypoint indicies that represent the edge/path.</param>
        /// <param name="isRoundTrip">Indicates if the edge should be round trip and return to the first waypoint in the indicies array.</param>
        /// <returns>The total travel time between all waypoints indicies.</returns>
        public double GetEdgeTime(int[] waypointIndicies, bool isRoundTrip)
        {
            var time = GetEdgeTime(waypointIndicies);

            if (isRoundTrip && waypointIndicies.Length >= 2)
            {
                var t = GetTime(waypointIndicies[waypointIndicies.Length - 1], waypointIndicies[0]);

                if (t > 0)
                {
                    time += t;
                }
            }

            return time;
        }

        /// <summary>
        /// Retrives the distance matrix cell for a specified origin-destination pair.
        /// Returns -1 if a cell can not be found in the results or had an error in calculation.
        /// </summary>
        /// <param name="originIdx">The index of the origin in the requests origins array.</param>
        /// <param name="destinationIdx">The index of the destination in the requests destinations array.</param>
        /// <returns>The distance matrix cell for a specified origin-destination pair.</returns>
        public DistanceMatrixCell GetCell(int originIdx, int destinationIdx)
        {
            return GetCell(originIdx, destinationIdx, 0);
        }

        /// <summary>
        /// Retrives the distance matrix cell for a specified origin-destination pair and time interval.
        /// Returns -1 if a cell can not be found in the results or had an error in calculation.
        /// </summary>
        /// <param name="originIdx">The index of the origin in the requests origins array.</param>
        /// <param name="destinationIdx">The index of the destination in the requests destinations array.</param>
        /// <param name="timeInterval">The time interval to retrieve the distance matrix cell for.</param>
        /// <returns>The distance matrix cell for a specified origin-destination pair and time interval.</returns>
        public DistanceMatrixCell GetCell(int originIdx, int destinationIdx, DateTime timeInterval)
        {
            if (TimeIntervals != null && TimeIntervals.Contains(timeInterval))
            {
                return GetCell(originIdx, destinationIdx, TimeIntervals.IndexOf(timeInterval));
            }

            return null;
        }

        /// <summary>
        /// Retrives the distance matrix cell for a specified origin-destination pair and time interval.
        /// Returns -1 if a cell can not be found in the results or had an error in calculation.
        /// </summary>
        /// <param name="originIdx">The index of the origin in the requests origins array.</param>
        /// <param name="destinationIdx">The index of the destination in the requests destinations array.</param>
        /// <param name="timeIntervalIdx">The time interval to retrieve the distance matrix cell for.</param>
        /// <returns>The distance matrix cell for a specified origin-destination pair and time interval.</returns>
        public DistanceMatrixCell GetCell(int originIdx, int destinationIdx, int timeIntervalIdx)
        {
            if(cellIndex == null)
            {
                GenerateIndex();
            }

            if (cellIndex != null && originIdx >= 0 && destinationIdx >= 0 && timeIntervalIdx >= 0 && cellIndex.Length > 0
                && originIdx < numOrigins && destinationIdx < numDestinations && timeIntervalIdx < numTimeIntervals)
            {
                var idx = cellIndex[originIdx][destinationIdx][timeIntervalIdx];
                if (idx > -1)
                {
                    return results[idx];
                }
            }

            return null;
        }

        /// <summary>
        /// Gets all cells for the specified origin and destination index, ordered by time (ascending).
        /// </summary>
        /// <param name="originIdx">The index of the origin in the requests origins array.</param>
        /// <param name="destinationIdx">The index of the destination in the requests destinations array.</param>
        /// <returns>All cells for the specified origin and destination index, ordered by time.</returns>
        public DistanceMatrixCell[] GetCells(int originIdx, int destinationIdx)
        {
            if (cellIndex == null)
            {
                GenerateIndex();
            }

            if (cellIndex != null && originIdx >= 0 && destinationIdx >= 0 && cellIndex.Length > 0
                && originIdx < numOrigins && destinationIdx < numDestinations)
            {
                var indices = cellIndex[originIdx][destinationIdx];
                if (indices != null && indices.Length > 0)
                {
                    var cells = new List<DistanceMatrixCell>(indices.Length);

                    for (int i = 0; i < indices.Length; i++)
                    {
                        cells.Add(results[indices[i]]);
                    }

                    //Sort the cells by time.
                    cells = cells.OrderBy(x => x.DepartureTimeUtc).ToList();

                    return cells.ToArray();
                }
            }

            return null;
        }

        /// <summary>
        /// Retrives the travel distance for a specified origin-destination pair.
        /// Returns -1 if a cell can not be found in the results or had an error in calculation.
        /// </summary>
        /// <param name="originIdx">The index of the origin in the requests origins array.</param>
        /// <param name="destinationIdx">The index of the destination in the requests destinations array.</param>
        /// <returns>The travel distance for a specified origin-destination pair.</returns>
        public double GetDistance(int originIdx, int destinationIdx)
        {
            return GetDistance(originIdx, destinationIdx, 0);
        }

        /// <summary>
        /// Retrives the travel distance for a specified origin-destination pair and time interval.
        /// Returns -1 if a cell can not be found in the results or had an error in calculation.
        /// </summary>
        /// <param name="originIdx">The index of the origin in the requests origins array.</param>
        /// <param name="destinationIdx">The index of the destination in the requests destinations array.</param>
        /// <param name="timeInterval">The time interval to retrieve the travel distance for.</param>
        /// <returns>The travel distance for a specified origin-destination pair and time interval.</returns>
        public double GetDistance(int originIdx, int destinationIdx, DateTime timeInterval)
        {
            if (TimeIntervals != null && TimeIntervals.Contains(timeInterval))
            {
                return GetDistance(originIdx, destinationIdx, TimeIntervals.IndexOf(timeInterval));
            }

            return -1;
        }

        /// <summary>
        /// Retrives the travel distance for a specified origin-destination pair and time interval.
        /// Returns -1 if a cell can not be found in the results or had an error in calculation.
        /// </summary>
        /// <param name="originIdx">The index of the origin in the requests origins array.</param>
        /// <param name="destinationIdx">The index of the destination in the requests destinations array.</param>
        /// <param name="timeIntervalIdx">The time interval to retrieve the travel distance for.</param>
        /// <returns>The travel distance for a specified origin-destination pair and time interval.</returns>
        public double GetDistance(int originIdx, int destinationIdx, int timeIntervalIdx)
        {
            var c = GetCell(originIdx, destinationIdx, timeIntervalIdx);

            if (c != null)
            {
                return c.TravelDistance;
            }

            return -1;
        }

        /// <summary>
        /// Gets all travel distances for the specified origin and destination index, ordered by time (ascending).
        /// </summary>
        /// <param name="originIdx">The index of the origin in the requests origins array.</param>
        /// <param name="destinationIdx">The index of the destination in the requests destinations array.</param>
        /// <returns>All travel distances for the specified origin and destination index, ordered by time.</returns>
        public double[] GetDistances(int originIdx, int destinationIdx)
        {
            var cells = GetCells(originIdx, destinationIdx);

            if (cells != null)
            {
                var distances = new double[cells.Length];

                for (int i = 0; i < cells.Length; i++)
                {
                    distances[i] = cells[i].TravelDistance;
                }

                return distances;
            }

            return null;
        }

        /// <summary>
        /// Retrives the travel time for a specified origin-destination pair.
        /// Returns -1 if a cell can not be found in the results or had an error in calculation.
        /// </summary>
        /// <param name="originIdx">The index of the origin in the requests origins array.</param>
        /// <param name="destinationIdx">The index of the destination in the requests destinations array.</param>
        /// <returns>The travel time for a specified origin-destination pair.</returns>
        public double GetTime(int originIdx, int destinationIdx)
        {
            return GetTime(originIdx, destinationIdx, 0);
        }

        /// <summary>
        /// Retrieves the travel time for a specified origin-destination pair and time interval.
        /// Returns -1 if a cell can not be found in the results or had an error in calculation.
        /// </summary>
        /// <param name="originIdx">The index of the origin in the requests origins array.</param>
        /// <param name="destinationIdx">The index of the destination in the requests destinations array.</param>
        /// <param name="timeInterval">The time interval to retrieve the travel time for.</param>
        /// <returns>The travel time for a specified origin-destination pair and time interval.</returns>
        public double GetTime(int originIdx, int destinationIdx, DateTime timeInterval)
        {
            if (TimeIntervals != null && TimeIntervals.Contains(timeInterval))
            {
                return GetTime(originIdx, destinationIdx, TimeIntervals.IndexOf(timeInterval));
            }

            return -1;
        }

        /// <summary>
        /// Retrieves the travel time for a specified origin-destination pair and time interval.
        /// Returns -1 if a cell can not be found in the results or had an error in calculation.
        /// </summary>
        /// <param name="originIdx">The index of the origin in the requests origins array.</param>
        /// <param name="destinationIdx">The index of the destination in the requests destinations array.</param>
        /// <param name="timeIntervalIdx">The index of the time interval.</param>
        /// <returns>The travel time for a specified origin-destination pair and time interval.</returns>
        public double GetTime(int originIdx, int destinationIdx, int timeIntervalIdx)
        {
            var c = GetCell(originIdx, destinationIdx, timeIntervalIdx);

            if (c != null && c.TravelDuration >= 0)
            {
                return c.TravelDuration;
            }

            return -1;
        }

        /// <summary>
        /// Gets all travel times for the specified origin and destination index, ordered by time (ascending).
        /// </summary>
        /// <param name="originIdx">The index of the origin in the requests origins array.</param>
        /// <param name="destinationIdx">The index of the destination in the requests destinations array.</param>
        /// <returns>All travel times for the specified origin and destination index, ordered by time.</returns>
        public double[] GetTimes(int originIdx, int destinationIdx)
        {
            var cells = GetCells(originIdx, destinationIdx);

            if (cells != null)
            {
                var times = new double[cells.Length];

                for (int i = 0; i < cells.Length; i++)
                {
                    if(cells[i].TravelDuration <= 0)
                    {
                        times[i] = -1;
                    }
                    else
                    {
                        times[i] = cells[i].TravelDuration;
                    }                    
                }

                return times;
            }

            return null;
        }

        /// <summary>
        /// Creates a NxN distance matrix with straight line distances.
        /// </summary>
        /// <param name="waypoints">The waypoints to generate a matrix for.</param>
        /// <param name="distanceUnits">The distance units to calculate the distances in.</param>
        /// <param name="bingMapsKey">A bing maps key that can be used to geocode waypoints, if needed.</param>
        /// <returns>A NxN distance matrix with straight line distances.</returns>
        public static async Task<DistanceMatrix> CreateStraightLineNxNMatrix(List<SimpleWaypoint> waypoints, DistanceUnitType distanceUnits, string bingMapsKey)
        {
            //Ensure all the waypoints are geocoded.
            if (waypoints == null || waypoints.Count < 2)
            {
                throw new Exception("Not enough Waypoints specified.");
            }

            if (!string.IsNullOrEmpty(bingMapsKey))
            {
                await SimpleWaypoint.TryGeocodeWaypoints(waypoints, bingMapsKey).ConfigureAwait(false);
            }

            var numWaypoints = waypoints.Count;

            var cells = new DistanceMatrixCell[numWaypoints * numWaypoints];

            await Task.Run(() => Parallel.For(0, numWaypoints, i =>
            {
                for (var j = 0; j < numWaypoints; j++)
                {
                    double distance = -1;

                    if (i != j && waypoints[i].Coordinate != null && waypoints[j].Coordinate != null)
                    {
                        distance = SpatialTools.HaversineDistance(waypoints[i].Coordinate, waypoints[j].Coordinate, distanceUnits);
                    }

                    cells[i * numWaypoints + j] = new DistanceMatrixCell()
                    {
                        OriginIndex = i,
                        DestinationIndex = j,
                        TravelDistance = distance
                    };
                }
            })).ConfigureAwait(false);

            return new DistanceMatrix()
            {
                Origins = waypoints,
                Destinations = waypoints,
                Results = cells
            };
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loops through results and generates an result index based on the origin, destination and time interval indices.
        /// </summary>
        private void GenerateIndex()
        {
            if (results == null)
            {
                cellIndex = null;
                return;
            }

            //Scan results and capture time intervals, origin and destination sizes. 
            numOrigins = -1;
            numDestinations = -1;
            TimeIntervals = new List<DateTime>();

            foreach (var c in results)
            {
                if(c.OriginIndex > numOrigins)
                {
                    numOrigins = c.OriginIndex;
                }

                if (c.DestinationIndex > numDestinations)
                {
                    numDestinations = c.DestinationIndex;
                }

                var date = c.DepartureTimeUtc;

                if (date != null && date.HasValue && !TimeIntervals.Contains(date.Value))
                {
                    TimeIntervals.Add(date.Value);
                }
            }

            numOrigins++;
            numDestinations++;

            if (TimeIntervals.Count == 0)
            {
                TimeIntervals.Add(DateTime.Now);
            }
            else
            {
                //Sort the time intervals.
                TimeIntervals.Sort();
            }

            if (numDestinations == 0 && numOrigins > 0)
            {
                numDestinations = numOrigins;
            }

            if (numOrigins > 0 && numDestinations > 0)
            {
                numTimeIntervals = TimeIntervals.Count;

                //Initialize cell indexer
                cellIndex = new int[numOrigins][][];

                Parallel.For(0, numOrigins, o =>
                {
                    cellIndex[o] = new int[numDestinations][];

                    for (int d = 0; d < numDestinations; d++)
                    {
                        cellIndex[o][d] = new int[numTimeIntervals];

                        for (int t = 0; t < numTimeIntervals; t++)
                        {
                            cellIndex[o][d][t] = -1;
                        }
                    }
                });

                //Loop through results and generate index.
                if (numTimeIntervals == 1)
                {
                    Parallel.For(0, results.Length, i =>
                    {
                        var cell = results[i];
                        cellIndex[cell.OriginIndex][cell.DestinationIndex][0] = i;
                    });
                }
                else
                {
                    Parallel.For(0, results.Length, i =>
                    {
                        var cell = results[i];
                        if (cell.DepartureTimeUtc != null && cell.DepartureTimeUtc.HasValue && !TimeIntervals.Contains(cell.DepartureTimeUtc.Value))
                        {
                            cellIndex[cell.OriginIndex][cell.DestinationIndex][TimeIntervals.IndexOf(cell.DepartureTimeUtc.Value)] = i;
                        }
                    });
                }
            }
        }

        #endregion
    }
}
