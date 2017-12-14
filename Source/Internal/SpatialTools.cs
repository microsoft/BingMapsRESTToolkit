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

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// A set of useful spatial math tools.
    /// </summary>
    internal static class SpatialTools
    {
        #region Public Methods

        #region Unit Conversion

        /// <summary>
        /// Converts distances from miles to km or km to miles.
        /// </summary>
        /// <param name="distance">Distance to convert.</param>
        /// <param name="fromUnits">The units that the distance is in.</param>
        /// <param name="toUnits">The units to convert the distance to.</param>
        /// <returns>A distance in the specified unit of measurement.</returns>
        public static double ConvertDistance(double distance, DistanceUnitType fromUnits, DistanceUnitType toUnits)
        {
            if(fromUnits == toUnits || double.IsNaN(distance))
            {
                return distance;
            }

            //Convert the distance to kilometers
            if (fromUnits == DistanceUnitType.Miles)
            {
                distance *= 1.609344;
            }

            if (toUnits == DistanceUnitType.Miles)
            {
                distance *= 0.62137119;
            }

            return distance;
        }

        /// <summary>
        /// Converts dimensions from meters to feet or feet to meters.
        /// </summary>
        /// <param name="dimension">Dimension to convert.</param>
        /// <param name="fromUnits">The units that the dimension is in.</param>
        /// <param name="toUnits">The units to convert the dimension to.</param>
        /// <returns>A dimension in the specified unit of measurement.</returns>
        public static double CovertDimension(double dimension, DimensionUnitType fromUnits, DimensionUnitType toUnits)
        {
            if (fromUnits == toUnits || double.IsNaN(dimension))
            {
                return dimension;
            }

            //Convert the distance to meters
            if (fromUnits == DimensionUnitType.Feet)
            {
                dimension *= 0.3048;
            }

            if (toUnits == DimensionUnitType.Feet)
            {
                dimension *= 3.2808399;
            }

            return dimension;
        }

        /// <summary>
        /// Converts weights from kg to lbs or lbs to kg.
        /// </summary>
        /// <param name="weight">Weight to convert.</param>
        /// <param name="fromUnits">The units that the weights is in.</param>
        /// <param name="toUnits">The units to convert the weights to.</param>
        /// <returns>A weights in the specified unit of measurement.</returns>
        public static double CovertWeight(double weight, WeightUnitType fromUnits, WeightUnitType toUnits)
        {
            if (fromUnits == toUnits || double.IsNaN(weight))
            {
                return weight;
            }

            //Convert the distance to kilograms
            if (fromUnits == WeightUnitType.Pound)
            {
                weight *= 0.45359237;
            }

            if (toUnits == WeightUnitType.Pound)
            {
                weight *= 2.20462262;
            }

            return weight;
        }

        #endregion
        
        #region Haversine Distance Calculation method

        /// <summary>
        /// Calculate the distance between two coordinates on the surface of a sphere (Earth).
        /// </summary>
        /// <param name="origin">First coordinate to calculate distance between.</param>
        /// <param name="destination">Second coordinate to calculate distance between.</param>
        /// <param name="units">Unit of distance measurement.</param>
        /// <returns>The shortest distance in the specifed units.</returns>
        public static double HaversineDistance(Coordinate origin, Coordinate destination, DistanceUnitType units)
        {
            return HaversineDistance(origin.Latitude, origin.Longitude, destination.Latitude, destination.Longitude, units);
        }

        /// <summary>
        /// Calculate the distance between two coordinates on the surface of a sphere (Earth).
        /// </summary>
        /// <param name="origLat">Origin Latitude.</param>
        /// <param name="origLon">Origin Longitude.</param>
        /// <param name="destLat">Destination Latitude.</param>
        /// <param name="destLon">Destination Longitude.</param>
        /// <param name="units">Unit of distance measurement.</param>
        /// <returns>The shortest distance in the specifed units.</returns>
        public static double HaversineDistance(double origLat, double origLon, double destLat, double destLon, DistanceUnitType units)
        {
            double radius = GetEarthRadius(units);

            double dLat = ToRadians(destLat - origLat);
            double dLon = ToRadians(destLon - origLon);

            double a = Math.Pow(Math.Sin(dLat / 2), 2) + Math.Pow(Math.Cos(ToRadians(origLat)), 2) * Math.Pow(Math.Sin(dLon / 2), 2);
            double centralAngle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return radius * centralAngle;
        }

        #endregion

        /// <summary>Calculates the bearing between two coordinates.</summary>
        /// <param name="origin" type="Coordinate">Initial location.</param>
        /// <param name="dest" type="Coordinate">Second location.</param>
        /// <returns>The bearing angle between two coordinates.</returns>
        public static double CalculateBearing(Coordinate origin, Coordinate dest)
        {
            var lat1 = ToRadians(origin.Latitude);
            var lon1 = origin.Longitude;
            var lat2 = ToRadians(dest.Latitude);
            var lon2 = dest.Longitude;
            var dLon = ToRadians(lon2 - lon1);
            var y = Math.Sin(dLon) * Math.Cos(lat2);
            var x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);
            return (ToDegrees(Math.Atan2(y, x)) + 360) % 360;
        }

        /// <summary>Calculates a destination coordinate given a starting coordinate, bearing, and distance in KM's.</summary>
        /// <param name="origin" type="Coordinate">Initial location.</param>
        /// <param name="brng" type="Number">Bearing pointing towards new location.</param>
        /// <param name="arcLength" type="Number">A distance in KM's.</param>
        /// <param name="units" type="DistanceUnitType">Unit of measurement of the arcLength</param>
        /// <returns>A destination coordinate based on an origin point, bearing and distance.</returns>
        public static Coordinate CalculateCoord(Coordinate origin, double brng, double arcLength, DistanceUnitType units)
        {
            double earthRadius = GetEarthRadius(units);
            double lat1 = ToRadians(origin.Latitude),
                lon1 = ToRadians(origin.Longitude),
                centralAngle = arcLength / earthRadius;

            var lat2 = Math.Asin(Math.Sin(lat1) * Math.Cos(centralAngle) + Math.Cos(lat1) * Math.Sin(centralAngle) * Math.Cos(ToRadians(brng)));
            var lon2 = lon1 + Math.Atan2(Math.Sin(ToRadians(brng)) * Math.Sin(centralAngle) * Math.Cos(lat1), Math.Cos(centralAngle) - Math.Sin(lat1) * Math.Sin(lat2));

            return new Coordinate(){
                Latitude = ToDegrees(lat2), 
                Longitude = ToDegrees(lon2)
            };
        }

        #endregion

        #region Internal Methods

        #region Degree and Radian Conversions

        /// <summary>
        /// Converts an angle that is in degrees to radians. Angle * (PI / 180)
        /// </summary>
        /// <param name="angle">An angle in degrees</param>
        /// <returns>An angle in radians</returns>
        internal static double ToRadians(double angle)
        {
            return angle * (Math.PI / 180);
        }

        /// <summary>
        /// Converts an angle that is in radians to degress. Angle * (180 / PI)
        /// </summary>
        /// <param name="angle">An angle in radians</param>
        /// <returns>An angle in degrees</returns>
        internal static double ToDegrees(double angle)
        {
            return angle * (180 / Math.PI);
        }

        #endregion

        #region Earth Related Constants

        /// <summary>
        /// The approximate spherical radius of the Earth
        /// </summary>
        internal static class EarthRadius
        {
            /// <summary>
            /// Earth Radius in Kilometers
            /// </summary>
            public const double KM = 6378.135;

            /// <summary>
            /// Earth Radius in Meters
            /// </summary>
            public const double Meters = 6378135;

            /// <summary>
            /// Earth Radius in Miles
            /// </summary>
            public const double Miles = 3963.189;

            /// <summary>
            /// Earth Radius in Feet
            /// </summary>
            public const double Feet = 20925640;
        }

        #endregion

        #region Earth Radius

        /// <summary>
        /// Retrieves the radius of the earth in a specific distance unit for WGS84.
        /// </summary>
        /// <param name="units">Unit of distance measurement.</param>
        /// <returns>The radius of the earth in a specified distance units.</returns>
        internal static double GetEarthRadius(DistanceUnitType units)
        {
            switch (units)
            {
                case DistanceUnitType.Miles:
                    return EarthRadius.Miles;
                case DistanceUnitType.Kilometers:
                default:
                    return EarthRadius.KM;
            }
        }

        #endregion

        #endregion
    }
}
