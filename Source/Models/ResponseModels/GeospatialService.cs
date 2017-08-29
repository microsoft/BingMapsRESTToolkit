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
using System.Runtime.Serialization;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// Information for a geospatial service that is available in the country or region and language specified in the request.
    /// </summary>
    [DataContract]
    public class GeospatialService
    {
        /// <summary>
        /// The URL service endpoint to use in this region. Note that to use the service, you must typically add parameters specific to 
        /// the service. These parameters are not described in this documentation.
        /// </summary>
        [DataMember(Name = "endpoint", EmitDefaultValue = false)]
        public string Endpoint { get; set; }

        /// <summary>
        /// Set to true if the service supports the language in the request for the region. If the language is supported, then the 
        /// service endpoint will return responses using this language. If it is not supported, then the service will use the fallback language.
        /// </summary>
        [DataMember(Name = "fallbackLanguage", EmitDefaultValue = false)]
        public string FallbackLanguage { get; set; }

        /// <summary>
        /// Specifies the secondary or fallback language in this region or country. If the requested language is not supported 
        /// and a fallback language is not available, United States English (en-us) is used.
        /// </summary>
        [DataMember(Name = "languageSupported", EmitDefaultValue = false)]
        public bool LanguageSupported { get; set; }

        /// <summary>
        /// An abbreviated name for the service.
        /// </summary>
        [DataMember(Name = "serviceName", EmitDefaultValue = false)]
        public string ServiceName { get; set; }
    }
}
