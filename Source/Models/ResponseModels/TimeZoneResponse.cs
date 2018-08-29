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

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// TimeZone Resource
    /// </summary>
    [DataContract(Name = "timeZone")]
    public class TimeZoneResponse : Resource
    {

        /// <summary>
        /// Standard name of the time zone, e.g. Pacific standard time
        /// </summary>
        [DataMember(Name = "genericName", EmitDefaultValue = false)]
        public string GenericName { get; set; }

        /// <summary>
        /// Abbreviation for the time zone
        /// </summary>
        [DataMember(Name = "abbreviation", EmitDefaultValue = false)]
        public string Abbreviation { get; set; }

        /// <summary>
        /// Time zone name per the IANA standard
        /// </summary>
        [DataMember(Name = "ianaTimeZoneId", EmitDefaultValue = false)]
        public string IANATimeZoneId { get; set; }

        /// <summary>
        /// Time zone name as per the Microsoft Windows standard
        /// </summary>
        [DataMember(Name = "windowsTimeZoneId", EmitDefaultValue = false)]
        public string WindowsTimeZoneId { get; set; }

        /// <summary>
        /// Offset of time zone from UTC, in (+/-)hh:mm format
        /// </summary>
        [DataMember(Name = "utcOffset", EmitDefaultValue = false)]
        public string UtcOffset { get; set; }

        /// <summary>
        /// ConvertedTime Resource List
        /// </summary>
        [DataMember(Name = "convertedTime", EmitDefaultValue = false)]
        public ConvertedTimeResource ConvertedTime { get; set; }

        /// <summary>
        /// Dst Rule Resource List
        /// </summary>
        [DataMember(Name = "dstRule", EmitDefaultValue = false)]
        public DstRuleResource DstRule { get; set; }

    }
}
