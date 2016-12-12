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
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// A static class that processes requests to the Bing Maps REST services.
    /// </summary>
    public static class ServiceManager
    {
        /// <summary>
        /// Processes a REST requests that returns data.
        /// </summary>
        /// <param name="request">The REST request to process.</param>
        /// <returns>The response from the REST service.</returns>
        public static async Task<Response> GetResponseAsync(BaseRestRequest request)
        {
            Stream responseStream = null;
            if (request is ElevationRequest)
            {
                var r = request as ElevationRequest;

                if(r.Points != null && r.Points.Count > 50)
                {
                    //Make a post request when there are more than 50 points as there is a risk of URL becoming too large for a GET request.
                    responseStream = await ServiceHelper.PostStringAsync(new Uri(r.GetPostRequestUrl()), r.GetPointsAsString(), null);
                }
                else
                {
                    responseStream = await ServiceHelper.GetStreamAsync(new Uri(r.GetRequestUrl()));
                }
            }
            else if (request is ImageryRequest)
            {
                var r = request as ImageryRequest;

                r.GetMetadata = true;

                if (r.Pushpins != null && r.Pushpins.Count > 18)
                {
                    //Make a post request when there are more than 18 pushpins as there is a risk of URL becoming too large for a GET request.
                    responseStream = await ServiceHelper.PostStringAsync(new Uri(r.GetPostRequestUrl()), r.GetPushpinsAsString(), null);
                }
                else
                {
                    responseStream = await ServiceHelper.GetStreamAsync(new Uri(r.GetRequestUrl()));
                }
            }
            else
            {
                responseStream = await ServiceHelper.GetStreamAsync(new Uri(request.GetRequestUrl()));
            }

            if (responseStream != null)
            {
                var ser = new DataContractJsonSerializer(typeof(Response));                
                var r = ser.ReadObject(responseStream) as Response;
                responseStream.Dispose();
                return r;                
            }

            return null;
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

                if (r.Pushpins != null && r.Pushpins.Count > 18)
                {
                    //Make a post request when there are more than 18 pushpins as there is a risk of URL becoming too large for a GET request.
                    return await ServiceHelper.PostStringAsync(new Uri(r.GetPostRequestUrl()), r.GetPushpinsAsString(), null);
                }
                else
                {
                    return await ServiceHelper.GetStreamAsync(new Uri(r.GetRequestUrl()));
                }
            }
            else
            {
                return await ServiceHelper.GetStreamAsync(new Uri(imageryRequest.GetRequestUrl()));
            }
        }
    }
}
