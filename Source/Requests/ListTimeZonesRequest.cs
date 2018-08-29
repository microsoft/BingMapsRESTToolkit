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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// List Resoruce of Time Zones
    /// </summary>
    public class ListTimeZonesRequest : BaseRestRequest
    {
        #region Private Props
        /// <summary>
        /// Internal TimeZoneStandardType Enum: IANA vs WINDOWS
        /// </summary>
        private TimeZoneStandardType tz_standard { get; set; }

        /// <summary>
        /// Used to call the List operaiton or Lookup TZ info
        /// </summary>
        private bool list_operation { get; set; }
        #endregion

        #region Public Props
        /// <summary>
        ///  If set to true then DST rule information will be returned in the response.
        /// </summary>
        public bool IncludeDstRules { get; set; }

        /// <summary>
        /// The ID of the destination time zone.
        /// </summary>
        public string DestinationTZID { get; set; }

        /// <summary>
        /// Insert TZ Standards as String
        /// </summary>
        public string TimeZoneStandard
        {
            get
            {
                switch (tz_standard)
                {
                    case TimeZoneStandardType.IANA:
                        return "iana";
                    case TimeZoneStandardType.WINDOWS:
                    default:
                        return "windows";
                }
            }

            set
            {
                switch(value.Trim().ToLower())
                {
                    case "iana":
                        tz_standard = TimeZoneStandardType.IANA;
                        break;
                    case "windows":
                        tz_standard = TimeZoneStandardType.WINDOWS;
                        break;
                }
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for using the List Operaiton
        /// </summary>
        /// <param name="use_list_operation">Whether to use the List operation</param>
        public ListTimeZonesRequest(bool use_list_operation)
        {
            tz_standard = TimeZoneStandardType.WINDOWS;
            IncludeDstRules = false;
            list_operation = use_list_operation;
        }
        #endregion

        #region Public Method
        public override string GetRequestUrl()
        {
            List<string> param_list = new List<string>()
            {
                string.Format("key={0}", BingMapsKey.ToString()),
                string.Format("includeDstRules={0}", IncludeDstRules.ToString().ToLower())
            };

            string headStr;
            if (list_operation)
            {
                headStr = "TimeZone/List/?";
                if (TimeZoneStandard != null)
                    param_list.Add(string.Format("tzstd={0}", TimeZoneStandard));
                else
                    throw new Exception("Standard TZ Name ('Windows' or 'IANA') required.");
            }
            else
            {
                headStr = "TimeZone/?";
                if (DestinationTZID != null)
                    param_list.Add(string.Format("desttz={0}", DestinationTZID));
                else
                    throw new Exception("Destination TZ ID required.");


            }

            return this.Domain + headStr + string.Join("&", param_list);
        }
        #endregion
    }
}
