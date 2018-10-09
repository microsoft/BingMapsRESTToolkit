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
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BingMapsRESTToolkit
{

    public class CoordWithRadius : Coordinate
    {
        /// <summary>
        /// Radius in Meters
        /// </summary>
        public int Radius { get; set; }

        /// <summary>
        /// Default Constuctor
        /// </summary>
        public CoordWithRadius() : base()
        {
        }

        /// <summary>
        /// Return Comma-separated list of Lat,Lon,Radius
        /// </summary>
        /// <returns></returns>
        public override string ToString() => string.Format("{0},{1},{2}", Latitude.ToString(), Longitude.ToString(), Radius.ToString());
    }


    public class AutosuggestRequest : BaseRestRequest
    {

        #region Private Properties

        private const int max_maxResults = 10;

        #endregion

        #region Constructor

        public AutosuggestRequest()
        {
            AutoLocation = AutosuggestLocationType.userLocation;
            IncludeEntityTypes = new List<AutosuggestEntityType>()
                {
                    AutosuggestEntityType.Address,
                    AutosuggestEntityType.LocalBusiness,
                    AutosuggestEntityType.Place,
                };
            Culture = "en-US";
            UserRegion = "US";
            CountryFilter = null;
            Query = "";
            UserLoc = null;
        }

        #endregion

        #region Public Properties

        public CoordWithRadius UserLoc { get; set; } 

        public string CountryFilter { get; set; }

        public List<AutosuggestEntityType> IncludeEntityTypes {get; set;}

        public AutosuggestLocationType AutoLocation { get; set; }

        public string Query { get; set; }

        public int? MaxResults { get; set; }

        #endregion

        #region Public Methods

        public override Task<Response> Execute()
        {
            return Execute(null);
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
            if (Query == "")
                throw new Exception("Empty Query value in Autosuggest REST Request");
            
            string queryStr = string.Format("q={0}", Uri.EscapeDataString(Query));

            string maxStr = string.Format("maxRes={0}", (max_maxResults < MaxResults) ? max_maxResults : MaxResults);

            string locStr = null;

            switch(AutoLocation)
            {
                case AutosuggestLocationType.userCircularMapView:
                    locStr = string.Format("ucmv={0}", UserCircularMapView.ToString());
                    break;
                case AutosuggestLocationType.userMapView:
                    locStr = string.Format("umv={0}", UserMapView.ToString());
                    break;
                case AutosuggestLocationType.userLocation:
                    if (UserLoc == null)
                        throw new Exception("User Location is Requred");
                    locStr = string.Format("ul={0}", UserLoc.ToString());
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
                maxStr,
                string.Format("key={0}", BingMapsKey)
            };

            if (CountryFilter != null)
            {
                string country_filterStr = string.Format("cf={0}", CountryFilter.ToString());
                param_list.Add(country_filterStr);
            }

            return Domain + "Autosuggest?" + string.Join("&", param_list);
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
                    case AutosuggestEntityType.Address:
                        vals.Add("Address");
                        break;
                    case AutosuggestEntityType.LocalBusiness:
                        vals.Add("Business");
                        break;
                    case AutosuggestEntityType.Place:
                        vals.Add("Place");
                        break;
                }
            }

            return string.Join(",", vals);
        }

        #endregion

    }
}
