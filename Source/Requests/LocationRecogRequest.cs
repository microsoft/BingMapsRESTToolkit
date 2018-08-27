using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BingMapsRESTToolkit
{
    class LocationRecogRequest : BaseRestRequest
    {

        #region Private Properties

        private DistanceUnitType _DistanceUnitType { get; set; }
        private readonly uint MaxTop = 20;

        private readonly double maxRadKilo = 2.0;
        private readonly double maxRadMile = SpatialTools.ConvertDistance(2.0, DistanceUnitType.Kilometers, DistanceUnitType.Miles);

        private List<LocationRecogEntityTypes> _IncludeEntityTypes { get; set; }

        #endregion

        #region Constructor

        LocationRecogRequest()
        {
            _DistanceUnitType = DistanceUnitType.Kilometers;
            Radius = .25;
            Top = 20;
        }

        #endregion

        #region Public Properties

        public Coordinate CenterPoint { get; set; }

        public DateTime? DateTimeInput { get; set; }

        public string DistanceUnitTypeInput
        {
            get
            {
                return EnumHelper.DistanceUnitTypeToString(this._DistanceUnitType);
            }

            set
            {
                this._DistanceUnitType = EnumHelper.DistanceUnitStringToEnum(value);
            }
        }

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

                this._IncludeEntityTypes = _types;
            }
        }

        public uint Top
        {
            get
            {
                return this.Top;
            }

            set
            {
                this.Top = (value > MaxTop) ? MaxTop : value;
            }
        }

        /// <summary>
        /// Search radius, in Kilometers.
        /// The maximum radius is 2 Kilometers
        /// </summary>
        public double Radius { get; set; }

        public bool VerbosePlaceNames { get; set; } 

        #endregion

        #region Private Methods

        private bool ValidateRadius()
        {
            switch(_DistanceUnitType)
            {
                case DistanceUnitType.Kilometers:
                    if (Radius <= maxRadKilo)
                        return true;
                    else
                        throw new Exception($"The maximum radius is {maxRadKilo} KM but {Radius} KM was entered.");
                case DistanceUnitType.Miles:
                    if (Radius <= maxRadMile)
                        return true;
                    else
                        throw new Exception($"The maximum radius is {maxRadMile} Miles but {Radius} Miles was entered.");
            }

            return false;
        }

        #endregion


        #region Public Methods

        public override async Task<Response> Execute()
        {
            return await this.Execute(null);
        }

        public override async Task<Response> Execute(Action<int> remainingTimeCallback)
        {
            Stream responseStream = null;

            responseStream = await ServiceHelper.GetStreamAsync(new Uri(GetRequestUrl()));


            if (responseStream != null)
            {
                var r = ServiceHelper.DeserializeStream<Response>(responseStream);
                responseStream.Dispose();
                return r;
            }

            return null;
        }

        public override string GetRequestUrl()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
