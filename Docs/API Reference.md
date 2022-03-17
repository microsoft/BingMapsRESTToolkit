# Table of contents 

* [Request Classes](#RequestClasses)
    - [Base Classes](#base-classes)
        - [BaseRestRequest Class](#BaseRestRequest) 
        - [BaseImageryRestRequest Class](#BaseImageryRestRequest)
    - [Time Zone API](#TimeZoneAPI)
        - [FindTimeZoneRequest Class](#FindTimeZoneRequest) 
        - [ConvertTimeZoneRequest](#ConvertTimeZoneRequest)
        - [ListTimeZoneRequest](#ListTimeZonesRequest)
    - [Locations API](#locations-api)
        - [GeocodeRequest Class](#GeocodeRequest)
        - [ReverseGeocodeRequest Class](#ReverseGeocodeRequest)
        - [LocationRecogRequest Class](#LocationRecogRequest)
        - [LocalSearchRequest Class](#LocalSearchRequest)
        - [LocalInsightsRequest Class](#LocalInsightsRequest)
        - [AutosuggestRequest Class](#AutosuggestRequest)
    - [Routes API](#routes-api)
        - [DistanceMatrixRequest Class](#DistanceMatrixRequest)
        - [IsochroneRequest Class](#IsochroneRequest)
        - [RouteMajorRoadsRequest Class](#RouteMajorRoadsRequest) 
        - [RouteRequest Class](#RouteRequest) 
        - [SnapToRoadRequest Class](#SnapToRoadRequest)
        - [TrafficRequest Class](#TrafficRequest) 
        - [OptimizeItineraryRequest class](#OptimizeItineraryRequest)
    - [Elevation API](#elevation-api)
        - [ElevationRequest Class](#ElevationRequest)  
    - [Imagery API](#imagery-api) 
        - [ImageryMetadataRequest Class](#ImageryMetadataRequest) 
        - [ImageryRequest Class](#ImageryRequest) 

* [Common Classes](#CommonClasses) 
    - [BoundingBox Class](#BoundingBox) 
    - [Coordinate Class](#Coordinate) 
    - [CustomMapStyleManager Class](#CustomMapStyleManager)
    - [ImageryPushpin Class](#ImageryPushpin) 
    - [PointCompression Class](#PointCompression) 
    - [RouteOptions Class](#RouteOptions) 
    - [ServiceManager Class](#ServiceManager) 
    - [SimpleAddress Class](#SimpleAddress)
    - [SimpleWaypoint Class](#SimpleWaypoint) 
    - [VehicleSpec Class](#VehicleSpec)  
* [Enumerations](#Enumerations) 
    - [AvoidType Enumeration](#AvoidType) 
    - [ConfidenceLevelType Enumeration](#ConfidenceLevelType) 
    - [DimensionUnitType Enumeration](#DimensionUnitType) 
    - [DistanceUnitType Enumeration](#DistanceUnitType) 
    - [ElevationType Enumeration](#ElevationType) 
    - [EntityType Enumeration](#EntityType) 
    - [HazardousMaterialPermitType Enumeration](#HazardousMaterialPermitType) 
    - [HazardousMaterialType Enumeration](#HazardousMaterialType) 
    - [ImageFormatType Enumeration](#ImageFormatType) 
    - [ImageryType Enumeration](#ImageryType) 
    - [RouteAttributeType Enumeration](#RouteAttributeType) 
    - [RouteOptimizationType Enumeration](#RouteOptimizationType) 
    - [RouteTimeType Enumeration](#RouteTimeType) 
    - [SeverityType Enumeration](#SeverityType) 
    - [SpeedUnitType Enumeration](#SpeedUnitType)
    - [TimeUnitType Enumeration](#TimeUnitType)
    - [TrafficType Enumeration](#TrafficType) 
    - [TravelModeType Enumeration](#TravelModeType) 
    - [WeightUnitType Enumeration](#WeightUnitType) 
* [Enhanced Response Classes](#EnhancedResponseClasses)
    - [DistanceMatrix Class](#DistanceMatrix)
* [Extension Classes](#ExtensionClasses)
    - [TravellingSalesmen Classes](#TravellingSalesmen)
    - [TspOptimizationType Enumeration](#TspOptimizationType)
    - [TspSolution Class](#TspSolution)
* [Response class extensions](#response-class-extensions)


This documentation does not include the class definitions for the REST Response. These are documented in the Bing Maps MSDN documentation [here](https://msdn.microsoft.com/en-us/library/ff701707.aspx).

# <a name="RequestClasses"></a> Request Classes

## Base Classes

### <a name="BaseRestRequest"></a> `BaseRestRequest` Class

An abstract class in which all REST service requests derive from.

#### Methods

| Name            | Return Type | Description |
|-----------------|-------------|------------|
| `Execute()`       | `Task<Response>` | Executes the request.                                        |
| `Execute(Action<int> remainingTimeCallback)` | `Task<Response>` | Executes the request.             |
| `GetRequestUrl()` | `string`  | Abstract method which generates the Bing Maps REST request URL. |

#### Properties

| Name         | Type        | Description |
|--------------|-------------|------|
| `BingMapsKey`  | `string`| The Bing Maps key for making the request.  |
| `Culture`      | `string`      | The culture to use for the request.                             |
| `Domain`       | `string`      | The domain of the REST service. Default: https://dev.virtualearth.net/REST/v1/ |
| `UserIp`       | `string`      | An Internet Protocol version 4 (IPv4) address.                  |
| `UserLocation` | [`Coordinate`](#Coordinate)  | The user's current position.                                    |
| `UserMapView`  | [`BoundingBox`](#BoundingBox) | The geographic region that corresponds to the current viewport. |

### <a name="BaseImageryRestRequest"></a>` BaseImageryRestRequest` Class

Abstract class that all Imagery rest requests will derive from. Inherits from the BaseRestRequest class and currently exposes all the same properties and methods.


## <a name="TimeZoneAPI"></a> Time Zone API Requests

Three request classes for getting Time Zones at a location, converting Time Zones, and getting Time Zone information. Inherits from the `BaseRestRequest` class.

### <a name="FindTimeZoneRequest"></a> `FindTimeZoneRequest` Class

Find the Time Zone at a specific location based on a point or query. Inherits from the BaseRestRequest class.

*Either* instantiate this class by specifiying a `Point` (`Coordinate` class instance) or a Query (`string`), but *not* both.

#### Constructor

There are three constructors:


> `public FindTimeZoneRequest();`

Find Time Zone by Point:

> `public FindTimeZoneRequest(Coordinate point);`

Find Time Zone by Query:

> `public FindTimeZoneRequest(string query, DateTime datetime);`

#### Methods 

| Name            | Return Type | Description                                              |
|-----------------|-------------|----------------------------------------------------------|
|`Execute()`       | `Task<Response>` | Executes the request.                                        |
| `Execute(Action<int> remainingTimeCallback)` | `Task<Response>` | Executes the request.             |
| `GetRequestUrl()` | `string`      | Gets the request URL to perform a reverse geocode query. |


#### Properties

|Name | Properties | Description |
|-----|------------|-------------|
|`Point` | `Coordinate` | Point for location on Earth for which to get time zone information. |
|`Query` | `string` | Query to find location on Earth for which to get time zone information. |
| `LocationDateTime` | `DateTime` | The `DateTime` for the specified location.|
|`IncludeDstRules` | `bool` | Whether to include the `DstRule` for converted time zone in the response. |

### <a name="ConvertTimeZoneRequest"></a> `ConvertTimeZoneRequest` Class

Convert one timezone at a specific UTC datetime to another time zone.

#### Constructor

This request requires two parameters when calling its constructor: a `DateTime` for the local UTC datetime and a `string` with a Windows or IANA timezone ID.

> `public ConvertTimeZoneRequest(DateTime datetime, string DestID);`

#### Methods 

| Name            | Return Type | Description                                              |
|-----------------|-------------|----------------------------------------------------------|
|`Execute()`       | `Task<Response>` | Executes the request.                                        |
| `Execute(Action<int> remainingTimeCallback)` | `Task<Response>` | Executes the request.             |
| `GetRequestUrl()` | `string`      | Gets the request URL to perform a reverse geocode query. |

#### Properties

|Name | Properties | Description |
|-----|------------|-------------|
|`IncludeDstRules` | `bool` | Whether to include the `DstRule` for converted time zone in the response. |
| `LocationDateTime` | `DateTime` | The `DateTime` for the specified location. |
| `DestinationTZID` | `string` |  The [Windows](https://support.microsoft.com/en-us/help/973627/microsoft-time-zone-index-values) or [IANA](https://en.wikipedia.org/wiki/List_of_tz_database_time_zones) time zone ID. For more detail see the [Bing Maps Convert TimeZone API](https://msdnstage.redmond.corp.microsoft.com/en-us/library/mt829733.aspx). |

### <a name="ListTimeZonesRequest"></a> `ListTimeZonesRequest` Class

The [Find a Time Zone API](https://msdn.microsoft.com/en-US/library/mt829734.aspx) has two basic operations: get time zone information about a particular time zone, or retrieve a list of [Windows](https://support.microsoft.com/en-us/help/973627/microsoft-time-zone-index-values) or [IANA](https://en.wikipedia.org/wiki/List_of_tz_database_time_zones) time zone standards.

#### Constructor

There is one constructor.

> `public ListTimeZonesRequest(bool use_list_operation);`

If `use_list_operaiton` is true, a List operation request is created, which will return a list of IANA or Windows standards.

Set the `TimeZoneStandard` property, which takes `string` values, of an `TimeTimeZoneRequest` instance to either "IANA" or "WINDOWS".

Example for List Operation:

```CSharp
var request = new ListTimeZonesRequest(true);
request.TimeZoneStandard = "Windows";
```

To find information about a particular time zone, set `use_list_operation` to false when calling the constructor:

```CSharp
var request = new ListTimeZonesRequest(false);
request.TimeZoneStandard = "Iana";
request.DestinationTZID = "America/Vancouver";
```

#### Methods

| Name            | Return Type | Description                                              |
|-----------------|-------------|----------------------------------------------------------|
|`Execute()`       | `Task<Response>` | Executes the request.                                        |
| `Execute(Action<int> remainingTimeCallback)` | `Task<Response>` | Executes the request.             |
| `GetRequestUrl()` | `string`      | Gets the request URL to perform a reverse geocode query. |

#### Properties

|Name | Properties | Description |
|-----|------------|-------------|
|`IncludeDstRules` | `bool` | Whether to include the `DstRule` for converted time zone in the response. |
| `LocationDateTime` | `DateTime` | The `DateTime` for the specified location. |
|`TimeZoneStandard` | `string` |  Any `string` of either a valid IANA or Windows time zone standard ID.|

## Locations API

### <a name="GeocodeRequest"></a> `GeocodeRequest` Class

Geocodes a query to its coordinates. Inherits from the BaseRestRequest class.

#### Methods

| Name            | Return Type | Description       |
|-----------------|-------------|-------------------|
|`Execute()`       | `Task<Response>` | Executes the request.                                        |
| `Execute(Action<int> remainingTimeCallback)` | `Task<Response>` | Executes the request.             |
| `GetRequestUrl()` | `string`      | Gets the request URL. If both a Query and Address are specified, the Query value will be used. Throws an exception if a Query or Address value is not specified. |

#### Properties

| Name                | Type          | Description         |
|---------------------|---------------|---------------------|
|` Address`             | [`SimpleAddress`](#SimpleAddress) | The Address to geocode.    |
| `IncludeIso2`         | `bool`           | When you specified the two-letter ISO country code is included for addresses in the response. |
| `IncludeNeighborhood` | `bool`           | Specifies to include the neighborhood in the response when it is available.                   |
| `MaxResults`          | `int`           | Specifies the maximum number of locations to return in the response.                          |
| `Query`               | `string`        | A free form string address or Landmark. Overrides the Address values if both are specified.   |


### <a name="ReverseGeocodeRequest"></a> `ReverseGeocodeRequest` Class

Requests a that converts a coordinate into a location such as an address. Inherits from the BaseRestRequest class.

#### Methods

| Name            | Return Type | Description                                              |
|-----------------|-------------|----------------------------------------------------------|
|`Execute()`       | `Task<Response>` | Executes the request.                                        |
| `Execute(Action<int> remainingTimeCallback)` | `Task<Response>` | Executes the request.             |
| `GetRequestUrl()` | `string`      | Gets the request URL to perform a reverse geocode query. |

#### Properties

| Name                | Type                   | Description    |
|---------------------|------------------------|----------------|
| `IncludeEntityTypes`  | `List<EntityType>` | Specifies the entity types that you want to return in the response. Only the types you specify will be returned. If the point cannot be mapped to the entity types you specify, no location information is returned in the response. |
| `IncludeIso2`         | `bool`                    | When you specified the two-letter ISO country code is included for addresses in the response.                                  |
| `IncludeNeighborhood` | `bool`                    | Specifies to include the neighborhood in the response when it is available.                                                    |
| `Point`               | [`Coordinate`](#Coordinate)             | A central coordinate to perform the nearby search.                                                           |


### <a name="LocationRecogRequest"></a> `LocationRecogRequest` Class

The [Location Recognition API](https://msdn.microsoft.com/en-us/library/mt847173.aspx) returns a list of entities ranked by their proximity to a specified location.

#### Constructor

> `public LocationRecogRequest()`

#### Methods

| Name            | Return Type | Description                                              |
|-----------------|-------------|----------------------------------------------------------|
|`Execute()`       | `Task<Response>` | Executes the request.                                        |
| `Execute(Action<int> remainingTimeCallback)` | `Task<Response>` | Executes the request.             |
| `GetRequestUrl()` | `string`      | Gets the request URL to perform a reverse geocode query. |

#### Properties


| Name                | Type                   | Description    |
|---------------------|------------------------|----------------|
| `CenterPoint` | [`Coordinate`](#Coordinate) | A central coordinate to perform the nearby search. |
| `DateTimeInput` | `DateTime?` | The date time to search for local entities. |
| `DistanceUnits` | [`DistanceUnitType`](#DistanceUnitType) | Enum used to specify distance units: either `DistanceUnitType.Kilometers` or `DistanceUnitType.Miles`.|
|`IncludeEntityTypes` | `string` | Comma-separated string for kinds of entities to search for:<br /><br />- `"address"`: Address. <br />- `"businessAndPOI"`: Business and Point of Interest Entity.<br />- `"naturalPOI"`: Point of Interest entities.<br /><br />*Example:*  `IncludeEntityTypes="businessAndPOI,address"`|
|`Top`| `int` | Max number of entities to return, integers from `0` to `20`.|
|`Radius`| `double`| Maximum search radius, from `0` to `2` kilometers. |
|`VerbosePlaceNames`| `bool`| Whether to include verbose entity names.| 

### <a name="LocalSearchRequest"></a> `LocalSearchRequest` Class

The [Local Search API](https://docs.microsoft.com/en-us/bingmaps/rest-services/locations/local-search) returns a list of business entities centered around a location or a geographic region.

#### Methods

| Name            | Return Type | Description                                              |
|-----------------|-------------|----------------------------------------------------------|
| `Execute()`       | `Task<Response>` | Executes the request.                                        |
| `Execute(Action<int> remainingTimeCallback)` | `Task<Response>` | Executes the request.             |
| `GetRequestUrl()` | `string`      | Gets the request URL to perform a reverse geocode query. |

#### Properties


| Name                | Type                   | Description    |
|---------------------|------------------------|----------------|
| `MaxResults`        | `int`           | Specifies the maximum number of locations to return in the response.                          |
| `Query`             | `string`        | A free form string address or Landmark. Overrides the Address values if both are specified.   |
| `Types`             | `List<string>`  | The specified types used to filter the local entities returned by the Local Search API. A comma-separated list of string type identifiers. See the [list of available Type IDs](https://docs.microsoft.com/en-us/bingmaps/rest-services/common-parameters-and-types/type-identifiers/) |

### <a name="LocalInsightsRequest"></a> `LocalInsightsRequest` Class

The [Local Insights API](https://docs.microsoft.com/en-us/bingmaps/rest-services/routes/local-insights) returns a list of local entities within the specified maximum driving time or distance traveled from a specified point on Earth.

#### Methods

| Name            | Return Type | Description                                              |
|-----------------|-------------|----------------------------------------------------------|
| `Execute()`       | `Task<Response>` | Executes the request.                                        |
| `Execute(Action<int> remainingTimeCallback)` | `Task<Response>` | Executes the request.             |
| `GetRequestUrl()` | `string`      | Gets the request URL to perform a reverse geocode query. |

#### Properties

| Name                | Type                   | Description    |
|---------------------|------------------------|----------------|
| `DateTime` | `DateTime` | The dateTime parameter identifies the desired time to be used when calculating an isochrone route. This is supported for driving. When calculating, driving routes the route optimization type should be TimeWithTraffic. The route time will be used as the departure time. |
| `DistanceUnits` | [`DistanceUnitType`](#DistanceUnitType) | The units in which the maxTime value is specified. | 
| `MaxDistance` | `double`  | The maximum travel distance in the specified distance units in which the isochrone polygon is generated. Cannot be set when maxTime is set. |
| `MaxTime` | `double`  | The maximum travel time in the specified time units in which the isochrone polygon is generated. Cannot be set when maxDistance is set. Maximum value is 120 minutes. |
| `Optimize` | [`RouteOptimizationType`](#RouteOptimizationType)          | Specifies what parameters to use to optimize the isochrone route. One of the following values:<br/><br/>- distance: The route is calculated to minimize the distance. Traffic information is not used. Use with maxDistance.<br/>- time [default]: The route is calculated to minimize the time. Traffic information is not used. Use with maxTime.<br/>- timeWithTraffic: The route is calculated to minimize the time and uses current or predictive traffic information depending on if a dateTime value is specified. Use with maxTime. |
| `TimeUnit` | [TimeUnitType](#TimeUnitType) | The units in which the maxTime value is specified. Default: **Seconds** |
| `TravelMode` | [TravelModeType](#TravelModeType) | The mode of travel for the route. Default: Driving.  |
| `Types`             | `List<string>`  | The specified types used to filter the local entities returned by the Local Search API. A comma-separated list of string type identifiers. See the [list of available Type IDs](https://docs.microsoft.com/en-us/bingmaps/rest-services/common-parameters-and-types/type-identifiers/) |
| `Waypoint` | [SimplyWaypoint](#SimplyWaypoint) | The point around which the isochrone will be calculated. |


## <a name="AutosuggestRequest"></a> AutosuggestRequest Class

The [Autosuggest API](https://docs.microsoft.com/en-us/bingmaps/rest-services/autosuggest) returns a list of suggested entities which the user is most likely searching for.

#### Methods

| Name            | Return Type | Description                                              |
|-----------------|-------------|----------------------------------------------------------|
| `Execute()`       | `Task<Response>` | Executes the request.                                        |
| `Execute(Action<int> remainingTimeCallback)` | `Task<Response>` | Executes the request.             |
| `GetRequestUrl()` | `string`      | Gets the request URL to perform a reverse geocode query. |

#### Properties

| Name                | Type                   | Description    |
|---------------------|------------------------|----------------|
| `CountryFilter`          | `string`           | A list of returned entity types. |
| `IncludeEntityTypes`          | `List<AutosuggestEntityType>`           | Specifies the maximum number of locations to return in the response. |
| `MaxResults`          | `int`           | Specifies the maximum number of locations to return in the response.                          |
| `Query`               | `string`        | A free form string address or Landmark. Overrides the Address values if both are specified.   |

## Routes API

### <a name="DistanceMatrixRequest"></a> Distance Matrix Request

 A request that calculates a distance matrix between origins and destinations. Inherits from the BaseRestRequest class.

#### Methods

| Name                      | Return Type            | Description    |
|---------------------------|------------------------|----------------|
|`Execute()`       | `Task<Response>` | Executes the request.                                        |
|` Execute(Action<int> remainingTimeCallback)` | `Task<Response>` | Executes the request.             |
| `GeocodeWaypoints()` | `Task` | Geocodes the origins and destinations.  |
| `GetEuclideanDistanceMatrix()` | `Task<DistanceMatrix>` | Calculates a Distance Matrix for the origins and destinations based on the euclidean distance (straight line/as the crow flies). This calculation only uses; Origins, Destinations, and Distance Units properties from the request and only calculates travel distance. |
| `GetNumberOfCoordinatePairs()`       | `int`                 | Returns the number of coordinate pairs that would be in the resulting matrix based on the number of origins and destinations in the request. |
| `GetPostRequestBody()`       | `string`                 | Returns a JSON string object representing the request. |
| `GetRequestUrl()`           | `string`                 | Gets the request URL to perform a query for a distance matrix when using POST. |

#### Properties

| Name           | Type                   | Description   |
|------------|-------------|---|
| `Origins`    | `List<SimpleWaypoint>` |**Required**. List of origins.  |
| `Destinations`| `List<SimpleWaypoint>` | **Required**. List of destinations. |
| `TravelMode`  | [`TravelModeType`](#TravelModeType) | **Required**. Specifies the mode of transportation to use when calculating the distance matrix. |
| `StartTime`  | `DateTime` | **Optional for Driving**. Specifies the start or departure time of the matrix to calculate and uses predictive traffic data. |
| `EndTime` | `DateTime` | **Optional for Driving**. If specified, a matrix based on traffic data with contain a histogram of travel times and distances for the specified resolution intervals (default is 15 minutes) between the start and end times. A start time must be specified for the request to be valid and the total time between start and end cannot be greater than 24 hours.  |
| `Resolution`     | `int` | **Optional for Driving**. The number of intervals to calculate a histogram of data for each cell where a single interval is a quarter of an hour. Can be one of the following values:<br/><br/> - **1** - 15 minutes<br/> - **2** - 30 minutes<br/> - **3** - 45 minutes<br/> - **4** - an hour<br/><br/>If start time is specified and `resolution` is not, it will default to an interval of 1 (15 minutes).<br/><br/>**Example**: resolution=2 |
| `DistanceUnit`   | [`DistanceUnitType`](#DistanceUnitType) | **Optional.** The units to use for distances in the response. |
| `TimeUnit`  | [`TimeUnitType`](#TimeUnitType) | **Optional.** The units to use for time durations in the response. |
| `VehicleSpec` | [`VehicleSpec`](#VehicleSpec) | Truck routing specific vehicle attribute.  |

### <a name="IsochroneRequest"></a> `IsochroneRequest` Class

Requests a that requests an isochrone (drive time polygon). Inherits from the BaseRestRequest class.

#### Methods

| Name            | Return Type | Description                                              |
|-----------------|-------------|----------------------------------------------------------|
|`Execute()`       | `Task<Response>` | Executes the request.                                        |
| `Execute(Action<int> remainingTimeCallback)` | `Task<Response>` | Executes the request.             |
| `GetRequestUrl()` | `string`      | Gets the request URL for an asynchronous isochrone request. |

#### Properties

| Name                | Type                   | Description    |
|---------------------|------------------------|----------------|
| `DateTime` | `DateTime` | The dateTime parameter identifies the desired time to be used when calculating an isochrone route. This is supported for driving. When calculating, driving routes the route optimization type should be TimeWithTraffic. The route time will be used as the departure time. |
| `DistanceUnits` | [`DistanceUnitType`](#DistanceUnitType) | The units in which the maxTime value is specified. | 
| `MaxDistance` | `double`  | The maximum travel distance in the specified distance units in which the isochrone polygon is generated. Cannot be set when maxTime is set. |
| `MaxTime` | `double`  | The maximum travel time in the specified time units in which the isochrone polygon is generated. Cannot be set when maxDistance is set. Maximum value is 120 minutes. |
| `Optimize` | [`RouteOptimizationType`](#RouteOptimizationType)          | Specifies what parameters to use to optimize the isochrone route. One of the following values:<br/><br/>- distance: The route is calculated to minimize the distance. Traffic information is not used. Use with maxDistance.<br/>- time [default]: The route is calculated to minimize the time. Traffic information is not used. Use with maxTime.<br/>- timeWithTraffic: The route is calculated to minimize the time and uses current or predictive traffic information depending on if a dateTime value is specified. Use with maxTime. |
| `TimeUnit` | [TimeUnitType](#TimeUnitType) | The units in which the maxTime value is specified. Default: **Seconds** |
| `TravelMode` | [TravelModeType](#TravelModeType) | The mode of travel for the route. Default: Driving.  |
| `Waypoint` | [SimplyWaypoint](#SimplyWaypoint) | The point around which the isochrone will be calculated. |

### <a name="RouteMajorRoadsRequest"></a> RouteMajorRoadsRequest Class

Requests routes from a location to major nearby roads. Inherits from the BaseRestRequest class.

#### Methods

| Name            | Return Type | Description|
|-----------------|-------------|-------------|
|`Execute()`       | `Task<Response>` | Executes the request.|
| `Execute(Action<int> remainingTimeCallback)` | `Task<Response>` | Executes the request. |
| `GetRequestUrl()` | `string`      | Gets the request URL to perform a query for routes using major roads. |

#### Properties

| Name                | Type                           | Description   |
|---------------------|--------------------------------|---------------|
| `Destination` | [`SimpleWaypoint`](#SimpleWaypoint)  | Specifies the final location for all the routes. A destination can be specified as a Point, a landmark, or an address.   |
| `DistanceUnits` | [`DistanceUnitType`](#DistanceUnitType)   | The units to use for distance. |
| `ExcludeInstructions` | `bool` | Specifies to return only starting points for each major route in the response. When this option is not specified, detailed directions for each route are returned. |
| `RouteAttributes` | `List<RouteAttributeType>` | Specifies to include or exclude parts of the routes response.  |

### <a name="RouteRequest"></a> RouteRequest Class

A request that calculates routes between waypoints. Inherits from the BaseRestRequest class.

#### Methods

| Name            | Return Type | Description |
|-----------------|-------------|------|
|`Execute()`       | `Task<Response>` | Executes the request.|
| `Execute(Action<int> remainingTimeCallback)` | `Task<Response>` | Executes the request.             |
| `GetRequestUrl()` | `string`| Gets the request URL to perform a query for route directions. |

#### Properties

| Name         | Type                       | Description       |
|--------------|----------------------------|-------------------|
| `RouteOptions` | [`RouteOptions`](#RouteOptions) | Options to use when calculate route.  |
|`Waypoints`  | `List<SimpleWaypoint>` | Specifies two or more locations that define the route and that are in sequential order. A route is defined by a set of waypoints and viaWaypoints (intermediate locations that the route must pass through). You can have a maximum of 25 waypoints, and a maximum of 10 viaWaypoints between each set of waypoints. The start and end points of the route cannot be viaWaypoints. |

#### Extended Properties

Some additional options have been added to the route request to increase its functionality. 

| Name         | Type                       | Description       |
|--------------|----------------------------|-------------------|
| `BatchSize` | `int` | The maximium number of waypoints that can be in a single request. If the batchSize is smaller than the number of waypoints, when the request is executed, it will break the request up into multiple requests, thus allowing routes with more than 25 waypoints to be . Must by between 2 and 25. Default: 25. |
| `WaypointOptimization` | [TspOptimizationType](#TspOptimizationType) | Specifies if the waypoint order should be optimized using a travelling salesmen algorithm which metric to optimize on. If less than 10 waypoints, brute force is used, for more than 10 waypoints, a genetic algorithm is used.  Ignores IsViaPoint on waypoints and makes them waypoints. Default: **false**<br/><br/>**Warning**: If travel time or travel distance is used, a standard Bing Maps key will need to be required, not a session key, as the distance matrix API will be used to process the waypoints. This can generate a lot of billable transactions. |

### <a name="SnapToRoadRequest"></a> SnapToRoadRequest Class

Snaps a set of coordinates to roads. Inherits from the BaseRestRequest class.

#### Methods

| Name | Return Type | Description|
|-----------------|-------------|-------------|
|`Execute()`       | `Task<Response>` | Executes the request.|
| Execute(Action\<int\> remainingTimeCallback) | `Task<Response>` | Executes the request. |
| `GetRequestUrl()` | `string`      | Gets the request URL to perform a snap to road query. |

#### Properties

| Name         | Type                       | Description       |
|--------------|----------------------------|-------------------|
| `Points` | `List<Coordinate>` | A set of points to snap to roads. Up to 100 -points may be passed in. |
| `Interpolate` | `bool`  | Indicates if the space between the snapped points should be filled with additional points along the road, thus returning the full route path. Default: false |
| `IncludeSpeedLimit` | `bool`  | Indicates if speed limitation data should be returned for the snapped points. Default: false |
| `IncludeTruckSpeedLimit` | `bool`  | Indicates if speed limitation data should be returned for the snapped points. Default: false |
| `SpeedUnit` | [`SpeedUnitType`](#SpeedUnitType) | Indicates the units in which the returned speed limit data is in. |
| `TravelMode` | [`TravelModeType`](#TravelModeType)  | Indicates which routing profile to snap the points to. Default: Driving |

### <a name="TrafficRequest"></a> TrafficRequest Class

Requests traffic information. Inherits from the BaseRestRequest class.

#### Methods

| Name            | Return Type | Description                                               |
|-----------------|-------------|-----------------------------------------------------------|
| `Execute()`       | `Task<Response>` | Executes the request.                                  |
| `Execute(Action<int> remainingTimeCallback)` | `Task<Response>` | Executes the request.       |
| `GetRequestUrl()` | `string`      | Gets a URL for requesting traffic data for a GET request. |

#### Properties

| Name                 | Type                     | Description    |
|----------------------|--------------------------|----------------|
| `IncludeLocationCodes` | `bool`                      | Specifies whether to include traffic location codes in the response. Traffic location codes provide traffic incident information for pre-defined road segments. A subscription is typically required to be able to interpret these codes for a geographical area or country. Default is **false**. |
| `MapArea`  | [BoundingBox](#BoundingBox) | Specifies the area to search for traffic incident information. A rectangular area specified as a bounding box. The size of the area can be a maximum of 500 km x 500 km.                     |
| `Severity` | `List<SeverityType>` | Specifies severity level of traffic incidents to return. The default is to return traffic incidents for all severity levels.   |
| `TrafficType` | `List<TrafficType>`;  | Specifies the type of traffic incidents to return. |


### <a name="OptimizeItineraryRequest"></a> OptimizeItineraryRequest class

Request for Bing Maps Multi-Itinerary Optimization API.

#### Methods

| Name            | Return Type | Description                                               |
|-----------------|-------------|-----------------------------------------------------------|
| `Execute()`       | `Task<Response>` | Executes the request.                                  |
| `Execute(Action<int> remainingTimeCallback)` | `Task<Response>` | Executes the request.       |
| `GetRequestUrl()` | `string`      | Gets a URL for requesting traffic data for a GET request. |

#### Properties

| Name                 | Type                     | Description    |
|----------------------|--------------------------|----------------|
| `Agents` | `List<Agent>`  | List of agent itinerary information: including the agent name, shift starting and ending locations for agent, and capacity of the agent's vehicle. |
| `ItineraryItems`  | `List<OptimizeItineraryItem>` | List of itinerary items to be scheduled among the specified agents, including the location name, location (lat/lon), priority, dwell time, business closing and opening times for each item to be scheduled, quantity to be delivered to or picked up from each location, and pickup/drop off sequence dependency with other itineraryItems. |
| `Type` | `OptimizationType` | Specifies whether traffic data should used to optimize the order of waypoint items. Default: `SimpleRequest` Note: If the ‘type’ parameter is set to ‘TrafficRequest’, it will automatically use ‘true’ as the ‘roadnetwork’ parameter value. |
| `RoadNetwork` | `bool`;  | Optional. If true, uses actual road network information, and both travel distances and travel times between the itinerary locations to calculate optimizations. If false, a constant radius is used to measure distances and a constant travel speed is used to calculate travel times between locations. |
| `CostValue` | `CostValueType` | A parameter used to optimize itineraries in addition to maximizing the sum of item priorities. Default: TravelTime |

## Elevations API

### <a name="ElevationRequest"></a> ElevationRequest Class

A request for elevation data. Inherits from the BaseRestRequest class.

#### Methods

| Name                      | Return Type            | Description    |
|---------------------------|------------------------|----------------|
|`Execute()`       | `Task<Response>` | Executes the request.                                        |
| `Execute(Action<int> remainingTimeCallback)` | `Task<Response>` | Executes the request.             |
| `GetElevationCoordinates()` | `List<Coordinate>` | Gets a list of coordinates that are related to the returned index of the elevation data.                                                                                            |
|  `GetPointsAsString()`        | `string`                 | Returns the Point information as a formatted string. Only the first 1024 points will be used. Example: `points=38.8895,77.0501,38.8877,-77.0472,38.8904,-77.0474,38.8896,77.0351` |
| `GetPostRequestUrl()`       | `string`                 | Gets a URL for requesting elevation data for a POST request.|
| `GetRequestUrl()`           | `string`                 | Gets a URL for requesting elevation data for a GET request. |

#### Properties

| Name           | Type                   | Description                     |
|----------------|------------------------|---------------------------------|
| `Bounds`         | [`BoundingBox`](#BoundingBox)            | Specifies the rectangular area over which to provide elevation values.   |
| `Col`            | `int`                    | Specifies the number of columns to use to divide the bounding box area into a grid. The rows and columns that define the bounding box each count as two (2) of the rows and columns. Elevation values are returned for all vertices of the grid. |
| `GetGeoidOffset` | `bool`                    | A boolean indicating if the offset from the geoid should be returned. Requires a list of points to be specified.                                                                                                                                 |
| `Height` | [`ElevationType`](#ElevationType)          | Specifies which sea level model to use to calculate elevation.|
| `Points`        | `List<Coordinate>` | A set of coordinates on the Earth to use in elevation calculations. The exact use of these points depends on the type of elevation request. Overrides the Bounds value if both are specified. The maximum number of points is 1024.              |
| `Row`          | `int`                    | Specifies the number of rows to use to divide the bounding box area into a grid. The rows and columns that define the bounding box each count as two (2) of the rows and columns. Elevation values are returned for all vertices of the grid.    |
| `Samples`        | `int`                    | Specifies the number of equally-spaced elevation values to provide along a polyline path. Used when Points value is set. Make = 1024                             |
| `Bounds`         | [`BoundingBox`](#BoundingBox)            | Specifies the rectangular area over which to provide elevation values.

### <a name="ImageryMetadataRequest"></a> ImageryMetadataRequest Class

Requests imagery metadata information from Bing Maps. Inherits from the BaseRestRequest class.

#### Methods

| Name            | Return Type | Description                                |
|-----------------|-------------|--------------------------------------------|
|`Execute()`       | `Task<Response>` | Executes the request.                                        |
| `Execute(Action<int> remainingTimeCallback)` | `Task<Response>` | Executes the request.             |
| `GetRequestUrl()` | string      | Gets the request URL. Throws an exception if a zoom level is not specified when a centerPoint is specified when ImagerySet is Road, Aerial and AerialWithLabels. |

#### Properties

| Name                    | Type        | Description                     |
|-------------------------|-------------|---------------------------------|
| `CenterPoint`             | [`Coordinate`](#Coordinate)  | Required when imagerySet is Birdseye or BirdseyeWithLabels. Optional for other imagery sets. The center point to use for the imagery metadata.      |
|`GetBasicInfo`            | `bool`         | Get only the basic metadata for an imagery set at a specific location. This URL does not return a map tile URL.                                     |
| `ImagerySet`              | [`ImageryType`](#ImageryType) | The type of imagery for which you are requesting metadata.                                                                                          |
| `IncludeImageryProviders` | `bool`         | When you specified the two-letter ISO country code is included for addresses in the response.                                                       |
| `Orientation`             | `double`       | The orientation of the viewport to use for the imagery metadata. This option only applies to Birdseye imagery.                                      |
| `UseHTTPS`                | `bool`         | When set to true tile URL's will use HTTPS.                                                                                                         |
| `ZoomLevel`               | `int`         | Required if a centerPoint is specified and imagerySet is set to Road, Aerial or AerialWithLabels The level of zoom to use for the imagery metadata. |

### <a name="ImageryRequest"></a> ImageryRequest Class

Requests an image from the REST imagery service. Inherits from the BaseImageryRestRequest class.

#### Methods

| Name                  | Return Type | Description                |
|-----------------------|-------------|----------------------------|
|`Execute()`       | `Task<Response>` | Executes the request.                                        |
| `Execute(Action<int> remainingTimeCallback)` | `Task<Response>` | Executes the request.             |
| `GetPostRequestUrl()`   | `string`      | Gets a URL for requesting imagery data for a POST request.  |
| `GetPushpinsAsString()` | `string`      | Returns the Pushpin information as a formatted string.      |
| `GetRequestUrl()`       | `string`      | Gets the request URL. If both a Query and Address are specified, the Query value will be used. Throws an exception if a Query or Address value is not specified. |

#### Properties

| Name            | Type                       | Description   |
|-----------------|----------------------------|----------------|
| `CenterPoint`     | [`Coordinate`](#Coordinate)                 | Required when imagerySet is Birdseye or BirdseyeWithLabels. Optional for other imagery sets. The center point to use for the imagery metadata.                         |
| `DeclutterPins`   | `bool`                        | Specifies whether to change the display of overlapping pushpins so that they display separately on a map.  |
| `EntityType`      | [`EntityType`](#EntityType)                 | Indicates the type of entity that should be highlighted. The entity of this type that contains the centerPoint will be highlighted. Supported EntityTypes: CountryRegion, AdminDivision1, or PopulatedPlace.   |
| `Format`          | [`ImageFormatType`](#ImageFormatType)            | The image format to use for the static map.                        |
| `GetMetadata`     | `bool`                        | Optional. Specifies whether to return metadata for the static map instead of the image. The static map metadata includes the size of the static map and the placement and size of the pushpins on the static map.   |
| `HighlightEntity` | `bool`                        | Highlights a polygon for an entity.      |
| `ImagerySet`      | [`ImageryType`](#ImageryType)                | The type of imagery for which you are requesting metadata.        |
| `MapArea`         | [`BoundingBox`](#BoundingBox)                | Required when a center point or set of route points are not specified. The geographic area to display on the map.        |
| `MapHeight`       | `int`                        | The height of the map. Default is **350px**.  |
| `MapWidth`        | `int`                        | The width of the map. Default is **350px**.   |
| `Pushpins`        | `List<ImageryPushpin>`| List of pushpins to display on the map.                            |
| `Query`           | `string`                     | A query string that is used to determine the map location to display. |
| `RouteOptions`    | [`RouteOptions`](#RouteOptions)               | Options for calculating route.  |
| `ShowTraffic`     | `bool`                        | Specifies if the traffic flow layer should be displayed on the map or not. Default is **false**. |
| `Style`           | `string` | The custom map style to apply to the image. |
| `Waypoints`       | `List<SimpleWaypoint>` | Specifies two or more locations that define the route and that are in sequential order. A route is defined by a set of waypoints and viaWaypoints (intermediate locations that the route must pass through). You can have a maximum of 25 waypoints, and a maximum of 10 viaWaypoints between each set of waypoints. The start and end points of the route cannot be viaWaypoints. |
| `ZoomLevel` | `int`                        | The level of zoom to display.  |

# <a name="CommonClasses"></a> Common Classes

## <a name="BoundingBox"></a> BoundingBox Class

### Methods

| Name       | Return Type | Description |
|------------|-------------|-------------|
| ToString() | `string`      | Returns a string in the format `SouthLatitude,WestLongitude,NorthLatitude,EastLongitude` |

### Properties

| Name          | Type   | Description |
|---------------|--------|-------------|
| EastLongitude | `double`  | The eastern most longitude value. |
| NorthLatitude | `double`  | The northern most latitude value. |
| SouthLatitude | `double`  | The southern most latitude value. |
| WestLongitude | `double`  | The western most longitude value. |

## <a name="Coordinate"></a> Coordinate Class

A class that defines location coordinate value.

### Constructor

> Coordinate()

> Coordinate(double latitude, double longitude)

### Static Methods

| Name            | Return Type | Description                                                   |
|-----------------|-------------|---------------------------------------------------------------|
| Parse(string coordinateString) | Coordinate | Parses a coordinate value from a string with the format "latitude,longitude".  |


### Properties

| Name      | Type   | Description           |
|-----------|--------|-----------------------|
| Latitude  | `double`  | Latitude coordinate.  |
| Longitude | `double`  | Longitude coordinate. |

## <a name="CustomMapStyleManager"></a> CustomMapStyleManager Class

A static class to assist with working with Bing Maps Customer Map Styles.

### Static Methods

| Name                                                 | Return Type          | Description          |
|------------------------------------------------------|----------------------|----------------------|
| GetRestStyle(string style)    | `string` | Converts a custom JSON map style, into a style using the REST parameter format. If the style is already in the REST parameter formatter, it will be unaltered.  |


## <a name="ImageryPushpin"></a> ImageryPushpin Class

Pushpin defination for Bing Maps REST imagery service as documented [here](https://msdn.microsoft.com/en-us/library/ff701719.aspx)

### Methods

| Name       | Return Type | Description    |
|------------|-------------|----------------|
| ToString() | `string`      | Returns a string version of the pushpin in the format `pushpin=latitude,longitude;iconStyle;label` |

### Properties

| Name      | Type       | Description                            |
|-----------|------------|----------------------------------------|
| IconStyle | `int`        | The icon style to use for the pushpin. |
| Label     | `string`     | Label to display on top of pushpin.    |
| Location  | [`Coordinate`](#Coordinate) | Coordinate to display pushpin.   |

## <a name="PointCompression"></a> PointCompression Class

This is a static class that exposes a compression algorithm to encodes/decodes a collections of coordinates into a string. This algorithm is used for generating a compressed collection of coordinates for use with the Bing Maps REST Elevation Service and also used for decoding the compressed coordinates returned by the GeoData API.

These algorithms come from the following documentation:

[http://msdn.microsoft.com/en-us/library/jj158958.aspx](http://msdn.microsoft.com/en-us/library/jj158958.aspx)

[http://msdn.microsoft.com/en-us/library/dn306801.aspx](http://msdn.microsoft.com/en-us/library/dn306801.aspx)

### Static Methods

| Name                                                            | Return Type | Description |
|-----------------------------------------------------------------|-------------|-------------|
| Encode(List\<[Coordinate](#Coordinate\> points)                           | `string`      | Compresses a list of coordinates into a string. Based on: [http://msdn.microsoft.com/en-us/library/jj158958.aspx](http://msdn.microsoft.com/en-us/library/jj158958.aspx)  |
| TryDecode(string value, out List\<[Coordinate](#Coordinate)\> parsedValue) | `bool`         | Decodes a collection of coordinates from a compressed string. Returns a boolean indicating if the algorithm was able to decode the compressed coordinates or not. Based on: [http://msdn.microsoft.com/en-us/library/dn306801.aspx](http://msdn.microsoft.com/en-us/library/dn306801.aspx) |

## <a name="RouteOptions"></a> RouteOptions Class

A class that defines the options that can to use when calculating a route.

### Properties

| Name                    | Type                           | Description   |
|-------------------------|--------------------------------|---------------|
| Avoid                   | List\<[AvoidType](#AvoidType)\>          | Specifies the road types to minimize or avoid when a route is created for the driving travel mode.  |
| DateTime                | DateTime                       | The dateTime parameter identifies the desired time to be used when calculating a route. This is supported by driving and transit routes. When calculating, driving routes the route optimization type should be TimeWithTraffic. The route time will be used as the departure time. When calculating transit routes timeType can be specified. |
| DistanceBeforeFirstTurn | `int`                            | Specifies the distance before the first turn is allowed in the route. This option only applies to the driving travel mode. An integer distance specified in meters. Use this parameter to make sure that the moving vehicle has enough distance to make the first turn. |
| DistanceUnits           | [DistanceUnitType](#DistanceUnitType)               | The units to use for distance. |
| Heading                 | `int`                            | Specifies the initial heading for the route. An integer value between 0 and 359 that represents degrees from north where north is 0 degrees and the heading is specified clockwise from north. |
| MaxSolutions            | `int`                            | Specifies the maximum number of transit or driving routes to return. An interger between 1 and 3. This parameter is available for the Driving and Transit travel modes for routes between two waypoints. This parameter does not support routes with more than two waypoints. For driving routes, you must not set the avoid and distanceBeforeFirstTurn parameters. The maxSolutions parameter is supported for routes in the United States, Canada, Mexico, United Kingdom, Australia, and India. |
| Optimize                | [RouteOptimizationType](#RouteOptimizationType)          | Specifies what parameters to use to optimize the route. |
| RouteAttributes         | List\<[RouteAttributeType](#RouteAttributeType)\> | Specifies to include or exclude parts of the routes response. |
| TimeType                | [RouteTimeType](#RouteTimeType)                  | Specifies how to interpret the date and transit time value that is specified by the dateTime parameter.   |
| Tolerances              | List\<double\>            | Specifies a series of tolerance values. Each value produces a subset of points that approximates the route that is described by the full set of points. This parameter is only valid when the routePathOutput parameter is set to Points. You can specify a maximum of seven (7) tolerance values.   |
| TravelMode              | [TravelModeType](#TravelModeType)                 | The mode of travel for the route. Default: Driving.   |
| VehicleSpec | [VehicleSpec](#VehicleSpec) | Truck routing specific vehicle attribute.  |

## <a name="ServiceManager"></a> ServiceManager Class

This is a static class that is used for processing all requests to the Bing Maps REST Services asynchronously. Note that all requests classes now have an Execute function which will retrun a Response object which can be used as an alternative.

### Static Properties

| Name          | Type      | Description   |
|---------------|-----------|---------------|
| Proxy         | IWebProxy |  Proxy settings to be used when making web requests.  |
| QpsLimit      | `int`       | The number of queries per second to limit certain requests to. This is primarily used when batching multiple requests in a single process such as when geoeocidng all waypoints for the distance matrix API, or when manually generating a truck based distance matrix using the routing API.  | 

### Static Methods

| Name                                                 | Return Type          | Description          |
|------------------------------------------------------|----------------------|----------------------|
| GetResponseAsync([BaseRestRequest](#BaseRestRequest) request)            | `Task<Response>`| Processes a REST requests that returns data.            |
| GetResponseAsync([BaseRestRequest](#BaseRestRequest) request, Action\<int\> remainingTimeCallback)            | `Task<Response>` | Processes a REST requests that returns data.            |
| GetImageAsync([BaseImageryRestRequest](#BaseImageryRestRequest) imageryRequest) | Task\<Stream\>   | Processes a REST requests that returns an image stream. |

## <a name="SimpleAddress"></a> SimpleAddress Class

A simple address class that can be passed in to queries.

### Properties

| Name          | Type   | Description   |
|---------------|--------|---------------|
| AddressLine   | `string` | The official street line of an address relative to the area, as specified by the Locality, or PostalCode, properties. Typical use of this element would be to provide a street address or any official address.   |
| AdminDistrict | `string` | The subdivision name in the country or region for an address. This element is typically treated as the first order administrative subdivision, but in some cases, it is the second, third, or fourth order subdivision in a country, dependency, or region. |
| CountryRegion | `string` | The ISO country code for the country.     |
| Locality      | `string` | The locality, such as the city or neighborhood, that corresponds to an address.  |
| PostalCode    | `string` | The post code, postal code, or ZIP Code of an address. |

## <a name="SimpleWaypoint"></a> SimpleWaypoint Class

A simple waypoint class that can be used to calculate a route.

### Constructor

> SimpleWaypoint()

> SimpleWaypoint([`Coordinate`](#Coordinate) coordinate)

> SimpleWaypoint(string address)

> SimpleWaypoint([`Coordinate`](#Coordinate) coordinate, string address)

> SimpleWaypoint(double latitude, double longitude) 

### Methods

| Name            | Return Type | Description                                                   |
|-----------------|-------------|---------------------------------------------------------------|
| TryGeocode(SimpleWaypoint waypoint, string bingMapsKey) | Task | Tries to geocode a simple waypoint.  |
| TryGeocode(SimpleWaypoint waypoint, BaseRestRequest baseRequest) | Task | Tries to geocode a simple waypoint.  |
| TryGeocodeWaypoints(List\<SimpleWaypoint\> waypoints, string bingMapsKey) | Task      | Attempts to geocode a list of simple waypoints. |
| TryGeocodeWaypoints(List\<SimpleWaypoint\> waypoints, BaseRestRequest baseRequest) | Task      | Attempts to geocode a list of simple waypoints. |

### Static Methods

| Name            | Return Type | Description                                                   |
|-----------------|-------------|---------------------------------------------------------------|
| Parse(string waypointString) | SimpleWaypoint | Parses a simple waypoint value from a string. If it has the format "latitude,longitude", it will be used as a coordinate, otherwise as an address.  |

### Properties

| Name       | Type       | Description   |
|------------|------------|---------------|
| Address    | `string`     | The address query for the waypoint.    |
| Coordinate | [`Coordinate`](#Coordinate) | The coordinate of the waypoint. When specified this will be used instead of the Address value in requests. |
| IsViaPoint | `bool`        | A bool indicating whether the waypoint is a via point.           |

## <a name="VehicleSpec"></a> VehicleSpec Class

A class that defines the options that can to use when calculating a truck route. Extends the [RouteOptions class](#RouteOptions).

### Properties

| Name                    | Type                           | Description        |
|-------------------------|--------------------------------|--------------------|
| DimensionUnit | [DimensionUnitType](#DimensionUnitType) | The unit of measurement of width, height, length. |
| WeightUnit | [WeightUnitType](#WeightUnitType) | The unit of measurement of weight. |
| VehicleHeight | `double`  | The height of the vehicle in the specified dimension units. |
| VehicleWidth | `double`  | The width of the vehicle in the specified dimension units. |
| VehicleLength | `double`  | The length of the vehicle in the specified dimension units. |
| VehicleWeight | `double`  | The weight of the vehicle in the specified weight units. |
| VehicleAxles | `int` | The number of axles. |
| VehicleSemi | `bool`  | Indicates if the truck is pulling a semi-trailer. Semi-trailer restrictions are mostly used in North America. |
| VehicleTrailers | `int` | Specifies number of trailers pulled by a vehicle. The provided value must be between 0 and 4.  |
| VehicleMaxGradient | `double`  | The max gradient the vehicle can drive measured in degrees. |
| VehicleMinTurnRadius | `double`  | The mini-mum required radius for the vehicle to turn in the specified dimension units. |
| VehicleAvoidCrossWind | `bool`  | Indicates if the vehicle shall avoid crosswinds. |
| VehicleAvoidGroundingRisk | `bool`  | Indicates if the route shall avoid the risk of grounding. |
| VehicleHazardousMaterials | List\<[HazardousMaterialType](#HazardousMaterialType)\> | A list of one or more hazardous materials for which the vehicle is transporting. |
| VehicleHazardousPermits | List\<[HazardousMaterialPermitType](#HazardousMaterialPermitType)\> | A list of one or more hazardous materials for which the vehicle has permits. |

# <a name="Enumerations"></a> Enumerations

## <a name="AvoidType"></a> AvoidType Enumeration

Specifies the road types to minimize or avoid when the route is created for the driving travel mode.

| Name             | Description                                                    |
|------------------|----------------------------------------------------------------|
| Highways         | Avoids the use of highways in the route.                       |
| MinimizeHighways | Minimizes (tries to avoid) the use of highways in the route.   |
| MinimizeTolls    | Minimizes (tries to avoid) the use of toll roads in the route. |
| Tolls            | Avoids the use of toll roads in the route.                     |

## <a name="ConfidenceLevelType"></a> ConfidenceLevelType Enumeration

The level of confidence that the geocoded location result is a match.

| Name    | Description              |
|---------|--------------------------|
| High    | High confidence match.   |
| Low     | Low confidence match.    |
| Medium  | Medium confidence match. |
| None    | No confidence level set. |

## <a name="DimensionUnitType"></a> DimensionUnitType Enumeration

Measurement units of vehicle dimensions.

| Name    | Description              |
|---------|--------------------------|
| Meter   | Dimensions in meters.   |
| Foot     | Dimensions in feet.    |


## <a name="DistanceUnitType"></a> DistanceUnitType Enumeration

Units of measurements for distances.

| Name  | Description              |
|-------|--------------------------|
| Kilometers    | Distances in Kilometers. |
| Miles | Distances in Miles.      |

## <a name="ElevationType"></a> ElevationType Enumeration

Relative elevation type.

| Name       | Description                       |
|------------|-----------------------------------|
| Ellipsoid  | Ellipsoid Earth model (WGS84).    |
| Sealevel   | Geoid Earth model (EGM2008 2.5-). |

## <a name="EntityType"></a> EntityType Enumeration

Types of location based entities.

| Name            | Description                                                                                        |
|-----------------|----------------------------------------------------------------------------------------------------|
| Address         | A street address or RoadBlock.                                                                     |
| AdminDivision1  | First administrative level within the country/region level, such as a state or a province.         |
| AdminDivision2  | Second administrative level within the country/region level, such as a county.                     |
| CountryRegion   | Country or region                                                                                  |
| Neighborhood    | A section of a populated place that is typically well-known, but often with indistinct boundaries. |
| PopulatedPlace  | A concentrated area of human settlement, such as a city, town or village.                          |
| Postcode1       | The smallest post code category, such as a zip code.                                               |

## <a name="HazardousMaterialPermitType"></a> HazardousMaterialPermitType Enumeration

Types of hazardous material permits for truck routing.

| Name  | Description                                                                                  |
|-------|----------------------------------------------------------------------------------------------|
| AllAppropriateForLoad | Permit for goods which are all appropriate for load. |
| Combustible | Permit for combustible material. |
| Corrosive | Permit for corrosive material. |
| Explosive | Permit for explosive material. |
| Flammable | Permit for flammable material. |
| FlammableSolid | Permit for flammable solid material. |
| Gas | Permit for gases. |
| Organic | Permit for organic material. |
| Poison | Permit for poisonous material. |
| PoisonousInhalation | Permit for poisonous inhalation material. |
| Radioactive | Permit for radioactive material. |

## <a name="HazardousMaterialType"></a> HazardousMaterialType Enumeration

Hazardous materials in which a truck may transport.

| Name  | Description                                                                                  |
|-------|----------------------------------------------------------------------------------------------|
| Combustable | Combustable materials. |
| Corrosive | Corrosive materials. |
| Explosive | Explosive materials. |
| Flammable | Flammable materials. |
| FlammableSolid | Flammable Solid materials. |
| Gas | Gases. |
| GoodsHarmfulToWater | Goods Harmful To Water. |
| Organic | Organic materials. |
| Poison | Poisonous materials. |
| PoisonousInhalation | Poisonous Inhalation materials. |
| Radioactive | Radioactive materials. |

## <a name="ImageFormatType"></a> ImageFormatType Enumeration

Imagery format types.

| Name  | Description                                                                                  |
|-------|----------------------------------------------------------------------------------------------|
| GIF   | GIF image format.                                                                            |
| JPEG  | JPEG image format. JPEG format is the default for Road, Aerial and AerialWithLabels imagery. |
| PNG   | PNG image format. PNG is the default format for CollinsBart and OrdnanceSurvey imagery.      |

## <a name="ImageryType"></a> ImageryType Enumeration

Types of map imagery.

| Name                | Description                             |
|---------------------|-----------------------------------------|
| Aerial              | Aerial imagery.                         |
| AerialWithLabels    | Aerial imagery with a road overlay.     |
| AerialWithLabelsOnDemand | Aerial imagery with on-demand road overlay. |
| Birdseye            | Bird-s eye (oblique-angle) imagery      |
| BirdseyeWithLabels  | Bird-s eye imagery with a road overlay. |
| BirdseyeV2            | The second generation Bird-s eye (oblique-angle) imagery.      |
| BirdseyeV2WithLabels  | The second generation Bird-s eye (oblique-angle) imagerywith a road overlay. |
| CanvasDark | A dark version of the road maps. |
| CanvasGray | A grayscale version of the road maps. |
| CanvasLight | A lighter version of the road maps which also has some of the details such as hill shading disabled. |
| OrdnanceSurvey      | Ordnance Survey imagery.                |
| Road                | Roads without additional imagery.       |
| RoadOnDemand        | Roads without additional imagery. Uses dynamic tile service.      |

## <a name="RouteAttributeType"></a> RouteAttributeType Enumeration

The type of route attributes to include in a route response.

| Name                | Description                                                                                                                                                                          |
|---------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| All                 | Used to specify the following attributes as a group: excluteItinerary, routePath, and transitStops.                                                                                  |
| ExcludeItinerary    | Do not include detailed directions in the response. Detailed directions are provided as itinerary items and contain details such as written instructions and traffic location codes. |
| RoutePath           | Include a set of point (latitude and longitude) values that describe the route-s path in the response.                                                                               |
| RouteSummariesOnly  | Include only travel time and distance for the route, and does not provide other information.    
| RegionTravelSummary  | Include travel summary of distance, time, and toll road distance by two entity types: country (e.g. US, Canada) and administrative division or subregion (e.g. “state” in US and “province” in Canada). |
| TransitStops        | Include information about transit stops for transit routes.                                                                                                                          |

## <a name="RouteOptimizationType"></a> RouteOptimizationType Enumeration

Specifies what parameters to use to optimize the route on the map.

| Name              | Description          |
|-------------------|----------------------|
| Distance          | Optimizes route for shortest distance.                                                                                    |
| Time              | Optimizes route for shortest travel time.                                                                                 |
| TimeAvoidClosure  | The route is calculated to minimize the time and avoid road closures. Traffic information is not used in the calculation. |
| TimeWithTraffic   | Optimizes route for shortest travel time with respect to current traffic conditions.                                      |

## <a name="RouteTimeType"></a> RouteTimeType Enumeration

Specifies how to interpret the date and transit time value that is specified by the dateTime parameter.

| Name           | Description       |
|----------------|--------------------|
| Arrival        | The dateTime parameter contains the desired arrival time for a transit request.            |
| Departure      | The dateTime parameter contains the desired departure time for a transit request.          |
| LastAvailable  | The dateTime parameter contains the latest departure time available for a transit request. |

## <a name="SeverityType"></a> SeverityType Enumeration

Specifies the severity level of a traffic incident.

| Name       | Description          |
|------------|----------------------|
| LowImpact  | Low impact severity. |
| Minor      | Minor severity.      |
| Moderate   | Moderate severity.   |
| Serious    | Serious severity.    |

## <a name="SpeedUnitType"></a> SpeedUnitType Enumeration

Unit of speed.

| Name       | Description          |
|------------|----------------------|
| KPH        | Kilometers per hour. |
| MPH        | Miles per hour.      |

## <a name="TimeUnitType"></a> TimeUnitType Enumeration

Represents the units in which time is measured.

| Name            | Description                     |
|-----------------|---------------------------------|
| Minute          | Time is in minutes.             |
| Second          | Time is in seconds.             |
 
## <a name="TrafficType"></a> TrafficType Enumeration

Specifies the type of a traffic incident.

| Name             | Description                     |
|------------------|---------------------------------|
| Accident         | Accident incident type.         |
| Congestion       | Congestion incident type.       |
| DisabledVehicle  | Disabled vehicle incident type. |
| MassTransit      | Mass transit incident type.     |
| Miscellaneous    | Miscellaneous incident type.    |
| OtherNews        | Other news incident type.       |
| PlannedEvent     | Planned event incident type.    |
| RoadHazard       | Road hazard incident type.      |
| Construction     | Construction incident type.     |
| Alert            | Alert incident type.            |
| Weather          | Weather incident type.          |

## <a name="TravelModeType"></a> TravelModeType Enumeration

The mode of travel for the route.

| Name     | Description   |
|----------|---------------|
| Driving  | Driving mode. |
| Walking  | Walking mode. |
| Transit  | Transit mode. |
| Truck    | Truck driving mode. |

## <a name="WeightUnitType"></a> WeightUnitType Enumeration

Unit of measurement for vehicle weights.

| Name    | Description              |
|---------|--------------------------|
| Kilograms   | Weight in kilograms.   |
| Pounds     | Weight in pounds.    |

# <a name="EnhancedResponseClasses"></a> Enhanced Response Classes

These are response classes that have been extended extensively to make them easier to use. 

## <a name="DistanceMatrix"><a/> DistanceMatrix Class

### Static Methods

| Name            | Return Type | Description                                              |
|-----------------|-------------|----------------------------------------------------------|
|  CreateStraightLineNxNMatrix(List\<SimpleWaypoint\> waypoints, DistanceUnitType distanceUnits, string bingMapsKey) | Distance Matrix | Creates a NxN distance matrix with straight line distances. |

### Methods

An indexing system has been added to the DistanceMatrix class to make it easy to retrieve cells by origin and destination index.

| Name            | Return Type | Description                                              |
|-----------------|-------------|----------------------------------------------------------|
| GetCell(int originIdx, int destinationIdx) | DistanceMatrixCell | Retrives the distance matrix cell for a specified origin-destination pair. Returns -1 if a cell can not be found in the results or had an error in calculation. |
| GetCell(int originIdx, int destinationIdx, DateTime timeInterval) | DistanceMatrixCell | Retrives the distance matrix cell for a specified origin-destination pair and time interval. Returns -1 if a cell can not be found in the results or had an error in calculation. |
| GetCell(int originIdx, int destinationIdx, int timeIntervalIdx) | DistanceMatrixCell | Retrives the distance matrix cell for a specified origin-destination pair and time interval. Returns -1 if a cell can not be found in the results or had an error in calculation. |
| GetCells(int originIdx, int destinationIdx) | DistanceMatrixCell[] | Gets all cells for the specified origin and destination index, ordered by time (ascending). |
| GetDistance(int originIdx, int destinationIdx) | `double`  | Retrives the travel distance for a specified origin-destination pair. Returns -1 if a cell can not be found in the results or had an error in calculation. |
| GetDistance(int originIdx, int destinationIdx, DateTime timeInterval) | `double`  | Retrives the travel distance for a specified origin-destination pair and time interval. Returns -1 if a cell can not be found in the results or had an error in calculation. |
| GetDistance(int originIdx, int destinationIdx, int timeIntervalIdx) | `double`  | Retrives the travel distance for a specified origin-destination pair and time interval. Returns -1 if a cell can not be found in the results or had an error in calculation. |
| GetDistances(int originIdx, int destinationIdx) | double[] | Gets all travel distances for the specified origin and destination index, ordered by time (ascending). |
| GetEdgeDistance(int[] waypointIndicies) | `double`  | Retrieves the total travel distance between all waypoints indicies which represent an edge (graph/path). If a path between to waypoints is not routable, a large distance value will be returned.| 
| GetEdgeDistance(int[] waypointIndicies, bool isRoundTrip) | `double`  | Retrieves the total travel distance between all waypoints indicies which represent an edge (graph/path). If a path between to waypoints is not routable, a large distance value will be returned.| 
| GetEdgeTime(int[] waypointIndicies) | `double`  | Retrieves the total travel time between all waypoints indicies which represent an edge (graph/path). If a path between to waypoints is not routable, a large time value will be returned. |
| GetEdgeTime(int[] waypointIndicies, bool isRoundTrip) | `double`  | Retrieves the total travel time between all waypoints indicies which represent an edge (graph/path). If a path between to waypoints is not routable, a large time value will be returned.|
| GetTime(int originIdx, int destinationIdx) | `double`  | Retrives the travel time for a specified origin-destination pair. Returns -1 if a cell can not be found in the results or had an error in calculation. |
| GetTime(int originIdx, int destinationIdx, DateTime timeInterval) | `double`  | Retrieves the travel time for a specified origin-destination pair and time interval. Returns -1 if a cell can not be found in the results or had an error in calculation. |
| GetTime(int originIdx, int destinationIdx, int timeIntervalIdx) | `double`  | Retrieves the travel time for a specified origin-destination pair and time interval. Returns -1 if a cell can not be found in the results or had an error in calculation. |
| GetTimes(int originIdx, int destinationIdx) | double[] | Gets all travel times for the specified origin and destination index, ordered by time (ascending). |

### Properties

| Name                | Type                   | Description    |
|---------------------|------------------------|----------------|
| Destinations | [SimpleWaypoint](#SimpleWaypoint)\[\]         | The array of destinations that were used to calculate the distance matrix.                               |
| ErrorMessage| `string` | Details of an error that may have occurred when processing the request.  |
| Origins      | [SimpleWaypoint](#SimpleWaypoint)\[\]         | The array of destinations that were used to calculate the distance matrix.                               |
| Result       | DistanceMatrixCell\[\] | Array of distance matrix cell results containing information for each coordinate pair and time interval. |
| TimeIntervals | List\<DateTime\> | A list of time intervals in which the distance matrix calculated for. |


# <a name="ExtensionClasses"></a> Extension Classes

## <a name="TravellingSalesmen"></a> TravellingSalesmen Class

This is a static class that solves the [travelling salesmen problem](https://en.wikipedia.org/wiki/Travelling_salesman_problem). Uses a greedy algrithm when 10 or less waypoints are specified, and a genetic algorithm for larger waypoint sets. 

### Methods

| Name            | Return Type | Description                                                     |
|-----------------|-------------|-----------------------------------------------------------------|
|  Solve(List\<[SimpleWaypoint](#SimpleWaypoint)\> waypoints, [TravelModeType](#TravelModeType)? travelMode, [TspOptimizationType](#TspOptimizationType)? tspOptimization, DateTime? departureTime, string bingMapsKey) | Task\<[TspSolution](#TspSolution)\> | Solves the travelling salesmen problem. |
| Solve([DistanceMatrix](#DistanceMatrix) matrix, [TspOptimizationType](#TspOptimizationType) tspOptimization)| Task\<[TspSolution](#TspSolution)\> | Solves the travelling salesmen problem. |

## <a name="TspOptimizationType"></a> TspOptimizationType Enumeration

Metrics in which the travelling salesmen problem is solved for.

| Name  | Description              |
|-------|--------------------------|
| StraightLineDistance | Optimizes based on straight line distances (as the crow flies). |
| TravelDistance | Optimizes based on travel distances (roads). Uses the Distance Matrix API. |
| TravelTime | Optimizes based on travel times (roads). Uses the Distance Matrix API. |

## <a name="TspSolution"></a> TspSolution Class

The result from a Travelling Salesmen calculation.

### Properties

| Name         | Type        | Description                                                     |
|--------------|-------------|-----------------------------------------------------------------|
| DistanceMatrix | [DistanceMatrix](#DistanceMatrix) | The distance matrix used in the calculation. |
| IsRoundTrip | `bool`  | Indicates if the path is for a round trip and returns to the origin or not. |
| OptimizedWaypoints | List\<[SimpleWaypoint](#SimpleWaypoint)\>  | A list of the waypoints in an optimized ordered.  |
| OptimizedWeight | `double`  | The optimized weight (time or distance) between all waypoints based on the TspOptimizationType. |
| TravelMode | [TravelModeType](#TravelModeType) | The travel mode used to calculate the distance matrix. |
| TspOptimization | [TspOptimizationType](#TspOptimizationType) | The metric used to solve the travelling salesmen problem for. |

## Response class extensions

Extensions that have been added to response classes.

### Response class extension

Extensions to the `Response` class to assist with common tasks.

#### Static Methods

| Name            | Return Type | Description                                              |
|-----------------|-------------|----------------------------------------------------------|
| HasResource(Response response) | bool | Check that a response has one or more resources. This is a helper class to save on having to check all the parts of the response tree. |
| GetFirstResource(Response response) | Resource | Gets the first resource in a response. |

### RoutePath class extension

Extensions to the `RoutePath` class to assist with common tasks.

## Methods

| Name            | Return Type | Description                                              |
|-----------------|-------------|----------------------------------------------------------|
| GetCoordinates() | Coordinate[] | Gets an array of coordinate objects for the route path. |
