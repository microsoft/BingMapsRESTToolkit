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

using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// An abstract class in which all REST service requests derive from.
    /// </summary>
    public abstract class BaseRestRequest
    {
        #region Private Properties

        private string _restServiceDomain = "https://dev.virtualearth.net/REST/v1/";

        #endregion

        #region Public Properties

        /// <summary>
        /// The Bing Maps key for making the request.
        /// </summary>
        public string BingMapsKey { get; set; }

        /// <summary>
        /// The culture to use for the request. An IETF language code, that includes the language and region code subtags, such as en-us or zh-hans. 
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// The geographic region that corresponds to the current viewport.
        /// </summary>
        public BoundingBox UserMapView { get; set; }

        /// <summary>
        ///  The circular geographic region that corresponds to the current viewport. 
        /// </summary>
        public CircularView UserCircularMapView {get; set; }

        /// <summary>
        /// The user’s current position.
        /// </summary>
        public Coordinate UserLocation { get; set; }

        /// <summary>
        /// An ISO 3166-1 alpha-2 region code, such as US, IN, and CN.
        /// </summary>
        public string UserRegion { get; set; }

        /// <summary>
        /// An Internet Protocol version 4 (IPv4) address.
        /// </summary>
        public string UserIp { get; set; }

        /// <summary>
        /// The domain of the REST service. Default: https://dev.virtualearth.net/REST/v1/
        /// </summary>
        public string Domain {
            get
            {
                return _restServiceDomain;
            }
            set
            {
                this._restServiceDomain = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Abstract method which generates the Bing Maps REST request URL.
        /// </summary>
        /// <returns>A Bing Maps REST request URL.</returns>
        public abstract string GetRequestUrl();

        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <returns>A response containing the requested data.</returns>
        public virtual async Task<Response> Execute()
        {
            return await this.Execute(null).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <param name="remainingTimeCallback">A callback function in which the estimated remaining time in seconds is sent.</param>
        /// <returns>A response containing the requested data.</returns>
        public virtual async Task<Response> Execute(Action<int> remainingTimeCallback)
        {
            Response r = null;

            using (var responseStream = await ServiceHelper.GetStreamAsync(new Uri(GetRequestUrl())).ConfigureAwait(false))
            {
                r = ServiceHelper.DeserializeStream<Response>(responseStream);
            }

            return r;
        }

        /// <summary>
        /// Gets the base request URL.
        /// </summary>
        /// <returns>The base request URL.</returns>
        internal string GetBaseRequestUrl()
        {
            var url = string.Empty;

            if (!string.IsNullOrWhiteSpace(Culture))
            {
                url += "&c=" + Culture;
            }

            if(UserMapView != null){
                //South Latitude, West Longitude, North Latitude, East Longitude
                url += string.Format("&umv={0}", UserMapView.ToString());
            }

            if (UserCircularMapView != null)
            {
                //latitude, longitude, radius
                url += string.Format("&ucmv={0}", UserCircularMapView.ToString());
            }

            if (UserLocation != null)
            {
                url += string.Format("&ul={0}", UserLocation);
            }

            if (!string.IsNullOrWhiteSpace(UserIp))
            {
                url += "&uip=" + UserIp;
            }

            return url + "&key=" + BingMapsKey + "&clientApi=" + InternalSettings.ClientApi;
        }

        #endregion
    }
}
