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
    /// ConvertTime API Operation Reqeust
    /// </summary>
    public class ConvertTimeZoneRequest : BaseRestRequest
    {
        #region Public Props
        /// <summary>
        ///  If set to true then DST rule information will be returned in the response.
        /// </summary>
        public bool IncludeDstRules { get; set; }

        /// <summary>
        /// The UTC date time string for the specified location. The date must be specified to apply the correct DST.
        /// </summary>
        public DateTime LocationDateTime { get; set; }

        /// <summary>
        /// The ID of the destination time zone.
        /// </summary>
        public string DestinationTZID { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Create a Covert TZ Request for Given Datetime `datetime` and Destition ID string `DestID`
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="DestID"></param>
        public ConvertTimeZoneRequest(DateTime datetime, string DestID)
        {
            LocationDateTime = datetime;
            DestinationTZID = Uri.EscapeDataString(DestID);
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the URI for Convert TZ API request
        /// </summary>
        /// <returns></returns>
        public override string GetRequestUrl()
        {
            string headStr = "TimeZone/Convert/?";

            List<string> param_list = new List<string>()
            {
                string.Format("key={0}", BingMapsKey.ToString()),
                string.Format("includeDstRules={0}", IncludeDstRules.ToString().ToLower()),
                string.Format("dt={0}", DateTimeHelper.GetUTCString(LocationDateTime)),
                string.Format("desttz={0}", DestinationTZID)
            };

            return this.Domain + headStr + string.Join("&", param_list);
        }
        #endregion
    }
}
