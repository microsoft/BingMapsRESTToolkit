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
    /// The status of an asynchronous request.
    /// </summary>
    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    [KnownType(typeof(DistanceMatrixAsyncStatus))]
    [KnownType(typeof(RouteProxyAsyncResult))]
    public class AsyncStatus: Resource
    {
        #region Public Properties

        /// <summary>
        /// Specifies if the request is accepted. 
        /// </summary>
        [DataMember(Name = "isAccepted", EmitDefaultValue = false)]
        public bool IsAccepted { get; set; }

        /// <summary>
        /// Specifies if the request has completed.
        /// </summary>
        [DataMember(Name = "isCompleted", EmitDefaultValue = false)]
        public bool IsCompleted { get; set; }

        /// <summary>
        /// A unique identifier for an asynchronous request. This can be used to retrieve the results of an asynchronous request when it has completed.
        /// </summary>
        [DataMember(Name = "requestId", EmitDefaultValue = false)]
        public string RequestId { get; set; }

        /// <summary>
        /// Details of an error that may have occurred when processing the request.
        /// </summary>
        [DataMember(Name = "errorMessage", EmitDefaultValue = false)]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// An estimated number of seconds to wait before calling back for results when making an asynchronous request.
        /// </summary>
        [DataMember(Name = "callbackInSeconds", EmitDefaultValue = false)]
        public int CallbackInSeconds { get; set; }

        /// <summary>
        /// The callback URL to use to check the status of the request.
        /// </summary>
        [DataMember(Name = "callbackUrl", EmitDefaultValue = false)]
        public string CallbackUrl { get; set; }

        /// <summary>
        /// The URL where the results can be downloaded from. 
        /// </summary>
        [DataMember(Name = "resultUrl", EmitDefaultValue = false)]
        public string ResultUrl { get; set; }

        #endregion
    }
}
