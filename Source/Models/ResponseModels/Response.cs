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
    /// A response object from the Bing Maps REST Services.
    /// </summary>
    [DataContract]
    public class Response
    {
        /// <summary>
        /// Copyright information.
        /// </summary>
        [DataMember(Name = "copyright", EmitDefaultValue = false)]
        public string Copyright { get; set; }

        /// <summary>
        /// A URL that references a brand image to support contractual branding requirements.
        /// </summary>
        [DataMember(Name = "brandLogoUri", EmitDefaultValue = false)]
        public string BrandLogoUri { get; set; }

        /// <summary>
        /// The HTTP Status code for the request.
        /// </summary>
        [DataMember(Name = "statusCode", EmitDefaultValue = false)]
        public int StatusCode { get; set; }

        /// <summary>
        /// A description of the HTTP status code.
        /// </summary>
        [DataMember(Name = "statusDescription", EmitDefaultValue = false)]
        public string StatusDescription { get; set; }

        /// <summary>
        /// A status code that offers additional information about authentication success or failure. Will be one of the following values: 
        /// ValidCredentials, InvalidCredentials, CredentialsExpired, NotAuthorized, NoCredentials, None
        /// </summary>
        [DataMember(Name = "authenticationResultCode", EmitDefaultValue = false)]
        public string AuthenticationResultCode { get; set; }

        /// <summary>
        /// A collection of error descriptions.
        /// </summary>
        [DataMember(Name = "errorDetails", EmitDefaultValue = false)]
        public string[] ErrorDetails { get; set; }

        /// <summary>
        /// A unique identifier for the request.
        /// </summary>
        [DataMember(Name = "traceId", EmitDefaultValue = false)]
        public string TraceId { get; set; }

        /// <summary>
        /// A collection of ResourceSet objects. A ResourceSet is a container of Resources returned by the request. 
        /// </summary>
        [DataMember(Name = "resourceSets", EmitDefaultValue = false)]
        public ResourceSet[] ResourceSets { get; set; }

        /// <summary>
        /// Check that a response has one or more resources. This is a helper class to save on having to check all the parts of the response tree.
        /// </summary>
        /// <param name="response">A response object.</param>
        /// <returns>Boolean indicating if the response has one or more resources.</returns>
        public static bool HasResource(Response response)
        {
            return response.ResourceSets != null && 
                response.ResourceSets.Length > 0 && 
                response.ResourceSets[0].Resources != null && 
                response.ResourceSets[0].Resources.Length > 0 &&
                response.ResourceSets[0].Resources[0] != null;
        }

        /// <summary>
        /// Gets the first resource in a response.
        /// </summary>
        /// <param name="response">A response object.</param>
        /// <returns>The first resource in a response, or null.</returns>
        public static Resource GetFirstResource(Response response)
        {
            if (HasResource(response))
            {
                return response.ResourceSets[0].Resources[0];
            }

            return null;
        }
    }
}
