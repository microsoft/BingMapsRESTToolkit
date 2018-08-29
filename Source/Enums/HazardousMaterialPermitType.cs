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
namespace BingMapsRESTToolkit
{
    /// <summary>
    /// Types of hazardous material permits for truck routing.
    /// </summary>
    public enum HazardousMaterialPermitType
    {
        /// <summary>
        /// Permit for goods which are all appropriate for load.
        /// </summary>
        AllAppropriateForLoad,

        /// <summary>
        /// Permit for combustible material.
        /// </summary>
        Combustible,

        /// <summary>
        /// Permit for corrosive material.
        /// </summary>
        Corrosive,

        /// <summary>
        /// Permit for explosive material.
        /// </summary>
        Explosive,

        /// <summary>
        /// Permit for flammable material.
        /// </summary>
        Flammable,

        /// <summary>
        /// Permit for flammable solid material.
        /// </summary>
        FlammableSolid,

        /// <summary>
        /// Permit for gases.
        /// </summary>
        Gas,

        /// <summary>
        /// No hazardous material permits.
        /// </summary>
        None, 

        /// <summary>
        /// Permit for organic material.
        /// </summary>
        Organic,

        /// <summary>
        /// Permit for poisonous material.
        /// </summary>
        Poison,

        /// <summary>
        /// Permit for poisonous inhalation material.
        /// </summary>
        PoisonousInhalation,

        /// <summary>
        /// Permit for radioactive material.
        /// </summary>
        Radioactive
    }
}
