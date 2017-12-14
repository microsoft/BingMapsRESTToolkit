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

using System.Collections.Generic;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// Truck routing specific vehicle attribute. 
    /// </summary>
    public class VehicleSpec
    {
        /// <summary>
        /// The unit of measurement of width, height, length. 
        /// </summary>
        public DimensionUnitType DimensionUnit { get; set; }

        /// <summary>
        /// The unit of measurement of weight. 
        /// </summary>
        public WeightUnitType WeightUnit { get; set; }

        /// <summary>
        /// The height of the vehicle in the specified dimension units. 
        /// </summary>
        public double VehicleHeight { get; set; }

        /// <summary>
        /// The width of the vehicle in the specified dimension units.
        /// </summary>
        public double VehicleWidth { get; set; }

        /// <summary>
        /// The length of the vehicle in the specified dimension units.
        /// </summary>
        public double VehicleLength { get; set; }

        /// <summary>
        /// The weight of the vehicle in the specified weight units.
        /// </summary>
        public double VehicleWeight { get; set; }

        /// <summary>
        /// The number of axles.
        /// </summary>
        public int VehicleAxles { get; set; }

        /// <summary>
        /// Indicates if the truck is pulling a semi-trailer. Semi-trailer restrictions are mostly used in North America.
        /// </summary>
        public bool VehicleSemi { get; set; }

        /// <summary>
        /// Specifies number of trailers pulled by a vehicle. The provided value must be between 0 and 4. 
        /// </summary>
        public int VehicleTrailers { get; set; }

        /// <summary>
        /// The max gradient the vehicle can drive measured in degrees.
        /// </summary>
        public double VehicleMaxGradient { get; set; }

        /// <summary>
        /// The mini-mum required radius for the vehicle to turn in the specified dimension units.
        /// </summary>
        public double VehicleMinTurnRadius { get; set; }

        /// <summary>
        /// Indicates if the vehicle shall avoid crosswinds.
        /// </summary>
        public bool VehicleAvoidCrossWind { get; set; }

        /// <summary>
        /// Indicates if the route shall avoid the risk of grounding.
        /// </summary>
        public bool VehicleAvoidGroundingRisk { get; set; }

        /// <summary>
        /// A list of one or more hazardous materials for which the vehicle is transporting. 
        /// </summary>
        public List<HazardousMaterialType> VehicleHazardousMaterials { get; set; }

        /// <summary>
        /// A list of one or more hazardous materials for which the vehicle has permits. 
        /// </summary>
        public List<HazardousMaterialPermitType> VehicleHazardousPermits { get; set; }
    }
}
