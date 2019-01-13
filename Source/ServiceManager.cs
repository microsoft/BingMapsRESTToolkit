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
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// A static class that processes requests to the Bing Maps REST services.
    /// </summary>
    public static class ServiceManager
    {
        /// <summary>
        /// Proxy settings to use when making requests. 
        /// </summary>
        public static IWebProxy Proxy
        {
            get
            {
                return ServiceHelper.Proxy;
            }
            set
            {
                ServiceHelper.Proxy = value;
            }
        }

        /// <summary>
        /// The number of queries per second to limit certain requests to. 
        /// This is primarily used when batching multiple requests in a single process such as when 
        /// geoeocidng all waypoints for the distance matrix API, or when manually generating a truck 
        /// based distance matrix using the routing API. 
        /// </summary>
        public static int QpsLimit = 5;

        /// <summary>
        /// Processes a REST requests that returns data.
        /// </summary>
        /// <param name="request">The REST request to process.</param>
        /// <returns>The response from the REST service.</returns>
        public static async Task<Response> GetResponseAsync(BaseRestRequest request)
        {
            return await request.Execute(null).ConfigureAwait(false);
        }

        /// <summary>
        /// Processes a REST requests that returns data.
        /// </summary>
        /// <param name="request">The REST request to process.</param>
        /// <param name="remainingTimeCallback">A callback function in which the estimated remaining time in seconds is sent.</param>
        /// <returns>The response from the REST service.</returns>
        public static async Task<Response> GetResponseAsync(BaseRestRequest request, Action<int> remainingTimeCallback)
        {
            return await request.Execute(remainingTimeCallback).ConfigureAwait(false);
        }

        /// <summary>
        /// Processes a REST requests that returns an image stream.
        /// </summary>
        /// <param name="imageryRequest">The REST request to process.</param>
        /// <returns>A stream containing an image.</returns>
        public static async Task<Stream> GetImageAsync(BaseImageryRestRequest imageryRequest)
        {
            if (imageryRequest is ImageryRequest)
            {
                var r = imageryRequest as ImageryRequest;

                r.GetMetadata = false;

                if (r.Pushpins != null && (r.Pushpins.Count > 18 || r.Style != null))
                {
                    //Make a post request when there are more than 18 pushpins as there is a risk of URL becoming too large for a GET request.
                    return await ServiceHelper.PostStringAsync(new Uri(r.GetPostRequestUrl()), r.GetPushpinsAsString(), null).ConfigureAwait(false);
                }
                else
                {
                    return await ServiceHelper.GetStreamAsync(new Uri(r.GetRequestUrl())).ConfigureAwait(false);
                }
            }
            else
            {
                return await ServiceHelper.GetStreamAsync(new Uri(imageryRequest.GetRequestUrl())).ConfigureAwait(false);
            }
        }
    }
}
