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
    /// Represents the response from an OptimizeItineraryRequest.
    /// </summary>
    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class OptimizeItinerary : Resource
    {
        /// <summary>
        /// Name of entity.
        /// </summary>
        [DataMember(Name = "agentItineraries", EmitDefaultValue = false)]
        public AgentItinerary[] AgentItineraries { get; set; }

        /// <summary>
        /// Name of entity.
        /// </summary>
        [DataMember(Name = "isAccepted", EmitDefaultValue = false)]
        public bool IsAccepted { get; set; }

        /// <summary>
        /// Name of entity.
        /// </summary>
        [DataMember(Name = "isCompleted", EmitDefaultValue = false)]
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Name of entity.
        /// </summary>
        [DataMember(Name = "unscheduledItems", EmitDefaultValue = false)]
        public OptimizeItineraryItem[] UnscheduledItems { get; set; }

        /// <summary>
        /// List of Agent items that were not used to construct the current solution.
        /// </summary>
        [DataMember(Name = "unusedAgents", EmitDefaultValue = false)]
        public Agent[] UnusedAgents { get; set; }
    }
}
