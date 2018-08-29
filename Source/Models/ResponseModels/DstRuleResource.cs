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

using System.Runtime.Serialization;
using System;
namespace BingMapsRESTToolkit
{
    /// <summary>
    /// Resource used by TimeZone API
    /// </summary>
    [DataContract]
    public class DstRuleResource
    {
        /// <summary>
        /// Internal Start Datetime
        /// </summary>
        private DateTime StartTime {get; set;}
        /// <summary>
        /// Interntal End Time datetime
        /// </summary>
        private DateTime EndTime { get; set; }

        /// <summary>
        /// The month (three-letter abbreviation) when DST starts, e.g. Mar
        /// </summary>
        [DataMember(Name = "dstStartMonth", EmitDefaultValue = false)]
        public string DstStartMonth { get; set; }

        /// <summary>
        /// DST starting date rule: See https://data.iana.org/time-zones/tz-how-to.html
        /// </summary>
        [DataMember(Name = "dstStartDateRule", EmitDefaultValue = false)]
        public string DstStartDateRule { get; set; }

        /// <summary>
        /// The local time when DST starts, hh:mm format
        /// </summary>
        [DataMember(Name = "dstStartTime", EmitDefaultValue = false)]
        public string DstStartTime
        {
            get
            {
                return DateTimeHelper.GetUTCString(StartTime);
            }
            set
            {
                StartTime = DateTimeHelper.GetDateTimeFromUTCString(value);
            }
        }

        /// <summary>
        /// The offset to apply during DST, (+/-)h:mm format
        /// </summary>
        [DataMember(Name = "dstAdjust1", EmitDefaultValue = false)]
        public string DstAdjust1 { get; set; }

        /// <summary>
        /// The month (three-letter abbreviation) when DST Ends, e.g. Sep
        /// </summary>
        [DataMember(Name = "dstEndMonth", EmitDefaultValue = false)]
        public string DstEndMonth { get; set; }

        /// <summary>
        /// DST ending date rule: See https://data.iana.org/time-zones/tz-how-to.html
        /// </summary>
        [DataMember(Name = "dstEndDateRule", EmitDefaultValue = false)]
        public string DstEndDateRule { get; set; }

        /// <summary>
        /// The local time when DST starts, hh:mm format
        /// </summary>
        [DataMember(Name = "dstEndTime", EmitDefaultValue = false)]
        public string DstEndTime
        {
            get
            {
                return DateTimeHelper.GetUTCString(EndTime);
            }
            set
            {
                EndTime = DateTimeHelper.GetDateTimeFromUTCString(value);
            }
        }

        /// <summary>
        /// The offset to apply outside DST, (+/-)h:mm format
        /// </summary>
        [DataMember(Name = "dstAdjust2", EmitDefaultValue = false)]
        public string DstAdjust2 { get; set; }
    }
}
