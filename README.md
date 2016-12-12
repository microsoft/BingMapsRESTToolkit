This is a portable .NET class library which provides a set of tools that make it easy to access the Bing Maps REST services in .NET based apps.

## Getting Started ##



## Features ##

* Uses HTTPS by default.
* Imagery Metadata allows HTTPS tile urls to be returned. ????
* Automatically encodes query parameters. A commonly overlooked stepped which greatly reduces th chances of invalid queries being sent to the service.
* Handles errors and rate limiting by catching exception and returning response with error message.
* Extentions added to make it easier to work with elevation data:
    * Method to get the coordinates that relate to each elevation data point.
	* Automatically determines if a POST request should be made.

## Supports Target Platforms ##

* .NET Framework 4.5+ 
* ASP.NET Core 1.0
* Universal Windows Platform (UWP) 
* Windows 8
* Windows 8.1
* Windows Phone 8.1
* Windows Phone 10
* Windows Phone Silverlight 8
* Windows Phone Silverlight 8.1
* Xamarin.Android
* Xamarin.iOS
* Xamarin.iOS (Classic)

## Additional Resources ##

* [Bing Maps REST Services MSDN Documentation](https://msdn.microsoft.com/en-us/library/ff701713.aspx)
* [Bing Maps MSDN Docuemntation](https://msdn.microsoft.com/en-us/library/dd877180.aspx)
* [Bing Maps Blog](http://blogs.bing.com/maps)
* [Bing Maps forums](https://social.msdn.microsoft.com/Forums/en-US/home?forum=bingmapsajax&filter=alltypes&sort=lastpostdesc)
* [Bing Maps for Enterpise site](https://www.microsoft.com/maps/)

## Contributing ##

We welcome contributions. Feel free to file issues and pull requests on the repo and we'll address them as we can. Learn more about how you can help on our [Contribution Rules & Guidelines](CONTRIBUTING.md). 

You can reach out to us anytime with questions and suggestions using our communities below:
* [MSDN Forums](https://social.msdn.microsoft.com/Forums/en-US/home?forum=bingmapsajax&filter=alltypes&sort=lastpostdesc)
* [StackOverflow](http://stackoverflow.com/questions/tagged/bing-maps)

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information, see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## License ##

MIT
 
See [License](LICENSE.md) for full license text.
