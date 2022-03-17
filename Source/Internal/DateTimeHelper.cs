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
using System.Text.RegularExpressions;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// A helper class for working iwth OData Dates.
    /// </summary>
    public static class DateTimeHelper
    {
        private static string dt_format = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'";

        /// <summary>
        /// Get a UTC string from a Datetime
        /// </summary>
        /// <param name="dt">Returns UTC Datetime Object</param>
        /// <returns></returns>
        public static string GetUTCString(DateTime dt)
        {
            return dt.ToUniversalTime().ToString(dt_format);
        }

        /// <summary>
        /// Return a Datetime from UTC datetime string
        /// </summary>
        /// <param name="dt_string">UTC datetime string</param>
        /// <returns></returns>
        public static DateTime GetDateTimeFromUTCString(string dt_string)
        {
            return DateTime.Parse(dt_string);
        }

        /// <summary>
        /// Converts a DateTime object into an OData date.
        /// </summary>
        /// <param name="dateTime">The DateTime to convert.</param>
        /// <returns>An OData version of the DateTime.</returns>
        internal static string ToOdataJson(DateTime dateTime)
        {
            var d1 = new DateTime(1970, 1, 1);
            var d2 = dateTime.ToUniversalTime();
            var ts = new TimeSpan(d2.Ticks - d1.Ticks);
            return "\\/Date(" + ts.TotalMilliseconds.ToString("#") + ")\\/";
        }

        /// <summary>
        /// Converts an OData Date into a DateTime object.
        /// </summary>
        /// <param name="jsonDate">OData Date to convert.</param>
        /// <returns>The converted DateTime object.</returns>
        internal static DateTime FromOdataJson(string jsonDate)
        {
            //  /Date(1235764800000)/
            //  /Date(1467298867000-0700)/

            jsonDate = jsonDate.Replace("\\/Date(", "").Replace("/Date(", "").Replace(")/", "").Replace(")\\/", "");

            long ms = 0;    // number of milliseconds since midnight Jan 1, 1970
            long hours = 0;

            int pIdx = jsonDate.IndexOf("+");
            int mIdx = jsonDate.IndexOf("-");

            if (pIdx > 0)
            {
                ms = long.Parse(jsonDate.Substring(0, pIdx));

                //Hack: The offset is meant to be in minutes, but for some reason the response from the REST services uses 700 which is meant to be 7 hours.
                hours = long.Parse(jsonDate.Substring(mIdx)) / 100;
            }
            else if (mIdx > 0)
            {
                ms = long.Parse(jsonDate.Substring(0, mIdx));

                //Hack: The offset is meant to be in minutes, but for some reason the response from the REST services uses 700 which is meant to be 7 hours.
                hours = long.Parse(jsonDate.Substring(mIdx)) / 100;
            }
            else
            {
                ms = long.Parse(jsonDate);
            }
            
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(ms).AddHours(hours); 
        }
    }
}
