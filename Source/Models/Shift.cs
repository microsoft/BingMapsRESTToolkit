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
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// Details about an agents shift.
    /// </summary>
    [DataContract]
    public class Shift
    {
        /// <summary>
        /// Time that the shift starts for agent.
        /// </summary>
        [DataMember(Name = "startTime")]
        internal string StartTime { get; set; }

        /// <summary>
        /// Time that the shift starts for agent.
        /// </summary>
        public DateTime StartTimeUtc
        {
            get
            {
                if (string.IsNullOrEmpty(StartTime))
                {
                    return DateTime.Now;
                }
                else
                {
                    return DateTimeHelper.GetDateTimeFromUTCString(StartTime);
                }
            }
            set
            {
                if (value == null)
                {
                    StartTime = string.Empty;
                }
                else
                {
                    var v = DateTimeHelper.GetUTCString(value);

                    if (v != null)
                    {
                        StartTime = v;
                    }
                    else
                    {
                        StartTime = string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// Location of the agent at start of shift.
        /// </summary>
        [DataMember(Name = "startLocation")]
        public SimpleWaypoint StartLocation { get; set; }

        /// <summary>
        /// Time that the shift ends for agent.
        /// </summary>
        [DataMember(Name = "endTime")]
        internal string EndTime { get; set; }

        /// <summary>
        /// Time that the shift ends for agent.
        /// </summary>
        public DateTime EndTimeUtc
        {
            get
            {
                if (string.IsNullOrEmpty(EndTime))
                {
                    return DateTime.Now;
                }
                else
                {
                    return DateTimeHelper.GetDateTimeFromUTCString(EndTime);
                }
            }
            set
            {
                if (value == null)
                {
                    EndTime = string.Empty;
                }
                else
                {
                    var v = DateTimeHelper.GetUTCString(value);

                    if (v != null)
                    {
                        EndTime = v;
                    }
                    else
                    {
                        EndTime = string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// Location of the agent at end of shift.
        /// </summary>
        [DataMember(Name = "endLocation")]
        public SimpleWaypoint EndLocation { get; set; }

        /// <summary>
        /// Breaks the agent has during the shift.
        /// </summary>
        [DataMember(Name = "breaks")]
        public Break[] Breaks { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("{");

            if (StartTimeUtc != null)
            {
                sb.AppendFormat("\"startTime\":\"{0}\",", StartTime);
            } 
            else
            {
                throw new Exception("No start time for shift specified.");
            }

            if (StartLocation.Coordinate != null)
            {
                sb.Append("\"startLocation\":{");
                sb.AppendFormat(CultureInfo.InvariantCulture, "\"latitude\":{0:0.#####},\"longitude\":{1:0.#####}", StartLocation.Latitude, StartLocation.Longitude);
                sb.Append("},");
            }
            else if (!string.IsNullOrWhiteSpace(StartLocation.Address))
            {
                sb.AppendFormat("\"startAddress\":\"{0}\",", StartLocation.Address);
            }
            else
            {
                throw new Exception("Start location must be specified in shift.");
            }

            if (EndTimeUtc != null)
            {
                sb.AppendFormat("\"endTime\":\"{0}\",", EndTime);
            }
            else
            {
                throw new Exception("No end time for shift specified.");
            }

            if (EndLocation.Coordinate != null)
            {
                sb.Append("\"endLocation\":{");
                sb.AppendFormat(CultureInfo.InvariantCulture, "\"latitude\":{0:0.#####},\"longitude\":{1:0.#####}", EndLocation.Latitude, EndLocation.Longitude);
                sb.Append("},");
            } 
            else if (!string.IsNullOrWhiteSpace(EndLocation.Address))
            {
                sb.AppendFormat("\"endAddress\":\"{0}\",", EndLocation.Address);
            } 
            else
            {
                throw new Exception("End location must be specified in shift.");
            }

            if(Breaks != null && Breaks.Length > 0)
            {
                sb.Append("\"breaks\":[");

                foreach(var b in Breaks)
                {
                    sb.Append(b.ToString());
                    sb.Append(",");
                }

                sb.Length--;
                sb.Append("],");
            }

            //Remove trailing comma.
            sb.Length--;

            sb.Append("}");

            return sb.ToString();
        }
    }
}
