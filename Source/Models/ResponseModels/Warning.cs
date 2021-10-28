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
    /// Itinerary step warning information.
    /// </summary>
    [DataContract]
    public class Warning
    {
        /// <summary>
        /// Where the warning starts.
        /// </summary>
        [DataMember(Name = "origin", EmitDefaultValue = false)]
        public string Origin { get; set; }

        /// <summary>
        /// Where the warning starts.
        /// </summary>
        public Coordinate OriginCoordinate
        {
            get
            {
                if (!string.IsNullOrEmpty(Origin))
                {
                    var latLng = Origin.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    double lat;
                    double lon;

                    if (latLng.Length >= 2 && double.TryParse(latLng[0], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out lat) 
                        && double.TryParse(latLng[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out lon))
                    {
                        return new Coordinate(lat, lon);
                    }
                }

                    return null;
            }
            set
            {
                if (value == null)
                {
                    Origin = string.Empty;
                }
                else
                {
                    Origin = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0},{1}", value.Latitude, value.Longitude);
                }
            }
        }

        /// <summary>
        /// Severity can have the following values: Low Impact, Minor, Moderate, or Serious.
        /// </summary>
        [DataMember(Name = "severity", EmitDefaultValue = false)]
        public string Severity { get; set; }

        /// <summary>
        /// The warning information.
        /// </summary>
        [DataMember(Name = "text", EmitDefaultValue = false)]
        public string Text { get; set; }

        /// <summary>
        /// Where the warning ends.
        /// </summary>
        [DataMember(Name = "to", EmitDefaultValue = false)]
        public string To { get; set; }

        /// <summary>
        /// Where the warning ends.
        /// </summary>
        public Coordinate ToCoordinate
        {
            get
            {
                if (!string.IsNullOrEmpty(To))
                {
                    var latLng = To.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    double lat;
                    double lon;

                    if (latLng.Length >= 2 && double.TryParse(latLng[0], out lat) && double.TryParse(latLng[1], out lon))
                    {
                        return new Coordinate(lat, lon);
                    }
                }

                return null;
            }
            set
            {
                if (value == null)
                {
                    To = string.Empty;
                }
                else
                {
                    To = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0},{1}", value.Latitude, value.Longitude);
                }
            }
        }

        /// <summary>
        /// The type of warning.
        /// </summary>
        [DataMember(Name = "warningType", EmitDefaultValue = false)]
        public string WarningType { get; set; }

        /// <summary>
        /// An enumeration version of the warning type.
        /// </summary>
        public WarningType WarningTypeEnum
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(WarningType))
                {
                    try
                    {
                        return (WarningType)Enum.Parse(typeof(WarningType), WarningType);
                    }
                    catch { }
                }

                return BingMapsRESTToolkit.WarningType.None;
            }
            set
            {
                WarningType = value.ToString();
            }
        }
    }
}
