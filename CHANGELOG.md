## Version 1.1.5 - 3/15/2022

* Added AutoSuggest, LocalSearch, LocalInsights, and OptimizeItineraryRequest APIs. Added samples for each.
* Added a visual sample for OptimizeItineraryRequest.
* Added support for regional travel summary in routes.
* Fixed backlog of issues and updates.
* Add helper methods to existing classes
	- SimpleWaypoint and Coordinate classes - static Parse method.
	- Resource class - static HasResource and GetFirstResouce methods.
	- RoutePath class - method to retrieve route path as an array of coordinates.

## Version 1.1.4 - 3/9/2018

* Async support added to SnapToRoadRequest, thus allowing up to 1,000 points to be snapped in a single request.
* Added additional check of Snap to Road requests to ensure distance between points is less than 2.5KM to align with documented limit.
* Add Optimize option to IsochroneRequests.

## Version 1.1.3 - 2/21/2018

* Fix globalization issue with coordinate to string conversion in route and imagery requests.

## Version 1.1.2 - 2/20/2018

* Modified Truck Routing and Isochrone requests modified to use Async requests when appropriate. 
* Added new Origin property to IsochroneResponse class.
* Extended the RouteRequest class so that it can support more than 25 waypoints for truck routes. It will simply break the request up into multiple sub-requests, process them, then merge the responses together. Tolerances are ignored. 
* Added static methods to the SimpleWaypoint class for easy geocoding individual and a list of SimpleWaypoints.
* Fix TSP optimized waypoint ordering for routes with less than 10 waypoints such that the first waypoint doesn't change.

## Version 1.1.1 - 2/1/2018

* .NET Standard Library support upgraded to v2.0.
* Static Proxy setting option added to ServiceManager.
* Truck routing based distance matrix support added (wraps truck routing service).
* QPS limiting setting added to ServiceManager. Used for batch geocode and truck routing calls which occur with distance matrix.

## Version 1.0.9 - 12/14/2017
 
* Add support for Truck Routing API.
* Add support for Isochrones API.
* Add support for Snap to Road API.
* Add Travelling Salesmen extension which includes 2 different TSP algorithms; greedy brute force and genetic approximation. 
* Extended the RouteRequest class to support waypoint optimization as part of the request.
* Added a WPF sample for the travelling salesmen which demostrates the ease of calculating a route with optimized waypoints.
* Extended DistanceMatrix class with method to get edge/path (array of waypoint indicies to pass through) time/distance for easier analysis.

## Version 1.0.8 - 10/23/2017

* Fix stack overflow issue when calculating short routes.

## Version 1.0.7 - 10/19/2017

* Extended the RouteRequest class so that it can support more than 25 waypoints for driving, walking and transit routes. It will simply break the request up into multiple sub-requests, process them, then merge the responses together. Tolerances are ignored. 
* Bug fix for Distance Matrix waypoint geocoding.

## Version 1.0.6 - 10/2/2017

* Created a .NET Standard v1.4 assembly.
* Add Custom Map Styles support for static images.
* Add Distance Matrix API support. Automatically determines is sync or async request is needed and monitors the request till completion. Distance matrix result class contains many help methods to make it easy to integrate into various applicaitons.
* Seperate out response classes into individual files for easier management.
* Add Execute method to all requests which contains custom business logic for processing each request. No need to use the ServiceManager now.
* Add new imagery types; AerialWithLabelsOnDemand, BirdseyeV2, BirdseyeV2WithLabels, CanvasGray, CanvasDark, CanvasLight, and RoadOnDemand.

## Version 1.0.5 - 3/22/2017 

* Add support for Geospatial Endpoint service.

## Version 1.0.4 - 2/16/2017 

* Added support for catching network related exceptions.

## Version 1.0.3 - 2/15/2017 

* Added Globalization support for coordinates.

## Version 1.0.2 - 2/8/2017 

* Removed DetailAddress class and replaced with the standard Address class.

## Version 1.0.1 - 1/30/2017 

* Added Imagery Providers to response classes. 
