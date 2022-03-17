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
    /// A request that calculates a distance matrix between origins and destinations.
    /// </summary>
    public class DistanceMatrixRequest : BaseRestRequest
    {
        #region Private Properties

        /// <summary>
        /// The maximum number of coordinate pairs that can be in a standar distance matrix request.
        /// </summary>
        private const int MaxCoordinatePairs = 625;

        /// <summary>
        /// The maximum number of coordinate pairs that can be in an Async request for a distance matrix histogram.
        /// </summary>
        private const int MaxAsyncCoordinatePairsHistogram = 100;

        /// <summary>
        /// The maximum number of hours between the start and end time when calculating a distance matrix histogram. 
        /// </summary>
        private const double MaxTimeSpanHours = 24;

        #endregion 

        #region Constructure

        /// <summary>
        /// A request that calculates a distance matrix between origins and destinations.
        /// </summary>
        public DistanceMatrixRequest() : base()
        {
            TravelMode = TravelModeType.Driving;
            DistanceUnits = DistanceUnitType.Kilometers;
            TimeUnits = TimeUnitType.Minute;
            Resolution = 1;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Required. List of origins described as coordinate pairs with latitudes and longitudes. 
        /// </summary>
        public List<SimpleWaypoint> Origins { get; set; }

        /// <summary>
        /// List of destinations described as coordinate pairs with latitudes and longitudes.
        /// </summary>
        public List<SimpleWaypoint> Destinations { get; set; }

        /// <summary>
        /// Specifies the mode of transportation to use when calculating the distance matrix. Can be one of the following values: driving [default], walking, transit.
        /// </summary>
        public TravelModeType TravelMode { get; set; }

        /// <summary>
        /// Optional for Driving. Specifies the start or departure time of the matrix to calculate and uses traffic data in calculations. 
        /// This option is only supported when the mode is set to driving.
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Optional for Driving. If specified, a matrix based on traffic data with contain a histogram of travel times and distances for 
        /// the specified resolution (default is 15 minutes) between the start and end times. A start time must be specified for the request to be valid. 
        /// This option is only supported when the mode is set to driving.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// The number of intervals to calculate a histogram of data for each cell for where a single interval is a quarter 
        /// of an hour, 2 intervals would be 30 minutes, 3 intervals would be 45 minutes, 4 intervals would be for an hour. 
        /// If start time is specified and resolution is not, it will default to an interval of 1 (15 minutes).
        /// </summary>
        public int Resolution { get; set; }

        /// <summary>
        /// The units to use for distance. Default: Kilometers.
        /// </summary>
        public DistanceUnitType DistanceUnits { get; set; }

        /// <summary>
        /// The units to use for time. Default: Minute.
        /// </summary>
        public TimeUnitType TimeUnits { get; set; }

        /// <summary>
        /// Truck routing specific vehicle attribute. 
        /// </summary>
        public VehicleSpec VehicleSpec { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <returns>A response containing the requested distance matrix.</returns>
        public override async Task<Response> Execute() {
            return await this.Execute(null).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <param name="remainingTimeCallback">A callback function in which the estimated remaining time in seconds is sent.</param>
        /// <returns>A response containing the requested distance matrix.</returns>
        public override async Task<Response> Execute(Action<int> remainingTimeCallback)
        {
            if (TravelMode != TravelModeType.Truck)
            {
                //Make sure all origins and destinations are geocoded.
                await GeocodeWaypoints().ConfigureAwait(false);

                var requestUrl = GetRequestUrl();
                var requestBody = GetPostRequestBody();

                var response = await ServiceHelper.MakeAsyncPostRequest<DistanceMatrix>(requestUrl, requestBody, remainingTimeCallback).ConfigureAwait(false);

                var dm = Response.GetFirstResource(response) as DistanceMatrix;

                //TODO: Overwrite origins/destinations for now as we have added support for geocoding in this library, but this is not yet supported by the Distance Matirx API.
                dm.Origins = this.Origins;

                if (this.Destinations != null)
                {
                    dm.Destinations = this.Destinations;
                }

                if (dm.Results != null)
                {
                    return response;
                }
                //else if (!string.IsNullOrEmpty(dm.ErrorMessage))
                //{
                //    throw new Exception(dm.ErrorMessage);
                //}
            }
            else
            {
                var requestUrl = GetRequestUrl();

                //Generate truck routing based distnace matrix by wrapping routing service.
                return await new Extensions.TruckDistanceMatrixGenerator().Calculate(this, remainingTimeCallback).ConfigureAwait(false);
            }

            return null;           
        }

        /// <summary>
        /// Geocodes the origins and destinations.
        /// </summary>
        /// <returns>A task for geocoding the origins and destinations.</returns>
        public async Task GeocodeWaypoints()
        {
            //Ensure all the origins are geocoded.
            if (Origins != null)
            {
                await SimpleWaypoint.TryGeocodeWaypoints(Origins, this).ConfigureAwait(false);
            }

            //Ensure all the destinations are geocoded.
            if (Destinations != null)
            {
                await SimpleWaypoint.TryGeocodeWaypoints(Destinations, this).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Calculates a Distance Matrix for the origins and destinations based on the euclidean distance (straight line/as the crow flies). This calculation only uses; Origins, Destinations, and Distance Units properties from the request and only calculates travel distance.
        /// </summary>
        /// <returns>A Distance Matrix for the origins and destinations based on the euclidean distance (straight line/as the crow flies).</returns>
        public async Task<DistanceMatrix> GetEuclideanDistanceMatrix()
        {
            if(this.Origins != null && this.Origins.Count > 0)
            //Make sure all origins and destinations are geocoded.
            await GeocodeWaypoints().ConfigureAwait(false);

            var dm = new DistanceMatrix()
            {
                Origins = this.Origins
            };

            int cnt = 0;

            if (this.Destinations == null || this.Destinations.Count == 0)
            {
                dm.Destinations = this.Origins;
                dm.Results = new DistanceMatrixCell[this.Origins.Count * this.Origins.Count];

                for (var i = 0; i < Origins.Count; i++)
                {
                    for (var j = 0; j < Origins.Count; j++)
                    {
                        dm.Results[cnt] = new DistanceMatrixCell()
                        {
                            OriginIndex = i,
                            DestinationIndex = j,
                            TravelDistance = SpatialTools.HaversineDistance(Origins[i].Coordinate, Origins[j].Coordinate, DistanceUnits)
                        };

                        cnt++;
                    }
                }
            }
            else
            {
                dm.Destinations = this.Destinations;
                dm.Results = new DistanceMatrixCell[this.Origins.Count * this.Destinations.Count];

                for (var i = 0; i < Origins.Count; i++)
                {
                    for (var j = 0; j < Destinations.Count; j++)
                    {
                        dm.Results[cnt] = new DistanceMatrixCell()
                        {
                            OriginIndex = i,
                            DestinationIndex = j,
                            TravelDistance = SpatialTools.HaversineDistance(Origins[i].Coordinate, Destinations[j].Coordinate, DistanceUnits)
                        };

                        cnt++;
                    }
                }
            }

            return dm;
        }

        /// <summary>
        /// Returns the number of coordinate pairs that would be in the resulting matrix based on the number of origins and destinations in the request.
        /// </summary>
        /// <returns>The number of coordinate pairs that would be in the resulting matrix based on the number of origins and destinations in the request.</returns>
        public int GetNumberOfCoordinatePairs()
        {
            int numCoordPairs = Origins.Count;

            if (Destinations != null)
            {
                numCoordPairs *= Destinations.Count;
            }
            else
            {
                numCoordPairs *= Origins.Count;
            }

            return numCoordPairs;
        }
        
        /// <summary>
        /// Gets the request URL to perform a query for a distance matrix when using POST.
        /// </summary>
        /// <returns>A request URL to perform a query for a distance matrix when using POST.</returns>
        public override string GetRequestUrl()
        {
            //Matrix when using POST
            //https://dev.virtualearth.net/REST/v1/Routes/DistanceMatrix?key=BingMapsKey

            ValidateLocations(Origins, "Origin");
            
            if (Destinations != null)
            {
                ValidateLocations(Destinations, "Destination");
            }

            bool isAsyncRequest = false;

            int numCoordPairs = GetNumberOfCoordinatePairs();

            if (numCoordPairs > MaxCoordinatePairs && TravelMode != TravelModeType.Truck)
            {
                throw new Exception("The number of Origins and Destinations provided would result in a matrix that has more than 625 coordinate pairs.");
            }

            if (StartTime.HasValue)
            {
                if(TravelMode != TravelModeType.Driving && TravelMode != TravelModeType.Truck)
                {
                    throw new Exception("Start time parameter can only be used with the driving or truck travel mode.");
                }

                //Since start time is specified, an asynchronous request will be made which allows up to 100 coordinate pairs in the matrix (coordinate pairs).
                if (numCoordPairs > MaxAsyncCoordinatePairsHistogram)
                {
                    throw new Exception("The number of Origins and Destinations provided would result in a matrix that has more than 100 coordinate pairs which is the limit when a histogram is requested.");
                }

                isAsyncRequest = true;
            }

            if (EndTime.HasValue)
            {
                if(!StartTime.HasValue)
                {
                    throw new Exception("End time specified without a corresponding start time.");
                }

                var timeSpan = EndTime.Value.Subtract(StartTime.Value);

                if(timeSpan.TotalHours > MaxTimeSpanHours)
                {
                    throw new Exception("The time span between start and end time is more than 24 hours.");
                }

                if(Resolution < 0 || Resolution > 4)
                {
                    throw new Exception("Invalid resolution specified. Should be 1, 2, 3, or 4.");
                }
            }
            
            if(TravelMode == TravelModeType.Truck)
            {
                return string.Empty;
            }

            return this.Domain + "Routes/DistanceMatrix" + ((isAsyncRequest)? "Async?" : "" + "?") + GetBaseRequestUrl();
        }

        /// <summary>
        /// Returns a JSON string object representing the request. 
        /// </summary>
        /// <returns></returns>
        public string GetPostRequestBody()
        {
            if(TravelMode == TravelModeType.Truck)
            {
                return string.Empty;
            }

            //Build the JSON object using string builder as faster than JSON serializer.

            var sb = new StringBuilder();

            sb.Append("{\"origins\":[");

            foreach (var o in Origins)
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, "{{\"latitude\":{0:0.#####},\"longitude\":{1:0.#####}}},", o.Latitude, o.Longitude);
            }

            //Remove trailing comma.
            sb.Length--;

            sb.Append("]");

            if (Destinations != null && Destinations.Count > 0)
            {
                sb.Append(",\"destinations\":[");

                foreach (var d in Destinations)
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{{\"latitude\":{0:0.#####},\"longitude\":{1:0.#####}}},", d.Latitude, d.Longitude);
                }

                //Remove trailing comma.
                sb.Length--;

                sb.Append("]");
            }

            string mode;
            
            switch (TravelMode)
            {
                case TravelModeType.Transit:
                    mode = "transit";
                    break;
                case TravelModeType.Walking:
                    mode = "walking";
                    break;
                case TravelModeType.Driving:
                default:
                    mode = "driving";
                    break;
            }

            sb.AppendFormat(",\"travelMode\":\"{0}\"", mode);

            if (StartTime.HasValue)
            {
                sb.AppendFormat(DateTimeFormatInfo.InvariantInfo, ",\"startTime\":\"{0:O}\"", StartTime.Value);

                if (EndTime.HasValue)
                {
                    sb.AppendFormat(DateTimeFormatInfo.InvariantInfo, ",\"endTime\":\"{0:O}\"", EndTime.Value);
                    sb.AppendFormat(",\"resolution\":{0}", Resolution);
                }
            }

            string tu;

            switch (TimeUnits)
            {
                case TimeUnitType.Minute:
                    tu = "minute";
                    break;
                case TimeUnitType.Second:
                default:
                    tu = "second";
                    break;
            }

            sb.AppendFormat(",\"timeUnit\":\"{0}\"", tu);

            string du;

            switch (DistanceUnits)
            {
                case DistanceUnitType.Miles:
                    du = "mile";
                    break;
                case DistanceUnitType.Kilometers:
                default:
                    du = "kilometer";
                    break;
            }

            sb.AppendFormat(",\"distanceUnit\":\"{0}\"}}", du);            

            return sb.ToString();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Varifies that an array of locations are valid and geocoded.
        /// </summary>
        /// <param name="locations">The array of locations to validate.</param>
        /// <param name="name">The name of the locations array.</param>
        private void ValidateLocations(List<SimpleWaypoint> locations, string name)
        {
            if (locations == null)
            {
                throw new Exception(name + "s not specified.");
            }
            else if (locations.Count < 1)
            {
                throw new Exception("Not enough " + name + "s specified.");
            }

            //Verify all waypoints are geocoded.
            for (int i = 0; i < locations.Count; i++)
            {
                if (locations[i] == null) {
                    throw new Exception(name + " " + i + " is null.");
                }
                else if(locations[i].Coordinate == null)
                {
                    if (!string.IsNullOrEmpty(locations[i].Address))
                    {
                        throw new Exception(name + " " + i + " has no location information.");
                    }
                    else
                    {
                        throw new Exception(name + " " + i + " not geocoded. Address: " + locations[i].Address);
                    }
                }
            }
        }
        
        #endregion
    }
}