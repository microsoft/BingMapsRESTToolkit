using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// Request for Bing Maps Multi-Itinerary Optimization API.
    /// </summary>
    public class OptimizeItineraryRequest : BaseRestRequest
    {
        #region Private Properties

        /// <summary>
        /// Maximum number of agents supported in an asynchronous Optimize Itinerary request from an enterprise account.
        /// </summary>
        private int maxAgents = 200;

        /// <summary>
        /// Maximum number of itinerary items supported in an asynchronous Optimize Itinerary request from an enterprise account.
        /// </summary>
        private int maxItineraryItems = 2000;

        #endregion

        #region Constructor 

        /// <summary>
        /// Request for Bing Maps Multi-Itinerary Optimization API.
        /// </summary>
        public OptimizeItineraryRequest() : base()
        {
            CostValue = CostValueType.TravelTime;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// List of agent itinerary information: including the agent name, shift starting and ending locations for agent, and capacity of the agent's vehicle.
        /// </summary>
        public List<Agent> Agents { get; set; }

        /// <summary>
        /// List of itinerary items to be scheduled among the specified agents, including the location name, location (lat/lon), priority, dwell time, business closing and opening times for each item to be scheduled, quantity to be delivered to or picked up from each location, and pickup/drop off sequence dependency with other itineraryItems.
        /// </summary>
        public List<OptimizeItineraryItem> ItineraryItems { get; set; }

        private OptimizationType _type = OptimizationType.SimpleRequest;

        /// <summary>
        /// Specifies whether traffic data should used to optimize the order of waypoint items. Default: SimpleRequest
        /// Note: If the ‘type’ parameter is set to ‘TrafficRequest’, it will automatically use ‘true’ as the ‘roadnetwork’ parameter value.
        /// </summary>
        public OptimizationType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;

                if (value == OptimizationType.TrafficRequest) {
                    RoadNetwork = true;
                } 
            }
        }

        /// <summary>
        /// Optional. 
        /// If true, uses actual road network information, and both travel distances and travel times between the itinerary locations to calculate optimizations. 
        /// If false, a constant radius is used to measure distances and a constant travel speed is used to calculate travel times between locations.
        /// </summary>
        public bool RoadNetwork { get; set; }

        /// <summary>
        /// A parameter used to optimize itineraries in addition to maximizing the sum of item priorities. Default: TravelTime
        /// </summary>
        public CostValueType CostValue { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Executes the request. 
        /// </summary>
        /// <returns>A response containing the requested data.</returns>
        public override async Task<Response> Execute()
        {
            return await this.Execute(null).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes the request. 
        /// </summary>
        /// <param name="remainingTimeCallback">A callback function in which the estimated remaining time in seconds is sent.</param>
        /// <returns>A response containing the requested data.</returns>
        public override async Task<Response> Execute(Action<int> remainingTimeCallback)
        {
            var requestUrl = GetRequestUrl();
            var requestBody = GetPostRequestBody();

            return await ServiceHelper.MakeAsyncPostRequest(requestUrl, requestBody, remainingTimeCallback).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the request URL to perform a query to snap points to roads. This method will only generate an post URL. 
        /// </summary>
        /// <returns>A request URL to perform a query to snap points to roads.</returns>
        public override string GetRequestUrl()
        {
            if(Agents == null || Agents.Count == 0)
            {
                throw new Exception("Agents not specified.");
            }
            else if (Agents.Count > maxAgents)
            {
                throw new Exception(string.Format("More than {0} Agents specified.", maxAgents));
            }

            if (ItineraryItems == null || ItineraryItems.Count == 0)
            {
                throw new Exception("ItineraryItems not specified.");
            } 
            else if (ItineraryItems.Count > maxItineraryItems)
            {
                throw new Exception(string.Format("More than {0} ItineraryItems specified.", maxItineraryItems));
            }

            //Make an async request.
            return this.Domain + "Routes/OptimizeItineraryAsync?key=" + this.BingMapsKey;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets a POST body for a snap to road request.
        /// </summary>
        /// <returns>A POST body for a snap to road request.</returns>
        private string GetPostRequestBody()
        {
            var sb = new StringBuilder();

            sb.Append("{");

            sb.AppendFormat("\"agents\":[{0}],", string.Join(",",Agents));
            sb.AppendFormat("\"itineraryItems\":[{0}],", string.Join(",", ItineraryItems));

            sb.AppendFormat("\"type\":\"{0}\",", Enum.GetName(typeof(OptimizationType), Type));
            sb.AppendFormat("\"roadnetwork\":{0},", RoadNetwork.ToString().ToLowerInvariant());
            sb.AppendFormat("\"costvalue\":\"{0}\"", Enum.GetName(typeof(CostValueType), CostValue));

            sb.Append("}");

            return sb.ToString();
        }

        #endregion
    }
}
