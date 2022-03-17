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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// Location Recog Request: Given a pair of location coordinates (latitude, longitude), the Location Recognition API returns a list of entities ranked by their proximity to that location.
    /// </summary>
    public class LocationRecogRequest : BaseRestRequest
    {

        #region Private Properties
        /// <summary>
        /// Constant Max value for Top param: 20
        /// </summary>
        private const int MaxTop = 20;

        /// <summary>
        /// Constant Max distance for Radius in KM : 2 KM
        /// </summary>
        private const double maxRadKilo = 2.0;

        /// <summary>
        /// Constant Max distance for Radius in Miles : Using built-in distance converter
        /// </summary>
        private readonly double maxRadMile = SpatialTools.ConvertDistance(2.0, DistanceUnitType.Kilometers, DistanceUnitType.Miles);

        /// <summary>
        /// Radius value
        /// </summary>
        private double _Radius;

        /// <summary>
        /// Top value
        /// </summary>
        private int _Top;

        /// <summary>
        /// List of LocationRecog EntityType Enums
        /// </summary>
        private List<LocationRecogEntityTypes> _IncludeEntityTypes;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor with default values
        /// </summary>
        public LocationRecogRequest() : base()
        {
            DistanceUnits = DistanceUnitType.Kilometers; 
            VerbosePlaceNames = false;
            _IncludeEntityTypes = new List<LocationRecogEntityTypes>() { LocationRecogEntityTypes.BusinessAndPOI };
            _Radius = 0.25;
            _Top = 10;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Location Recog at this point (lat,lon)
        /// </summary>
        public Coordinate CenterPoint { get; set; }

        /// <summary>
        /// Optional DateTime parameter.
        /// </summary>
        public DateTime? DateTimeInput { get; set; }

        /// <summary>
        /// Library `DistanceUnitType` Enum, either `Kilometers` or `Miles`
        /// </summary>
        public DistanceUnitType DistanceUnits { get; set; }

        /// <summary>
        /// Consumer-facing IncludeEntity Types: 
        ///     - setter with value as comma-separated string, e.g. "Address, NaturalPoi".
        ///     - getter as comma-separated string, all lower-case and trimmed, e.g. "address,naturalpoi".
        /// </summary>
        public string IncludeEntityTypes
        {
            get
            {
                List<string> ret = new List<string>();
                foreach(var include_type in this._IncludeEntityTypes)
                {
                    switch(include_type)
                    {
                        case LocationRecogEntityTypes.Address:
                            ret.Add("address");
                            break;
                        case LocationRecogEntityTypes.BusinessAndPOI:
                            ret.Add("businessAndPOI");
                            break;
                        case LocationRecogEntityTypes.NaturalPOI:
                            ret.Add("naturalPOI");
                            break;
                    }
                }

                return string.Join(",", ret);
            }

            set
            {
                List<LocationRecogEntityTypes> _types = new List<LocationRecogEntityTypes>();
                foreach(string _type in value.Split(','))
                {
                    switch(_type.Trim().ToLower())
                    {
                        case "address":
                            _types.Add(LocationRecogEntityTypes.Address);
                            break;
                        case "businessandpoi":
                            _types.Add(LocationRecogEntityTypes.BusinessAndPOI);
                            break;
                        case "naturalpoi":
                            _types.Add(LocationRecogEntityTypes.NaturalPOI);
                            break;
                        default:
                            throw new Exception($"Unable to parse Include Entity Type in Location Recognition: '{_type}'");
                    }
                }

          
                _IncludeEntityTypes = _types;
            }
        }

        /// <summary>
        /// The Max number of Entities returned
        /// </summary>
        public int Top
        {
            get
            {
                return _Top;
            }

            set
            {
                _Top = (value >= MaxTop) ? MaxTop : value;
            }
        }

        /// <summary>
        /// Search radius, in Kilometers.
        /// The maximum radius is 2 Kilometers
        /// </summary>
        public double Radius
        {
            get
            {
                return _Radius;
            }
            set
            {
                double rad = (double)value;
                switch (DistanceUnits)
                {
                    case DistanceUnitType.Kilometers:
                        if (0 <= rad && rad <= maxRadKilo)
                            _Radius = rad;
                        else
                            throw new Exception($"The maximum radius is {maxRadKilo} KM but {rad} KM was entered.");
                        break;
                    case DistanceUnitType.Miles:
                        if (0 <= rad && rad <= maxRadMile)
                            _Radius = rad;
                        else
                            throw new Exception($"The maximum radius is {maxRadMile} Miles but {rad} Miles was entered.");
                        break;
                }
            }
        }

        /// <summary>
        /// Whether Returned names should be verbose
        /// </summary>
        public bool VerbosePlaceNames { get; set; } 

        #endregion

        #region Public Methods

        public override string GetRequestUrl()
        {
            string pointStr = string.Format("LocationRecog/{0}?", CenterPoint.ToString());

            string du;

            List<string> param_list = new List<string>
            {
                string.Format("r={0}", Radius.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                string.Format("top={0}", Top.ToString()),
                string.Format("distanceUnit={0}", EnumHelper.DistanceUnitTypeToString(DistanceUnits)),
                string.Format("verboseplacenames={0}", VerbosePlaceNames.ToString().ToLower()),
                string.Format("key={0}", BingMapsKey.ToString()),
                string.Format("includeEntityTypes={0}", IncludeEntityTypes)
            };

            if (DateTimeInput.HasValue)
                param_list.Add(string.Format("dateTime={0}", DateTimeHelper.GetUTCString(DateTimeInput.Value)));
            
            return this.Domain + pointStr + string.Join("&", param_list);
        }

        #endregion
    }
}
