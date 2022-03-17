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
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// Returns a list of suggested entities which the user is most likely searching for.
    /// </summary>
    public class AutosuggestRequest : BaseRestRequest
    {
        #region Private Properties

        private int maxResults = 7;

        #endregion

        #region Constructor

        /// <summary>
        /// Returns a list of suggested entities which the user is most likely searching for.
        /// </summary>
        public AutosuggestRequest()
        {
            IncludeEntityTypes = new List<AutosuggestEntityType>()
            {
                AutosuggestEntityType.Address,
                AutosuggestEntityType.Business,
                AutosuggestEntityType.Place
            };
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// A free form string address or Landmark. Overrides the Address values if both are specified.
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Used to constrain entity suggestions to a single country denoted by a 2-letter country code abbreviation.
        /// </summary>
        public string CountryFilter { get; set; }

        /// <summary>
        /// A list of returned entity types.
        /// </summary>
        public List<AutosuggestEntityType> IncludeEntityTypes { get; set; }

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

        #endregion

        #region Public Methods

        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <returns>A response containing the requested data.</returns>
        public override async Task<Response> Execute()
        {
            return await this.Execute(null).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <param name="remainingTimeCallback">A callback function in which the estimated remaining time in seconds is sent.</param>
        /// <returns>A response containing the requested data.</returns>
        public override async Task<Response> Execute(Action<int> remainingTimeCallback)
        {
            Response r = null;

            using (var responseStream = await ServiceHelper.GetStreamAsync(new Uri(GetRequestUrl())).ConfigureAwait(false))
            {
                using (var sr = new StreamReader(responseStream))
                {
                    var s = sr.ReadToEnd();

                    //Replace "__type" with "type" for Entities to work around inconsistent logic used in Bing Maps REST APIs.
                    s = s.Replace("\"__type\":\"Address\"", "\"type\":\"Address\"")
                        .Replace("\"__type\":\"LocalBusiness\"", "\"type\":\"LocalBusiness\"")
                        .Replace("\"__type\":\"Place\"", "\"type\":\"Place\"");

                    var bytes = Encoding.UTF8.GetBytes(s);
                    using (var stream = new MemoryStream(bytes))
                    {
                        r = ServiceHelper.DeserializeStream<Response>(stream);
                    }                    
                }
            }

            return r;
        }

        public override string GetRequestUrl()
        {
            if (string.IsNullOrWhiteSpace(Query))
            {
                throw new Exception("A Query value must specified.");
            }

            if(UserCircularMapView == null && UserLocation == null && UserMapView == null)
            {
                throw new Exception("A UserLocation, UserCircularMapView, UserMapView must specified.");
            }

            var sb = new StringBuilder(this.Domain);
            sb.Append("Autosuggest");

            sb.AppendFormat("?q={0}", Uri.EscapeDataString(Query));

            if (maxResults != 5)
            {
                sb.AppendFormat("&maxResults={0}", maxResults);
            }

            if (!string.IsNullOrWhiteSpace(CountryFilter))
            {
                sb.AppendFormat("&countryFilter={0}", CountryFilter);
            }

            if (IncludeEntityTypes != null && IncludeEntityTypes.Count > 0)
            {
                sb.Append("&includeEntityTypes=");

                var t = typeof(AutosuggestEntityType);

                foreach (var iet in IncludeEntityTypes)
                {
                    sb.AppendFormat("{0},", Enum.GetName(t, iet));
                }

                //Remove trailing comma.
                sb.Length--;
            }

            sb.Append(GetBaseRequestUrl());

            return sb.ToString();
        }

        #endregion
    }
}
