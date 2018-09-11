![Bing Maps Logo](https://github.com/Microsoft/Bing-Maps-V8-TypeScript-Definitions/blob/master/images/BingMapsLogoTeal.png)

[![NuGet](https://img.shields.io/badge/NuGet-1.1.4-blue.svg)](https://www.nuget.org/packages/BingMapsRESTToolkit)
[![license](https://img.shields.io/badge/license-MIT-yellow.svg)](https://github.com/Microsoft/BingMapsRESTToolkit/blob/master/LICENSE.md)

# Bing Maps REST Toolkit for .NET 

This is a portable .NET class library which provides a set of tools that make it easy to access the Bing Maps REST services in .NET based apps. Take a look at the [Getting Started documentation](https://github.com/Microsoft/BingMapsRESTToolkit/blob/master/Docs/Getting%20Started.md). The Bing Maps REST Services provides the following functionality:

* **Forward and reverse geocoding**
* **Route calculations** - for driving, walking, transit and truck
* **Distance matricies** - time and distance based matrices between a set of origins and destinations. Optionally retrieve this data over a period of time using predictive traffic data
* **Isochrones** (drive time polygons)
* **Snap to Road API** - snap GPS points to their closest logical point on a road. Also provides speed limit data
* **Traffic incident data**
* **Elevation data**
* **Static map imagery and metadata**

## What's New

* September 2018: Requests for [Location Recognition](https://msdn.microsoft.com/en-US/library/mt847173.aspx) and the [Time Zone API](https://msdn.microsoft.com/en-us/library/mt829726.aspx), including Data Contracts, are now available.

## Toolkit Features

* Uses HTTPS by default.
* Implements the documented [best practices for Bing Maps](https://msdn.microsoft.com/en-us/library/dn894107.aspx). For example, it automatically encodes query parameters. A commonly overlooked stepped which greatly reduces the chances of invalid queries being sent to the service.
* Handles errors and rate limiting by catching exception and returning response with error message.
* Automatically determines when a POST request should be made instead of a GET request.
* Fast indexed lookups of Distance Matrix results.
* Supports calculating driving, truck, walking and transit routes that have more than 25 waypoints.
* **Travelling Salesmen** algorithms that tie into the distance matrix API [documentation](https://github.com/Microsoft/BingMapsRESTToolkit/blob/master/Docs/Getting%20Started.md#TravellingSalesmen). 
* **Truck routing based Distance Matricies** - The Bing Maps distance matric API does not support truck routing based matricies. This library adds support for this by wrapping the truck routing API. 

## NuGet Package

The Bing Maps REST Services Toolkit is available as a [NuGet package](https://www.nuget.org/packages/BingMapsRESTToolkit). If using Visual Studio, open the nuget package manager, select the Browse tab and search for "Bing Maps REST". This should reduce the list of results enough to find the "BingMapsRESTToolkit" package. The owner of the package is bingmaps and the author is Microsoft.

Alternatively, if you are using the nuget command line:

> PM&gt; Install-Package BingMapsRESTToolkit

If you prefer to use the NuGet package manager interface, ere are the steps to add the Bing Maps REST toolkit to your project:

1. In Visual Studio select Tools -> Nuget Package Manager -> Managed Nuget Packages for Solution 
2. Select Browse tab and search for "BingMapsRESTToolkit".
3. Select the top result from Microsoft.
4. Check the projects you want to add it to then press install.

## Supported Platforms

* .NET Framework 4.5+ 
* .NET Standard 2.0+
* Universal Windows Platform (UWP) 
* Windows 8+
* Xamarin.Android
* Xamarin.iOS
* Xamarin.iOS (Classic)

## Contributing

We welcome contributions. Feel free to file issues and pull requests on the repo and we'll address them as we can. Learn more about how you can help on our [Contribution Rules & Guidelines](CONTRIBUTING.md). 

You can reach out to us anytime with questions and suggestions using our communities below:
* [MSDN Forums](https://social.msdn.microsoft.com/Forums/en-US/home?forum=bingmapsajax&filter=alltypes&sort=lastpostdesc)
* [StackOverflow](http://stackoverflow.com/questions/tagged/bing-maps)

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information, see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Related Projects

* [Bing Maps SDS Toolkit for .NET](https://github.com/Microsoft/BingMapsSDSToolkit)

## Additional Resources

* [Bing Maps REST Services MSDN Documentation](https://msdn.microsoft.com/en-us/library/ff701713.aspx)
* [Bing Maps MSDN Documentation](https://msdn.microsoft.com/en-us/library/dd877180.aspx)
* [Bing Maps Blog](http://blogs.bing.com/maps)
* [Bing Maps forums](https://social.msdn.microsoft.com/Forums/en-US/home?forum=bingmapsajax&filter=alltypes&sort=lastpostdesc)
* [Bing Maps for Enterprise site](https://www.microsoft.com/maps/)

## License 

MIT
 
See [License](LICENSE.md) for full license text.