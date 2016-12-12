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
using System.Windows;
using System.Windows.Media.Imaging;

namespace RESTToolkitTestApp
{
    public partial class MainWindow : Window
    {
        #region Private Properties
        
        private string BingMapsKey = System.Configuration.ConfigurationManager.AppSettings.Get("BingMapsKey");

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
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
                Query = "New York, NY",
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
                    DateTime = new DateTime(2016, 6, 30, 8, 0, 0, DateTimeKind.Utc),
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

            ProcessImageRequest(r);           
        }

        #endregion

        #region Private Methods


        private async void ProcessRequest(BaseRestRequest request)
        {
            try
            {
                ProcessingTimeTbx.Text = "";
                ResultTreeView.ItemsSource = null;

                RequestUrlTbx.Text = request.GetRequestUrl();

                var start = DateTime.Now;

                //Process the request by using the ServiceManager.
                var response = await ServiceManager.GetResponseAsync(request);

                var end = DateTime.Now;

                var processingTime = end - start;

                ProcessingTimeTbx.Text = string.Format("{0:0} ms", processingTime.TotalMilliseconds);

                List<ObjectNode> nodes = new List<ObjectNode>();
                nodes.Add(new ObjectNode("result", response));
                ResultTreeView.ItemsSource = nodes;

                ResponseTab.IsSelected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void ProcessImageRequest(BaseImageryRestRequest imageRequest)
        {
            try
            {
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
        }

        #endregion
    }
}
