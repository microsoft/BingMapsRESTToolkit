/*
 * Copyright(c) 2018 Microsoft Corporation. All rights reserved. 
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

namespace BingMapsRESTToolkit
{

    /// <summary>
    /// Request to Find a Time Zone by Query or Point
    /// </summary>
    public class FindTimeZoneRequest : BaseRestRequest
    {
        #region Public Props
        /// <summary>
        /// The Query to Find TZ
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// The Point of Find TZ
        /// </summary>
        public Coordinate Point { get; set; }

        /// <summary>
        /// Optional. The UTC date time string for the specified location. The date must be specified to apply the correct DST.
        /// </summary>
        public DateTime? LocationDateTime { get; set; }

        /// <summary>
        /// Optional. If set to true then DST rule information will be returned in the response.
        /// </summary>
        public bool IncludeDstRules { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Construct or Finding a Time Zone by Query and Datetime
        /// </summary>
        /// <param name="query"></param>
        /// <param name="datetime"></param>
        public FindTimeZoneRequest(string query, DateTime datetime)
        {
            Point = null;
            Query = query;
            IncludeDstRules = false;
            LocationDateTime = datetime;
        }

        /// <summary>
        /// Find a TimeZone at a specified Location
        /// </summary>
        /// <param name="point"></param>
        public FindTimeZoneRequest(Coordinate point)
        {
            Point = point;
            Query = "";
            IncludeDstRules = false;
        }

        /// <summary>
        /// Constuctor for Find Time Zone without Specified Point or Query
        /// </summary>
        public FindTimeZoneRequest()
        {
            Point = null;
            Query = "";
            IncludeDstRules = false;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Returns URI for Find TimeZone Request
        /// </summary>
        /// <returns></returns>
        public override string GetRequestUrl()
        {
            string headStr;

            if ((Query != "" && Query != null) && Point == null)
            {
                var query = Uri.EscapeDataString(Query);
                headStr = string.Format("TimeZone/?query={0}&", query);
            }
            else if (Point != null && (Query == "" || Query == null))
            {
                headStr = string.Format("TimeZone/{0}?", Point.ToString());
            }
            else
            {
                throw new Exception("To use Find a Timezone specify either `Point` or `Query` but not both.");
            }

            List<string> param_list = new List<string>()
            {
                string.Format("key={0}", BingMapsKey.ToString()),
                string.Format("includeDstRules={0}", IncludeDstRules.ToString().ToLower())
            };

            if (LocationDateTime.HasValue)
                param_list.Add(string.Format("dt={0}", DateTimeHelper.GetUTCString(LocationDateTime.Value)));

            return this.Domain + headStr + string.Join("&", param_list);
        }
        #endregion
    }
}
