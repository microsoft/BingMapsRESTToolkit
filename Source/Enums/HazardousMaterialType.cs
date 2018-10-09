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
    /// Hazardous materials in which a truck may transport.
    /// </summary>
    public enum HazardousMaterialType
    {
        /// <summary>
        /// Combustable materials.
        /// </summary>
        Combustable,

        /// <summary>
        /// Corrosive materials.
        /// </summary>
        Corrosive,

        /// <summary>
        /// Explosive materials.
        /// </summary>
        Explosive,

        /// <summary>
        /// Flammable materials.
        /// </summary>
        Flammable,

        /// <summary>
        /// Flammable Solid materials.
        /// </summary>
        FlammableSolid,

        /// <summary>
        /// Gases.
        /// </summary>
        Gas,

        /// <summary>
        /// Goods Harmful To Water.
        /// </summary>
        GoodsHarmfulToWater,

        /// <summary>
        /// No hazardous materials.
        /// </summary>
        None,

        /// <summary>
        /// Organic materials.
        /// </summary>
        Organic,

        /// <summary>
        /// Some other hazardous materials.
        /// </summary>
        Other,

        /// <summary>
        /// Poisonous materials.
        /// </summary>
        Poison,

        /// <summary>
        /// Poisonous Inhalation materials.
        /// </summary>
        PoisonousInhalation,

        /// <summary>
        /// Radioactive materials.
        /// </summary>
        Radioactive
    }
}
