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

using System.Globalization;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// Pushpin defination for Bing Maps REST imagery service: https://msdn.microsoft.com/en-us/library/ff701719.aspx
    /// </summary>
    public class ImageryPushpin
    {
        #region Private Properties

        private int iconStyle = 1;
        private string label = string.Empty;

        #endregion

        #region Constructor
        
        /// <summary>
        /// A pushpin that is rendered on a static map image.
        /// </summary>
        public ImageryPushpin()
        {
        }

        /// <summary>
        /// A pushpin that is rendered on a static map image.
        /// </summary>
        /// <param name="latitude">The latitude coordinate.</param>
        /// <param name="longitude">The longitude coordinate.</param>
        public ImageryPushpin(double latitude, double longitude): 
            this(new Coordinate(latitude, longitude), 1, null)
        {
        }

        /// <summary>
        /// A pushpin that is rendered on a static map image.
        /// </summary>
        /// <param name="coord">The coordinate where to position the pushpin.</param>
        public ImageryPushpin(Coordinate coord) :
           this(coord, 1, null)
        {
        }


        /// <summary>
        /// A pushpin that is rendered on a static map image.
        /// </summary>
        /// <param name="latitude">The latitude coordinate.</param>
        /// <param name="longitude">The longitude coordinate.</param>
        /// <param name="iconStyle">The style of icon to render as a pushpin.</param>
        /// <param name="label">The label to put on top of the pushpin.</param>
        public ImageryPushpin(double latitude, double longitude, int iconStyle, string label): 
            this(new Coordinate(latitude, longitude), iconStyle, label)
        {            
        }

        /// <summary>
        /// A pushpin that is rendered on a static map image.
        /// </summary>
        /// <param name="coord">The coordinate where to position the pushpin.</param>
        /// <param name="iconStyle">The style of icon to render as a pushpin.</param>
        /// <param name="label">The label to put on top of the pushpin.</param>
        public ImageryPushpin(Coordinate coord, int iconStyle, string label)
        {
            this.label = label;
            this.iconStyle = iconStyle;
            this.Location = coord;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Coordinate to display pushpin.
        /// </summary>
        public Coordinate Location { get; set; }

        /// <summary>
        /// The icon style to use for the pushpin.
        /// </summary>
        public int IconStyle
        {
            get { return iconStyle; }
            set
            {
                if (value < 0)
                {
                    iconStyle = 0;
                }
                else
                {
                    iconStyle = value;
                }
            }
        }

        /// <summary>
        /// Label to display on top of pushpin.
        /// </summary>
        public string Label
        {
            get { return label; }
            set
            {
                if (value == null)
                {
                    label = string.Empty;
                }
                else
                {
                    label = value;
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a string representing a pushpin in the format "pp=latitude,longitude;iconStyle;label;".
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Location != null)
            {
                return string.Format(CultureInfo.InvariantCulture, "pp={0:0.#####},{1:0.#####};{2};{3}", Location.Latitude, Location.Longitude, iconStyle, label);
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion
    }
}
