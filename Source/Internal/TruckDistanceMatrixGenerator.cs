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

namespace BingMapsRESTToolkit.Extensions
{
    /// <summary>
    /// Helper class used to generate distance matricies based on truck driving attributes.
    /// </summary>
    internal class TruckDistanceMatrixGenerator
    {
        #region Private Properties

        /// <summary>
        /// List of cells that have been manually generated.
        /// </summary>
        private List<DistanceMatrixCell> MatrixCells;

        private List<DateTime> TimeIntervals;

        #endregion

        #region Public Methods

        /// <summary>
        /// Calculates a distance matrix based on truck routing attributes.
        /// </summary>
        /// <param name="request">The distance matrix request to base the request on.</param>
        /// <param name="remainingTimeCallback">A callback function in which the estimated remaining time in seconds is sent.</param>
        /// <returns>A response which contains the truck routing based distance matrix.</returns>
        public async Task<Response> Calculate(DistanceMatrixRequest request, Action<int> remainingTimeCallback)
        {
            if (request.Destinations == null || request.Destinations.Count == 0)
            {
                request.Destinations = request.Origins;
            }

            int numIntervals = 1;
            int intervalMin = 0;
            double numBatches = 0;

            if (request.StartTime != null && request.StartTime.HasValue)
            {
                TimeIntervals = new List<DateTime>() { request.StartTime.Value };

                if (request.EndTime != null && request.EndTime.HasValue)
                {
                    intervalMin = request.Resolution * 15;
                    numIntervals = (int)Math.Floor((request.EndTime.Value - request.StartTime.Value).TotalMinutes / intervalMin);
                }

                numBatches = Math.Ceiling((double)(request.Destinations.Count * request.Origins.Count * numIntervals) / (double)ServiceManager.QpsLimit);
            }
            else
            {
                TimeIntervals = null;
                numBatches = Math.Ceiling((double)(request.Destinations.Count * request.Origins.Count) / (double)ServiceManager.QpsLimit);
            }

            //Assume an average processing time of 2 seconds per batch.
            remainingTimeCallback?.Invoke((int)Math.Round(numBatches * 2));

            MatrixCells = new List<DistanceMatrixCell>();
            
            //Calculate the first cell on it's own to verify that the request can be made. If it fails, do not proceed.
            var firstResponse = await CalculateTruckRoute(request.Origins[0], request.Destinations[0], 0, request).ConfigureAwait(false);

            if(firstResponse != null && firstResponse.ErrorDetails != null && firstResponse.ErrorDetails.Length > 0){
                return firstResponse;
            }
            else if (!Response.HasResource(firstResponse))
            {
                return new Response()
                {
                    ErrorDetails = new string[] { "Unable to calculate distance matrix." },
                    StatusCode = 400,
                    StatusDescription = "Bad request"
                };
            }

            var truckRoute = Response.GetFirstResource(firstResponse) as Route;

            MatrixCells.Add(new DistanceMatrixCell()
            {
                OriginIndex = 0,
                DestinationIndex = 0,
                HasError = false,
                TravelDistance = truckRoute.TravelDistance,
                TravelDuration = (request.TimeUnits == TimeUnitType.Minute) ? truckRoute.TravelDuration * 60 : truckRoute.TravelDuration
            });

            var cellTasks = new List<Task>();

            if (request.StartTime != null && request.StartTime.HasValue)
            {
                TimeIntervals.Clear();

                for (var k = 0; k < numIntervals; k++)
                {
                    TimeIntervals.Add(request.StartTime.Value.AddMinutes(k * intervalMin));

                    for (var i = 0; i < request.Origins.Count; i++)
                    {
                        for (var j = 0; j < request.Destinations.Count; j++)
                        {
                            //Skip the first cell as we already calculated it.
                            if (!(k==0 && i == 0 && j == 0))
                            {
                                cellTasks.Add(CalculateCell(i, j, k, request));
                            }
                        }
                    }
                }
            }
            else
            {
                for (var i = 0; i < request.Origins.Count; i++)
                {
                    for (var j = 0; j < request.Destinations.Count; j++)
                    {
                        //Skip the first cell as we already calculated it.
                        if (!(i == 0 && j == 0))
                        {
                            cellTasks.Add(CalculateCell(i, j, -1, request));
                        }
                    }
                }
            }

            await ServiceHelper.WhenAllTaskLimiter(cellTasks).ConfigureAwait(false);

            var dm = new DistanceMatrix()
            {
                Origins = request.Origins,
                Destinations = request.Destinations,
                Results = MatrixCells.ToArray(),
                TimeIntervals = TimeIntervals
            };

            return new Response()
            {
                StatusCode = firstResponse.StatusCode,
                StatusDescription = firstResponse.StatusDescription,
                TraceId = firstResponse.TraceId,
                AuthenticationResultCode = firstResponse.AuthenticationResultCode,
                BrandLogoUri = firstResponse.BrandLogoUri,
                Copyright = firstResponse.Copyright,
                ResourceSets = new ResourceSet[]
                {
                    new ResourceSet() {
                        Resources = new Resource[] { dm }
                    }
                }
            };
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Calculates a truck ruting based matrix cell.
        /// </summary>
        /// <param name="originIdx">The index of the origin.</param>
        /// <param name="destIdx">The index of the destination.</param>
        /// <param name="timeIdx">The time interval index.</param>
        /// <param name="dmRequest">The distance matrix request.</param>
        /// <returns>A task which calculates the matrix cell and addes it to the MatrixCells array.</returns>
        private async Task CalculateCell(int originIdx, int destIdx, int timeIdx, DistanceMatrixRequest dmRequest)
        {
            DateTime? dt = null;

            if (TimeIntervals != null && timeIdx >= 0)
            {
                dt = TimeIntervals[timeIdx];
            }

            try
            {
                var response = await CalculateTruckRoute(dmRequest.Origins[originIdx], dmRequest.Destinations[destIdx], timeIdx, dmRequest).ConfigureAwait(false);

                if (Response.HasResource(response))
                {
                    var truckRoute = Response.GetFirstResource(response) as Route;
                                     
                    MatrixCells.Add(new DistanceMatrixCell()
                    {
                        OriginIndex = originIdx,
                        DestinationIndex = destIdx,
                        DepartureTimeUtc = dt,
                        HasError = false,
                        TravelDistance = truckRoute.TravelDistance,
                        TravelDuration = (dmRequest.TimeUnits == TimeUnitType.Minute) ? truckRoute.TravelDuration / 60 : truckRoute.TravelDuration                      
                    });

                    return;
                }
            }
            catch { }

            //If we made it this far, the cell can't be calculated.
            MatrixCells.Add(new DistanceMatrixCell()
            {
                OriginIndex = originIdx,
                DestinationIndex = destIdx,
                DepartureTimeUtc = dt,
                HasError = true,
                TravelDistance = -1,
                TravelDuration = -1
            });
        }

        /// <summary>
        /// Requests the truck route information needed for a matrix cell.
        /// </summary>
        /// <param name="origin">Origin of the route.</param>
        /// <param name="destination">Destination of the route.</param>
        /// <param name="timeIdx">The time interval index.</param>
        /// <param name="dmRequest">The distance matrix request.</param>
        /// <returns>Truck route information needed for a matrix cell.</returns>
        private async Task<Response> CalculateTruckRoute(SimpleWaypoint origin, SimpleWaypoint destination, int timeIdx, DistanceMatrixRequest dmRequest)
        {
            var request = new RouteRequest()
            {
                BingMapsKey = dmRequest.BingMapsKey,
                Culture = dmRequest.Culture,
                Domain = dmRequest.Domain,
                UserIp = dmRequest.UserIp,
                UserLocation = dmRequest.UserLocation,
                UserMapView = dmRequest.UserMapView,
                UserRegion = dmRequest.UserRegion,
                RouteOptions = new RouteOptions()
                {
                    MaxSolutions = 1,
                    DistanceUnits = dmRequest.DistanceUnits,
                    Optimize = RouteOptimizationType.Time,
                    TravelMode = TravelModeType.Truck,
                    VehicleSpec = dmRequest.VehicleSpec
                },
                Waypoints = new List<SimpleWaypoint>() { origin, destination }
            };

            if(TimeIntervals != null && timeIdx >= 0)
            {
                request.RouteOptions.DateTime = TimeIntervals[timeIdx];
                request.RouteOptions.Optimize = RouteOptimizationType.TimeWithTraffic;
            }

            return await request.Execute().ConfigureAwait(false);
        }

        #endregion
    }
}
