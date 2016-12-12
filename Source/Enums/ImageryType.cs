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

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// Types of map imagery.
    /// </summary>
    public enum ImageryType
    {
        /// <summary>
        /// Aerial imagery.
        /// </summary>
        Aerial,
        
        /// <summary>
        /// Aerial imagery with a road overlay.
        /// </summary>
        AerialWithLabels,

        /// <summary>
        /// Bird’s eye (oblique-angle) imagery
        /// </summary>
        Birdseye,

        /// <summary>
        /// Bird’s eye imagery with a road overlay.
        /// </summary>
        BirdseyeWithLabels,

        /// <summary>
        /// Roads without additional imagery.
        /// </summary>
        Road,

        /// <summary>
        /// Ordnance Survey imagery.
        /// </summary>
        OrdnanceSurvey,

        /// <summary>
        /// Collins Bart imagery
        /// </summary>
        CollinsBart
    }
}
