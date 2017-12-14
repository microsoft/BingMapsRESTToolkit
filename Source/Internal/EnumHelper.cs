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
        internal static string DistanceUnitTypeToString(DistanceUnitType dut)
        {
            if(dut == DistanceUnitType.Miles)
            {
                return "mile";
            }

            return "kilometer";
        }

        /// <summary>
        /// Converts a string into a DistanceUnitType.
        /// </summary>
        /// <param name="dut">A string to convert.</param>
        /// <returns>A distance unit type.</returns>
        internal static DistanceUnitType DistanceUnitStringToEnum(string dut)
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
        /// Converts a string into a TimeUnitType.
        /// </summary>
        /// <param name="tut">A string to convert.</param>
        /// <returns>A time unit type.</returns>
        internal static TimeUnitType TimeUnitStringToEnum(string tut)
        {
            switch (tut.ToLowerInvariant())
            {                
                case "minutes":
                case "minute":
                case "min":
                case "mins":
                case "m":
                    return TimeUnitType.Minute;
                case "seconds":
                case "second":
                case "secs":
                case "sec":
                case "s":
                default:
                    return TimeUnitType.Second;
            }
        }

        /// <summary>
        /// Converts a string into a ConfidenceLevelType.
        /// </summary>
        /// <param name="confidence">A string to convert.</param>
        /// <returns>A confidence level type.</returns>
        internal static ConfidenceLevelType ConfidenceLevelTypeStringToEnum(string confidence)
        {
            if (!string.IsNullOrWhiteSpace(confidence))
            {
                switch (confidence.ToLowerInvariant())
                {
                    case "high":
                        return ConfidenceLevelType.High;
                    case "medium":
                        return ConfidenceLevelType.Medium;
                    case "low":
                        return ConfidenceLevelType.Low;
                }
            }

            return ConfidenceLevelType.None;
        }

        /// <summary>
        /// Converts a HazardousMaterialPermitType enumeration into its short form string format.
        /// </summary>
        /// <param name="hmpt">HazardousMaterialPermitType enumeration to convert.</param>
        /// <returns>A HazardousMaterialPermitType enumeration convrted into its short form string format.</returns>
        internal static string HazardousMaterialPermitTypeToString(HazardousMaterialPermitType hmpt)
        {
            switch (hmpt)
            {
                case HazardousMaterialPermitType.AllAppropriateForLoad:
                    return "AllAppropriateForLoad";
                case HazardousMaterialPermitType.Combustible:
                    return "C";
                case HazardousMaterialPermitType.Corrosive:
                    return "Cr";
                case HazardousMaterialPermitType.Explosive:
                    return "E";
                case HazardousMaterialPermitType.Flammable:
                    return "F";
                case HazardousMaterialPermitType.FlammableSolid:
                    return "FS";
                case HazardousMaterialPermitType.Gas:
                    return "G";
                case HazardousMaterialPermitType.Organic:
                    return "O";
                case HazardousMaterialPermitType.Poison:
                    return "P";
                case HazardousMaterialPermitType.PoisonousInhalation:
                    return "PI";
                case HazardousMaterialPermitType.Radioactive:
                    return "R";
                case HazardousMaterialPermitType.None:
                default:
                    return "None";
            }
        }

        /// <summary>
        /// Converts a HazardousMaterialType enumeration into its short form string format.
        /// </summary>
        /// <param name="hmt">HazardousMaterialType enumeration to convert.</param>
        /// <returns>A HazardousMaterialType enumeration convrted into its short form string format.</returns>
        internal static string HazardousMaterialTypeToString(HazardousMaterialType hmt)
        {
            switch (hmt)
            {
                case HazardousMaterialType.Combustable:
                    return "C";
                case HazardousMaterialType.Corrosive:
                    return "Cr";
                case HazardousMaterialType.Explosive:
                    return "E";
                case HazardousMaterialType.Flammable:
                    return "F";
                case HazardousMaterialType.FlammableSolid:
                    return "FS";
                case HazardousMaterialType.Gas:
                    return "G";
                case HazardousMaterialType.GoodsHarmfulToWater:
                    return "WH";
                case HazardousMaterialType.Organic:
                    return "O";
                case HazardousMaterialType.Other:
                    return "Other";
                case HazardousMaterialType.Poison:
                    return "P";
                case HazardousMaterialType.PoisonousInhalation:
                    return "PI";
                case HazardousMaterialType.Radioactive:
                    return "R";
                case HazardousMaterialType.None:
                default:
                    return "None";
            }
        }
    }
}
