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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BingMapsRESTToolkit
{
    //https://msdn.microsoft.com/en-us/library/ff701724.aspx

    /// <summary>
    /// Requests an image from the REST imagery service.
    /// </summary>
    public class ImageryRequest : BaseImageryRestRequest
    {
        #region Private Properties

        private int mapWidth = 350;
        private int mapHeight = 350;
        private int zoomLevel = 0;

        private const int minMapWidth = 80;
        private const int minMapHeight = 80;
        private const int maxMapWidth = 2000;
        private const int maxMapHeight = 1500;

        #endregion

        #region Public Properties

        /// <summary>
        /// The type of imagery for which you are requesting metadata.
        /// </summary>
        public ImageryType ImagerySet { get; set; }

        /// <summary>
        /// Required when imagerySet is Birdseye or BirdseyeWithLabels. Optional for other imagery sets. The center point to use for the imagery metadata.
        /// </summary>
        public Coordinate CenterPoint { get; set; }

        /// <summary>
        /// Specifies whether to change the display of overlapping pushpins so that they display separately on a map. 
        /// </summary>
        public bool DeclutterPins { get; set; }

        /// <summary>
        /// The image format to use for the static map.
        /// </summary>
        public ImageFormatType? Format { get; set; }

        /// <summary>
        /// Required when a center point or set of route points are not specified. The geographic area to display on the map.
        /// </summary>
        public BoundingBox MapArea { get; set; }

        /// <summary>
        /// Specifies if the traffic flow layer should be displayed on the map or not. Default is false.
        /// </summary>
        public bool ShowTraffic { get; set; }

        /// <summary>
        /// The width of the map. Default is 350px.
        /// </summary>
        public int MapWidth
        {
            get { return mapWidth; }
            set
            {
                if (value < minMapWidth)
                {
                    mapWidth = minMapWidth;
                }
                else if (value > maxMapWidth)
                {
                    mapWidth = maxMapWidth;
                }
                else
                {
                    mapWidth = value;
                }
            }
        }

        /// <summary>
        /// The height of the map. Default is 350px.
        /// </summary>
        public int MapHeight
        {
            get { return mapHeight; }
            set
            {
                if (value < minMapHeight)
                {
                    mapHeight = minMapHeight;
                }
                else if (value > maxMapHeight)
                {
                    mapHeight = maxMapHeight;
                }
                else
                {
                    mapHeight = value;
                }
            }
        }

        /// <summary>
        /// Optional. Specifies whether to return metadata for the static map instead of the image. 
        /// The static map metadata includes the size of the static map and the placement and size 
        /// of the pushpins on the static map.
        /// </summary>
        public bool GetMetadata { get; set; }

        /// <summary>
        /// A query string that is used to determine the map location to display.
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// The level of zoom to display.
        /// </summary>
        public int ZoomLevel
        {
            get { return zoomLevel; }
            set
            {
                if (value < 1)
                {
                    zoomLevel = 1;
                }
                else if (value > 21)
                {
                    zoomLevel = 21;
                }
                else
                {
                    zoomLevel = value;
                }
            }
        }

        /// <summary>
        /// Highlights a polygon for an entity.
        /// </summary>
        public bool HighlightEntity { get; set; }

        /// <summary>
        /// Indicates the type of entity that should be highlighted. The entity of this type that 
        /// contains the centerPoint will be highlighted. Supported EntityTypes: CountryRegion, AdminDivision1, or PopulatedPlace.
        /// </summary>
        public EntityType? EntityType { get; set; }

        /// <summary>
        /// List of pushpins to display on the map. 
        /// </summary>
        public List<ImageryPushpin> Pushpins { get; set; }

        /// <summary>
        /// Specifies two or more locations that define the route and that are in sequential order. 
        /// A route is defined by a set of waypoints and viaWaypoints (intermediate locations that the route must pass through). 
        /// You can have a maximum of 25 waypoints, and a maximum of 10 viaWaypoints between each set of waypoints. 
        /// The start and end points of the route cannot be viaWaypoints.
        /// </summary>
        public List<SimpleWaypoint> Waypoints { get; set; }

        /// <summary>
        /// Options for calculating route.
        /// </summary>
        public RouteOptions RouteOptions { get; set; }

        /// <summary>
        /// Specifies the resolution of the labels on the image to retrieve.
        /// </summary>
        public ImageResolutionType Resolution { get; set; }

        /// <summary>
        /// The custom map style to apply to the image.
        /// </summary>
        public string Style { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <returns>A response containing the requested data.</returns>
        public override async Task<Response> Execute()
        {
            return await this.Execute(null).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <param name="remainingTimeCallback">A callback function in which the estimated remaining time in seconds is sent.</param>
        /// <returns>A response containing the requested data.</returns>
        public override async Task<Response> Execute(Action<int> remainingTimeCallback)
        {
            Stream responseStream = null;

            GetMetadata = true;

            if (Pushpins != null && Pushpins.Count > 18)
            {
                //Make a post request when there are more than 18 pushpins as there is a risk of URL becoming too large for a GET request.
                responseStream = await ServiceHelper.PostStringAsync(new Uri(GetPostRequestUrl()), GetPushpinsAsString(), null).ConfigureAwait(false);
            }
            else
            {
                responseStream = await ServiceHelper.GetStreamAsync(new Uri(GetRequestUrl())).ConfigureAwait(false);
            }

            if (responseStream != null)
            {
                var r = ServiceHelper.DeserializeStream<Response>(responseStream);
                responseStream.Dispose();

                return r;
            }

            return null;
        }

        /// <summary>
        /// Gets the request URL. If both a Query and Address are specified, the Query value will be used. 
        /// Throws an exception if a Query or Address value is not specified.
        /// </summary>
        /// <returns>Geocode request URL for GET request.</returns>
        public override string GetRequestUrl()
        {
            return GetPostRequestUrl() + "&" + GetPushpinsAsString();
        }

        /// <summary>
        /// Gets a URL for requesting imagery data for a POST request.
        /// </summary>
        /// <returns>Imagery request URL for POST request.</returns>
        public string GetPostRequestUrl()
        {
            var isQuery = !string.IsNullOrWhiteSpace(Query);
            var isRoute = (Waypoints != null && Waypoints.Count >= 2);

            var sb = new StringBuilder(this.Domain);
            sb.Append("Imagery/Map/");

            sb.Append(Enum.GetName(typeof(ImageryType), ImagerySet));

            if (CenterPoint != null)
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, "/{0:0.#####},{1:0.#####}/", CenterPoint.Latitude, CenterPoint.Longitude);

                if (zoomLevel != 0)
                {
                    sb.Append(zoomLevel);
                }
                else if (HighlightEntity)
                {
                    sb.Append(Enum.GetName(typeof(EntityType), EntityType));
                }
                else if (ImagerySet == ImageryType.Road || ImagerySet == ImageryType.Aerial || ImagerySet == ImageryType.AerialWithLabels ||
                  ImagerySet == ImageryType.RoadOnDemand || ImagerySet == ImageryType.AerialWithLabelsOnDemand || ImagerySet == ImageryType.CanvasDark ||
                  ImagerySet == ImageryType.CanvasGray || ImagerySet == ImageryType.CanvasLight)
                {
                    throw new Exception("Zoom Level must be specified when a centerPoint is specified and ImagerySet is Road, Aerial, AerialWithLabels, or any variation of these (Canvas/OnDemand).");
                }
            }
            else if (isQuery)
            {
                sb.AppendFormat("/{0}", Uri.EscapeDataString(Query));
            }
            
            if (isRoute)
            {
                sb.AppendFormat("/Routes/{0}?", Enum.GetName(typeof(TravelModeType), (RouteOptions != null) ? RouteOptions.TravelMode : TravelModeType.Driving));
            }
            else
            {
                sb.Append("?");
            }

            sb.AppendFormat("ms={0},{1}", mapWidth, mapHeight);

            if (DeclutterPins)
            {
                sb.Append("&dcl=1");
            }

            if (Format.HasValue)
            {
                sb.AppendFormat("&fmt={0}", Enum.GetName(typeof(ImageFormatType), Format.Value));
            }

            if (MapArea != null && (CenterPoint == null || !isRoute))
            {
                sb.AppendFormat("&ma={0}", MapArea.ToString());
            }

            if (ShowTraffic)
            {
                sb.Append("&ml=TrafficFlow");
            }

            if (GetMetadata)
            {
                sb.Append("&mmd=1");
            }

            if (HighlightEntity)
            {
                sb.Append("&he=1");
            }

            if(Resolution == ImageResolutionType.High)
            {
                sb.Append("&dpi=Large");
            }

            //Routing Parameters

            if (isRoute)
            {                
                if (Waypoints.Count > 25)
                {
                    throw new Exception("More than 25 waypoints in route request.");
                }

                for (int i = 0; i < Waypoints.Count; i++)
                {

                    sb.AppendFormat("&wp.{0}=", i);

                    if (Waypoints[i].Coordinate != null)
                    {
                        sb.AppendFormat(CultureInfo.InvariantCulture, "{0:0.#####},{1:0.#####}", Waypoints[i].Coordinate.Latitude, Waypoints[i].Coordinate.Longitude);
                    }
                    else
                    {
                        sb.AppendFormat("{0}", Uri.EscapeDataString(Waypoints[i].Address));
                    }
                }

                if (RouteOptions != null)
                {
                    sb.Append(RouteOptions.GetUrlParam(0));
                }                
            }
            
            sb.Append(GetBaseRequestUrl());

            if (!string.IsNullOrWhiteSpace(Style))
            {
                var s = CustomMapStyleManager.GetRestStyle(Style);

                if (!string.IsNullOrWhiteSpace(s))
                {
                    sb.AppendFormat("&st={0}", s);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns the Pushpin information as a formatted string. 
        /// </summary>
        /// <returns>A formatted string of pushpin data.</returns>
        public string GetPushpinsAsString()
        {
            //pp=38.889586530732335,-77.05010175704956;23;LMT&pp=38.88772364638439,-77.0472639799118;7;KMT

            if (Pushpins != null && Pushpins.Count > 0)
            {
                var s = String.Join("&", Pushpins);
                return s;
            }

            return string.Empty;
        }

        #endregion
    }
}
