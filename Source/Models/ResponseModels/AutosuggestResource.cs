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
    /// Autosuggest Resource Enity: Used for `Place`, `Address`, and `LocalBusiness`
    /// </summary>
    [DataContract]
    public class AutosuggestEntityResource
    {
        /// <summary>
        /// Address of the Entity
        /// </summary>
        [DataMember(Name ="address", EmitDefaultValue = false)]
        public Address Address { get; set; }

        /// <summary>
        /// Name of Entity
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// The type of the entity
        /// </summary>
        [DataMember(Name = "type", EmitDefaultValue = false)]
        public string Type { get; set; }
    }

    /// <summary>
    /// Resource returned by Autosuggest API
    /// </summary>
    [DataContract(Name ="Autosuggest", Namespace ="http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class Autosuggest : Resource
    {
        /// <summary>
        /// List if Autosuggest Entities
        /// </summary>
        [DataMember(Name ="value")]
        public AutosuggestEntityResource[] Value { get; set; }
    }
}
