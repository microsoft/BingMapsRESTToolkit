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
using System.Runtime.Serialization;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// A LocationRecog response object returned by the LocationRecog operation.
    /// </summary>
    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class LocationRecog : Resource
    {
        /// <summary>
        /// Internal Variable to store string version of `IsPrivateResidence` value
        /// </summary>
        [DataMember(Name = "isPrivateResidence", EmitDefaultValue = false)]
        public string _IsPrivateResidence { get; set; }

        /// <summary>
        /// IsPrivateResidence: "True" or "False" in JSON
        /// </summary>
        public bool IsPrivateResidence
        {
            get
            {
                return bool.Parse(_IsPrivateResidence);
            }
            set
            {
                _IsPrivateResidence = value.ToString();
            }
        }

        /// <summary>
        /// Array of LocalBusiness Resources
        /// </summary>
        [DataMember(Name = "businessesAtLocation", EmitDefaultValue = false)]
        public BusinessAtLocation[] BusinessAtLocation { get; set; }

        /// <summary>
        /// Array of Business Addressess
        /// </summary>
        [DataMember(Name = "addressOfLocation", EmitDefaultValue = false)]
        public Address[] AddressOfLocation { get; set; }

        /// <summary>
        /// Array Point of Interest Entities
        /// </summary>
        [DataMember(Name = "naturalPOIAtLocation", EmitDefaultValue = false)]
        public NaturalPOIAtLocationEntity[] NaturalPOIAtLocation { get; set;}

    }
}
