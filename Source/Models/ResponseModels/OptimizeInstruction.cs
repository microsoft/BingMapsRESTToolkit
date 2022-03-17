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
using System.Runtime.Serialization;

namespace BingMapsRESTToolkit
{
    [DataContract]
    public class OptimizeInstruction
    {
        /// <summary>
        /// Type of instruction, any of the following:
        ///- LeaveFromStartPoint
        ///- TravelBetweenLocations
        ///- VisitLocation
        ///- TakeABreak
        ///- ArriveToEndPoint
        /// </summary>
        public OptimizeInstructionType InstructionType { get; set; }

        /// <summary>
        /// The unit used for distance.
        /// </summary>
        [DataMember(Name = "instructionType", EmitDefaultValue = false)]
        internal string instructionType
        {
            get
            {
                return System.Enum.GetName(typeof(OptimizeInstructionType), InstructionType);
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && System.Enum.TryParse<OptimizeInstructionType>(value, out OptimizeInstructionType x))
                {
                    InstructionType = x;
                    return;
                }

                InstructionType = OptimizeInstructionType.TravelBetweenLocations;
            }
        }

        /// <summary>
        /// Estimated travel time. Applies only for TravelBetweenLocations instructions.
        /// </summary>
        [DataMember(Name = "duration")]
        internal string Duration { get; set; }

        /// <summary>
        /// Estimated travel time. Applies only for TravelBetweenLocations instructions.
        /// </summary>
        public TimeSpan? DurationTimeSpan
        {
            get
            {
                if (TimeSpan.TryParse(Duration, out TimeSpan ts))
                {
                    return ts;
                }

                return null;
            }
            set
            {
                if (value == null)
                {
                    Duration = string.Empty;
                }
                else
                {
                    Duration = string.Format("{0:g}", value);
                }
            }
        }

        /// <summary>
        /// Estimate travel distance, in meters.
        /// </summary>
        [DataMember(Name = "distance")]
        public double Distance { get; set; }

        /// <summary>
        /// The estimated start time of the instruction.
        /// </summary>
        [DataMember(Name = "startTime")]
        internal string StartTime { get; set; }

        /// <summary>
        ///  The estimated start time of the instruction.
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
        /// The estimated end time of the instruction.
        /// </summary>
        [DataMember(Name = "endTime")]
        internal string EndTime { get; set; }

        /// <summary>
        /// The end of the time window for a break.
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
        /// Optional value present when the instruction type references a location, agent shift start/end point, or a defined itinerary item as defined in the request.
        /// </summary>
        [DataMember(Name = "itineraryItem")]
        public OptimizeItineraryItem ItineraryItem { get; set; }
    }
}
