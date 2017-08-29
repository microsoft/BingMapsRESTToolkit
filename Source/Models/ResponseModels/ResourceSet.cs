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
    /// A ResourceSet is a container of Resources returned by the request.
    /// </summary>
    [DataContract]
    public class ResourceSet
    {
        /// <summary>
        /// An estimate of the total number of resources in the ResourceSet.
        /// </summary>
        [DataMember(Name = "estimatedTotal", EmitDefaultValue = false)]
        public long EstimatedTotal { get; set; }

        /// <summary>
        /// A collection of one or more resources. The resources that are returned depend on the request.
        /// </summary>
        [DataMember(Name = "resources", EmitDefaultValue = false)]
        public Resource[] Resources { get; set; }
    }
}
