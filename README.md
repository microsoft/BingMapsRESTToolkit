![Bing Maps Logo](https://github.com/Microsoft/Bing-Maps-V8-TypeScript-Definitions/blob/master/images/BingMapsLogoTeal.png)

[![NuGet](https://img.shields.io/badge/NuGet-1.0.1-blue.svg)](https://www.nuget.org/packages/BingMapsRESTToolkit)
[![license](https://img.shields.io/badge/license-MIT-yellow.svg)](https://github.com/Microsoft/BingMapsRESTToolkit/blob/master/LICENSE.md)

# Bing Maps REST Toolkit for .NET #

This is a portable .NET class library which provides a set of tools that make it easy to access the Bing Maps REST services in .NET based apps. Take a look at the [Getting Started documentation](https://github.com/Microsoft/BingMapsRESTToolkit/wiki/Getting-Started).

## Features ##

* Uses HTTPS by default.
* Implements the documented [best practices for Bing Maps](https://msdn.microsoft.com/en-us/library/dn894107.aspx). For example, it automatically encodes query parameters. A commonly overlooked stepped which greatly reduces the chances of invalid queries being sent to the service.
* Handles errors and rate limiting by catching exception and returning response with error message.
* Automatically determines when a POST request should be made instead of a GET request.

## NuGet Package ##

The Bing Maps REST Services Toolkit is available as a [NuGet package](https://www.nuget.org/packages/BingMapsRESTToolkit). If using Visual Studio, open the nuget package manager, select the Browse tab and search for "Bing Maps REST". This should reduce the list of results enough to find the "BingMapsRESTToolkit" package. The owner of the package is bingmaps and the author is Microsoft.

Alternatively, if you are using the nuget command line:

> PM&gt; Install-Package BingMapsRESTToolkit

## Supported Platforms ##

* .NET Framework 4.5+ 
* ASP.NET Core 1.0
* Universal Windows Platform (UWP) 
* Windows 10
* Windows 8.1
* Windows 8
* Windows Phone 10
* Windows Phone 8.1
* Windows Phone Silverlight 8.1
* Windows Phone Silverlight 8
* Xamarin.Android
* Xamarin.iOS
* Xamarin.iOS (Classic)

## Contributing ##

We welcome contributions. Feel free to file issues and pull requests on the repo and we'll address them as we can. Learn more about how you can help on our [Contribution Rules & Guidelines](CONTRIBUTING.md). 

You can reach out to us anytime with questions and suggestions using our communities below:
* [MSDN Forums](https://social.msdn.microsoft.com/Forums/en-US/home?forum=bingmapsajax&filter=alltypes&sort=lastpostdesc)
* [StackOverflow](http://stackoverflow.com/questions/tagged/bing-maps)

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information, see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## License ##

MIT
 
See [License](LICENSE.md) for full license text.

## Additional Resources ##

* [Bing Maps REST Services MSDN Documentation](https://msdn.microsoft.com/en-us/library/ff701713.aspx)
* [Bing Maps MSDN Docuemntation](https://msdn.microsoft.com/en-us/library/dd877180.aspx)
* [Bing Maps Blog](http://blogs.bing.com/maps)
* [Bing Maps forums](https://social.msdn.microsoft.com/Forums/en-US/home?forum=bingmapsajax&filter=alltypes&sort=lastpostdesc)
* [Bing Maps for Enterprise site](https://www.microsoft.com/maps/)
