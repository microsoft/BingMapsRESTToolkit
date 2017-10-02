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

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// Helper class for converting enums between their string versions.
    /// </summary>
    internal static class EnumHelper
    {
        /// <summary>
        /// Converts distance unit type to a string.
        /// </summary>
        /// <param name="dut">Distance unit to convert.</param>
        /// <returns>A string version of the distance unit.</returns>
        public static string DistanceUnitTypeToString(DistanceUnitType dut)
        {
            if(dut == DistanceUnitType.Miles)
            {
                return "miles";
            }

            return "kilometers";
        }

        /// <summary>
        /// Converts a string into a DistanceUnitType.
        /// </summary>
        /// <param name="dut">A string to convert.</param>
        /// <returns>A distance unit type.</returns>
        public static DistanceUnitType DistanceUnitStringToEnum(string dut)
        {
            switch (dut.ToLowerInvariant())
            {
                case "miles":
                case "mile":
                case "mi":
                case "imperial":
                    return DistanceUnitType.Miles;
                case "kilometers":
                case "kiloemeter":
                case "km":
                case "kilometres":
                case "kilometre":
                case "metric":
                default:
                    return DistanceUnitType.Kilometers;
            }
        }

        /// <summary>
        /// Converts time unit type to a string.
        /// </summary>
        /// <param name="tut">Time unit to convert.</param>
        /// <returns>A string version of the time unit.</returns>
        public static string TimeUnitTypeToString(TimeUnitType tut)
        {
            if (tut == TimeUnitType.Minutes)
            {
                return "minutes";
            }

            return "seconds";
        }

        /// <summary>
        /// Converts a string into a TimeUnitType.
        /// </summary>
        /// <param name="tut">A string to convert.</param>
        /// <returns>A time unit type.</returns>
        public static TimeUnitType TimeUnitStringToEnum(string tut)
        {
            switch (tut.ToLowerInvariant())
            {
                case "minutes":
                case "min":
                case "mins":
                case "m":
                    return TimeUnitType.Minutes;
                case "seconds":
                case "second":
                case "secs":
                case "sec":
                case "s":
                default:
                    return TimeUnitType.Seconds;
            }
        }
    }
}
