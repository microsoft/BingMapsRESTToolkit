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

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// The Geospatial Endpoint Service is a REST service that provides information about Geospatial Platform services 
    /// for the language and geographical region you specify. The service information includes available service endpoints 
    /// and language support for these endpoints. Disputed geographical areas and embargoed countries or regions that 
    /// do not have any service support are also identified. 
    /// https://msdn.microsoft.com/en-us/library/dn948525.aspx
    /// </summary>
    public class GeospatialEndpointRequest : BaseRestRequest
    {
        #region Public Properties

        /// <summary>
        /// When set to true the returned service URL's will use HTTPS.
        /// </summary>
        public bool UseHTTPS { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the request URL. If both a Query and Address are specified, the Query value will be used. Throws an exception if a Query or Address value is not specified.
        /// </summary>
        /// <returns>Geocode request URL for GET request.</returns>
        public override string GetRequestUrl()
        {
            string url = this.Domain + "GeospatialEndpoint";

            if (!string.IsNullOrWhiteSpace(Culture))
            {
                url += string.Format("/{0}", Culture);
            }
            else
            {
                throw new Exception("Invalid Language value specified.");
            }

            if (!string.IsNullOrWhiteSpace(UserRegion))
            {
                url += string.Format("/{0}", UserRegion);
            }
            else
            {
                throw new Exception("Invalid UserRegion value specified.");
            }

            if(UserLocation != null)
            {
                url += string.Format("/{0}", UserLocation);
            }

            if (UseHTTPS)
            {
                url += "?uriScheme=http";
            }
            else
            {
                url += "?";
            }

            return url + GetBaseRequestUrl();
        }

        #endregion
    }
}
