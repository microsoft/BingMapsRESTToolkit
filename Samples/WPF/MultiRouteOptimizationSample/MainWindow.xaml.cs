using BingMapsRESTToolkit;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MultiRouteOptimizationSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Properties

        private string BingMapsKey = System.Configuration.ConfigurationManager.AppSettings.Get("BingMapsKey");

        OptimizeItineraryRequest request = new OptimizeItineraryRequest()
        {
            Agents = new List<Agent>()
            {
                new Agent()
                {
                    Name = "Bob",
                    Shifts = new List<Shift>()
                    {
                        new Shift()
                        {
                            StartTimeUtc = new DateTime(2022, 1, 1, 8, 0, 0), //8 am
                            StartLocation = new SimpleWaypoint("1603 NW 89th St, Seattle, WA 98117, US"),
                            EndTimeUtc = new DateTime(2022, 1, 1, 18, 0, 0), //6pm
                            EndLocation = new SimpleWaypoint(47.7070790545669, -122.355226696231),
                            Breaks = new Break[]
                            {
                                new Break()
                                {
                                    StartTimeUtc = new DateTime(2022, 1, 1, 12, 0, 0), //12pm/noon
                                    EndTimeUtc = new DateTime(2022, 1, 1, 14, 0, 0),   //2pm
                                    DurationTimeSpan = new TimeSpan(0, 30, 0) //30 minutes.
                                },
                                new Break()
                                {
                                    StartTimeUtc = new DateTime(2022, 1, 1, 16, 0, 0), //4pm
                                    EndTimeUtc = new DateTime(2022, 1, 1, 16, 30, 0)   //4:30pm
                                }
                            }
                        }
                    },
                    Price = new Price()
                    {
                        FixedPrice = 100,
                        PricePerHour = 5,
                        PricePerKM = 1
                    },
                    Capacity = new int[] { 16 }
                },
                new Agent()
                {
                    Name = "Charlie",
                    Shifts = new List<Shift>()
                    {
                        new Shift()
                        {
                            StartTimeUtc = new DateTime(2022, 1, 1, 8, 0, 0), //8 am
                            StartLocation = new SimpleWaypoint("1 Microsoft way, WA 98052, US"),
                            EndTimeUtc = new DateTime(2022, 1, 1, 18, 0, 0), //6pm
                            EndLocation = new SimpleWaypoint(47.7070790545669, -122.355226696231),
                            Breaks = new Break[]
                            {
                                new Break()
                                {
                                    StartTimeUtc = new DateTime(2022, 1, 1, 12, 0, 0), //12pm/noon
                                    EndTimeUtc = new DateTime(2022, 1, 1, 14, 0, 0),   //2pm
                                    DurationTimeSpan = new TimeSpan(0, 30, 0) //30 minutes.
                                },
                                new Break()
                                {
                                    StartTimeUtc = new DateTime(2022, 1, 1, 16, 0, 0), //4pm
                                    EndTimeUtc = new DateTime(2022, 1, 1, 16, 30, 0)   //4:30pm
                                }
                            }
                        }
                    },
                    Price = new Price()
                    {
                        FixedPrice = 100,
                        PricePerHour = 5,
                        PricePerKM = 1
                    },
                    Capacity = new int[] { 16 }
                },
                new Agent()
                {
                    Name = "Terry",
                    Shifts = new List<Shift>()
                    {
                        new Shift()
                        {
                            StartTimeUtc = new DateTime(2022, 1, 1, 8, 0, 0), //8 am
                            StartLocation = new SimpleWaypoint(47.746135, -122.199314),
                            EndTimeUtc = new DateTime(2022, 1, 1, 18, 0, 0), //6pm
                            EndLocation = new SimpleWaypoint(47.746135, -122.199314),
                            Breaks = new Break[]
                            {
                                new Break()
                                {
                                    StartTimeUtc = new DateTime(2022, 1, 1, 12, 0, 0), //12pm/noon
                                    EndTimeUtc = new DateTime(2022, 1, 1, 14, 0, 0),   //2pm
                                    DurationTimeSpan = new TimeSpan(0, 30, 0) //30 minutes.
                                },
                                new Break()
                                {
                                    StartTimeUtc = new DateTime(2022, 1, 1, 16, 0, 0), //4pm
                                    EndTimeUtc = new DateTime(2022, 1, 1, 16, 30, 0)   //4:30pm
                                }
                            }
                        }
                    },
                    Price = new Price()
                    {
                        FixedPrice = 100,
                        PricePerHour = 5,
                        PricePerKM = 1
                    },
                    Capacity = new int[] { 16 }
                }
            },
            ItineraryItems = new List<OptimizeItineraryItem>()
            {
                new OptimizeItineraryItem()
                {
                    Name = "Customer 1",
                    OpeningTimeUtc = new DateTime(2022, 1, 1, 9, 0, 0), //9am
                    ClosingTimeUtc = new DateTime(2022, 1, 1, 18, 0, 0),   //6pm
                    DwellTimeSpan = new TimeSpan(0, 32, 0), //32 minutes
                    Priority = 5,
                    Quantity = new int[] { 4 },
                    //Waypoint = new SimpleWaypoint(47.692290770423,-122.385954752402),
                    Waypoint = new SimpleWaypoint("8712 Jones Pl NW, Seattle, WA 98117, US")
                },
                new OptimizeItineraryItem()
                {
                    Name = "Customer 2",
                    OpeningTimeUtc = new DateTime(2022, 1, 1, 9, 0, 0), //9am
                    ClosingTimeUtc = new DateTime(2022, 1, 1, 18, 0, 0),   //6pm
                    DwellTimeSpan = new TimeSpan(1, 34, 0), //1 hour 34 minutes
                    Priority = 16,
                    Quantity = new int[] { -3 },
                    Waypoint = new SimpleWaypoint(47.6962193175262,-122.342180147243),
                    DropOffFrom = new string[] { "Customer 3" }
                },
                new OptimizeItineraryItem()
                {
                    Name = "Customer 3",
                    OpeningTimeUtc = new DateTime(2022, 1, 1, 9, 0, 0), //9am
                    ClosingTimeUtc = new DateTime(2022, 1, 1, 18, 0, 0),   //6pm
                    DwellTimeSpan = new TimeSpan(1, 0, 0), //1 hour
                    Priority = 88,
                    Quantity = new int[] { 3 },
                    Waypoint = new SimpleWaypoint(47.6798098928389,-122.383036445391)
                },
                new OptimizeItineraryItem()
                {
                    Name = "Customer 4",
                    OpeningTimeUtc = new DateTime(2022, 1, 1, 9, 0, 0), //9am
                    ClosingTimeUtc = new DateTime(2022, 1, 1, 18, 0, 0),   //6pm
                    DwellTimeSpan = new TimeSpan(0, 25, 0), //25 minutes
                    Priority = 3,
                    Quantity = new int[] { -3 },
                    Waypoint = new SimpleWaypoint(47.6867440824094,-122.354711700877),
                    DropOffFrom = new string[] { "Customer 1" }
                },
                new OptimizeItineraryItem()
                {
                    Name = "Customer 5",
                    OpeningTimeUtc = new DateTime(2022, 1, 1, 9, 0, 0), //9am
                    ClosingTimeUtc = new DateTime(2022, 1, 1, 18, 0, 0),   //6pm
                    DwellTimeSpan = new TimeSpan(0, 18, 0), //18 minutes
                    Priority = 1,
                    Quantity = new int[] { -1 },
                    Waypoint = new SimpleWaypoint(47.6846639223203,-122.364839942855),
                    DropOffFrom = new string[] { "Customer 1" }
                },
                new OptimizeItineraryItem()
                {
                    Name = "Customer 6",
                    OpeningTimeUtc = new DateTime(2022, 1, 1, 9, 0, 0), //9am
                    ClosingTimeUtc = new DateTime(2022, 1, 1, 18, 0, 0),   //6pm
                    DwellTimeSpan = new TimeSpan(0, 18, 0), //18 minutes
                    Priority = 1,
                    Quantity = new int[] { 5 },
                    Waypoint = new SimpleWaypoint(47.668492, -122.193820)
                },
                new OptimizeItineraryItem()
                {
                    Name = "Customer 7",
                    OpeningTimeUtc = new DateTime(2022, 1, 1, 9, 0, 0), //9am
                    ClosingTimeUtc = new DateTime(2022, 1, 1, 18, 0, 0),   //6pm
                    DwellTimeSpan = new TimeSpan(0, 18, 0), //18 minutes
                    Priority = 1,
                    Quantity = new int[] { 5 },
                    Waypoint = new SimpleWaypoint(47.745673, -122.246702)
                },
                new OptimizeItineraryItem()
                {
                    Name = "Customer 8",
                    OpeningTimeUtc = new DateTime(2022, 1, 1, 9, 0, 0), //9am
                    ClosingTimeUtc = new DateTime(2022, 1, 1, 18, 0, 0),   //6pm
                    DwellTimeSpan = new TimeSpan(0, 18, 0), //18 minutes
                    Priority = 1,
                    Quantity = new int[] { 5 },
                    Waypoint = new SimpleWaypoint(47.609720, -122.143685)
                },
                new OptimizeItineraryItem()
                {
                    Name = "Customer 9",
                    OpeningTimeUtc = new DateTime(2022, 1, 1, 9, 0, 0), //9am
                    ClosingTimeUtc = new DateTime(2022, 1, 1, 18, 0, 0),   //6pm
                    DwellTimeSpan = new TimeSpan(0, 18, 0), //18 minutes
                    Priority = 1,
                    Quantity = new int[] { 5 },
                    Waypoint = new SimpleWaypoint(47.536975, -122.167035)
                },
                new OptimizeItineraryItem()
                {
                    Name = "Customer 10",
                    OpeningTimeUtc = new DateTime(2022, 1, 1, 9, 0, 0), //9am
                    ClosingTimeUtc = new DateTime(2022, 1, 1, 18, 0, 0),   //6pm
                    DwellTimeSpan = new TimeSpan(0, 18, 0), //18 minutes
                    Priority = 1,
                    Quantity = new int[] { 5 },
                    Waypoint = new SimpleWaypoint(47.594438, -122.311946)
                },
                new OptimizeItineraryItem()
                {
                    Name = "Customer 11",
                    OpeningTimeUtc = new DateTime(2022, 1, 1, 9, 0, 0), //9am
                    ClosingTimeUtc = new DateTime(2022, 1, 1, 18, 0, 0),   //6pm
                    DwellTimeSpan = new TimeSpan(0, 18, 0), //18 minutes
                    Priority = 1,
                    Quantity = new int[] { 5 },
                    Waypoint = new SimpleWaypoint(47.583784, -122.180084)
                },
                new OptimizeItineraryItem()
                {
                    Name = "Customer 12",
                    OpeningTimeUtc = new DateTime(2022, 1, 1, 9, 0, 0), //9am
                    ClosingTimeUtc = new DateTime(2022, 1, 1, 18, 0, 0),   //6pm
                    DwellTimeSpan = new TimeSpan(0, 18, 0), //18 minutes
                    Priority = 1,
                    Quantity = new int[] { 5 },
                    Waypoint = new SimpleWaypoint(47.658778, -122.047535)
                },
                new OptimizeItineraryItem()
                {
                    Name = "Customer 13",
                    OpeningTimeUtc = new DateTime(2022, 1, 1, 9, 0, 0), //9am
                    ClosingTimeUtc = new DateTime(2022, 1, 1, 18, 0, 0),   //6pm
                    DwellTimeSpan = new TimeSpan(0, 18, 0), //18 minutes
                    Priority = 1,
                    Quantity = new int[] { 5 },
                    Waypoint = new SimpleWaypoint(47.727659, -122.101104)
                },
                new OptimizeItineraryItem()
                {
                    Name = "Customer 14",
                    OpeningTimeUtc = new DateTime(2022, 1, 1, 9, 0, 0), //9am
                    ClosingTimeUtc = new DateTime(2022, 1, 1, 18, 0, 0),   //6pm
                    DwellTimeSpan = new TimeSpan(0, 18, 0), //18 minutes
                    Priority = 1,
                    Quantity = new int[] { 5 },
                    Waypoint = new SimpleWaypoint(47.701782, -122.298898)
                },
                new OptimizeItineraryItem()
                {
                    Name = "Customer 15",
                    OpeningTimeUtc = new DateTime(2022, 1, 1, 9, 0, 0), //9am
                    ClosingTimeUtc = new DateTime(2022, 1, 1, 18, 0, 0),   //6pm
                    DwellTimeSpan = new TimeSpan(0, 40, 0), //18 minutes
                    Priority = 1,
                    Quantity = new int[] { 15 },
                    Waypoint = new SimpleWaypoint(47.791548, -122.204411)
                }
            }
        };

        OptimizeItinerary result = null;

        //Only used for import/export feature in sample.
        DataContractJsonSerializer requestSerializer = new DataContractJsonSerializer(typeof(OptimizeItineraryRequest));
        DataContractJsonSerializer resultSerializer = new DataContractJsonSerializer(typeof(OptimizeItinerary));

        Color[] AgentColors = new Color[] {
            Colors.Blue, Colors.Orange, Colors.Red
        };

        MapLayer pinLayer = new MapLayer();
        MapLayer lineLayer = new MapLayer();

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();

            //Set the credentials on the map.
            MyMap.CredentialsProvider = new ApplicationIdCredentialsProvider(BingMapsKey);

            //Add a layers for pushpins and lines to the map after it has loaded.
            MyMap.Loaded += (s, e) =>
            {
                MyMap.Children.Add(lineLayer);
                MyMap.Children.Add(pinLayer);
            };

            LoadRequestInsights();
        }

        #endregion

        #region Button Handlers

        private void InstructionsBtn_Clicked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Instructions:\n\n1. Click \"Calculate itinerary\" to make a Multi-route optimization request.\n2. Hover over pins to see some details.\n3. Click \"Calculate route path\" to calculate and show the route path along the roads.\n4. Optionally export/import requests and results.");
        }

        private async void CalculateBtn_Clicked(object sender, RoutedEventArgs e)
        {
            result = null;

            //Show loading screen and clear output. 
            LoadingScreen.Visibility = Visibility.Visible;
            OutputTbx.Text = string.Empty;
            CalculateRoutePathBtn.IsEnabled = false;

            //Clear data on map.
            lineLayer.Children.Clear();
            pinLayer.Children.Clear();

            try
            {
                //Ensure request has credentials set.
                request.BingMapsKey = BingMapsKey;

                //Execute the request.
                var response = await request.Execute();

                //Ensure the response contains results.
                if (Response.HasResource(response))
                {
                    //Get the result.
                    result = Response.GetFirstResource(response) as OptimizeItinerary;
                    
                    ShowResultOnMap();
                } 
                else
                {
                    MessageBox.Show("Unable to calculate optimized route itinerary.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //Hide loading screen.
            LoadingScreen.Visibility = Visibility.Collapsed;
        }

        private async void CalculateRoutePathBtn_Clicked(object sender, RoutedEventArgs e)
        {
            //Remove any existing lines.
            lineLayer.Children.Clear();

            //Loop through each agent and calculate a route along the roads. 
            for (int i = 0; i < result.AgentItineraries.Length; i++)
            {
                var a = result.AgentItineraries[i];

                //Create a route request to get a route paths along a road.
                var r = new RouteRequest()
                {
                    //Customize these options based on agents.
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
                            //Only retrieving route path information for visualization. Change this to "All" if you want the instructions too.
                            RouteAttributeType.RoutePath
                        },
                        Optimize = RouteOptimizationType.TimeWithTraffic
                    },
                    Waypoints = a.Route.GetAllWaypoints(),
                    BingMapsKey = BingMapsKey
                };

                var route = await r.Execute();

                //Show the route on the map.
                ShowRouteOnMap(route, AgentColors[i]);
            }
        }

        [STAThread]
        private void ExportRequestBtn_Clicked(object sender, RoutedEventArgs e)
        {
            //Searilize the request object into a string and save as a JSON file. 

            //Remove credendtials from request for security reasons.
            request.BingMapsKey = string.Empty;

            var sfd = new SaveFileDialog();
            sfd.FileName = "MOIRequest.json";
            sfd.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            sfd.RestoreDirectory = true;

            var dl = sfd.ShowDialog();
            if (dl.HasValue && dl.Value)
            {
                using (var fs = sfd.OpenFile())
                {
                    try
                    {
                        requestSerializer.WriteObject(fs, request);
                    }
                    catch
                    {
                        MessageBox.Show("Error exporting request.");
                    }
                }
            }
        }

        [STAThread]
        private void ImportRequestBtn_Clicked(object sender, RoutedEventArgs e)
        {
            //Load a request from a JSON file.
            var ofd = new OpenFileDialog();

            ofd.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            ofd.RestoreDirectory = true;

            var dl = ofd.ShowDialog();
            if (dl.HasValue && dl.Value)
            {
                using (var fs = ofd.OpenFile())
                {
                    try
                    {
                        request = requestSerializer.ReadObject(fs) as OptimizeItineraryRequest;
                        LoadRequestInsights();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Error importing request.");
                    }
                }
            }
        }

        [STAThread]
        private void ExportResultBtn_Clicked(object sender, RoutedEventArgs e)
        {
            if(result == null)
            {
                MessageBox.Show("No result to export.");
                return;
            }

            //Searilize the result object into a string and save as a JSON file. 

            var sfd = new SaveFileDialog();
            sfd.FileName = "OptimizedItinerary.json";
            sfd.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            sfd.RestoreDirectory = true;

            var dl = sfd.ShowDialog();
            if (dl.HasValue && dl.Value)
            { 
                using (var fs = sfd.OpenFile())
                {
                    try
                    {
                        resultSerializer.WriteObject(fs, result);
                    }
                    catch
                    {
                        MessageBox.Show("Error exporting result.");
                    }
                }
            }
        }

        [STAThread]
        private void ImportResultBtn_Clicked(object sender, RoutedEventArgs e)
        {
            //Load a request from a JSON file.
            var ofd = new OpenFileDialog();

            ofd.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            ofd.RestoreDirectory = true;

            var dl = ofd.ShowDialog();
            if (dl.HasValue && dl.Value)
            {
                using (var fs = ofd.OpenFile())
                {
                    try
                    {
                        result = resultSerializer.ReadObject(fs) as OptimizeItinerary;
                        ShowResultOnMap();
                    }
                    catch
                    {
                        MessageBox.Show("Error importing result.");
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private void ShowResultOnMap()
        {
            //Get the order of stops for each agent.
            var sb = new StringBuilder();

            foreach (var a in result.AgentItineraries)
            {
                sb.AppendLine("Stops for agent " + a.Agent.Name);

                int cnt = 1;

                foreach (var i in a.Instructions)
                {
                    switch (i.InstructionType)
                    {
                        case OptimizeInstructionType.LeaveFromStartPoint:
                            sb.Append("\tStart\n");
                            break;
                        case OptimizeInstructionType.TakeABreak:
                            sb.AppendFormat("\t{0}) Break\n", cnt);
                            cnt++;
                            break;
                        case OptimizeInstructionType.VisitLocation:
                            sb.AppendFormat("\t{0}) {1}\n", cnt, i.ItineraryItem.Name);
                            cnt++;
                            break;
                        case OptimizeInstructionType.ArriveToEndPoint:
                            sb.Append("\tEnd\n");
                            break;
                        case OptimizeInstructionType.TravelBetweenLocations:
                            break;
                    }
                }
            }

            if (result.UnscheduledItems != null && result.UnscheduledItems.Length > 0)
            {
                sb.AppendLine("\nUnscheduled stops:");
                foreach (var ui in result.UnscheduledItems)
                {
                    sb.AppendLine(ui.Name);
                }
            }

            if (result.UnusedAgents != null && result.UnusedAgents.Length > 0)
            {
                sb.AppendLine("\nUnscheduled agents:");
                foreach (var ua in result.UnusedAgents)
                {
                    sb.AppendLine(ua.Name);
                }
            }

            OutputTbx.Text = sb.ToString();

            //Show the response information on the map.
            var allLocations = new List<Microsoft.Maps.MapControl.WPF.Location>();

            //Loop through each agent.
            for (int i = 0; i < result.AgentItineraries.Length; i++)
            {
                var ai = result.AgentItineraries[i];
                var tooltipInfo = new StringBuilder();

                foreach (var ins in ai.Instructions)
                {
                    tooltipInfo.Clear();
                    string txt = string.Empty;
                    Microsoft.Maps.MapControl.WPF.Location loc = null;

                    //Show Start/End, and stop locations as pushpins.
                    switch (ins.InstructionType)
                    {
                        case OptimizeInstructionType.LeaveFromStartPoint:
                            loc = CoordToLoc(ins.ItineraryItem.Waypoint.Coordinate);
                            txt = ai.Agent.Name + ": Start";
                            break;
                        case OptimizeInstructionType.ArriveToEndPoint:
                            loc = CoordToLoc(ins.ItineraryItem.Waypoint.Coordinate);
                            txt = ai.Agent.Name + ": End";
                            break;
                        case OptimizeInstructionType.VisitLocation:
                            loc = CoordToLoc(ins.ItineraryItem.Waypoint.Coordinate);
                            txt = ins.ItineraryItem.Name;
                            break;
                    }

                    if (loc != null)
                    {
                        tooltipInfo.AppendFormat("Agent: {0}\n", ai.Agent.Name);
                        tooltipInfo.AppendFormat("Instruction type: {0}\n", ins.InstructionType);
                        tooltipInfo.AppendFormat("Instruction: {0}\n", txt);
                        tooltipInfo.AppendFormat("Start time: {0}\n", ins.StartTimeUtc);
                        tooltipInfo.AppendFormat("End time: {0}\n", ins.EndTimeUtc);
                        tooltipInfo.AppendFormat("Duration: {0:g}", ins.DurationTimeSpan);

                        /*pinLayer.Children.Add(new Pushpin()
                        {
                            Location = loc,
                            Content = ins.ItineraryItem.Name
                        });*/

                        //Create custom pushpin for location.
                        var pin = new Grid();
                        pin.Children.Add(new Ellipse()
                        {
                            Stroke = new SolidColorBrush(Colors.White),
                            StrokeThickness = 3,
                            Fill = new SolidColorBrush(AgentColors[i]),
                            Width = 20,
                            Height = 20,
                            HorizontalAlignment = HorizontalAlignment.Center
                        });
                        pin.Children.Add(new TextBlock()
                        {
                            Text = txt,
                            FontSize = 14,
                            FontWeight = FontWeights.Bold,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Margin = new Thickness(0,0,0,40),
                            Background = new SolidColorBrush(Colors.White)
                        });
                        MapLayer.SetPosition(pin, loc);
                        MapLayer.SetPositionOrigin(pin, PositionOrigin.Center);
                        pinLayer.Children.Add(pin);

                        //Create a tooltip for the pushpin.
                        ToolTipService.SetToolTip(pin, new ToolTip()
                        {
                            Content = tooltipInfo.ToString(),
                            Style = Application.Current.Resources["CustomInfoboxStyle"] as Style
                        });
                    }
                }

                //Get the ordered coordinates between the start/stops/breaks/end.
                var lc = new LocationCollection();

                lc.Add(CoordToLoc(ai.Route.StartLocation));

                foreach (var wp in ai.Route.Waypoints)
                {
                    lc.Add(CoordToLoc(wp));
                }

                lc.Add(CoordToLoc(ai.Route.EndLocation));

                //Capture all locations for use later when calculating map view.
                allLocations.AddRange(lc);

                lineLayer.Children.Clear();

                //Create a polyline for the path and add it to the map.
                lineLayer.Children.Add(new MapPolyline()
                {
                    Locations = lc,
                    Stroke = new SolidColorBrush(AgentColors[i]),
                    StrokeThickness = 5
                });
            }

            //Calculate the best map view to view the results.
            Microsoft.Maps.MapControl.WPF.Location center;
            double zoom;

            GetBestMapView(allLocations, MyMap.ActualWidth, MyMap.ActualHeight, 30, out center, out zoom);

            MyMap.Center = center;
            MyMap.ZoomLevel = zoom;

            //Since there is a result now, enable route path calculation.
            CalculateRoutePathBtn.IsEnabled = true;
        }

        private void ShowRouteOnMap(Response routeResponse, Color agentColor)
        {
            if(Response.HasResource(routeResponse))
            {
                var route = Response.GetFirstResource(routeResponse) as Route;

                if (route != null)
                {
                    var lc = new LocationCollection();
                    var coords = route.RoutePath.GetCoordinates();

                    if (coords != null)
                    {
                        foreach (var c in coords)
                        {
                            lc.Add(CoordToLoc(c));
                        }

                        //Create a polyline for the path and add it to the map.
                        lineLayer.Children.Add(new MapPolyline()
                        {
                            Locations = lc,
                            Stroke = new SolidColorBrush(agentColor),
                            StrokeThickness = 5
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Converts a Coordinate from toolkit into a WPF Location.
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        private Microsoft.Maps.MapControl.WPF.Location CoordToLoc(Coordinate coord)
        {
            return new Microsoft.Maps.MapControl.WPF.Location(coord.Latitude, coord.Longitude);
        }

        /// <summary>
        /// Calculates the best map view for a list of locations for a map. Based on https://rbrundritt.wordpress.com/2009/07/21/determining-best-map-view-for-an-array-of-locations/
        /// </summary>
        /// <param name="locations">List of location objects</param>
        /// <param name="mapWidth">Map width in pixels</param>
        /// <param name="mapHeight">Map height in pixels</param>
        /// <param name="buffer">Width in pixels to use to create a buffer around the map. This is to keep pushpins from being cut off on the edge</param>
        /// <returns>The best center and zoom level for the specified set of locations.</returns>
        public void GetBestMapView(List<Microsoft.Maps.MapControl.WPF.Location> locations, double mapWidth, double mapHeight, int buffer, out Microsoft.Maps.MapControl.WPF.Location center, out double zoomLevel)
        {
            center = new Microsoft.Maps.MapControl.WPF.Location();
            zoomLevel = 0;

            double maxLat = -85;
            double minLat = 85;
            double maxLon = -180;
            double minLon = 180;

            //calculate bounding rectangle
            for (int i = 0; i < locations.Count; i++)
            {
                if (locations[i].Latitude > maxLat)
                {
                    maxLat = locations[i].Latitude;
                }

                if (locations[i].Latitude < minLat)
                {
                    minLat = locations[i].Latitude;
                }

                if (locations[i].Longitude > maxLon)
                {
                    maxLon = locations[i].Longitude;
                }

                if (locations[i].Longitude < minLon)
                {
                    minLon = locations[i].Longitude;
                }
            }

            center.Latitude = (maxLat + minLat) / 2;
            center.Longitude = (maxLon + minLon) / 2;

            double zoom1 = 0, zoom2 = 0;

            //Determine the best zoom level based on the map scale and bounding coordinate information
            if (maxLon != minLon && maxLat != minLat)
            {
                //best zoom level based on map width
                zoom1 = Math.Log(360.0 / 256.0 * (mapWidth - 2 * buffer) / (maxLon - minLon)) / Math.Log(2);
                //best zoom level based on map height
                zoom2 = Math.Log(180.0 / 256.0 * (mapHeight - 2 * buffer) / (maxLat - minLat)) / Math.Log(2);
            }

            //use the most zoomed out of the two zoom levels
            zoomLevel = (zoom1 < zoom2) ? zoom1 : zoom2;
        }

        public void LoadRequestInsights()
        {
            if(request != null)
            {
                //Define agent table.
                DataTable agentDataTable = new DataTable();
                agentDataTable.Columns.Add("Agent", typeof(string));
                agentDataTable.Columns.Add("Shift Start", typeof(string));
                agentDataTable.Columns.Add("Start Location", typeof(string));
                agentDataTable.Columns.Add("Shift End", typeof(string));
                agentDataTable.Columns.Add("End Location", typeof(string));
                agentDataTable.Columns.Add("Capacity", typeof(string));
                agentDataTable.Columns.Add("Fixed Price", typeof(double));
                agentDataTable.Columns.Add("Price / KM", typeof(double));
                agentDataTable.Columns.Add("Price / Hour", typeof(double));

                //Add data.
                foreach(var a in request.Agents)
                {
                    var firstShift = a.Shifts[0];
                    var lastShift = a.Shifts[a.Shifts.Count - 1];

                    agentDataTable.Rows.Add(
                        a.Name,
                        firstShift.StartTimeUtc.ToString(),
                        firstShift.StartLocation.ToString(),
                        lastShift.EndTimeUtc.ToString(),
                        lastShift.EndLocation.ToString(),
                        (a.Capacity != null) ? "[" + string.Join(",", a.Capacity) + "]" : "",
                        a.Price.FixedPrice,
                        a.Price.PricePerKM,
                        a.Price.PricePerHour
                    );                    
                }

                var ds = new DataSet();
                ds.Tables.Add(agentDataTable);
                AgentTable.ItemsSource = ds.Tables[0].DefaultView;

                //Load itinerary stops.

                //Define itinerary table.
                DataTable itineraryDataTable = new DataTable();
                itineraryDataTable.Columns.Add("Stop", typeof(int));
                itineraryDataTable.Columns.Add("Name", typeof(string));
                itineraryDataTable.Columns.Add("Opening", typeof(string));
                itineraryDataTable.Columns.Add("Closing", typeof(string));
                itineraryDataTable.Columns.Add("Dwell", typeof(string));
                itineraryDataTable.Columns.Add("Priority", typeof(int));
                itineraryDataTable.Columns.Add("Location", typeof(string));
                itineraryDataTable.Columns.Add("Quantity", typeof(string));
                itineraryDataTable.Columns.Add("Drop Off", typeof(string));

                //Add data.
                for(int i=0;i<request.ItineraryItems.Count;i++)
                {
                    var stop = request.ItineraryItems[i];

                    itineraryDataTable.Rows.Add(
                        i,
                        stop.Name,
                        (stop.OpeningTimeUtc.HasValue) ? stop.OpeningTimeUtc.Value.ToShortTimeString(): "",
                        (stop.ClosingTimeUtc.HasValue) ? stop.ClosingTimeUtc.Value.ToShortTimeString() : "",
                        (stop.DwellTimeSpan.HasValue) ? stop.DwellTimeSpan.Value.ToString() : "",
                        stop.Priority,
                        stop.Waypoint.ToString(), 
                        (stop.Quantity != null) ? "[" + string.Join(",", stop.Quantity) + "]" : "",
                        (stop.DropOffFrom != null) ? "[" + string.Join(",", stop.DropOffFrom) + "]" : ""
                    );
                }

                var ds2 = new DataSet();
                ds2.Tables.Add(itineraryDataTable);
                IniteraryStopTable.ItemsSource = ds2.Tables[0].DefaultView;
            }
        }

        #endregion
    }
}
