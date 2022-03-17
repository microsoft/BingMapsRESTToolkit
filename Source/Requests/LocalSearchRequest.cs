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

using System;
using System.Collections.Generic;
using System.Text;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// Returns a list of business entities centered around a location or a geographic region.
    /// </summary>
    public class LocalSearchRequest : BaseRestRequest
    {

        #region Private Properties

        private int maxResults = 5;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor with default values
        /// </summary>
        public LocalSearchRequest() : base()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The specified types used to filter the local entities returned by the Local Search API.
        /// A string that contains information about a location, such as an address or landmark name.
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Specifies the maximum number of locations to return in the response.
        /// </summary>
        public int MaxResults
        {
            get { return maxResults; }
            set
            {
                if (value > 0 && value <= 20)
                {
                    maxResults = value;
                }
            }
        }

        /// <summary>
        /// The specified types used to filter the local entities returned by the Local Search API.
        /// A comma-separated list of string type identifiers. 
        /// See the list of available Type IDs https://docs.microsoft.com/en-us/bingmaps/rest-services/common-parameters-and-types/type-identifiers/
        /// </summary>
        public List<string> Types { get; set; }

        #endregion

        #region Public Methods

        public override string GetRequestUrl()
        {
            if(UserCircularMapView == null && UserLocation == null && UserMapView == null)
            {
                throw new Exception("A user location must be specified.");
            }

            var sb = new StringBuilder(this.Domain);
            sb.Append("LocalSearch/");

            if (!string.IsNullOrWhiteSpace(Query))
            {
                sb.AppendFormat("?query={0}", Query);
            } 
            else if(Types != null && Types.Count > 0)
            {
                sb.AppendFormat("?type={0}", string.Join(",", Types));
            }
            else
            {
                throw new Exception("A query or types must be specified.");
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
