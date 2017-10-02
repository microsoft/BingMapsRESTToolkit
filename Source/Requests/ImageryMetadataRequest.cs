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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingMapsRESTToolkit
{
    //https://msdn.microsoft.com/en-us/library/ff701716.aspx
    
    /// <summary>
    /// Requests imagery metadata information from Bing Maps.
    /// </summary>
    public class ImageryMetadataRequest : BaseRestRequest
    {
        #region Private Properties

        private double orientation = 0;
        private int zoomLevel = 0;

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
        /// Get only the basic metadata for an imagery set at a specific location. This URL does not return a map tile URL. 
        /// </summary>
        public bool GetBasicInfo { get; set; }

        /// <summary>
        /// When you specified the two-letter ISO country code is included for addresses in the response. 
        /// </summary>
        public bool IncludeImageryProviders { get; set; }

        /// <summary>
        /// The orientation of the viewport to use for the imagery metadata. This option only applies to Birdseye imagery.
        /// </summary>
        public double Orientation
        {
            get { return orientation; }
            set
            {
                if (value < 0)
                {
                    orientation = value % 360 + 360;
                }
                else if (value > 360)
                {
                    orientation = value % 360;
                }
                else
                {
                    orientation = value;
                }
            }
        }

        /// <summary>
        /// Required if a centerPoint is specified and imagerySet is set to Road, Aerial or AerialWithLabels The level of zoom to use for the imagery metadata.
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
        /// When set to true tile URL's will use HTTPS.
        /// </summary>
        public bool UseHTTPS { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the request URL. Throws an exception if a zoom level is not specified when a centerPoint is specified when ImagerySet is Road, Aerial and AerialWithLabels.
        /// </summary>
        /// <returns>Imagery Metadata request URL for GET request.</returns>
        public override string GetRequestUrl()
        {
            string url = this.Domain + "Imagery/";

            if (GetBasicInfo)
            {
                url += "BasicMetadata/";
            }
            else
            {
                url += "Metadata/";
            }

            url += Enum.GetName(typeof(ImageryType), ImagerySet);

            if (CenterPoint != null)
            {
                url += string.Format(CultureInfo.InvariantCulture, "/{0:0.#####},{1:0.#####}?", CenterPoint.Latitude, CenterPoint.Longitude);

                if (ImagerySet == ImageryType.Road || ImagerySet == ImageryType.Aerial || ImagerySet == ImageryType.AerialWithLabels ||
                    ImagerySet == ImageryType.RoadOnDemand || ImagerySet == ImageryType.AerialWithLabelsOnDemand || ImagerySet == ImageryType.CanvasDark ||
                    ImagerySet == ImageryType.CanvasGray || ImagerySet == ImageryType.CanvasLight)
                {
                    if (zoomLevel == 0)
                    {
                        throw new Exception("Zoom Level must be specified when a centerPoint is specified and ImagerySet is Road, Aerial, AerialWithLabels, or any variation of these (Canvas/OnDemand).");
                    }
                    else
                    {
                        url += "zl=" + zoomLevel;
                    }
                }
            }
            else
            {
                url += "?";
            }

            if (orientation != 0)
            {
                url += "&dir=" + orientation;
            }

            if (IncludeImageryProviders)
            {
                url += "&incl=ImageryProviders";
            }

            if (UseHTTPS)
            {
                url += "&uriScheme=https";
            }

            return url + GetBaseRequestUrl();
        }

        #endregion
    }
}
