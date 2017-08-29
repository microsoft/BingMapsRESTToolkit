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

using System.Runtime.Serialization;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// Information about the transit line associated with the itinerary item.
    /// </summary>
    [DataContract]
    public class TransitLine
    {
        /// <summary>
        /// The full name of the transit line.
        /// </summary>
        [DataMember(Name = "verboseName", EmitDefaultValue = false)]
        public string VerboseName { get; set; }

        /// <summary>
        /// The abbreviated name of the transit line, such as the bus number.
        /// </summary>
        [DataMember(Name = "abbreviatedName", EmitDefaultValue = false)]
        public string AbbreviatedName { get; set; }

        /// <summary>
        /// The ID associated with the transit agency.
        /// </summary>
        [DataMember(Name = "agencyId", EmitDefaultValue = false)]
        public long AgencyId { get; set; }

        /// <summary>
        /// The name of the transit agency.
        /// </summary>
        [DataMember(Name = "agencyName", EmitDefaultValue = false)]
        public string AgencyName { get; set; }

        /// <summary>
        /// The color associated with the transit line. The color is provided as an RGB value.
        /// </summary>
        [DataMember(Name = "lineColor", EmitDefaultValue = false)]
        public long LineColor { get; set; }

        /// <summary>
        /// The color to use for text associated with the transit line. The color is provided as an RGB value.
        /// </summary>
        [DataMember(Name = "lineTextColor", EmitDefaultValue = false)]
        public long LineTextColor { get; set; }

        /// <summary>
        /// The URI for the transit agency.
        /// </summary>
        [DataMember(Name = "uri", EmitDefaultValue = false)]
        public string Uri { get; set; }

        /// <summary>
        /// The phone number of the transit agency.
        /// </summary>
        [DataMember(Name = "phoneNumber", EmitDefaultValue = false)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The contact information for the provider of the transit information.
        /// </summary>
        [DataMember(Name = "providerInfo", EmitDefaultValue = false)]
        public string ProviderInfo { get; set; }
    }
}
