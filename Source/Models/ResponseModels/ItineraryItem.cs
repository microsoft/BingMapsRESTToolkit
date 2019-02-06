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
    /// Information that defines a step in the route. 
    /// </summary>
    [DataContract]
    public class ItineraryItem
    {
        /// <summary>
        /// A collection of ItineraryItems that divides an itinerary item into smaller steps. An itinerary item can have only one set of ChildItineraryItems. 
        /// </summary>
        [DataMember(Name = "childItineraryItems", EmitDefaultValue = false)]
        public ItineraryItem[] ChildItineraryItems { get; set; }

        /// <summary>
        /// The direction of travel associated with a maneuver on a route, such as south or southwest. This value is not supported for the Transit travel mode.
        /// </summary>
        [DataMember(Name = "compassDirection", EmitDefaultValue = false)]
        public string CompassDirection { get; set; }

        /// <summary>
        /// Information about one of the maneuvers that is part of the itinerary item. An ItineraryItem can contain more than one Detail collection.
        /// </summary>
        [DataMember(Name = "details", EmitDefaultValue = false)]
        public Detail[] Details { get; set; }

        /// <summary>
        /// The name or number of the exit associated with this itinerary step.
        /// </summary>
        [DataMember(Name = "exit", EmitDefaultValue = false)]
        public string Exit { get; set; }

        /// <summary>
        /// Additional information that may be helpful in following a route. In addition to the hint text, this element has an attribute hintType that specifies 
        /// what the hint refers to, such as “NextIntersection.” Hint is an optional element and a route step can contain more than one hint.
        /// </summary>
        [DataMember(Name = "hints", EmitDefaultValue = false)]
        public Hint[] Hints { get; set; }

        /// <summary>
        /// The type of icon to display. Possible values include: None, Airline, Auto, Bus, Ferry, Train, Walk, Other
        /// </summary>
        [DataMember(Name = "iconType", EmitDefaultValue = false)]
        public string IconType { get; set; }

        /// <summary>
        /// A description of a maneuver in a set of directions. 
        /// </summary>
        [DataMember(Name = "instruction", EmitDefaultValue = false)]
        public Instruction Instruction { get; set; }

        /// <summary>
        /// The coordinates of a point on the Earth where a maneuver is required, such as a left turn. A ManeuverPoint contains Latitude and Longitude elements. 
        /// This value is not supported for ItineraryItems that are part of a ChildItineraryItems collection.
        /// </summary>
        [DataMember(Name = "maneuverPoint", EmitDefaultValue = false)]
        public Point ManeuverPoint { get; set; }

        /// <summary>
        /// The side of the street where the destination is found based on the arrival direction. This field applies to the last itinerary item only. 
        /// Possible values include: Left, Right, Unknown
        /// </summary>
        [DataMember(Name = "sideOfStreet", EmitDefaultValue = false)]
        public string SideOfStreet { get; set; }

        /// <summary>
        /// Signage text for the route. There may be more than one sign value for an itinerary item.
        /// </summary>
        [DataMember(Name = "signs", EmitDefaultValue = false)]
        public string[] Signs { get; set; }

        /// <summary>
        /// The arrival or departure time for the transit step.
        /// </summary>
        [DataMember(Name = "time", EmitDefaultValue = false)]
        public string Time { get; set; }

        /// <summary>
        /// The arrival or departure time for the transit step.
        /// </summary>
        public DateTime TimeUtc
        {
            get
            {
                if (string.IsNullOrEmpty(Time))
                {
                    return DateTime.Now;
                }
                else
                {
                    return DateTimeHelper.FromOdataJson(Time);
                }
            }
            set
            {
                if (value == null)
                {
                    Time = string.Empty;
                }
                else
                {
                    var v = DateTimeHelper.ToOdataJson(value);

                    if (v != null)
                    {
                        Time = v;
                    }
                    else
                    {
                        Time = string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// The name or number of the toll zone.
        /// </summary>
        [DataMember(Name = "tollZone", EmitDefaultValue = false)]
        public string TollZone { get; set; }

        /// <summary>
        /// The name of the street that the route goes towards in the first itinerary item.
        /// </summary>
        [DataMember(Name = "towardsRoadName", EmitDefaultValue = false)]
        public string TowardsRoadName { get; set; }

        /// <summary>
        /// Information about the transit line associated with the itinerary item.
        /// </summary>
        [DataMember(Name = "transitLine", EmitDefaultValue = false)]
        public TransitLine TransitLine { get; set; }

        /// <summary>
        /// The list of transit stops associated with the itinerary item.
        /// </summary>
        [DataMember(Name = "transitStops", EmitDefaultValue = false)]
        public TransitStop[] TransitStops { get; set; }

        /// <summary>
        /// The ID assigned to the transit stop by the transit agency.
        /// </summary>
        [DataMember(Name = "transitStopId", EmitDefaultValue = false)]
        public int TransitStopId { get; set; }

        /// <summary>
        /// The end destination for the transit line in the direction you are traveling.
        /// </summary>
        [DataMember(Name = "transitTerminus", EmitDefaultValue = false)]
        public string TransitTerminus { get; set; }

        /// <summary>
        /// The physical distance covered by this route step. This value is not supported for the Transit travel mode.
        /// </summary>
        [DataMember(Name = "travelDistance", EmitDefaultValue = false)]
        public double TravelDistance { get; set; }

        /// <summary>
        /// The time that it takes, in seconds, to travel a corresponding TravelDistance.
        /// </summary>
        [DataMember(Name = "travelDuration", EmitDefaultValue = false)]
        public double TravelDuration { get; set; }

        /// <summary>
        /// The mode of travel for a specific step in the route. This value is not supported for ItineraryItems that are part of a ChildItineraryItems collection.
        /// </summary>
        [DataMember(Name = "travelMode", EmitDefaultValue = false)]
        public string TravelMode { get; set; }

        /// <summary>
        /// Information about a condition that may affect a specific step in the route. Warning is an optional element and a route step can contain more than one warning. 
        /// Warnings can include traffic incident information such as congestion, accident and blocked road reports.
        /// Warning elements are further defined by two attributes: Severity and WarningType.
        /// Severity can have the following values: Low Impact, Minor, Moderate, or Serious.
        /// </summary>
        [DataMember(Name = "warnings", EmitDefaultValue = false)]
        public Warning[] Warnings { get; set; }
    }
}
