Documentation
==============

## Table of Contents ##

* [API Reference](#ApiReference) 
  - [ServiceManager Class](#ServiceManager) 
  - [BaseImageryRestRequest Class](#BaseImageryRestRequest) 
  - [BaseRestRequest Class](#BaseRestRequest) 
  - [BaseRestRequest Class](#BaseRestRequest) 
 
 
## <a name="ApiReference"></a> API Reference ##

### <a name="ServiceManager"></a> ServiceManager Class ###
 
A static class that processes requests to the Bing Maps REST services.

Name                                                 | Returns  | Description
-----------------------------------------------------|----------|--------------------------------------------------------
GetResponseAsync(BaseRestRequest request)            | Response | Processes a REST request that returns a data response.
GetImageAsync(BaseImageryRestRequest imageryRequest) | Stream   | Processes a REST request that returns an image stream.


### <a name="BaseRestRequest"></a> BaseRestRequest Class ###

An abstract class in which all REST service requests derive from.

#### Constants ####

Name           |   Type   | Description
---------------|----------|--------------------------------------------------------
baseServiceUrl | string   | 


#### Properties ####

Name         |   Type      | Description
-------------|-------------|--------------------------------------------------------
BingMapsKey  | string      | The Bing Maps key for making the request.
Culture      | string      | The culture to use for the request.
UserMapView  | BoundingBox | The geographic region that corresponds to the current viewport.
UserLocation | Coordinate  | The user’s current position.
UserIp       | string      | An Internet Protocol version 4 (IPv4) address.


### <a name="BaseImageryRestRequest"></a> BaseImageryRestRequest Class ###