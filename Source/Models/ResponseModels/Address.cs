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
    /// The postal address for the location.
    /// </summary>
    [DataContract]
    public class Address
    {
        /// <summary>
        /// The official street line of an address relative to the area, as specified by the Locality, or PostalCode, properties. 
        /// Typical use of this element would be to provide a street address or any official address.
        /// </summary>
        [DataMember(Name = "addressLine", EmitDefaultValue = false)]
        public string AddressLine { get; set; }

        /// <summary>
        /// A string specifying the subdivision name in the country or region for an address. This element is typically treated as 
        /// the first order administrative subdivision, but in some cases it is the second, third, or fourth order subdivision in a 
        /// country, dependency, or region. 
        /// </summary>
        [DataMember(Name = "adminDistrict", EmitDefaultValue = false)]
        public string AdminDistrict { get; set; }

        /// <summary>
        /// A string specifying the subdivision name in the country or region for an address. This element is used when there is 
        /// another level of subdivision information for a location, such as the county.
        /// </summary>
        [DataMember(Name = "adminDistrict2", EmitDefaultValue = false)]
        public string AdminDistrict2 { get; set; }

        /// <summary>
        /// A string specifying the country or region name of an address.
        /// </summary>
        [DataMember(Name = "countryRegion", EmitDefaultValue = false)]
        public string CountryRegion { get; set; }

        /// <summary>
        /// A string specifying the populated place for the address. This typically refers to a city, but may refer to a suburb or a 
        /// neighborhood in certain countries.
        /// </summary>
        [DataMember(Name = "locality", EmitDefaultValue = false)]
        public string Locality { get; set; }

        /// <summary>
        /// A string specifying the post code, postal code, or ZIP Code of an address.
        /// </summary>
        [DataMember(Name = "postalCode", EmitDefaultValue = false)]
        public string PostalCode { get; set; }

        /// <summary>
        /// A string specifying the two-letter ISO country code. You must specify include=ciso2 in your request to return this ISO 
        /// country code.
        /// </summary>
        [DataMember(Name = "countryRegionIso2", EmitDefaultValue = false)]
        public string CountryRegionIso2 { get; set; }

        /// <summary>
        /// A string specifying the complete address. This address may not include the country or region.
        /// </summary>
        [DataMember(Name = "formattedAddress", EmitDefaultValue = false)]
        public string FormattedAddress { get; set; }

        /// <summary>
        /// A string specifying the neighborhood for an address. You must specify includeNeighborhood=1 in your request to return 
        /// the neighborhood.
        /// </summary>
        [DataMember(Name = "neighborhood", EmitDefaultValue = false)]
        public string Neighborhood { get; set; }

        /// <summary>
        /// A string specifying the name of the landmark when there is a landmark associated with an address.
        /// </summary>
        [DataMember(Name = "landmark", EmitDefaultValue = false)]
        public string Landmark { get; set; }

        /// <summary>
        /// Only returned by Autosuggest API.
        /// </summary>
        [DataMember(Name = "houseNumber", EmitDefaultValue = false)]
        public string HouseNumber { get; set; }

        /// <summary>
        /// Only returned by Autosuggest API.
        /// </summary>
        [DataMember(Name = "streetName", EmitDefaultValue = false)]
        public string StreetName { get; set; }
    }
}
