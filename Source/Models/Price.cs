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

using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// Price of an Agent.
    /// </summary>
    [DataContract]
    public class Price
    {
        /// <summary>
        /// Fixed price cost.
        /// </summary>
        [DataMember(Name = "fixedPrice")] 
        public double? FixedPrice { get; set; }

        /// <summary>
        /// Cost per Kilometer.
        /// </summary>
        [DataMember(Name = "pricePerKM")]
        public double? PricePerKM { get; set; }

        /// <summary>
        /// Cost per hour.
        /// </summary>
        [DataMember(Name = "pricePerHour")]
        public double? PricePerHour { get; set; }


        public override string ToString()
        {
            if(FixedPrice.HasValue || PricePerKM.HasValue || PricePerHour.HasValue)
            {
                var sb = new StringBuilder("{");

                if (FixedPrice.HasValue)
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture, "\"fixedPrice\":{0},", FixedPrice);
                }

                if (PricePerKM.HasValue)
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture, "\"pricePerKM\":{0},", PricePerKM);
                }

                if (PricePerHour.HasValue)
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture, "\"pricePerHour\":{0},", PricePerHour);
                }

                //Remove trailing comma.
                sb.Length--;

                sb.Append("}");

                return sb.ToString();
            }

            return null;
        }
    }
}
