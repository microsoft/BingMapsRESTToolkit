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

using System.Runtime.Serialization;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// Map Elements which can be styled.
    /// </summary>
    [DataContract]
    public class MapElements
    {
        /// <summary>
        /// Admin1, state, province, etc.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public BorderedMapElementStyle adminDistrict { get; set; }

        /// <summary>
        /// Icon representing the capital of a state/province.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle adminDistrictCapital { get; set; }

        /// <summary>
        /// Area of land encompassing an airport.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle airport { get; set; }

        /// <summary>
        /// Area of land use, not to be confused with Structure
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle area { get; set; }

        /// <summary>
        /// An arterial road is a high-capacity urban road. Its primary function is to deliver traffic from collector roads to freeways or expressways, and between urban centers efficiently.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle arterialRoad { get; set; }

        /// <summary>
        /// A structure such as a house, store, factory.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle building { get; set; }

        /// <summary>
        /// Restaurant, hospital, school, etc.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle business { get; set; }

        /// <summary>
        /// Icon representing the capital populated place.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle capital { get; set; }

        /// <summary>
        /// Area of a cemetery
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle cemetery { get; set; }

        /// <summary>
        /// Area of a whole continent
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle continent { get; set; }

        /// <summary>
        /// A controlled-access highway is a type of road which has been designed for high-speed vehicular traffic, with all traffic flow and ingress/egress regulated. Also known as a highway, freeway, motorway, expressway, interstate, parkway.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle controlledAccessHighway { get; set; }

        /// <summary>
        /// A country or independent sovereign state.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public BorderedMapElementStyle countryRegion { get; set; }

        /// <summary>
        /// Icon representing the capital of a country/region.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle countryRegionCapital { get; set; }

        /// <summary>
        /// Admin2, county, etc.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public BorderedMapElementStyle district { get; set; }

        /// <summary>
        /// An area of land used for educational purposes such as a school campus.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle education { get; set; }

        /// <summary>
        /// A school or other educational building.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle educationBuilding { get; set; }

        /// <summary>
        /// Restaurant, café, etc.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle foodPoint { get; set; }

        /// <summary>
        /// Area of forest land.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle forest { get; set; }

        /// <summary>
        /// An area of land where the game of golf is played.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle golfCourse { get; set; }

        /// <summary>
        /// Lines representing ramps typically alongside ControlledAccessHighways
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle highSpeedRamp { get; set; }

        /// <summary>
        /// A highway.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle highway { get; set; }

        /// <summary>
        /// An area of land reserved for Indigenous people.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle indigenousPeoplesReserve { get; set; }

        /// <summary>
        /// Labeling of area of an island. 
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle island { get; set; }

        /// <summary>
        /// Major roads.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle majorRoad { get; set; }

        /// <summary>
        /// The base map element in which all other map elements inherit from.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle mapElement { get; set; }

        /// <summary>
        /// Area of land used for medical purposes. Generally, hospital campuses.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle medical { get; set; }

        /// <summary>
        /// A building which provides medical services.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle medicalBuilding { get; set; }

        /// <summary>
        /// A military area.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle military { get; set; }

        /// <summary>
        /// A natural point of interest.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle naturalPoint { get; set; }

        /// <summary>
        /// Area of land used for nautical purposes.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle nautical { get; set; }

        /// <summary>
        /// Area defined as a neighborhood. Labels only.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle neighborhood { get; set; }

        /// <summary>
        /// Area of any kind of park.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle park { get; set; }

        /// <summary>
        /// Icon representing the peak of a mountain.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle peak { get; set; }

        /// <summary>
        /// All point features that are rendered with an icon of some sort
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle point { get; set; }

        /// <summary>
        /// Restaurant, hospital, school, marina, ski area, etc.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle pointOfInterest { get; set; }

        /// <summary>
        /// A political border.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public BorderedMapElementStyle political { get; set; }

        /// <summary>
        /// Icon representing size of populated place (city, town, etc).
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle populatedPlace { get; set; }

        /// <summary>
        /// Railway lines
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle railway { get; set; }

        /// <summary>
        /// Line representing the connecting entrance/exit to a highway.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle ramp { get; set; }

        /// <summary>
        /// Area of nature reserve.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle reserve { get; set; }

        /// <summary>
        /// River, stream, or other passage. Note that this may be a line or polygon and may connect to non-river water bodies.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle river { get; set; }

        /// <summary>
        /// Lines that represent all roads
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle road { get; set; }

        /// <summary>
        /// Icon representing the exit, typically from a controlled access highway.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle roadExit { get; set; }

        /// <summary>
        /// Sign representing a compact name for a road. For example, I-5.
        /// </summary>
        //[DataMember(EmitDefaultValue = false)]
        public MapElementStyle roadShield { get; set; }

        /// <summary>
        /// Land area covered by a runway. See also Airport for the land area of the whole airport.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle runway { get; set; }

        /// <summary>
        /// Area generally used for beaches, but could be used for sandy areas/golf bunkers in the future.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle sand { get; set; }

        /// <summary>
        /// A shopping center or mall.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle shoppingCenter { get; set; }

        /// <summary>
        /// Area of a stadium.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle stadium { get; set; }

        /// <summary>
        /// A street.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle street { get; set; }

        /// <summary>
        /// Buildings and other building-like structures
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle structure { get; set; }

        /// <summary>
        /// A toll road.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle tollRoad { get; set; }

        /// <summary>
        /// Walking trail, either through park or hiking trail
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle trail { get; set; }

        /// <summary>
        /// Icon representing a bus stop, train stop, airport, etc.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle transit { get; set; }

        /// <summary>
        /// A transit building.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle transitBuilding { get; set; }

        /// <summary>
        /// Lines that are part of the transportation network (roads, trains, ferries, etc)
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle transportation { get; set; }

        /// <summary>
        /// An unpaved street.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle unpavedStreet { get; set; }

        /// <summary>
        /// Forests, grassy areas, etc.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle vegetation { get; set; }

        /// <summary>
        /// Icon representing the peak of a volcano.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle volcanicPeak { get; set; }

        /// <summary>
        /// Anything that looks like water
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle water { get; set; }

        /// <summary>
        /// Icon representing a water feature location such as a waterfall.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle waterPoint { get; set; }

        /// <summary>
        /// Ferry route lines
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MapElementStyle waterRoute { get; set; }
    }
}
