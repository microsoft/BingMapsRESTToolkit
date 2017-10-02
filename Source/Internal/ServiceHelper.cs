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
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// A helper class for making asynchronous requests to REST services.
    /// </summary>
    internal class ServiceHelper
    {
        #region Public Methods

        /// <summary>
        /// Downloads data as a string from a URL.
        /// </summary>
        /// <param name="url">URL that points to data to download.</param>
        /// <returns>A string with the data.</returns>
        public static Task<string> GetStringAsync(Uri url)
        {
            var tcs = new TaskCompletionSource<string>();

            var request = HttpWebRequest.Create(url);
            request.BeginGetResponse((a) =>
            {
                try
                {
                    var r = (HttpWebRequest)a.AsyncState;
                    using (var response = (HttpWebResponse)r.EndGetResponse(a))
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            tcs.SetResult(reader.ReadToEnd());
                        }
                    }
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                        {
                            tcs.SetResult(reader.ReadToEnd());
                        }
                    }
                    else
                    {
                        tcs.SetException(ex);
                    }
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }, request);

            return tcs.Task;
        }

        /// <summary>
        /// Downloads data as a stream from a URL.
        /// </summary>
        /// <param name="url">URL that points to data to download.</param>
        /// <returns>A stream with the data.</returns>
        public static Task<Stream> GetStreamAsync(Uri url)
        {
            var tcs = new TaskCompletionSource<Stream>();

            var request = HttpWebRequest.Create(url);
            request.BeginGetResponse((a) =>
            {
                try
                {
                    var r = (HttpWebRequest)a.AsyncState;
                    using (var response = (HttpWebResponse)r.EndGetResponse(a))
                    {
                        tcs.SetResult(CopyToMemoryStream(response.GetResponseStream()));                     
                    }
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        tcs.SetResult(CopyToMemoryStream(ex.Response.GetResponseStream()));
                    }
                    else
                    {
                        tcs.SetException(ex);
                    }
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }, request);

            return tcs.Task;
        }

        /// <summary>
        /// Makes a post request using string data. 
        /// </summary>
        /// <param name="url">URL to post data to.</param>
        /// <param name="data">String representation of data to be posted to service.</param>
        /// <param name="contentType">The content type of the data.</param>
        /// <returns>Response stream.</returns>
        public static Task<Stream> PostStringAsync(Uri url, string data, string contentType)
        {
            var tcs = new TaskCompletionSource<Stream>();

            var request = HttpWebRequest.Create(url);

            request.Method = "POST";

            if (!string.IsNullOrWhiteSpace(contentType))
            {
                request.ContentType = contentType;
            }
            else
            {
                request.ContentType = "text/plain;charset=utf-8";
            }

            request.BeginGetRequestStream((a) =>
            {
                try
                {
                    var r = (HttpWebRequest)a.AsyncState;

                    //Add data to request stream
                    using (var requestStream = r.EndGetRequestStream(a))
                    {
                        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
                        requestStream.Write(bytes, 0, bytes.Length);
                    }

                    request.BeginGetResponse((a2) =>
                    {
                        try
                        {
                            var r2 = (HttpWebRequest)a2.AsyncState;

                            using (var response = (HttpWebResponse)r2.EndGetResponse(a2))
                            {
                                tcs.SetResult(CopyToMemoryStream(response.GetResponseStream()));
                            }
                        }
                        catch (WebException ex)
                        {
                            if (ex.Response != null)
                            {
                                tcs.SetResult(CopyToMemoryStream(ex.Response.GetResponseStream()));
                            }
                            else
                            {
                                tcs.SetException(ex);
                            }
                        }
                        catch (Exception ex)
                        {
                            tcs.SetException(ex);
                        }
                    }, request);
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        tcs.SetResult(CopyToMemoryStream(ex.Response.GetResponseStream()));
                    }
                    else
                    {
                        tcs.SetException(ex);
                    }
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }, request);

            return tcs.Task;
        }

        /// <summary>
        /// Deserializes a response stream into a Response object.
        /// </summary>
        /// <param name="responseStream">The response stream to deserialize.</param>
        /// <returns>A Response object.</returns>
        public static T DeserializeStream<T>(Stream responseStream) where T : class
        {
            if (responseStream != null)
            {
                var ser = new DataContractJsonSerializer(typeof(T));
                var r = ser.ReadObject(responseStream) as T;
                return r;
            }

            return null;
        }

        #endregion

        #region Private Methods

        private static MemoryStream CopyToMemoryStream(Stream inputStream)
        {
            var ms = new MemoryStream();
            inputStream.CopyTo(ms);
            ms.Position = 0;
            return ms;
        }

        #endregion
    }
}
