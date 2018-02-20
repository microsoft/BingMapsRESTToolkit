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

using BingMapsRESTToolkit;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace RESTToolkitTestApp
{
    public partial class MainWindow : Window
    {
        #region Private Properties
        
        private string BingMapsKey = System.Configuration.ConfigurationManager.AppSettings.Get("BingMapsKey");

        private DispatcherTimer _timer;
        private TimeSpan _time;

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();

            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                if (_time != null)
                {
                    RequestProgressBarText.Text = string.Format("Time remaining: {0}", _time);

                    if (_time == TimeSpan.Zero)
                    {
                        _timer.Stop();
                    }

                    _time = _time.Add(TimeSpan.FromSeconds(-1));
                }
            }, Application.Current.Dispatcher);
        }

        #endregion

        #region Example Requests

        /// <summary>
        /// Demostrates how to make a Geocode Request.
        /// </summary>
        private void GeocodeBtn_Clicked(object sender, RoutedEventArgs e)
        {
            var r = new GeocodeRequest()
            {
                Query = "66-46 74th St, Middle Village, NY 11379",
                IncludeIso2 = true,
                IncludeNeighborhood = true,
                MaxResults = 25,
                BingMapsKey = BingMapsKey
            };

            ProcessRequest(r);
        }

        /// <summary>
        /// Demostrates how to make a Reverse Geocode Request.
        /// </summary>
        private void ReverseGeocodeBtn_Clicked(object sender, RoutedEventArgs e)
        {
            var r = new ReverseGeocodeRequest()
            {
                Point = new Coordinate(45, -110),
                IncludeEntityTypes = new List<EntityType>(){
                    EntityType.AdminDivision1,
                    EntityType.CountryRegion
                },
                IncludeNeighborhood = true,
                IncludeIso2 = true,
                BingMapsKey = BingMapsKey
            };

            ProcessRequest(r);
        }
       
        /// <summary>
        /// Demostrates how to make an Elevation Request.
        /// </summary>
        private void ElevationBtn_Clicked(object sender, RoutedEventArgs e)
        {
            var r = new ElevationRequest()
            {
                Points = new List<Coordinate>(){
                    new Coordinate(45, -100),
                    new Coordinate(50, -100),
                    new Coordinate(45, -110)
                },
                Samples = 1024,
                BingMapsKey = BingMapsKey
            };

            ProcessRequest(r);
        }

        /// <summary>
        /// Demostrates how to make a Driving Route Request.
        /// </summary>
        private void RouteBtn_Clicked(object sender, RoutedEventArgs e)
        {
            var r = new RouteRequest()
            {
                RouteOptions = new RouteOptions(){
                    Avoid = new List<AvoidType>()
                    {
                        AvoidType.MinimizeTolls
                    },
                    TravelMode = TravelModeType.Driving,
                    DistanceUnits = DistanceUnitType.Miles,
                    Heading = 45,
                    RouteAttributes = new List<RouteAttributeType>()
                    {
                        RouteAttributeType.RoutePath
                    },
                    Optimize = RouteOptimizationType.TimeWithTraffic
                },
                Waypoints = new List<SimpleWaypoint>()
                {
                    new SimpleWaypoint(){
                        Address = "Seattle, WA"
                    },
                    new SimpleWaypoint(){
                        Address = "Bellevue, WA",
                        IsViaPoint = true
                    },
                    new SimpleWaypoint(){
                        Address = "Redmond, WA"
                    }
                },
                BingMapsKey = BingMapsKey
            };

            ProcessRequest(r);
        }

        /// <summary>
        /// Demostrates how to make a Driving Route Request that has more than 25 waypoints.
        /// </summary>
        private void LongRouteBtn_Clicked(object sender, RoutedEventArgs e)
        {
            var r = new RouteRequest()
            {
                RouteOptions = new RouteOptions()
                {
                    Avoid = new List<AvoidType>()
                    {
                        AvoidType.MinimizeTolls
                    },
                    TravelMode = TravelModeType.Driving,
                    DistanceUnits = DistanceUnitType.Miles,
                    Heading = 45,
                    RouteAttributes = new List<RouteAttributeType>()
                    {
                        RouteAttributeType.RoutePath
                    },
                    Optimize = RouteOptimizationType.TimeWithTraffic
                },
                Waypoints = new List<SimpleWaypoint>() //29 waypoints, more than what the routing service normally handles, so the request will break this up into two requests and merge the results.
                {
                    new SimpleWaypoint(47.5886, -122.336),
                    new SimpleWaypoint(47.5553, -122.334),
                    new SimpleWaypoint(47.5557, -122.316),
                    new SimpleWaypoint(47.5428, -122.322),
                    new SimpleWaypoint(47.5425, -122.341),
                    new SimpleWaypoint(47.5538, -122.362),
                    new SimpleWaypoint(47.5647, -122.384),
                    new SimpleWaypoint(47.5309, -122.380),
                    new SimpleWaypoint(47.5261, -122.351),
                    new SimpleWaypoint(47.5137, -122.382),
                    new SimpleWaypoint(47.5101, -122.337),
                    new SimpleWaypoint(47.4901, -122.341),
                    new SimpleWaypoint(47.4850, -122.320),
                    new SimpleWaypoint(47.5024, -122.263),
                    new SimpleWaypoint(47.4970, -122.226),
                    new SimpleWaypoint(47.4736, -122.265),
                    new SimpleWaypoint(47.4562, -122.287),
                    new SimpleWaypoint(47.4452, -122.338),
                    new SimpleWaypoint(47.4237, -122.292),
                    new SimpleWaypoint(47.4230, -122.257),
                    new SimpleWaypoint(47.3974, -122.249),
                    new SimpleWaypoint(47.3765, -122.277),
                    new SimpleWaypoint(47.3459, -122.302),
                    new SimpleWaypoint(47.3073, -122.280),
                    new SimpleWaypoint(47.3115, -122.228),
                    new SimpleWaypoint(47.2862, -122.218),
                    new SimpleWaypoint(47.2714, -122.294),
                    new SimpleWaypoint(47.2353, -122.306),
                    new SimpleWaypoint(47.1912, -122.408)
                },
                BingMapsKey = BingMapsKey
            };

            ProcessRequest(r);

            RequestUrlTbx.Text = "Request broken up into multiple sub-requests.";
        }

        /// <summary>
        /// Demostrates how to make a Transit Route Request.
        /// </summary>
        private void TransitRouteBtn_Clicked(object sender, RoutedEventArgs e)
        {
            var r = new RouteRequest()
            {
                RouteOptions = new RouteOptions()
                {
                    TravelMode = TravelModeType.Transit,
                    DistanceUnits = DistanceUnitType.Miles,
                    RouteAttributes = new List<RouteAttributeType>()
                    {
                        RouteAttributeType.RoutePath,
                        RouteAttributeType.TransitStops
                    },
                    Optimize = RouteOptimizationType.TimeAvoidClosure,
                    DateTime = DateTime.Now,
                    TimeType = RouteTimeType.Departure
                },
                Waypoints = new List<SimpleWaypoint>()
                {
                    new SimpleWaypoint(){
                        Address = "London, UK"
                    },
                    new SimpleWaypoint(){
                        Address = "E14 3SP"
                    }
                },
                BingMapsKey = BingMapsKey
            };

            ProcessRequest(r);
        }

        /// <summary>
        /// Demostrates how to make a Truck Routing Request.
        /// </summary>
        private void TruckRouteBtn_Clicked(object sender, RoutedEventArgs e)
        {
            var r = new RouteRequest()
            {
                RouteOptions = new RouteOptions()
                {
                    Avoid = new List<AvoidType>()
                    {
                        AvoidType.MinimizeTolls
                    },
                    TravelMode = TravelModeType.Truck,
                    DistanceUnits = DistanceUnitType.Miles,
                    Heading = 45,
                    RouteAttributes = new List<RouteAttributeType>()
                    {
                        RouteAttributeType.RoutePath
                    },
                    Optimize = RouteOptimizationType.Time,
                    VehicleSpec = new VehicleSpec()
                    {
                        VehicleWidth = 3,
                        VehicleHeight = 5,
                        DimensionUnit = DimensionUnitType.Meter,
                        VehicleWeight = 15000,
                        WeightUnit = WeightUnitType.Pound,
                        VehicleHazardousMaterials = new List<HazardousMaterialType>()
                        {
                            HazardousMaterialType.Combustable,
                            HazardousMaterialType.Flammable
                        }
                    }
                },
                Waypoints = new List<SimpleWaypoint>()
                {
                    new SimpleWaypoint(){
                        Address = "Seattle, WA"
                    },
                     new SimpleWaypoint(){
                        Address = "Bellevue, WA",
                        IsViaPoint = true
                    },
                    new SimpleWaypoint(){
                        Address = "Redmond, WA"
                    }
                },
                BingMapsKey = BingMapsKey
            };

            ProcessRequest(r);
        }

        /// <summary>
        /// Demostrates how to make a Traffic Request.
        /// </summary>
        private void TrafficBtn_Clicked(object sender, RoutedEventArgs e)
        {
            var r = new TrafficRequest()
            {
                Culture = "en-US",
                TrafficType = new List<TrafficType>()
                {
                    TrafficType.Accident,
                    TrafficType.Congestion
                },
                //Severity = new List<SeverityType>()
                //{
                //    SeverityType.LowImpact,
                //    SeverityType.Minor
                //},
                MapArea = new BoundingBox()
                {
                    SouthLatitude = 46,
                    WestLongitude = -124,
                    NorthLatitude = 50,
                    EastLongitude = -117
                },
                IncludeLocationCodes = true,
                BingMapsKey = BingMapsKey
            };

            ProcessRequest(r);
        }

        /// <summary>
        /// Demostrates how to make an Imagery Metadata Request.
        /// </summary>
        private void ImageMetadataBtn_Clicked(object sender, RoutedEventArgs e)
        {
            var r = new ImageryMetadataRequest()
            {
                CenterPoint = new Coordinate(45, -110),
                ZoomLevel = 12,
                ImagerySet = ImageryType.AerialWithLabels,
                BingMapsKey = BingMapsKey
            };

            ProcessRequest(r);
        }

        /// <summary>
        /// Demostrates how to make a Static Imagery Metadata Request.
        /// </summary>
        private void StaticImageMetadataBtn_Clicked(object sender, RoutedEventArgs e)
        {
            var r = new ImageryRequest()
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
                BingMapsKey = BingMapsKey
            };

            ProcessRequest(r);
        }

        /// <summary>
        /// Demostrates how to make a Static Imagery Request.
        /// </summary>
        private void StaticImageBtn_Clicked(object sender, RoutedEventArgs e)
        {
            var r = new ImageryRequest()
            {
                CenterPoint = new Coordinate(45, -110),
                ZoomLevel = 1,
                ImagerySet = ImageryType.RoadOnDemand,
                Pushpins = new List<ImageryPushpin>(){
                    new ImageryPushpin(){
                        Location = new Coordinate(45, -110.01),
                        Label = "hi"
                    },
                    new ImageryPushpin(){
                        Location = new Coordinate(30, -100),
                        IconStyle = 3
                    },
                    new ImageryPushpin(){
                        Location = new Coordinate(25, -80),
                        IconStyle = 20
                    },
                    new ImageryPushpin(){
                        Location = new Coordinate(33, -75),
                        IconStyle = 24
                    }
                },
                BingMapsKey = BingMapsKey,
                Style = @"{
	                ""version"": ""1.*"",
	                ""settings"": {
		                ""landColor"": ""#0B334D""
	                },
	                ""elements"": {
		                ""mapElement"": {
			                ""labelColor"": ""#FFFFFF"",
			                ""labelOutlineColor"": ""#000000""
		                },
		                ""political"": {
			                ""borderStrokeColor"": ""#144B53"",
			                ""borderOutlineColor"": ""#00000000""
		                },
		                ""point"": {
			                ""iconColor"": ""#0C4152"",
			                ""fillColor"": ""#000000"",
			                ""strokeColor"": ""#0C4152""
                        },
		                ""transportation"": {
			                ""strokeColor"": ""#000000"",
			                ""fillColor"": ""#000000""
		                },
		                ""highway"": {
			                ""strokeColor"": ""#158399"",
			                ""fillColor"": ""#000000""
		                },
		                ""controlledAccessHighway"": {
			                ""strokeColor"": ""#158399"",
			                ""fillColor"": ""#000000""
		                },
		                ""arterialRoad"": {
			                ""strokeColor"": ""#157399"",
			                ""fillColor"": ""#000000""
		                },
		                ""majorRoad"": {
			                ""strokeColor"": ""#157399"",
			                ""fillColor"": ""#000000""
		                },
		                ""railway"": {
			                ""strokeColor"": ""#146474"",
			                ""fillColor"": ""#000000""
		                },
		                ""structure"": {
			                ""fillColor"": ""#115166""
		                },
		                ""water"": {
			                ""fillColor"": ""#021019""
		                },
		                ""area"": {
			                ""fillColor"": ""#115166""
		                }
	                }
                }"
            };

            ProcessImageRequest(r);           
        }

        /// <summary>
        /// Demostrates how to make a Geospatial Endpoint Request.
        /// </summary>
        private void GeospatialEndpointBtn_Clicked(object sender, RoutedEventArgs e)
        {
            var r = new GeospatialEndpointRequest()
            {
                //Language = "zh-CN",
                Culture = "zh-CN",
                UserRegion = "CN",
                UserLocation = new Coordinate(40, 116),
                BingMapsKey = BingMapsKey
            };

            ProcessRequest(r);
        }

        /// <summary>
        /// Demostrates how to make a Distance Matrix Request.
        /// </summary>
        private void DistanceMatrixBtn_Clicked(object sender, RoutedEventArgs e)
        {
            var r = new DistanceMatrixRequest()
            {
                Origins = new List<SimpleWaypoint>()
                {
                    new SimpleWaypoint(47.6044, -122.3345),
                    new SimpleWaypoint(47.6731, -122.1185),
                    new SimpleWaypoint(47.6149, -122.1936)
                },
                Destinations = new List<SimpleWaypoint>()
                {
                    new SimpleWaypoint(45.5347, -122.6231),
                    new SimpleWaypoint(47.4747, -122.2057),
                },
                BingMapsKey = BingMapsKey,
                TimeUnits = TimeUnitType.Minute,
                DistanceUnits = DistanceUnitType.Miles,
                TravelMode = TravelModeType.Driving
            };

            ProcessRequest(r);
        }

        /// <summary>
        /// Demostrates how to make a Distance Matrix Histogram Request.
        /// </summary>
        private void DistanceMatrixHistogramBtn_Clicked(object sender, RoutedEventArgs e)
        {
            var r = new DistanceMatrixRequest()
            {
                Origins = new List<SimpleWaypoint>()
                {
                    new SimpleWaypoint(47.6044, -122.3345),
                    new SimpleWaypoint(47.6731, -122.1185),
                    new SimpleWaypoint(47.6149, -122.1936)
                },
                Destinations = new List<SimpleWaypoint>()
                {
                    new SimpleWaypoint(45.5347, -122.6231),
                    new SimpleWaypoint(47.4747, -122.2057)
                },
                BingMapsKey = BingMapsKey,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(8),
                Resolution = 4
            };

            ProcessRequest(r);
        }

        /// <summary>
        /// Demostrates how to make an Isochrone Request.
        /// </summary>
        private void IsochroneBtn_Clicked(object sender, RoutedEventArgs e)
        {
            var r = new IsochroneRequest()
            {
                Waypoint = new SimpleWaypoint("Bellevue, WA"),
                MaxTime = 60,
                TimeUnit = TimeUnitType.Minute,
                DateTime = DateTime.Now.AddMinutes(15),
                TravelMode = TravelModeType.Driving,
                BingMapsKey = BingMapsKey
            };

            ProcessRequest(r);
        }

        /// <summary>
        /// Demostrates how to make an Snap to Road Request.
        /// </summary>
        private void SnapToRoadBtn_Clicked(object sender, RoutedEventArgs e)
        {
            var r = new SnapToRoadRequest()
            {
                Points = new List<Coordinate>()
                {
                    new Coordinate(47.590868, -122.336729),
                    new Coordinate(47.601604, -122.336042),
                    new Coordinate(47.60849, -122.34241),
                    new Coordinate(47.610568, -122.345064)
                },
                IncludeSpeedLimit = true,
                IncludeTruckSpeedLimit = true,
                Interpolate = true,
                SpeedUnit = SpeedUnitType.MPH,
                TravelMode = TravelModeType.Driving,
                BingMapsKey = BingMapsKey
            };

            ProcessRequest(r);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method has a lot of logic that is specific to the sample. To process a request you can easily just call the Execute method on the request.
        /// </summary>
        /// <param name="request"></param>
        private async void ProcessRequest(BaseRestRequest request)
        {
            try
            {
                RequestProgressBar.Visibility = Visibility.Visible;
                RequestProgressBarText.Text = string.Empty;

                ResultTreeView.ItemsSource = null;

                var start = DateTime.Now;

                //Execute the request.
                var response = await request.Execute((remainingTime) =>
                {
                    if (remainingTime > -1)
                    {
                        _time = TimeSpan.FromSeconds(remainingTime);

                        RequestProgressBarText.Text = string.Format("Time remaining {0} ", _time);
                        
                        _timer.Start();
                    }
                });

                RequestUrlTbx.Text = request.GetRequestUrl();

                var end = DateTime.Now;

                var processingTime = end - start;

                ProcessingTimeTbx.Text = string.Format(CultureInfo.InvariantCulture, "{0:0} ms", processingTime.TotalMilliseconds);
                RequestProgressBar.Visibility = Visibility.Collapsed;

                var nodes = new List<ObjectNode>();
                var tree = await ObjectNode.ParseAsync("result", response);
                nodes.Add(tree);
                ResultTreeView.ItemsSource = nodes;

                ResponseTab.IsSelected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            _timer.Stop();
            RequestProgressBar.Visibility = Visibility.Collapsed;
        }

        private async void ProcessImageRequest(BaseImageryRestRequest imageRequest)
        {
            try
            {
                RequestProgressBar.Visibility = Visibility.Visible;
                RequestProgressBarText.Text = string.Empty;

                RequestUrlTbx.Text = imageRequest.GetRequestUrl();

                //Process the request by using the ServiceManager.
                using (var s = await ServiceManager.GetImageAsync(imageRequest))
                {
                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = s;
                    bitmapImage.EndInit();
                    ImageBox.Source = bitmapImage;
                }

                ImageTab.IsSelected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            RequestProgressBar.Visibility = Visibility.Collapsed;
        }

        private void ExpandTree_Clicked(object sender, RoutedEventArgs e)
        {
            foreach (var item in ResultTreeView.Items)
            {
                DependencyObject dObject = ResultTreeView.ItemContainerGenerator.ContainerFromItem(item);
                ((TreeViewItem)dObject).ExpandSubtree();
            }
        }

        private void CollapseTree_Clicked(object sender, RoutedEventArgs e)
        {
            foreach (var item in ResultTreeView.Items)
            {
                DependencyObject dObject = ResultTreeView.ItemContainerGenerator.ContainerFromItem(item);
                CollapseTreeviewItems(((TreeViewItem)dObject));
            }
        }

        private void CollapseTreeviewItems(TreeViewItem Item)
        {
            Item.IsExpanded = false;

            foreach (var item in Item.Items)
            {
                DependencyObject dObject = ResultTreeView.ItemContainerGenerator.ContainerFromItem(item);

                if (dObject != null)
                {
                    ((TreeViewItem)dObject).IsExpanded = false;

                    if (((TreeViewItem)dObject).HasItems)
                    {
                        CollapseTreeviewItems(((TreeViewItem)dObject));
                    }
                }
            }
        }

        #endregion
    }
}
