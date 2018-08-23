using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BingMapsRESTToolkit.Requests
{
    class AutosuggestRequest : BaseRestRequest
    {

        #region Private Properties
        private const int max_maxResults = 10;

        #endregion

        #region Constructor

        public AutosuggestRequest()
        {
            this.AutoLocation = Enums.AutosuggestLocationType.userLocation;
            this.IncludeEntityTypes = new List<Enums.AutosuggestEntityType>()
                {
                    BingMapsRESTToolkit.Enums.AutosuggestEntityType.Address,
                    BingMapsRESTToolkit.Enums.AutosuggestEntityType.LocalBusiness,
                    BingMapsRESTToolkit.Enums.AutosuggestEntityType.Place,
                };
            this.Culture = "en-US";
            this.UserRegion = "US";
            this.CountryFilter = null;
            this.query = "";
            
        }

        #endregion

        #region Public Properties

        public Coordinate UserLocation { get; set; }

        public string UserRegion { get; set; }

        public String CountryFilter { get; set; }

        public List<BingMapsRESTToolkit.Enums.AutosuggestEntityType> IncludeEntityTypes {get; set;}

        public BingMapsRESTToolkit.Enums.AutosuggestLocationType AutoLocation { get; set; }

        public string query { get; set; }

        #endregion

        #region Public Methods

        public override Task<Response> Execute()
        {
            return this.Execute(null);
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
            if (this.query == "")
                throw new Exception("Empty Query in Autosuggest REST Request");
            
            string queryStr = string.Format("q={0}", this.query);

            string locStr = null;

            switch(AutoLocation)
            {
                case Enums.AutosuggestLocationType.userCircularMapView:
                    locStr = string.Format("ucmv={0}", this.UserCircularMapView.ToString());
                    break;
                case Enums.AutosuggestLocationType.userMapView:
                    locStr = string.Format("umv={0}", this.UserMapView.ToString());
                    break;
                case Enums.AutosuggestLocationType.userLocation:
                    locStr = string.Format("ul={0}", this.UserLocation.ToString());
                    break;
            }

            string inclEntityStr = string.Format("inclenttype={0}", getIncludeEntityTypeString());

            string cultureStr = string.Format("c={0}", Culture.ToString());

            List<string> param_list = new List<string>
            {
                queryStr,
                locStr,
                inclEntityStr,
                cultureStr,
                string.Format("key={0}", BingMapsKey)
            };

            return this.Domain + "Autosuggest?" + string.Join("&", param_list);
        }

        #endregion

        #region Private Methods

        private string getIncludeEntityTypeString()
        {
            List<string> vals = new List<string>();

            foreach(var entity_type in IncludeEntityTypes.Distinct())
            {
                switch(entity_type)
                {
                    case Enums.AutosuggestEntityType.Address:
                        vals.Add("Address");
                        break;
                    case Enums.AutosuggestEntityType.LocalBusiness:
                        vals.Add("Business");
                        break;
                    case Enums.AutosuggestEntityType.Place:
                        vals.Add("Place");
                        break;
                }
            }

            return string.Join(",", vals);
        }

        #endregion

    }
}
