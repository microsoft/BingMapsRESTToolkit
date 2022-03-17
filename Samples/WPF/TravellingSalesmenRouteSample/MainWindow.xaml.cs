using BingMapsRESTToolkit;
using BingMapsRESTToolkit.Extensions;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TravellingSalesmenRouteSample
{
    public partial class MainWindow : Window
    {
        #region Private Properties

        private string BingMapsKey = System.Configuration.ConfigurationManager.AppSettings.Get("BingMapsKey");

        private string SessionKey;

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            MyMap.CredentialsProvider = new ApplicationIdCredentialsProvider(BingMapsKey);
            MyMap.CredentialsProvider.GetCredentials((c) =>
            {
                SessionKey = c.ApplicationId;
            });

            //Add some sample locations to the input panel.
            InputTbx.Text = "Seattle, WA\r\nRedmond, WA\r\nBellevue, WA\r\n47.532122, -122.042934\r\nEverett, WA\r\nTacoma, WA\r\nKirkland, WA\r\nSammamish, WA\r\nLynnwood, WA\r\nRenton, WA\r\nDuvall, WA\r\nMonroe, WA\r\nSummer, WA";
        }
        
        private async void CalculateRouteBtn_Clicked(object sender, RoutedEventArgs e)
        {
            MyMap.Children.Clear();
            OutputTbx.Text = string.Empty;
            LoadingBar.Visibility = Visibility.Visible;

            var waypoints = GetWaypoints();

            if (waypoints.Count < 2)
            {
                MessageBox.Show("Need a minimum of 2 waypoints to calculate a route.");
                return;
            }

            var travelMode = (TravelModeType)Enum.Parse(typeof(TravelModeType), (string)(TravelModeTypeCbx.SelectedItem as ComboBoxItem).Content);
            var tspOptimization = (TspOptimizationType)Enum.Parse(typeof(TspOptimizationType), (string)(TspOptimizationTypeCbx.SelectedItem as ComboBoxItem).Tag);
            try
            {
                //Calculate a route between the waypoints so we can draw the path on the map. 
                var routeRequest = new RouteRequest()
                {
                    Waypoints = waypoints,

                    //Specify that we want the route to be optimized.
                    WaypointOptimization = tspOptimization,

                    RouteOptions = new RouteOptions()
                    {
                        TravelMode = travelMode,
                        RouteAttributes = new List<RouteAttributeType>()
                        {
                            RouteAttributeType.RoutePath,
                            RouteAttributeType.ExcludeItinerary
                        }
                    },

                    //When straight line distances are used, the distance matrix API is not used, so a session key can be used.
                    BingMapsKey = (tspOptimization == TspOptimizationType.StraightLineDistance)? SessionKey : BingMapsKey
                };                

                //Only use traffic based routing when travel mode is driving.
                if(routeRequest.RouteOptions.TravelMode != TravelModeType.Driving)
                {
                    routeRequest.RouteOptions.Optimize = RouteOptimizationType.Time;
                }
                else
                {
                    routeRequest.RouteOptions.Optimize = RouteOptimizationType.TimeWithTraffic;
                }

                var r = await routeRequest.Execute();

                RenderRouteResponse(routeRequest, r);                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            
            LoadingBar.Visibility = Visibility.Collapsed;
        }

        #region Private Methods

        /// <summary>
        /// Renders a route response on the map.
        /// </summary>
        private void RenderRouteResponse(RouteRequest routeRequest, Response response)
        {
            //Render the route on the map.
            if (response != null && response.ResourceSets != null && response.ResourceSets.Length > 0 &&
               response.ResourceSets[0].Resources != null && response.ResourceSets[0].Resources.Length > 0
               && response.ResourceSets[0].Resources[0] is Route)
            {
                var route = response.ResourceSets[0].Resources[0] as Route;

                var timeSpan = new TimeSpan(0, 0, (int)Math.Round(route.TravelDurationTraffic));

                if (timeSpan.Days > 0)
                {
                    OutputTbx.Text = string.Format("Travel Time: {3} days {0} hr {1} min {2} sec\r\n", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Days);
                }
                else
                {
                    OutputTbx.Text = string.Format("Travel Time: {0} hr {1} min {2} sec\r\n", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
                }

                var routeLine = route.RoutePath.Line.Coordinates;
                var routePath = new LocationCollection();

                for (int i = 0; i < routeLine.Length; i++)
                {
                    routePath.Add(new Microsoft.Maps.MapControl.WPF.Location(routeLine[i][0], routeLine[i][1]));
                }

                var routePolyline = new MapPolyline()
                {
                    Locations = routePath,
                    Stroke = new SolidColorBrush(Colors.Red),
                    StrokeThickness = 3
                };

                MyMap.Children.Add(routePolyline);

                var locs = new List<Microsoft.Maps.MapControl.WPF.Location>();

                //Create pushpins for the optimized waypoints.
                //The waypoints in the request were optimized for us.
                for (var i = 0; i < routeRequest.Waypoints.Count; i++)
                {
                    var loc = new Microsoft.Maps.MapControl.WPF.Location(routeRequest.Waypoints[i].Coordinate.Latitude, routeRequest.Waypoints[i].Coordinate.Longitude);

                    //Only render the last waypoint when it is not a round trip.
                    if (i < routeRequest.Waypoints.Count - 1)
                    {
                        MyMap.Children.Add(new Pushpin()
                        {
                            Location = loc,
                            Content = i
                        });
                    }

                    locs.Add(loc);
                }

                MyMap.SetView(locs, new Thickness(50), 0);
            }
            else if (response != null && response.ErrorDetails != null && response.ErrorDetails.Length > 0)
            {
                throw new Exception(String.Join("", response.ErrorDetails));
            }
        }
        
        /// <summary>
        /// Gets the inputted waypoints.
        /// </summary>
        private List<SimpleWaypoint> GetWaypoints()
        {
            var places = InputTbx.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var waypoints = new List<SimpleWaypoint>();

            foreach (var p in places)
            {
                if (!string.IsNullOrWhiteSpace(p))
                {
                    waypoints.Add(SimpleWaypoint.Parse(p));
                }
            }

            return waypoints;
        }

        #endregion
    }
}
