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
    /// <summary>
    /// A traffic incident response object.
    /// </summary>
    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class TrafficIncident : Resource
    {
        /// <summary>
        /// The latitude and longitude coordinates where you encounter the incident.
        /// </summary>
        [DataMember(Name = "point", EmitDefaultValue = false)]
        public Point Point { get; set; }

        /// <summary>
        /// A description of the congestion. 
        /// </summary>
        [DataMember(Name = "congestion", EmitDefaultValue = false)]
        public string Congestion { get; set; }

        /// <summary>
        /// A description of the incident.
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        /// A description of a detour.
        /// </summary>
        [DataMember(Name = "detour", EmitDefaultValue = false)]
        public string Detour { get; set; }

        /// <summary>
        /// The time the incident occurred as an Odata JSON Date string. 
        /// </summary>
        [DataMember(Name = "start", EmitDefaultValue = false)]
        public string Start { get; set; }

        /// <summary>
        /// The time the incident occurred as a DateTime Object.
        /// </summary>
        public DateTime StartDateTimeUtc
        {
            get
            {
                if (string.IsNullOrEmpty(Start))
                {
                    return DateTime.Now;
                }
                else
                {
                    return DateTimeHelper.FromOdataJson(Start);
                }
            }
            set
            {
                if (value == null)
                {
                    Start = string.Empty;
                }
                else
                {
                    var v = DateTimeHelper.ToOdataJson(value);

                    if (v != null)
                    {
                        Start = v;
                    }
                    else
                    {
                        Start = string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// The time the incident will end as an Odata JSON Date string. 
        /// </summary>
        [DataMember(Name = "end", EmitDefaultValue = false)]
        public string End { get; set; }

        /// <summary>
        /// The time the incident will end as a DateTime Object.
        /// </summary>
        public DateTime EndDateTimeUtc
        {
            get
            {
                if (string.IsNullOrEmpty(End))
                {
                    return DateTime.Now;
                }
                else
                {
                    return DateTimeHelper.FromOdataJson(End);
                }
            }
            set
            {
                if (value == null)
                {
                    End = string.Empty;
                }
                else
                {
                    var v = DateTimeHelper.ToOdataJson(value);

                    if (v != null)
                    {
                        End = v;
                    }
                    else
                    {
                        End = string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// A unique ID for the incident.
        /// </summary>
        [DataMember(Name = "incidentId", EmitDefaultValue = false)]
        public long IncidentId { get; set; }

        /// <summary>
        /// A description specific to lanes, such as lane closures. 
        /// </summary>
        [DataMember(Name = "lane", EmitDefaultValue = false)]
        public string Lane { get; set; }

        /// <summary>
        /// The time the incident information was last updated as an Odata JSON Date string. 
        /// </summary>
        [DataMember(Name = "lastModified", EmitDefaultValue = false)]
        public string LastModified { get; set; }

        /// <summary>
        /// The time the incident information was last updated as a DateTime Object. 
        /// </summary>
        public DateTime LastModifiedDateTimeUtc
        {
            get
            {
                if (string.IsNullOrEmpty(LastModified))
                {
                    return DateTime.Now;
                }
                else
                {
                    return DateTimeHelper.FromOdataJson(LastModified);
                }
            }
            set
            {
                if (value == null)
                {
                    LastModified = string.Empty;
                }
                else
                {
                    var v = DateTimeHelper.ToOdataJson(value);

                    if (v != null)
                    {
                        LastModified = v;
                    }
                    else
                    {
                        LastModified = string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// A value of true indicates that there is a road closure. 
        /// </summary>
        [DataMember(Name = "roadClosed", EmitDefaultValue = false)]
        public bool RoadClosed { get; set; }

        /// <summary>
        /// Specifies the level of importance of incident. Where: 1: LowImpact, 2: Minor, 3: Moderate, 4: Serious.
        /// </summary>
        [DataMember(Name = "severity", EmitDefaultValue = false)]
        public int Severity { get; set; }

        /// <summary>
        /// The coordinates of the end of a traffic incident, such as the end of a construction zone. 
        /// </summary>
        [DataMember(Name = "toPoint", EmitDefaultValue = false)]
        public Point ToPoint { get; set; }

        /// <summary>
        /// A collection of traffic location codes. This field is provided when you set the includeLocationCodes parameter to true in the request. 
        /// These codes associate an incident with pre-defined road segments. A subscription is typically required to be able to interpret these 
        /// codes for a geographical area or country.
        /// </summary>
        [DataMember(Name = "locationCodes", EmitDefaultValue = false)]
        public string[] LocationCodes { get; set; }

        /// <summary>
        /// Specifies the type of incident. Where 1: Accident, 2: Congestion, 3: DisabledVehicle, 4: MassTransit, 5: Miscellaneous, 6: OtherNews, 7: PlannedEvent, 8: RoadHazard, 9: Construction, 10: Alert, 11: Weather
        /// </summary>
        [DataMember(Name = "type", EmitDefaultValue = false)]
        public new int Type { get; set; }

        /// <summary>
        /// A value of true indicates that the incident has been visually verified or otherwise officially confirmed by a source like the local police department.
        /// </summary>
        [DataMember(Name = "verified", EmitDefaultValue = false)]
        public bool Verified { get; set; }
    }
}
