using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BingMapsRESTToolkit
{
    public class LocationRecogRequest : BaseRestRequest
    {

        #region Private Properties

        private readonly int MaxTop = 20;
        private readonly double maxRadKilo = 2.0;
        private readonly double maxRadMile = SpatialTools.ConvertDistance(2.0, DistanceUnitType.Kilometers, DistanceUnitType.Miles);

        #endregion

        #region Constructor

        public LocationRecogRequest() : base()
        {
            DistanceUnits = DistanceUnitType.Kilometers;
            Radius = 0.25;
            Top = 20;
            VerbosePlaceNames = false;
            IncludeEntityTypes = "businessAndPOI";
        }

        #endregion

        #region Public Properties

        public Coordinate CenterPoint { get; set; }

        public DateTime? DateTimeInput { get; set; }

        public DistanceUnitType DistanceUnits { get; set; }

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

        public int Top
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
        public double Radius
        {
            get
            {
                return Radius;
            }
            set
            {
                if (ValidateRadius(value))
                    Radius = value;
            }
        }

        public bool VerbosePlaceNames { get; set; } 

        #endregion

        #region Private Methods

        private bool ValidateRadius(double rad)
        {
            switch(_DistanceUnitType)
            {
                case DistanceUnitType.Kilometers:
                    if (0 <= rad && rad <= maxRadKilo)
                        return true;
                    else
                        throw new Exception($"The maximum radius is {maxRadKilo} KM but {rad} KM was entered.");
                case DistanceUnitType.Miles:
                    if (0 <= rad && rad <= maxRadMile)
                        return true;
                    else
                        throw new Exception($"The maximum radius is {maxRadMile} Miles but {rad} Miles was entered.");
            }

            return false;
        }

        #endregion


        #region Public Methods

        public override string GetRequestUrl()
        {
            string pointStr = string.Format("LocationRecog/{0}?", CenterPoint.ToString());

            string du;

            switch (DistanceUnits)
            {
                case DistanceUnitType.Miles:
                    du = "mile";
                    break;
                case DistanceUnitType.Kilometers:
                default:
                    du = "kilometer";
                    break;
            }

            List<string> param_list = new List<string>
            {
                string.Format("r={0}", Radius.ToString()),
                string.Format("top={0}", Top.ToString()),
                string.Format("distanceUnit={0}", du),
                string.Format("verboseplacenames={0}", VerbosePlaceNames.ToString().ToLower()),
                string.Format("key={0}", BingMapsKey.ToString()),
                string.Format("includeEntityTypes={0}", IncludeEntityTypes)
            };

            if (this.DateTimeInput != null)
                param_list.Add(string.Format("dateTime={0}", this.DateTimeInput.ToString()));
            
            return this.Domain + pointStr + string.Join("&", param_list);
        }

        #endregion
    }
}
