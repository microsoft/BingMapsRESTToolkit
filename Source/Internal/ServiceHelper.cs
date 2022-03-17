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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// A helper class for making asynchronous requests to REST services.
    /// </summary>
    internal class ServiceHelper
    {
        #region Private Properties

        /// <summary>
        /// The maximium number of times the retry the status check if it fails. This will allow for possible connection issues.
        /// </summary>
        private const int MaxStatusCheckRetries = 3;

        /// <summary>
        /// Number of seconds to delay a retry of a status check.
        /// </summary>
        private const int StatusCheckRetryDelay = 10;

        #endregion

        /// <summary>
        /// Proxy settings to use when making requests. 
        /// </summary>
        public static IWebProxy Proxy { get; set; }

        #region Public Methods

        /// <summary>
        /// Downloads data as a string from a URL.
        /// </summary>
        /// <param name="url">URL that points to data to download.</param>
        /// <returns>A string with the data.</returns>
        internal static Task<string> GetStringAsync(Uri url)
        {
            var tcs = new TaskCompletionSource<string>();

            var request = HttpWebRequest.Create(url);

            if (ServiceManager.Proxy != null) {
                request.Proxy = ServiceManager.Proxy;
            }

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
        internal static Task<Stream> GetStreamAsync(Uri url)
        {
            var tcs = new TaskCompletionSource<Stream>();

            var request = HttpWebRequest.Create(url);

            if (ServiceManager.Proxy != null)
            {
                request.Proxy = ServiceManager.Proxy;
            }

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
        internal static Task<Stream> PostStringAsync(Uri url, string data, string contentType)
        {
            var tcs = new TaskCompletionSource<Stream>();

            var request = HttpWebRequest.Create(url);

            if (ServiceManager.Proxy != null)
            {
                request.Proxy = ServiceManager.Proxy;
            }

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
        internal static T DeserializeStream<T>(Stream responseStream) where T : class
        {
            if (responseStream != null)
            {
                var ser = new DataContractJsonSerializer(typeof(T));
                var r = ser.ReadObject(responseStream) as T;
                return r;
            }

            return null;
        }
        
        /// <summary>
        /// Makes an Async request and monitors it to completion. 
        /// </summary>
        /// <param name="requestUrl">REST URL request.</param>
        /// <param name="remainingTimeCallback">A callback function in which the estimated remaining time in seconds is sent.</param>
        /// <returns>A completed response for a request.</returns>
        internal static async Task<Response> MakeAsyncGetRequest(string requestUrl, Action<int> remainingTimeCallback)
        {
            Response response = null;

            using (var responseStream = await ServiceHelper.GetStreamAsync(new Uri(requestUrl)).ConfigureAwait(false))
            {
                response = ServiceHelper.DeserializeStream<Response>(responseStream);
                return await ProcessAsyncResponse(response, remainingTimeCallback).ConfigureAwait(false);
            }
        }
        
        /// <summary>
        /// Makes an Async request and monitors it to completion. 
        /// </summary>
        /// <param name="requestUrl">REST URL request.</param>
        /// <param name="requestBody">The post request body.</param>
        /// <param name="remainingTimeCallback">A callback function in which the estimated remaining time in seconds is sent.</param>
        /// <returns>A completed response for a request.</returns>
        internal static async Task<Response> MakeAsyncPostRequest(string requestUrl, string requestBody, Action<int> remainingTimeCallback)
        {
            Response response = null;

            using (var responseStream = await ServiceHelper.PostStringAsync(new Uri(requestUrl), requestBody, "application/json").ConfigureAwait(false))
            {
                response = ServiceHelper.DeserializeStream<Response>(responseStream);
                return await ProcessAsyncResponse(response, remainingTimeCallback).ConfigureAwait(false);
            }
        }

        //TODO: this is temporary until distance matrix Async response updated.
        /// <summary>
        /// Makes an Async request and monitors it till completion. 
        /// </summary>
        /// <typeparam name="T">The type of resource to expect to be returned.</typeparam>
        /// <param name="requestUrl">REST URL request.</param>
        /// <param name="remainingTimeCallback">A callback function in which the estimated remaining time in seconds is sent.</param>
        /// <returns>A completed response for a request.</returns>
        internal static async Task<Response> MakeAsyncGetRequest<T>(string requestUrl, Action<int> remainingTimeCallback) where T : Resource
        {
            Response response = null;

            using (var responseStream = await ServiceHelper.GetStreamAsync(new Uri(requestUrl)).ConfigureAwait(false))
            {
                response = ServiceHelper.DeserializeStream<Response>(responseStream);
                return await ProcessAsyncResponse<T>(response, remainingTimeCallback).ConfigureAwait(false);
            }
        }

        //TODO: this is temporary until distance matrix Async response updated.
        /// <summary>
        /// Makes an Async request and monitors it till completion. 
        /// </summary>
        /// <typeparam name="T">The type of resource to expect to be returned.</typeparam>
        /// <param name="requestUrl">REST URL request.</param>
        /// <param name="requestBody">The post request body.</param>
        /// <param name="remainingTimeCallback">A callback function in which the estimated remaining time in seconds is sent.</param>
        /// <returns>A completed response for a request.</returns>
        internal static async Task<Response> MakeAsyncPostRequest<T>(string requestUrl, string requestBody, Action<int> remainingTimeCallback) where T: Resource
        {
            Response response = null;

            using (var responseStream = await ServiceHelper.PostStringAsync(new Uri(requestUrl), requestBody, "application/json").ConfigureAwait(false))
            {
                response = ServiceHelper.DeserializeStream<Response>(responseStream);
                return await ProcessAsyncResponse<T>(response, remainingTimeCallback).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Generates a unqiue hash for a list of items.
        /// </summary>
        /// <typeparam name="T">The type of a list. </typeparam>
        /// <param name="sequence">The list to create a hash for.</param>
        /// <returns>A unqiue hash for a list of items.</returns>
        internal static int GetSequenceHashCode<T>(IList<T> sequence)
        {
            const int seed = 487;
            const int modifier = 31;

            unchecked
            {
                return sequence.Aggregate(seed, (current, item) =>
                    (current * modifier) + item.GetHashCode());
            }
        }

        /// <summary>
        /// Processes an array of tasks but limits the number of queries made per second. 
        /// </summary>
        /// <param name="tasks">The tasks to process.</param>
        /// <returns>A task that processes the array of tasks</returns>
        internal static Task WhenAllTaskLimiter(List<Task> tasks)
        {
            return Task.Run(async () =>
            {
                var taskGroup = new List<Task>();

                for (var i = 0; i < tasks.Count; i++)
                {
                    taskGroup.Add(tasks[i]);

                    if (taskGroup.Count >= ServiceManager.QpsLimit)
                    {
                        var start = DateTime.Now;

                        await Task.WhenAll(taskGroup).ConfigureAwait(false);

                        var end = DateTime.Now;

                        taskGroup.Clear();

                        var ellapsed = (end - start); 

                        if (i != tasks.Count - 1 && ellapsed.TotalMilliseconds < 1000)
                        {
                            //Ensure that atleast a second has passed since the last batch of requests.
                            await Task.Delay(1000 - (int)ellapsed.TotalMilliseconds).ConfigureAwait(false);
                        }
                    }
                }

                if (taskGroup.Count > 0)
                {
                    await Task.WhenAll(taskGroup).ConfigureAwait(false);
                }
            });
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

        private static async Task<Response> ProcessAsyncResponse(Response response, Action<int> remainingTimeCallback)
        {
            if (response != null)
            {
                if (response.ErrorDetails != null && response.ErrorDetails.Length > 0)
                {
                    throw new Exception(String.Join("", response.ErrorDetails));
                }

                if (Response.HasResource(response))
                {
                    var res = Response.GetFirstResource(response);
                    if (res is AsyncStatus && !string.IsNullOrEmpty((res as AsyncStatus).RequestId))
                    {
                        var status = res as AsyncStatus;

                        status = await ServiceHelper.ProcessAsyncStatus(status, remainingTimeCallback).ConfigureAwait(false);

                        if (status != null && status.IsCompleted && !string.IsNullOrEmpty(status.ResultUrl))
                        {
                            try
                            {
                                using (var resultStream = await ServiceHelper.GetStreamAsync(new Uri(status.ResultUrl)).ConfigureAwait(false))
                                {
                                    return ServiceHelper.DeserializeStream<Response>(resultStream);
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("There was an issue downloading and serializing the results. Results Download URL: " + status.ResultUrl + "\r\n" + ex.Message);
                            }
                        }

                        return response;
                    }
                    else if (res is AsyncStatus && !string.IsNullOrEmpty((res as AsyncStatus).ErrorMessage))
                    {
                        throw new Exception((res as AsyncStatus).ErrorMessage);
                    }
                    else if (res is Resource && !(res is AsyncStatus))
                    {
                        return response;
                    }
                }
            }

            throw new Exception("No response returned by service.");
        }

        //TODO: this is temporary until distance matrix Async response updated.
        private static async Task<Response> ProcessAsyncResponse<T>(Response response, Action<int> remainingTimeCallback) where T: Resource
        {
            if (response != null)
            {
                if (response.ErrorDetails != null && response.ErrorDetails.Length > 0)
                {
                    throw new Exception(String.Join("", response.ErrorDetails));
                }

                if (Response.HasResource(response))
                {
                    var res = Response.GetFirstResource(response);
                    if (res is AsyncStatus && !string.IsNullOrEmpty((res as AsyncStatus).RequestId))
                    {
                        var status = res as AsyncStatus;

                        status = await ServiceHelper.ProcessAsyncStatus(status, remainingTimeCallback).ConfigureAwait(false);

                        if (status != null && status.IsCompleted && !string.IsNullOrEmpty(status.ResultUrl))
                        {
                            try
                            {
                                using (var resultStream = await ServiceHelper.GetStreamAsync(new Uri(status.ResultUrl)).ConfigureAwait(false))
                                {
                                    if (typeof(T) == typeof(DistanceMatrix))
                                    {
                                        //There is a bug in the distance matrix service that when some options are set, the response isn't wrapped with a resourceSet->resources like all other services.
                                        using (var sr = new StreamReader(resultStream))
                                        {
                                            var r = sr.ReadToEnd();

                                            //Remove first character from string.
                                            r = r.Remove(0, 1);

                                            //Add namespace type to JSON object.
                                            r = "{\"__type\":\"DistanceMatrix:http://schemas.microsoft.com/search/local/ws/rest/v1\"," + r;

                                            var bytes = Encoding.UTF8.GetBytes(r);
                                            using (var stream = new MemoryStream(bytes))
                                            {
                                                var resource = ServiceHelper.DeserializeStream<T>(stream);
                                                response.ResourceSets[0].Resources[0] = resource;
                                            }
                                        }
                                    } 
                                    else if (typeof(T) == typeof(SnapToRoadResponse))
                                    {
                                        //Snap to road for some reason includes a full resource set response while other async services don't.
                                        response = ServiceHelper.DeserializeStream<Response>(resultStream);
                                    }
                                    else
                                    {
                                        var resource = ServiceHelper.DeserializeStream<T>(resultStream);
                                        response.ResourceSets[0].Resources[0] = resource;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("There was an issue downloading and serializing the results. Results Download URL: " + status.ResultUrl + "\r\n" + ex.Message);
                            }
                        }

                        return response;
                    }
                    else if (res is AsyncStatus && !string.IsNullOrEmpty((res as AsyncStatus).ErrorMessage))
                    {
                        throw new Exception((res as AsyncStatus).ErrorMessage);
                    }
                    else if (res is Resource && !(res is AsyncStatus))
                    {
                        return response;
                    }
                }
            }

            throw new Exception("No response returned by service.");
        }

        /// <summary>
        /// Processes an Async Status until it is completed or runs into an error.
        /// </summary>
        /// <param name="status">The async status to process.</param>
        /// <param name="remainingTimeCallback">A callback function in which the estimated remaining time in seconds is sent.</param>
        /// <returns></returns>
        private static async Task<AsyncStatus> ProcessAsyncStatus(AsyncStatus status, Action<int> remainingTimeCallback)
        {
            var statusUrl = new Uri(status.CallbackUrl);

            if (status.CallbackInSeconds > 0 || !status.IsCompleted || string.IsNullOrEmpty(status.ResultUrl))
            {
                remainingTimeCallback?.Invoke(status.CallbackInSeconds);

                //Wait remaining seconds.
                await Task.Delay(TimeSpan.FromSeconds(status.CallbackInSeconds)).ConfigureAwait(false);

                status = await ServiceHelper.MonitorAsyncStatus(statusUrl, 0, remainingTimeCallback).ConfigureAwait(false);
            }

            if (status != null)
            {
                if (status.IsCompleted && !string.IsNullOrEmpty(status.ResultUrl))
                {
                    return status;
                }
                else if (!status.IsAccepted)
                {
                    throw new Exception("The request was not accepted. " + (!string.IsNullOrEmpty(status.ErrorMessage) ? status.ErrorMessage : ""));
                }
                else if (!string.IsNullOrEmpty(status.ErrorMessage))
                {
                    throw new Exception(status.ErrorMessage);
                }
            }

            return status;
        }

        /// <summary>
        /// Monitors the status of an async request.
        /// </summary>
        /// <param name="statusUrl">The status URL for the async request.</param>
        /// <param name="failedTries">The number of times the status check has failed consecutively.</param>
        /// <param name="remainingTimeCallback">A callback function in which the estimated remaining time in seconds is sent.</param>
        /// <returns>The final async status when the request completed, had an error, or was not accepted.</returns>
        private static async Task<AsyncStatus> MonitorAsyncStatus(Uri statusUrl, int failedTries, Action<int> remainingTimeCallback)
        {
            AsyncStatus status = null;

            try
            {
                using (var rs = await ServiceHelper.GetStreamAsync(statusUrl).ConfigureAwait(false))
                {
                    var r = ServiceHelper.DeserializeStream<Response>(rs);

                    if (r != null)
                    {
                        if (r.ErrorDetails != null && r.ErrorDetails.Length > 0)
                        {
                            throw new Exception(r.ErrorDetails[0]);
                        }
                        else if (Response.HasResource(r))
                        {
                            var res = Response.GetFirstResource(r);
                            if (res is AsyncStatus)
                            {
                                status = res as AsyncStatus;

                                if (!status.IsCompleted && status.CallbackInSeconds > 0)
                                {
                                    remainingTimeCallback?.Invoke(status.CallbackInSeconds);

                                    //Wait remaining seconds.
                                    await Task.Delay(TimeSpan.FromSeconds(status.CallbackInSeconds)).ConfigureAwait(false);
                                    return await MonitorAsyncStatus(statusUrl, 0, remainingTimeCallback).ConfigureAwait(false);
                                }
                            }
                            else if (res is OptimizeItinerary)
                            {
                                var oi = res as OptimizeItinerary;

                                status = new AsyncStatus()
                                {
                                    CallbackInSeconds = -1,
                                    IsCompleted = oi.IsCompleted,
                                    IsAccepted = oi.IsAccepted,
                                    CallbackUrl = statusUrl.OriginalString,
                                    ResultUrl = oi.IsCompleted ? statusUrl.OriginalString: null,
                                };
                            } else
                            {
                                var t = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Check to see how many times the status check has failed consecutively.
                if (failedTries < MaxStatusCheckRetries)
                {
                    //Wait some time and try again.
                    await Task.Delay(TimeSpan.FromSeconds(StatusCheckRetryDelay)).ConfigureAwait(false);
                    return await MonitorAsyncStatus(statusUrl, failedTries + 1, remainingTimeCallback).ConfigureAwait(false);
                }
                else
                {
                    status = new AsyncStatus()
                    {
                        ErrorMessage = "Failed to get status, and exceeded the maximium of " + MaxStatusCheckRetries + " retries. Error message: " + ex.Message,
                        CallbackInSeconds = -1,
                        IsCompleted = false
                    };
                }
            }

            //Should only get here is the request has completed, was not accepted or there was an error.
            return status;
        }

        #endregion
    }
}
