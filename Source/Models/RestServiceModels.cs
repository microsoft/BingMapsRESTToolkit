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

namespace BingMapsRESTToolkit
{
    /*
     * Data Contracts generated for the Bing Maps REST service Response object.
     */

    [DataContract]
    public class Address
    {
        [DataMember(Name = "addressLine", EmitDefaultValue = false)]
        public string AddressLine { get; set; }

        [DataMember(Name = "adminDistrict", EmitDefaultValue = false)]
        public string AdminDistrict { get; set; }

        [DataMember(Name = "adminDistrict2", EmitDefaultValue = false)]
        public string AdminDistrict2 { get; set; }

        [DataMember(Name = "countryRegion", EmitDefaultValue = false)]
        public string CountryRegion { get; set; }

        [DataMember(Name = "locality", EmitDefaultValue = false)]
        public string Locality { get; set; }

        [DataMember(Name = "postalCode", EmitDefaultValue = false)]
        public string PostalCode { get; set; }
    }

    [DataContract]
    public class CoverageArea
    {
        /// <summary>
        /// Bounding box of the coverage area. Structure [South Latitude, West Longitude, North Latitude, East Longitude]
        /// </summary>
        [DataMember(Name = "bbox", EmitDefaultValue = false)]
        public double[] BoundingBox { get; set; }

        [DataMember(Name = "zoomMax", EmitDefaultValue = false)]
        public int ZoomMax { get; set; }

        [DataMember(Name = "zoomMin", EmitDefaultValue = false)]
        public int ZoomMin { get; set; }
    }

    [DataContract]
    public class DetailedAddress : Address
    {
        [DataMember(Name = "countryRegionIso2", EmitDefaultValue = false)]
        public string CountryRegionIso2 { get; set; }

        [DataMember(Name = "formattedAddress", EmitDefaultValue = false)]
        public string FormattedAddress { get; set; }

        [DataMember(Name = "neighborhood", EmitDefaultValue = false)]
        public string Neighborhood { get; set; }

        [DataMember(Name = "landmark", EmitDefaultValue = false)]
        public string Landmark { get; set; }
    }

    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class BirdseyeMetadata : ImageryMetadata
    {
        [DataMember(Name = "orientation", EmitDefaultValue = false)]
        public double Orientation { get; set; }

        [DataMember(Name = "tilesX", EmitDefaultValue = false)]
        public int TilesX { get; set; }

        [DataMember(Name = "tilesY", EmitDefaultValue = false)]
        public int TilesY { get; set; }
    }

    [DataContract]
    public class Detail
    {
        [DataMember(Name = "compassDegrees", EmitDefaultValue = false)]
        public int CompassDegrees { get; set; }

        [DataMember(Name = "maneuverType", EmitDefaultValue = false)]
        public string ManeuverType { get; set; }

        [DataMember(Name = "startPathIndices", EmitDefaultValue = false)]
        public int[] StartPathIndices { get; set; }

        [DataMember(Name = "endPathIndices", EmitDefaultValue = false)]
        public int[] EndPathIndices { get; set; }

        [DataMember(Name = "roadType", EmitDefaultValue = false)]
        public string RoadType { get; set; }

        [DataMember(Name = "locationCodes", EmitDefaultValue = false)]
        public string[] LocationCodes { get; set; }

        [DataMember(Name = "names", EmitDefaultValue = false)]
        public string[] Names { get; set; }

        [DataMember(Name = "mode", EmitDefaultValue = false)]
        public string Mode { get; set; }

        [DataMember(Name = "roadShieldRequestParameters", EmitDefaultValue = false)]
        public RoadShield roadShieldRequestParameters { get; set; }
    }

    [DataContract]
    public class Generalization
    {
        [DataMember(Name = "pathIndices", EmitDefaultValue = false)]
        public int[] PathIndices { get; set; }

        [DataMember(Name = "latLongTolerance", EmitDefaultValue = false)]
        public double LatLongTolerance { get; set; }
    }

    [DataContract]
    public class Hint
    {
        [DataMember(Name = "hintType", EmitDefaultValue = false)]
        public string HintType { get; set; }

        [DataMember(Name = "text", EmitDefaultValue = false)]
        public string Text { get; set; }
    }

    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    [KnownType(typeof(StaticMapMetadata))]
    [KnownType(typeof(BirdseyeMetadata))]
    public class ImageryMetadata : Resource
    {
        [DataMember(Name = "imageHeight", EmitDefaultValue = false)]
        public int ImageHeight { get; set; }

        [DataMember(Name = "imageWidth", EmitDefaultValue = false)]
        public int ImageWidth { get; set; }

        [DataMember(Name = "imageryProviders", EmitDefaultValue = false)]
        public ImageryProvider[] ImageryProviders { get; set; }

        [DataMember(Name = "imageUrl", EmitDefaultValue = false)]
        public string ImageUrl { get; set; }

        [DataMember(Name = "imageUrlSubdomains", EmitDefaultValue = false)]
        public string[] ImageUrlSubdomains { get; set; }

        [DataMember(Name = "vintageEnd", EmitDefaultValue = false)]
        public string VintageEnd { get; set; }

        [DataMember(Name = "vintageStart", EmitDefaultValue = false)]
        public string VintageStart { get; set; }

        [DataMember(Name = "zoomMax", EmitDefaultValue = false)]
        public int ZoomMax { get; set; }

        [DataMember(Name = "zoomMin", EmitDefaultValue = false)]
        public int ZoomMin { get; set; }
    }

    [DataContract]
    public class ImageryProvider
    {
        [DataMember(Name = "attribution", EmitDefaultValue = false)]
        public string Attribution { get; set; }

        [DataMember(Name = "coverageAreas", EmitDefaultValue = false)]
        public CoverageArea[] CoverageAreas { get; set; }
    }

    [DataContract]
    public class Instruction
    {
        [DataMember(Name = "maneuverType", EmitDefaultValue = false)]
        public string ManeuverType { get; set; }

        [DataMember(Name = "text", EmitDefaultValue = false)]
        public string Text { get; set; }

        [DataMember(Name = "formattedText", EmitDefaultValue = false)]
        public string FormattedText { get; set; }
    }

    [DataContract]
    public class ItineraryItem
    {
        [DataMember(Name = "childItineraryItems", EmitDefaultValue = false)]
        public ItineraryItem[] ChildItineraryItems { get; set; }

        [DataMember(Name = "compassDirection", EmitDefaultValue = false)]
        public string CompassDirection { get; set; }

        [DataMember(Name = "details", EmitDefaultValue = false)]
        public Detail[] Details { get; set; }

        [DataMember(Name = "exit", EmitDefaultValue = false)]
        public string Exit { get; set; }

        [DataMember(Name = "hints", EmitDefaultValue = false)]
        public Hint[] Hints { get; set; }

        [DataMember(Name = "iconType", EmitDefaultValue = false)]
        public string IconType { get; set; }

        [DataMember(Name = "instruction", EmitDefaultValue = false)]
        public Instruction Instruction { get; set; }

        [DataMember(Name = "maneuverPoint", EmitDefaultValue = false)]
        public Point ManeuverPoint { get; set; }

        [DataMember(Name = "sideOfStreet", EmitDefaultValue = false)]
        public string SideOfStreet { get; set; }

        [DataMember(Name = "signs", EmitDefaultValue = false)]
        public string[] Signs { get; set; }

        [DataMember(Name = "time", EmitDefaultValue = false)]
        public string Time { get; set; }

        public DateTime TimeUtc
        {
            get
            {
                if (string.IsNullOrEmpty(Time))
                {
                    return DateTime.Now;
                }
                else
                {
                    return DateTimeHelper.FromOdataJson(Time);
                }
            }
            set
            {
                if (value == null)
                {
                    Time = string.Empty;
                }
                else
                {
                    var v = DateTimeHelper.ToOdataJson(value);

                    if (v != null)
                    {
                        Time = v;
                    }
                    else
                    {
                        Time = string.Empty;
                    }
                }
            }
        }

        [DataMember(Name = "tollZone", EmitDefaultValue = false)]
        public string TollZone { get; set; }

        [DataMember(Name = "towardsRoadName", EmitDefaultValue = false)]
        public string TowardsRoadName { get; set; }

        [DataMember(Name = "transitLine", EmitDefaultValue = false)]
        public TransitLine TransitLine { get; set; }

        [DataMember(Name = "transitStopId", EmitDefaultValue = false)]
        public int TransitStopId { get; set; }

        [DataMember(Name = "transitTerminus", EmitDefaultValue = false)]
        public string TransitTerminus { get; set; }

        [DataMember(Name = "travelDistance", EmitDefaultValue = false)]
        public double TravelDistance { get; set; }

        [DataMember(Name = "travelDuration", EmitDefaultValue = false)]
        public double TravelDuration { get; set; }

        [DataMember(Name = "travelMode", EmitDefaultValue = false)]
        public string TravelMode { get; set; }

        [DataMember(Name = "warning", EmitDefaultValue = false)]
        public Warning[] Warning { get; set; }
    }

    [DataContract]
    public class Line
    {
        [DataMember(Name = "type", EmitDefaultValue = false)]
        public string Type { get; set; }

        [DataMember(Name = "coordinates", EmitDefaultValue = false)]
        public double[][] Coordinates { get; set; }
    }

    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class Location : Resource
    {
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        [DataMember(Name = "point", EmitDefaultValue = false)]
        public Point Point { get; set; }

        [DataMember(Name = "entityType", EmitDefaultValue = false)]
        public string EntityType { get; set; }

        [DataMember(Name = "address", EmitDefaultValue = false)]
        public DetailedAddress Address { get; set; }

        [DataMember(Name = "confidence", EmitDefaultValue = false)]
        public string Confidence { get; set; }

        [DataMember(Name = "matchCodes", EmitDefaultValue = false)]
        public string[] MatchCodes { get; set; }

        [DataMember(Name = "geocodePoints", EmitDefaultValue = false)]
        public Point[] GeocodePoints { get; set; }

        [DataMember(Name = "queryParseValues", EmitDefaultValue = false)]
        public QueryParseValue[] QueryParseValues { get; set; }
    }

    [DataContract]
    public class QueryParseValue
    {
        [DataMember(Name = "property", EmitDefaultValue = false)]
        public string Property { get; set; }

        [DataMember(Name = "value", EmitDefaultValue = false)]
        public string Value { get; set; }
    }

    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class PushpinMetdata
    {
        [DataMember(Name = "anchor", EmitDefaultValue = false)]
        public Pixel Anchor { get; set; }

        [DataMember(Name = "bottomRightOffset", EmitDefaultValue = false)]
        public Pixel BottomRightOffset { get; set; }

        [DataMember(Name = "topLeftOffset", EmitDefaultValue = false)]
        public Pixel TopLeftOffset { get; set; }

        [DataMember(Name = "point", EmitDefaultValue = false)]
        public Point Point { get; set; }
    }

    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class Pixel
    {
        [DataMember(Name = "x", EmitDefaultValue = false)]
        public string X { get; set; }

        [DataMember(Name = "y", EmitDefaultValue = false)]
        public string Y { get; set; }
    }

    [DataContract]
    public class Point : Shape
    {
        [DataMember(Name = "type", EmitDefaultValue = false)]
        public string Type { get; set; }

        /// <summary>
        /// Latitude,Longitude
        /// </summary>
        [DataMember(Name = "coordinates", EmitDefaultValue = false)]
        public double[] Coordinates { get; set; }

        [DataMember(Name = "calculationMethod", EmitDefaultValue = false)]
        public string CalculationMethod { get; set; }

        [DataMember(Name = "usageTypes", EmitDefaultValue = false)]
        public string[] UsageTypes { get; set; }
    }

    [DataContract]
    [KnownType(typeof(Location))]
    [KnownType(typeof(Route))]
    [KnownType(typeof(TrafficIncident))]
    [KnownType(typeof(ImageryMetadata))]
    [KnownType(typeof(ElevationData))]
    [KnownType(typeof(SeaLevelData))]
    [KnownType(typeof(CompressedPointList))]
    public class Resource
    {
        /// <summary>
        /// Bounding box of the response. Structure [South Latitude, West Longitude, North Latitude, East Longitude]
        /// </summary>
        [DataMember(Name = "bbox", EmitDefaultValue = false)]
        public double[] BoundingBox { get; set; }

        [DataMember(Name = "__type", EmitDefaultValue = false)]
        public string Type { get; set; }
    }

    [DataContract]
    public class ResourceSet
    {
        [DataMember(Name = "estimatedTotal", EmitDefaultValue = false)]
        public long EstimatedTotal { get; set; }

        [DataMember(Name = "resources", EmitDefaultValue = false)]
        public Resource[] Resources { get; set; }
    }

    [DataContract]
    public class Response
    {
        [DataMember(Name = "copyright", EmitDefaultValue = false)]
        public string Copyright { get; set; }

        [DataMember(Name = "brandLogoUri", EmitDefaultValue = false)]
        public string BrandLogoUri { get; set; }

        [DataMember(Name = "statusCode", EmitDefaultValue = false)]
        public int StatusCode { get; set; }

        [DataMember(Name = "statusDescription", EmitDefaultValue = false)]
        public string StatusDescription { get; set; }

        [DataMember(Name = "authenticationResultCode", EmitDefaultValue = false)]
        public string AuthenticationResultCode { get; set; }

        [DataMember(Name = "errorDetails", EmitDefaultValue = false)]
        public string[] errorDetails { get; set; }

        [DataMember(Name = "traceId", EmitDefaultValue = false)]
        public string TraceId { get; set; }

        [DataMember(Name = "resourceSets", EmitDefaultValue = false)]
        public ResourceSet[] ResourceSets { get; set; }
    }

    [DataContract]
    public class RoadShield
    {
        [DataMember(Name = "bucket", EmitDefaultValue = false)]
        public int Bucket { get; set; }

        [DataMember(Name = "shields", EmitDefaultValue = false)]
        public Shield[] Shields { get; set; }
    }

    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class Route : Resource
    {
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id { get; set; }

        [DataMember(Name = "distanceUnit", EmitDefaultValue = false)]
        public string DistanceUnit { get; set; }

        [DataMember(Name = "durationUnit", EmitDefaultValue = false)]
        public string DurationUnit { get; set; }

        [DataMember(Name = "travelDistance", EmitDefaultValue = false)]
        public double TravelDistance { get; set; }

        [DataMember(Name = "travelDuration", EmitDefaultValue = false)]
        public double TravelDuration { get; set; }

        [DataMember(Name = "travelDurationTraffic", EmitDefaultValue = false)]
        public double TravelDurationTraffic { get; set; }

        [DataMember(Name = "trafficCongestion", EmitDefaultValue = false)]
        public string TrafficCongestion { get; set; }

        [DataMember(Name = "trafficDataUsed", EmitDefaultValue = false)]
        public string TrafficDataUsed { get; set; }

        [DataMember(Name = "routeLegs", EmitDefaultValue = false)]
        public RouteLeg[] RouteLegs { get; set; }

        [DataMember(Name = "routePath", EmitDefaultValue = false)]
        public RoutePath RoutePath { get; set; }
    }

    [DataContract]
    public class RouteLeg
    {
        [DataMember(Name = "travelDistance", EmitDefaultValue = false)]
        public double TravelDistance { get; set; }

        [DataMember(Name = "travelDuration", EmitDefaultValue = false)]
        public double TravelDuration { get; set; }

        [DataMember(Name = "cost", EmitDefaultValue = false)]
        public double Cost { get; set; }

        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }

        [DataMember(Name = "actualStart", EmitDefaultValue = false)]
        public Point ActualStart { get; set; }

        [DataMember(Name = "actualEnd", EmitDefaultValue = false)]
        public Point ActualEnd { get; set; }

        [DataMember(Name = "startLocation", EmitDefaultValue = false)]
        public Location StartLocation { get; set; }

        [DataMember(Name = "endLocation", EmitDefaultValue = false)]
        public Location EndLocation { get; set; }

        [DataMember(Name = "itineraryItems", EmitDefaultValue = false)]
        public ItineraryItem[] ItineraryItems { get; set; }

        [DataMember(Name = "routeRegion", EmitDefaultValue = false)]
        public string RouteRegion { get; set; }

        [DataMember(Name = "routeSubLegs", EmitDefaultValue = false)]
        public RouteSubLeg[] RouteSubLegs { get; set; }
        
        [DataMember(Name = "startTime", EmitDefaultValue = false)]
        public string StartTime { get; set; }

        public DateTime StartTimeUtc
        {
            get
            {
                if (string.IsNullOrEmpty(StartTime))
                {
                    return DateTime.Now;
                }
                else
                {
                    return DateTimeHelper.FromOdataJson(StartTime);
                }
            }
            set
            {
                if (value == null)
                {
                    StartTime = string.Empty;
                }
                else
                {
                    var v = DateTimeHelper.ToOdataJson(value);

                    if (v != null)
                    {
                        StartTime = v;
                    }
                    else
                    {
                        StartTime = string.Empty;
                    }
                }
            }
        }

        [DataMember(Name = "endTime", EmitDefaultValue = false)]
        public string EndTime { get; set; }

        public DateTime EndTimeUtc
        {
            get
            {
                if (string.IsNullOrEmpty(EndTime))
                {
                    return DateTime.Now;
                }
                else
                {
                    return DateTimeHelper.FromOdataJson(EndTime);
                }
            }
            set
            {
                if (value == null)
                {
                    EndTime = string.Empty;
                }
                else
                {
                    var v = DateTimeHelper.ToOdataJson(value);

                    if (v != null)
                    {
                        EndTime = v;
                    }
                    else
                    {
                        EndTime = string.Empty;
                    }
                }
            }
        }

        //TODO: What is the base class?
        [DataMember(Name = "alternateVias", EmitDefaultValue = false)]
        public object[] AlternateVias { get; set; }
    }

    [DataContract]
    public class RouteSubLeg
    {
        [DataMember(Name = "endWaypoint", EmitDefaultValue = false)]
        public Waypoint EndWaypoint { get; set; }

        [DataMember(Name = "startWaypoint", EmitDefaultValue = false)]
        public Waypoint StartWaypoint { get; set; }

        [DataMember(Name = "travelDistance", EmitDefaultValue = false)]
        public double TravelDistance { get; set; }

        [DataMember(Name = "travelDuration", EmitDefaultValue = false)]
        public double TravelDuration { get; set; }

        [DataMember(Name = "travelDurationTraffic", EmitDefaultValue = false)]
        public double TravelDurationTraffic { get; set; }
    }

    [DataContract]
    public class Waypoint : Point
    {
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }

        [DataMember(Name = "isVia", EmitDefaultValue = false)]
        public bool IsVia { get; set; }

        [DataMember(Name = "locationIdentifier", EmitDefaultValue = false)]
        public string LocationIdentifier { get; set; }

        [DataMember(Name = "routePathIndex", EmitDefaultValue = false)]
        public int RoutePathIndex { get; set; }
    }

    [DataContract]
    public class RoutePath
    {
        [DataMember(Name = "line", EmitDefaultValue = false)]
        public Line Line { get; set; }

        [DataMember(Name = "generalizations", EmitDefaultValue = false)]
        public Generalization[] Generalizations { get; set; }
    }

    [DataContract]
    [KnownType(typeof(Point))]
    public class Shape
    {
        [DataMember(Name = "boundingBox", EmitDefaultValue = false)]
        public double[] BoundingBox { get; set; }
    }

    [DataContract]
    public class Shield
    {
        [DataMember(Name = "labels", EmitDefaultValue = false)]
        public string[] Labels { get; set; }

        [DataMember(Name = "roadShieldType", EmitDefaultValue = false)]
        public int RoadShieldType { get; set; }
    }

    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class StaticMapMetadata : ImageryMetadata
    {
        [DataMember(Name = "mapCenter", EmitDefaultValue = false)]
        public Point MapCenter { get; set; }

        [DataMember(Name = "pushpins", EmitDefaultValue = false)]
        public PushpinMetdata[] Pushpins { get; set; }

        [DataMember(Name = "zoom", EmitDefaultValue = false)]
        public string Zoom { get; set; }
    }

    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class TrafficIncident : Resource
    {
        [DataMember(Name = "point", EmitDefaultValue = false)]
        public Point Point { get; set; }

        [DataMember(Name = "congestion", EmitDefaultValue = false)]
        public string Congestion { get; set; }

        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }

        [DataMember(Name = "detour", EmitDefaultValue = false)]
        public string Detour { get; set; }

        [DataMember(Name = "start", EmitDefaultValue = false)]
        public string Start { get; set; }

        public DateTime StartDateTimeUtc
        {
            get
            {
                if (string.IsNullOrEmpty(Start))
                {
                    return DateTime.Now;
                }
                else
                {
                    return DateTimeHelper.FromOdataJson(Start);
                }
            }
            set
            {
                if (value == null)
                {
                    Start = string.Empty;
                }
                else
                {
                    var v = DateTimeHelper.ToOdataJson(value);

                    if (v != null)
                    {
                        Start = v;
                    }
                    else
                    {
                        Start = string.Empty;
                    }
                }
            }
        }

        [DataMember(Name = "end", EmitDefaultValue = false)]
        public string End { get; set; }

        public DateTime EndDateTimeUtc
        {
            get
            {
                if (string.IsNullOrEmpty(End))
                {
                    return DateTime.Now;
                }
                else
                {
                    return DateTimeHelper.FromOdataJson(End);
                }
            }
            set
            {
                if (value == null)
                {
                    End = string.Empty;
                }
                else
                {
                    var v = DateTimeHelper.ToOdataJson(value);

                    if (v != null)
                    {
                        End = v;
                    }
                    else
                    {
                        End = string.Empty;
                    }
                }
            }
        }

        [DataMember(Name = "incidentId", EmitDefaultValue = false)]
        public long IncidentId { get; set; }

        [DataMember(Name = "lane", EmitDefaultValue = false)]
        public string Lane { get; set; }

        [DataMember(Name = "lastModified", EmitDefaultValue = false)]
        public string LastModified { get; set; }

        public DateTime LastModifiedDateTimeUtc
        {
            get
            {
                if (string.IsNullOrEmpty(LastModified))
                {
                    return DateTime.Now;
                }
                else
                {
                    return DateTimeHelper.FromOdataJson(LastModified);
                }
            }
            set
            {
                if (value == null)
                {
                    LastModified = string.Empty;
                }
                else
                {
                    var v = DateTimeHelper.ToOdataJson(value);

                    if (v != null)
                    {
                        LastModified = v;
                    }
                    else
                    {
                        LastModified = string.Empty;
                    }
                }
            }
        }

        [DataMember(Name = "roadClosed", EmitDefaultValue = false)]
        public bool RoadClosed { get; set; }

        [DataMember(Name = "severity", EmitDefaultValue = false)]
        public int Severity { get; set; }

        [DataMember(Name = "toPoint", EmitDefaultValue = false)]
        public Point ToPoint { get; set; }

        [DataMember(Name = "locationCodes", EmitDefaultValue = false)]
        public string[] LocationCodes { get; set; }

        [DataMember(Name = "type", EmitDefaultValue = false)]
        public new int Type { get; set; }

        [DataMember(Name = "verified", EmitDefaultValue = false)]
        public bool Verified { get; set; }
    }

    [DataContract]
    public class TransitLine
    {
        [DataMember(Name = "verboseName", EmitDefaultValue = false)]
        public string VerboseName { get; set; }

        [DataMember(Name = "abbreviatedName", EmitDefaultValue = false)]
        public string AbbreviatedName { get; set; }

        [DataMember(Name = "agencyId", EmitDefaultValue = false)]
        public long AgencyId { get; set; }

        [DataMember(Name = "agencyName", EmitDefaultValue = false)]
        public string AgencyName { get; set; }

        [DataMember(Name = "lineColor", EmitDefaultValue = false)]
        public long LineColor { get; set; }

        [DataMember(Name = "lineTextColor", EmitDefaultValue = false)]
        public long LineTextColor { get; set; }

        [DataMember(Name = "uri", EmitDefaultValue = false)]
        public string Uri { get; set; }

        [DataMember(Name = "phoneNumber", EmitDefaultValue = false)]
        public string PhoneNumber { get; set; }

        [DataMember(Name = "providerInfo", EmitDefaultValue = false)]
        public string ProviderInfo { get; set; }
    }

    [DataContract]
    public class Warning
    {
        [DataMember(Name = "origin", EmitDefaultValue = false)]
        public string Origin { get; set; }

        public Coordinate OriginLocation
        {
            get
            {
                if (string.IsNullOrEmpty(Origin))
                {
                    return null;
                }
                else
                {
                    var latLng = Origin.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    double lat, lon;

                    if(latLng.Length >= 2 && double.TryParse(latLng[0], out lat) && double.TryParse(latLng[1], out lon))
                    {
                        return new Coordinate(lat, lon);
                    }

                    return null;
                }
            }
            set
            {
                if (value == null)
                {
                    Origin = string.Empty;
                }
                else
                {
                    Origin = string.Format("{0},{1}", value.Latitude, value.Longitude);
                }
            }
        }

        [DataMember(Name = "severity", EmitDefaultValue = false)]
        public string Severity { get; set; }

        [DataMember(Name = "text", EmitDefaultValue = false)]
        public string Text { get; set; }

        [DataMember(Name = "to", EmitDefaultValue = false)]
        public string To { get; set; }

        public Coordinate ToCoordinate
        {
            get
            {
                if (string.IsNullOrEmpty(To))
                {
                    return null;
                }
                else
                {
                    var latLng = To.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    double lat, lon;

                    if (latLng.Length >= 2 && double.TryParse(latLng[0], out lat) && double.TryParse(latLng[1], out lon))
                    {
                        return new Coordinate(lat, lon);
                    }

                    return null;
                }
            }
            set
            {
                if (value == null)
                {
                    To = string.Empty;
                }
                else
                {
                    To = string.Format("{0},{1}", value.Latitude, value.Longitude);
                }
            }
        }

        [DataMember(Name = "warningType", EmitDefaultValue = false)]
        public string WarningType { get; set; }
    }

    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class CompressedPointList : Resource
    {
        [DataMember(Name = "value", EmitDefaultValue = false)]
        public string Value { get; set; }
    }

    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class ElevationData : Resource
    {
        [DataMember(Name = "elevations", EmitDefaultValue = false)]
        public int[] Elevations { get; set; }

        [DataMember(Name = "zoomLevel", EmitDefaultValue = false)]
        public int ZoomLevel { get; set; }
    }

    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class SeaLevelData : Resource
    {
        [DataMember(Name = "offsets", EmitDefaultValue = false)]
        public int[] Offsets { get; set; }

        [DataMember(Name = "zoomLevel", EmitDefaultValue = false)]
        public int ZoomLevel { get; set; }
    }
}