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

using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// Custom Map style object
    /// </summary>
    public class CustomMapStyle
    {
        private string _restStyle;

        /// <summary>
        /// A Bing Maps custom map style.
        /// </summary>
        /// <param name="style">A JSON or REST URL formatted custom map string</param>
        public CustomMapStyle(string style)
        {
            if (!string.IsNullOrWhiteSpace(style) && style.Contains("{"))
            {
                _restStyle = ConvertJsonStyle(style);
            }
            else
            {
                _restStyle = style;
            }
        }

        /// <summary>
        /// Gets the 
        /// </summary>
        /// <returns></returns>
        public string GetRestStyle()
        {
            return _restStyle;
        }

        #region Private Static Methods

        private static Dictionary<string, string> _elementMapping = new Dictionary<string, string>()
        {
            { "adminDistrict", "ad" },
            { "adminDistrictCapital", "adc" },
            { "airport", "ap" },
            { "area", "ar" },
            { "arterialRoad", "ard" },
            { "building", "bld" },
            { "business", "bs" },
            { "capital", "cp" },
            { "cemetery", "cm" },
            { "continent", "ct" },
            { "controlledAccessHighway", "cah" },
            { "countryRegion", "cr" },
            { "countryRegionCapital", "crc" },
            { "district", "ds" },
            { "education", "ed" },
            { "educationBuilding", "eb" },
            { "foodPoint", "fp" },
            { "forest", "fr" },
            { "golfCourse", "gc" },
            { "highSpeedRamp", "hsrp" },
            { "highway", "hg" },
            { "indigenousPeoplesReserve", "ipr" },
            { "military", "ima" },
            { "island", "is" },
            { "majorRoad", "mr" },
            { "mapElement", "me" },
            { "medical", "md" },
            { "medicalBuilding", "mb" },
            { "naturalPoint", "np" },
            { "nautical", "nt" },
            { "neighborhood", "nh" },
            { "park", "pr" },
            { "peak", "pk" },
            { "playingField", "pf" },
            { "point", "pt" },
            { "pointOfInterest", "poi" },
            { "political", "pl" },
            { "populatedPlace", "pp" },
            { "railway", "rl" },
            { "ramp", "rm" },
            { "reserve", "rsv" },
            { "river", "rv" },
            { "road", "rd" },
            { "roadExit", "re" },
            { "roadShield", "rs" },
            { "runway", "rw" },
            { "sand", "sn" },
            { "settings", "g" },
            { "shoppingCenter", "sct" },
            { "stadium", "sta" },
            { "street", "st" },
            { "structure", "str" },
            { "tollRoad", "tr" },
            { "trail", "trl" },
            { "transit", "trn" },
            { "transitBuilding", "tb" },
            { "transportation", "trs" },
            { "unpavedStreet", "us" },
            { "vegetation", "vg" },
            { "volcanicPeak", "vp" },
            { "water", "wt" },
            { "waterPoint", "wp" },
            { "waterRoute", "wr" }
        };

        private static Dictionary<string, string> _propertyMapping = new Dictionary<string, string>()
        {
            { "borderOutlineColor", "boc" },
            { "borderStrokeColor", "bsc" },
            { "borderVisible", "bv" },
            { "fillColor", "fc" },
            { "labelColor", "lbc" },
            { "labelOutlineColor", "loc" },
            { "labelVisible", "lv" },
            { "landColor", "lc" },
            { "shadedReliefVisible", null },
            { "strokeColor", "sc" },
            { "visible", "v" }
        };

        private static string ConvertJsonStyle(string jsonStyle)
        {
            var styleString = new StringBuilder();

            if (!string.IsNullOrEmpty(jsonStyle))
            {                
                var s = JObject.Parse(jsonStyle);

                var settings = s["settings"].ToObject<Dictionary<string, object>>();
                if (settings != null)
                {
                    parseProperties("g", settings, styleString);
                }

                var elements = s["elements"].ToObject<Dictionary<string, Dictionary<string, object>>>();
                if (elements != null)
                {
                    foreach(var key in elements.Keys)
                    {
                        if (elements[key] != null)
                        {
                            var shortName = _elementMapping[key];

                            //If element is not a valid element, don't process
                            if (!string.IsNullOrEmpty(shortName))
                            {
                                continue;
                            }

                            parseProperties(shortName, elements[key], styleString);
                        }
                    }
                }

                while (styleString[styleString.Length - 1] == '_')
                {
                    styleString.Length--;
                }
            }

            return styleString.ToString();
        }

        private static void parseProperties(string shortName, Dictionary<string, object> element, StringBuilder writer) {
            object propertyValue;
            string shortProperty;
            bool foundProperty = false;

            foreach (var key in element.Keys)
            {
                propertyValue = element[key];

                // find the abbreviation for the property
                shortProperty = _propertyMapping[key];

                // if property is not a valid property, don't process
                if (string.IsNullOrEmpty(shortProperty)) {
                    continue;
                }

                string stringValue;

                // if it's a boolean value, we want to just map it to a 1 or 0 (1 for true, 0 for false)
                if (propertyValue is bool)
                {
                    stringValue = ((bool)propertyValue) ? "1" : "0";
                }
                else
                {
                    // otherwise, it's a color, lets get the hex value of it
                    stringValue = getValidHexColor((string)propertyValue, false);
                }

                if (!string.IsNullOrEmpty(stringValue)) {
                    if (!foundProperty) {
                        if (writer.Length > 0) {
                            writer.Append('_');
                        }
                        
                        writer.AppendFormat("{0}|", shortName);
                        foundProperty = true;
                    } else {
                        writer.Append(';');
                    }

                    writer.AppendFormat("{0}:{1}", shortProperty, stringValue);
                }
            }
        }

        private static Regex _hexRx = new Regex("^#?([A-Fa-f0-9]{8}|[A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$");

        private static string getValidHexColor(string hexColor, bool includeHash) {
            if (_hexRx.IsMatch(hexColor)) {
                var color = hexColor.Replace("#", "");

                if (color.Length == 3) {
                    //Convert to 6 digit hex color.
                    color = color.Substring(0, 1) + color.Substring(0, 1) + color.Substring(1, 1) + color.Substring(1, 1) + color.Substring(2, 1) + color.Substring(2, 1);
                }

                if (includeHash) {
                    return '#' + color;
                }

                return color;
            }

            return null;
        }

        #endregion
    }
}
