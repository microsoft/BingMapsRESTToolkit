namespace BingMapsRESTToolkit
{
    /// <summary>
    /// Step warning type values that are returned by the Routes API.
    /// </summary>
    public enum WarningType
    {
        ///<summary>
        ///There is a traffic accident.
        ///</summary>
        Accident,

        ///<summary>
        ///The route has left one administrative division and entered another.
        ///</summary>
        AdminDivisionChange,

        ///<summary>
        ///The road is closed or blocked.
        ///</summary>
        BlockedRoad,

        ///<summary>
        ///Check a time table. This usually refers to a ferry or Autorail time table.
        ///</summary>
        CheckTimetable,

        ///<summary>
        ///The traffic is slow.
        ///</summary>
        Congestion,

        ///<summary>
        ///The route has left one country and entered another.
        ///</summary>
        CountryChange,

        ///<summary>
        ///There is a disabled vehicle.
        ///</summary>
        DisabledVehicle,

        ///<summary>
        ///A gate blocks the road and access is required to continue along the route.
        ///</summary>
        GateAccess,

        ///<summary>
        ///Get off the transit at this location.
        ///</summary>
        GetOffTransit,

        ///<summary>
        ///Get on the transit at this location.
        ///</summary>
        GetOnTransit,

        ///<summary>
        ///A U-turn is illegal at this location.
        ///</summary>
        IllegalUTurn,

        ///<summary>
        ///There is mass transit incident.
        ///</summary>
        MassTransit,

        ///<summary>
        ///A miscellaneous warning is available for this location.
        ///</summary>
        Miscellaneous,

        ///<summary>
        ///There is no incident at this location.
        ///</summary>
        NoIncident,

        ///<summary>
        ///There is no warning at this location.
        ///</summary>
        None,

        ///<summary>
        ///There is a warning at this location that cannot be classified as any other type of warning.
        ///</summary>
        Other,

        ///<summary>
        ///There is additional traffic incident information.
        ///</summary>
        OtherNews,

        ///<summary>
        ///There are other traffic incidents at this location.
        ///</summary>
        OtherTrafficIncidents,

        ///<summary>
        ///There are scheduled events in the area.
        ///</summary>
        PlannedEvents,

        ///<summary>
        ///The road being travelled on is private.
        ///</summary>
        PrivateRoad,

        ///<summary>
        ///The turn may be restricted.
        ///</summary>
        RestrictedTurn,

        ///<summary>
        ///There are road closures at this location.
        ///</summary>
        RoadClosures,

        ///<summary>
        ///There is a road hazard.
        ///</summary>
        RoadHazard,

        ///<summary>
        ///There is construction along the route. The ScheduledConstruction value is used for any type of construction and not just construction that has specific start and end dates.
        ///</summary>
        ScheduledConstruction,

        ///<summary>
        ///A seasonal closure occurs at this location.
        ///</summary>
        SeasonalClosures,

        ///<summary>
        ///A toll is required at this location to continue along the route.
        ///</summary>
        Tollbooth,

        ///<summary>
        ///The road is a toll road.
        ///</summary>
        TollRoad,

        ///<summary>
        ///The entrance to a toll zone.
        ///</summary>
        TollZoneEnter,

        ///<summary>
        ///The exit of a toll zone.
        ///</summary>
        TollZoneExit,

        ///<summary>
        ///The warning is about traffic flow.
        ///</summary>
        TrafficFlow,

        ///<summary>
        ///There is a transit line change but a change of vehicle is not required.
        ///</summary>
        TransitLineChange,

        ///<summary>
        ///The road is unpaved.
        ///</summary>
        UnpavedRoad,

        ///<summary>
        ///There is significant weather at this location.
        ///</summary>
        Weather
    }
}
