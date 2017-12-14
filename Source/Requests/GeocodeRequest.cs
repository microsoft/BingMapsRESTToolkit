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
using System.Text;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// Geocodes a query to its coordinates.
    /// </summary>
    public class GeocodeRequest : BaseRestRequest
    {
        #region Private Properties

        private int maxResults = 5;

        #endregion

        #region Public Properties

        /// <summary>
        /// A free form string address or Landmark. Overrides the Address values if both are specified.
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// The Address to geocode.
        /// </summary>
        public SimpleAddress Address { get; set; }

        /// <summary>
        /// Specifies the maximum number of locations to return in the response.
        /// </summary>
        public int MaxResults
        {
            get { return maxResults; }
            set {
                if (value > 0 && value <= 20)
                {
                    maxResults = value;
                }
            }
        }

        /// <summary>
        /// When you specified the two-letter ISO country code is included for addresses in the response. 
        /// </summary>
        public bool IncludeIso2 { get; set; }

        /// <summary>
        /// Specifies to include the neighborhood in the response when it is available. 
        /// </summary>
        public bool IncludeNeighborhood { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the request URL. If both a Query and Address are specified, the Query value will be used. Throws an exception if a Query or Address value is not specified.
        /// </summary>
        /// <returns>Geocode request URL for GET request.</returns>
        public override string GetRequestUrl()
        {
            var sb = new StringBuilder(this.Domain);
            sb.Append("Locations");
            
            if (!string.IsNullOrWhiteSpace(Query))
            {
                sb.AppendFormat("?q={0}", Uri.EscapeDataString(Query));
            }
            else if (Address != null)
            {
                string seperator = "?";

                if (!string.IsNullOrWhiteSpace(Address.AddressLine))
                {
                    sb.AppendFormat("{0}addressLine={1}", seperator, Uri.EscapeDataString(Address.AddressLine));
                    seperator = "&";
                }

                if (!string.IsNullOrWhiteSpace(Address.Locality))
                {
                    sb.AppendFormat("{0}locality={1}", seperator, Uri.EscapeDataString(Address.Locality));
                    seperator = "&";
                }

                if (!string.IsNullOrWhiteSpace(Address.AdminDistrict))
                {
                    sb.AppendFormat("{0}adminDistrict={1}", seperator, Uri.EscapeDataString(Address.AdminDistrict));
                    seperator = "&";
                }

                if (!string.IsNullOrWhiteSpace(Address.PostalCode))
                {
                    sb.AppendFormat("{0}postalCode={1}", seperator, Uri.EscapeDataString(Address.PostalCode));
                    seperator = "&";
                }

                if (!string.IsNullOrWhiteSpace(Address.CountryRegion))
                {
                    sb.AppendFormat("{0}countryRegion={1}", seperator, Uri.EscapeDataString(Address.CountryRegion));
                }
            }
            else
            {
                throw new Exception("No Query or Address value specified.");
            }

            if (IncludeIso2)
            {
                sb.Append("&incl=ciso2");
            }

            if (IncludeNeighborhood)
            {
                sb.Append("&inclnb=1");
            }

            if (maxResults != 5)
            {
                sb.AppendFormat("&maxResults={0}", maxResults);
            }

            sb.Append(GetBaseRequestUrl());

            return sb.ToString();
        }

        #endregion
    }
}
