* [Creating a Bing Maps key](#CreatingABingMapsKey)
* [Running the Samples](#RunningTheSamples)
* [Adding the Bing Maps REST Toolkit to your project](#AddingToolkitToProject)
* [How to make a Request to the REST services](#HowToMakeARequest)
* [QPS Limits](#QPSLimits)
* [Proxies](#Proxies)
* [Travelling Salesmen Problem](#TravellingSalesmen)

The Bing Maps REST services allow you to query the raw data that powers Bing Maps. The Bing Maps REST Services Toolkit provides an easy to use .NET wrapper around these services. To keep things easy the request and response classes in this library are aligned with the structure of the [documented Bing Maps REST Services API](https://msdn.microsoft.com/en-us/library/ff701713.aspx).

## <a name="CreatingABingMapsKey"></a> Creating a Bing Maps key

To use Bing Maps in your own application you will need a Bing Maps key. All Bing Maps map controls and services use a Bing Maps key for authentication. You can get a Bing Maps key in two ways:

**Through the Bing Maps Portal**

The [Bing Maps Account Center](http://www.bingmapsportal.com/) is the main portal where Bing Maps accounts can be managed and monitored. This is recommended if you are a Microsoft Volume license customer.

-   Go to <a href="https://www.bingmapsportal.com/" class="uri" class="uri">https://www.bingmapsportal.com/</a>

-   Press the sign in button and follow the steps to sign up.

-   Once logged in go to My Account -&gt; My Keys

-   Press the button to create a new key.

-   Add an application name and optionally a URL. It doesn’t matter what this is, it’s for your own information to help remember what the key is for. Set the key type to Basic, and the application type to public website. This will give you 125,000 transactions a year for free.

-   Now that you have a key, it can be used to access all the Bing Maps map controls and services.

**Through the Azure Marketplace**

If you are an Azure user, you can create a Bing Maps key through the [Azure marketplace](https://azure.microsoft.com/en-us/marketplace/partners/bingmaps/mapapis/). Going through the Azure Marketplace provides a bit more flexibility for licensing as you would license Bing Maps from month to month, rather than being locked into a 1 year+ contract. Additionally, smaller volumes of transactions can be purchased through the Azure marketplace than through Volume licensing. However, if your application does any of the following, you will want to create a Bing Maps account through the Bing Maps account center.

-   Your application will generate more than 500,000 transactions a month or you expect it to exceed this amount at some point. The Azure marketplace is limited to licensing this many transactions.

-   Your application will be used for used with real-time GPS positioned assets such as truck or personnel. This requires a Bing Maps account created through the Bing Maps account center.

-   If you want detailed usage reports. Currently the reporting functionality in the Azure marketplace is limited.

To find out about licensing options and learn about Bing Maps controls, please visit [www.microsoft.com/maps](http://www.microsoft.com/maps).

## <a name="RunningTheSamples"></a> Running the Samples

Download the complete project including the samples and the source code for the Bing Maps REST toolkit. All of the samples require a Bing Maps key to work. To add your Bing Maps key to the sample, open the **App.config** file of the sample and add it to the **BingMapsKey** property. This property likely has a placeholder value of **YOUR\_BING\_MAPS\_KEY**. Once this is done, simply run the sample in debug or release mode.

## <a name="AddingToolkitToProject"></a> Adding the Bing Maps REST Toolkit to your project

There are two options for adding the Bing Maps REST toolkit to your project.

**Use the NuGet Package**

The Bing Maps REST Services Toolkit is available as a [NuGet package](https://www.nuget.org/packages/BingMapsRESTToolkit). If using Visual Studio, open the **NuGet Package Manager**, select the **Browse** tab and search for "Bing Maps REST". This will reduce the list of results enough to find the "BingMapsRESTToolkit" package. If you want to verify you have the correct package, the listed owner of the package is bingmaps and the author is Microsoft.

Alternatively, if you are using the nuget command line:

> PM&gt; Install-Package BingMapsRESTToolkit

**Download the Source Code**

Download the source code and add the BingMapsRESTToolkit project to your solution. Once this is done you can a reference to it from your main project.

## <a name="HowToMakeARequest"></a> How to make a Request to the REST services

| Update | 
|--------|
| All requests classes now have an **Execute** method which can be used instead of the **ServiceManager** for all requests that return a response object to make things even easier. For requests that return and image stream, continue to use the **ServiceManager**. |

The Bing Maps REST Toolkit has two key components, a service manager and a set of request classes. The [ServiceManager](https://github.com/Microsoft/BingMapsRESTToolkit/blob/master/Docs/API%20Reference.md#ServiceManager) is a static class that makes it easy to asynchronously process any Bing Maps REST request which inherits from the [BaseRestReuqest](https://github.com/Microsoft/BingMapsRESTToolkit/blob/master/Docs/API%20Reference.md#BaseRestRequest) class. Here is a list of the different requests classes available:

- [**(NEW)** Time Zone API Request Classes](https://github.com/Microsoft/BingMapsRESTToolkit/blob/master/Docs/API%20Reference.md#TimeZoneAPI)

- [**(NEW**) LocationRecogRequest Class](https://github.com/Microsoft/BingMapsRESTToolkit/blob/master/Docs/API%20Reference.md#LocationRecogRequest) 

-   [DistanceMatrixRequest Class](https://github.com/Microsoft/BingMapsRESTToolkit/blob/master/Docs/API%20Reference.md#DistanceMatrixRequest)

-   [ElevationRequest Class](https://github.com/Microsoft/BingMapsRESTToolkit/blob/master/Docs/API%20Reference.md#ElevationRequest)

-   [GeocodeRequest Class](https://github.com/Microsoft/BingMapsRESTToolkit/blob/master/Docs/API%20Reference.md#GeocodeRequest)

-   [ImageryMetadataRequest Class](https://github.com/Microsoft/BingMapsRESTToolkit/blob/master/Docs/API%20Reference.md#ImageryMetadataRequest)

-   [ImageryRequest Class](https://github.com/Microsoft/BingMapsRESTToolkit/blob/master/Docs/API%20Reference.md#ImageryRequest)

-   [ReverseGeocodeRequest Class](https://github.com/Microsoft/BingMapsRESTToolkit/blob/master/Docs/API%20Reference.md#ReverseGeocodeRequest)

-   [RouteMajorRoadsRequest Class](https://github.com/Microsoft/BingMapsRESTToolkit/blob/master/Docs/API%20Reference.md#RouteMajorRoadsRequest)

-   [RouteRequest Class](https://github.com/Microsoft/BingMapsRESTToolkit/blob/master/Docs/API%20Reference.md#RouteRequest)

-   [TrafficRequest Class](https://github.com/Microsoft/BingMapsRESTToolkit/blob/master/Docs/API%20Reference.md#TrafficRequest)

The **ServiceManager** class has two static methods; **GetResponseAsync** and **GetImageAsync**. The **GetResponseAsync** method will return a Response object from the Bing Maps REST services which aligns with the documented Response object. The **GetImageAsync** method will return a stream containing the image data.

**Requesting a Response object**

The following is an example of how to make a geocode request and get the response from the Bing Maps REST services.

```Csharp
//Create a request.
var request = new GeocodeRequest()
{
    Query = "New York, NY",
    IncludeIso2 = true,
    IncludeNeighborhood = true,
    MaxResults = 25,
    BingMapsKey = "YOUR_BING_MAPS_KEY"
};

//Process the request by using the ServiceManager.
var response = await request.Execute();

if(response != null && 
    response.ResourceSets != null && 
    response.ResourceSets.Length > 0 && 
    response.ResourceSets[0].Resources != null && 
    response.ResourceSets[0].Resources.Length > 0)
{
    var result = response.ResourceSets[0].Resources[0] as BingMapsRESTToolkit.Location;

    //Do something with the result.
}
```

Here is the same code sample using the **ServiceManager**.

```Csharp
//Create a request.
var request = new GeocodeRequest()
{
    Query = "New York, NY",
    IncludeIso2 = true,
    IncludeNeighborhood = true,
    MaxResults = 25,
    BingMapsKey = "YOUR_BING_MAPS_KEY"
};

//Process the request by using the ServiceManager.
var response = await ServiceManager.GetResponseAsync(request);

if(response != null && 
    response.ResourceSets != null && 
    response.ResourceSets.Length > 0 && 
    response.ResourceSets[0].Resources != null && 
    response.ResourceSets[0].Resources.Length > 0)
{
    var result = response.ResourceSets[0].Resources[0] as BingMapsRESTToolkit.Location;

    //Do something with the result.
}
```

**Requesting an Image Result**

The following is an example of how to request a map image from the Bing Maps REST services to retrieve the image stream.

```Csharp
//Create an image request.
var request = new ImageryRequest()
{
    CenterPoint = new Coordinate(45, -110),
    ZoomLevel = 12,
    ImagerySet = ImageryType.AerialWithLabels,
    Pushpins = new List<ImageryPushpin>(){
        new ImageryPushpin(){
            Location = new Coordinate(45, -110.01),
            Label = "hi"
        },
        new ImageryPushpin(){
            Location = new Coordinate(45, -110.02),
            IconStyle = 3
        },
        new ImageryPushpin(){
            Location = new Coordinate(45, -110.03),
            IconStyle = 20
        },
        new ImageryPushpin(){
            Location = new Coordinate(45, -110.04),
            IconStyle = 24
        }
    },
    BingMapsKey = "YOUR_BING_MAPS_KEY"
};

//Process the request by using the ServiceManager.
using (var imageStream = await ServiceManager.GetImageAsync(request))
{
    //Do something with the image stream.

    //Here is how to display the image in an Image tag in a WPF app.
    var bitmapImage = new BitmapImage();
    bitmapImage.BeginInit();
    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
    bitmapImage.StreamSource = imageStream;
    bitmapImage.EndInit();
    MyImage.Source = bitmapImage;
}
```

## <a name="QPSLimits"></a> QPS Limits

The Bing Maps platform allows basic accounts to generate up to 5 QPS (queries per second) and Enterprise accounts 50 QPS (enterprise accounts can upgrade to higher QPS levels). This library has a few methods which will make multiple requests to the Bing Maps platform;

* The SimpleWaypoint class has a `TryGeocodeWaypoints` method which will geocode an array of SimpleWaypoints.
* The distance matrix API only accepts coordinates as waypoints, as such the `DistanceMatrixRequest` class will geocode all waypoints automatically if needed. Additionally, the distance matrix API does not support truck routing based matrices, but this library adds support by wrapping the truck routing service and making multiple requests to it. 

Since these methods can generate a lot of requests in a short period of time, a static `QpsLimit` property has been added the `ServiceManager` class which will allow you to fine tune this value as needed. By default this value is set to 5. To change this value to 50 (or some other value), add the following line of code before your request.

```
ServiceManager.QpsLimit = 50;
```

## <a name="Proxies"></a> Proxies

Some networks require requests to go through a proxy. The `ServiceManager` class has a static `Proxy` property which can be used to specify web proxy settings.

## <a name="TravellingSalesmen"></a> Travelling Salesmen Problem

This toolkit provides some classes which wrap the routing and distance matrix API's and provides solutions for the [travelling salesmen problem](https://en.wikipedia.org/wiki/Travelling_salesman_problem) (waypoint order optimization). You can optimize based on travel time, travel distance or straight line distance. When travel time/distance is specified, the distance matrix API which will generate Bing Maps transactions. For straight line distances, the haversine formula will be used to generate a matrix based on as-the-crow-flies distance between the waypoints. 

The travelling salesmen functionality is exposed in two ways; 
 - A TravellingSalesmen class which takes in a set of waypoints or a distance matrix and optimizes the waypoints. This uses a greedy algrithm when 10 or less waypoints are specified, and a genetic algorithm for larger waypoint sets. Here is an example of how to implement this:

 ```Csharp
 var tspResult = await TravellingSalesmen.Solve(new List<SimpleWaypoint>(){
        new SimpleWaypoint("Seattle, WA"),
        new SimpleWaypoint("Bellevue, WA"),
        new SimpleWaypoint("Redmond, WA"),
        new SimpleWaypoint("Kirkland, WA")
    }, TspOptimizationType.TravelTime);

//Do something with the results.
 ```

 - A WaypointOptimization option has been added to the RouteRequest class which, when set, will optimize the waypoints before calculating the requested route. Here is an example of how to implement this:

 ```Csharp
var routeRequest = new RouteRequest()
{
    Waypoints = new List<SimpleWaypoint>(){
        new SimpleWaypoint("Seattle, WA"),
        new SimpleWaypoint("Bellevue, WA"),
        new SimpleWaypoint("Redmond, WA"),
        new SimpleWaypoint("Kirkland, WA")
    },
    WaypointOptimization = TspOptimizationType.TravelTime,
    RouteOptions = new RouteOptions()
    {
        TravelMode = TravelModeType.Driving
    },
    BingMapsKey = BingMapsKey
};                

var response = await routeRequest.Execute();

//Do something with the route response which will be based on an optimized ordered set of waypoints.
 ```
