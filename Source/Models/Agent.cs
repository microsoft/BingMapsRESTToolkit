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
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// An agent (delivery person/vehicle).
    /// </summary>
   [DataContract]
    public class Agent
    {
        /// <summary>
        /// Maximum number of shifts a single agent can have (when using an enterprise account).
        /// </summary>
        internal static int maxShifts = 14;

        /// <summary>
        /// An agent (delivery person/vehicle).
        /// </summary>
        public Agent()
        {
            //Default the name as a Guid so that we start off with a unique name, even if not specified by the user.
            Name = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// A unique identifier for the agent. Default is a Guid.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Optional. Shift parameter allows you to defined a break window during the shift without specifying a location for it, the break 
        /// will be inserted in the agents itinerary alongside scheduled items. For example a lunch break of 30 min at noon between 12:00 and 14:00.
        /// </summary>
        [DataMember(Name = "shifts")]
        public List<Shift> Shifts { get; set; }

        /// <summary>
        /// Optional. Price parameter allows you to define a combination of fixed and variable prices for each agent to be used as 
        /// the optimization objective when the costvalue parameter is set to price. For example the fixedPrice can be used to denote the 
        /// one time cost of using a vehicle and pricePerKm or pricePerHour can be used to model the cost of how expensive that vehicle is to use.
        /// </summary>
        [DataMember(Name = "price")]
        public Price Price { get; set; }

        /// <summary>
        /// Optional. Capacity parameter is defined by numeric values that represent the vehicle capacity in terms of volume, weight, 
        /// pallet or case count, or passenger count.
        /// </summary>
        [DataMember(Name = "capacity")]
        public int[] Capacity { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder("{");

            sb.AppendFormat("\"name\":\"{0}\",", Name);

            if (Shifts != null && Shifts.Count > 0)
            {
                if(Shifts.Count > Agent.maxShifts)
                {
                    throw new Exception(string.Format("Agent has more than the max of {0} shifts specified.", Agent.maxShifts));
                }

                sb.AppendFormat("\"shifts\":[{0}],", string.Join(",", Shifts));
            }

            if (Price != null)
            {
                sb.AppendFormat("\"price\":{0},", Price.ToString());
            }

            if (Capacity != null && Capacity.Length > 0)
            {
                sb.Append("\"capacity\":[");

                //Loop through an append Capacity values with invariant culture.
                foreach (var item in Capacity)
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0}", item);
                    sb.Append(',');
                }
                sb.Length -= 1;
                sb.Append("],");
            }

            //Remove trailing comma.
            sb.Length--;

            sb.Append("}");

            return sb.ToString();
        }
    }
}
