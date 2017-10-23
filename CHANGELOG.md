## Version 1.0.8

* Fix stack overflow issue when calculating short routes.

## Version 1.0.7

* Extended the RouteRequest class so that it can support more than 25 waypoints. It will simply break the request up into multiple sub-requests, process them, then merge the responses together.
* Bug fix for Distance Matrix waypoint geocoding.

## Version 1.0.6 

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
