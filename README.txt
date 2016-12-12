Please report any bugs or issues to Ricky Brundritt: richbrun@microsoft.com

Features:
---------

- Uses HTTPS by default.
- Imagery Metadata allows HTTPS tile urls to be returned.
- Automatically encodes query parameters.
- Handles errors and rate limiting by catching exception and returning response with error message.

ElevationRequest
	- Method to get the coordinates that relate to each elevation data point.
	- Automatically determines if a POST request should be made.
